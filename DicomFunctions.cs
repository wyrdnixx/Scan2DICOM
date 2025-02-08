
using Dicom;
using Dicom.Log;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scan2Dicom
{




    public class DicomFunctions
    {
        

        public event EventHandler LogEvent;




        public async Task<String> DicomEchoAsync(String _Server, int _Port, String _callingAe, String _calledAe)
        {
            //var client = new DicomClient();
            var client = new Dicom.Network.Client.DicomClient(_Server, _Port, false, _callingAe, _calledAe);
            var cEchoRequest = new DicomCEchoRequest();

            String _res = null;
            
            //var cEchoRequest = new DicomCEchoRequest();
            cEchoRequest.OnResponseReceived += (request, response) =>
            {
                // Do something when we get back a response in the event, fire event, etc.
                Console.WriteLine("->" + response);
                _res =  response.ToString();
            };

            //client.AddRequest(cEchoRequest);
           
            var server = new Dicom.Network.DicomServer<DicomCEchoProvider>();

            //var server = new DicomServer<DicomCEchoProvider>(12345);

            //var client = DicomClientFactory.Create("127.0.0.1", 12345, false, "SCU", "ANY-SCP");
            client.NegotiateAsyncOps();
            for (int i = 0; i < 10; i++)
                await client.AddRequestAsync(cEchoRequest);

            try
            {
                await client.SendAsync();
            }catch(System.Net.Sockets.SocketException ex)
            {
                Console.WriteLine(ex.Message+ "\n");
                return ex.Message;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return _res;
        }
    
        public async Task<List<DicomDataset>> CFind(String _Server, int _Port, String _callingAe, String _calledAe, String patientID, String PatientName)
        {
            LogEvent("searching...");

            List<DicomDataset> searchResults = new List<DicomDataset>();

            var request = DicomCFindRequest.CreatePatientQuery(patientId: patientID, patientName: PatientName);
            request.OnResponseReceived += (DicomCFindRequest req, DicomCFindResponse rsp) => {
                
                    if (rsp.HasDataset)
                    {
                        Console.WriteLine("C-Find Response:\n" + rsp.Dataset.WriteToString());
                        searchResults.Add(rsp.Dataset);                    
                    }
                    else
                    {
                        //tbresult.Invoke(new MethodInvoker(delegate () { tbresult.Text += "nothing found"; }));
                     //   LogEvent("nothing found");
                        //Console.WriteLine("nothing found");
                    }                

            };

            //var client = new Dicom.Network.DicomClient();
            var client = new Dicom.Network.Client.DicomClient(_Server, _Port, false, _callingAe, _calledAe);
            await client.AddRequestAsync(request);
            try
            {
                await client.SendAsync();
            }catch(Exception ex)
            {
                // ToDo : Error in Textbox schreiben - Threat-Problem
                //MessageBox.Show("Search Error: " + ex.Message);
                LogEvent("Search Error: " + ex.Message);

            }
            if (searchResults.Count == 0)
            {
                LogEvent("nothing found");
            }
            

            return searchResults;

        }

        #region StoreSCU


        //public DicomClient CreateDicomStoreClient(string fileToTransmit)
        public DicomClient CreateDicomStoreClient(DicomFile fileToTransmit)
            
        {
            var client = new DicomClient();

            //request for DICOM store operation
            var dicomCStoreRequest = new DicomCStoreRequest(fileToTransmit);
            

            //attach an event handler when remote peer responds to store request 
            dicomCStoreRequest.OnResponseReceived += OnStoreResponseReceivedFromRemoteHost;
            client.AddRequest(dicomCStoreRequest);

            //Add a handler to be notified of any association rejections
            client.AssociationRejected += OnAssociationRejected;

            //Add a handler to be notified of any association information on successful connections
            client.AssociationAccepted += OnAssociationAccepted;

            //Add a handler to be notified when association is successfully released - this can be triggered by the remote peer as well
            client.AssociationReleased += OnAssociationReleased;

            return client;
        }


        private void OnStoreResponseReceivedFromRemoteHost(DicomCStoreRequest request, DicomCStoreResponse response)
        {
            //LogToDebugConsole("DICOM Store request was received by remote host for storage...");
            //LogToDebugConsole($"DICOM Store request was received by remote host for SOP instance transmitted for storage:{request.SOPInstanceUID}");
            //LogToDebugConsole($"Store operation response status returned was:{response.Status}");

            LogEvent("DICOM Store request was received by remote host for storage...");
            LogEvent($"DICOM Store request was received by remote host for SOP instance transmitted for storage:{request.SOPInstanceUID}");
            LogEvent($"Store operation response status returned was:{response.Status}");
        }

        private void OnAssociationAccepted(object sender, AssociationAcceptedEventArgs e)
        {
            //LogToDebugConsole($"Association was accepted by:{e.Association.RemoteHost}");
            LogEvent($"Association was accepted by:{e.Association.RemoteHost}");
        }

        private void OnAssociationRejected(object sender, AssociationRejectedEventArgs e)
        {
            //LogToDebugConsole($"Association was rejected. Rejected Reason:{e.Reason}");
            LogEvent($"Association was rejected. Rejected Reason:{e.Reason}");
        }

        private void OnAssociationReleased(object sender, EventArgs e)
        {
            //LogToDebugConsole("Association was released. BYE BYE!");
            LogEvent("Association was released. BYE BYE!");
        }

/*        private void LogToDebugConsole(string informationToLog)
        {
            Debug.WriteLine(informationToLog);
            //      logging(informationToLog);
        }*/



        #endregion

        /// <summary>
        /// generates DICOM Dataset
        /// </summary>
        /// <param name="PatientID">Patients ID</param>
        /// <param name="PatientName">Patients Name</param>
        /// <param name="PatientBirthDate">Patients birthdate in format yyyyMMdd</param>
        /// <param name="PatientSex">Patient sex (F|M)</param>
        /// <param name="PhysicanName">Doctor or document creator</param>
        /// <param name="DokTitle">Document title</param>
        /// <param name="DokDate">Document creation date</param>
        /// <param name="FileBytes">byte[] array document data</param>
        /// <returns></returns>
        public DicomDataset createPDFDataset(String PatientID, String PatientName, String PatientBirthDate, String PatientSex, String PhysicanName, String DokTitle, String DokDate, byte[] FileBytes)
        {
            var studyUID = new DicomUID("1.2.826.0.1.3680043.8.498", "Encapsulated PDF Storage", DicomUidType.SOPInstance);
            var generator = new DicomUIDGenerator();
            try
            {

            
            var dataset = new DicomDataset
            {
                { DicomTag.InstanceCreationDate, DateTime.Now },
                { DicomTag.InstanceCreationTime, DateTime.Now },
                { DicomTag.SOPClassUID, DicomUID.EncapsulatedPDFStorage },
                { DicomTag.SOPInstanceUID, generator.Generate( studyUID )  },
                { DicomTag.MediaStorageSOPClassUID, DicomUID.EncapsulatedPDFStorage }, //Encapsulated PDF Storage -1.2.840.10008.5.1.4.1.1.104.1
                { DicomTag.ContentDate, DateTime.Now },
                { DicomTag.ContentTime, DateTime.Now },
                { DicomTag.AcquisitionDateTime, DateTime.Now },
                { DicomTag.Modality, "DOC" },


                { DicomTag.Manufacturer, "JoHe-Software" },
                { DicomTag.ManufacturerModelName, "Scan2DICOM-Software" },
                { DicomTag.PerformingPhysicianName, PhysicanName },
                { DicomTag.DocumentTitle, DokTitle },
                { DicomTag.PatientName, PatientName},
                { DicomTag.PatientID, PatientID },
                { DicomTag.PatientBirthDate, PatientBirthDate },
                { DicomTag.PatientSex,PatientSex},
                //{ DicomTag.StudyDate, dpDokDate.Value.ToString("yyyyMMdd") },
                { DicomTag.StudyDate, DokDate },
                { DicomTag.StudyDescription, "imported PDF File" },
                //{ DicomTag.SeriesDate, DateTime.Now },
                //{ DicomTag.SeriesTime, DateTime.Now },
                { DicomTag.SeriesNumber, "1"},
                { DicomTag.InstanceNumber, "1"},
                { DicomTag.ConversionType, "WSD" },
                { DicomTag.SeriesDescription, ""},
                { DicomTag.SeriesInstanceUID, generator.Generate( studyUID ) },
                { DicomTag.StudyInstanceUID, generator.Generate( studyUID ) },
                { DicomTag.MIMETypeOfEncapsulatedDocument, "application/pdf" },                        
                { DicomTag.EncapsulatedDocument, FileBytes },
            };
                LogEvent("created encapsulated PDF dcm file");
                return dataset;
            }catch(Exception ex)
            {
                LogEvent(ex.Message);
                return null;
            }

            
        }

        #region JPEG2DICOM
        
        public DicomUID genStudyId()
        {
            var studyUID = new DicomUID("1.2.826.0.1.3680043.8.498", "Encapsulated PDF Storage", DicomUidType.SOPInstance);
            var generator = new DicomUIDGenerator();
            return generator.Generate(studyUID);
        }
        public DicomFile createJPGDataset(DicomUID uid, int InstanceNumber, Image image, String dicom_temp_file, String dcmtkpath, String PatientID, String PatientName, String PatientBirthDate, String PatientSex, String PhysicanName, String DokTitle, String DokDate)
        {
            image.Save(dicom_temp_file + ".jpg",ImageFormat.Jpeg);
            convertJPG(dicom_temp_file + ".jpg", dicom_temp_file, dcmtkpath);

            var studyUID = new DicomUID("1.2.826.0.1.3680043.8.498", "Encapsulated Document Storage", DicomUidType.SOPInstance);
            var generator = new DicomUIDGenerator();

            DicomFile dcmFile = DicomFile.Open(dicom_temp_file, FileReadOption.ReadAll);
            dcmFile.Dataset.AddOrUpdate();

            dcmFile.Dataset.AddOrUpdate(DicomTag.InstanceCreationDate, DateTime.Now);
            dcmFile.Dataset.AddOrUpdate(DicomTag.InstanceCreationTime, DateTime.Now);
            dcmFile.Dataset.AddOrUpdate(DicomTag.SOPClassUID, DicomUID.EncapsulatedPDFStorage);
            dcmFile.Dataset.AddOrUpdate(DicomTag.SOPInstanceUID, generator.Generate(studyUID));
            //dcmFile.Dataset.AddOrUpdate(DicomTag.SOPInstanceUID, uid);
            dcmFile.Dataset.AddOrUpdate(DicomTag.MediaStorageSOPClassUID, DicomUID.JPEG2000);
            dcmFile.Dataset.AddOrUpdate( DicomTag.ContentDate, DateTime.Now);
            dcmFile.Dataset.AddOrUpdate( DicomTag.ContentTime, DateTime.Now);
            dcmFile.Dataset.AddOrUpdate( DicomTag.AcquisitionDateTime, DateTime.Now);
            dcmFile.Dataset.AddOrUpdate( DicomTag.Modality, "DOC");
            dcmFile.Dataset.AddOrUpdate( DicomTag.Manufacturer, "JoHe-Software");
            dcmFile.Dataset.AddOrUpdate( DicomTag.ManufacturerModelName, "Scan2DICOM-Software");
            dcmFile.Dataset.AddOrUpdate( DicomTag.PerformingPhysicianName, PhysicanName);            
            dcmFile.Dataset.AddOrUpdate(DicomTag.InstitutionName, PhysicanName);
            dcmFile.Dataset.AddOrUpdate( DicomTag.DocumentTitle, DokTitle);
            dcmFile.Dataset.AddOrUpdate( DicomTag.PatientName, PatientName);
            dcmFile.Dataset.AddOrUpdate( DicomTag.PatientID, PatientID);
            dcmFile.Dataset.AddOrUpdate( DicomTag.PatientBirthDate, PatientBirthDate);
            dcmFile.Dataset.AddOrUpdate( DicomTag.PatientSex,PatientSex);
            dcmFile.Dataset.AddOrUpdate( DicomTag.StudyDate, DokDate);
            dcmFile.Dataset.AddOrUpdate( DicomTag.StudyDescription, "Document: "+ DokTitle);
            dcmFile.Dataset.AddOrUpdate( DicomTag.SeriesNumber, "1");
            //dcmFile.Dataset.AddOrUpdate( DicomTag.InstanceNumber, "1");
            dcmFile.Dataset.AddOrUpdate( DicomTag.InstanceNumber, InstanceNumber);
            dcmFile.Dataset.AddOrUpdate( DicomTag.ConversionType, "WSD");
            dcmFile.Dataset.AddOrUpdate( DicomTag.SeriesDescription, "Imported Document");
            dcmFile.Dataset.AddOrUpdate( DicomTag.SeriesInstanceUID, generator.Generate(uid));
            //dcmFile.Dataset.AddOrUpdate(DicomTag.StudyInstanceUID, generator.Generate(studyUID));
            dcmFile.Dataset.AddOrUpdate( DicomTag.SeriesInstanceUID, uid);
            dcmFile.Dataset.AddOrUpdate(DicomTag.StudyInstanceUID, uid);
            //dcmFile.Dataset.AddOrUpdate(DicomTag.StudyID, "U-ID000000"); // entfernt - Sectra PACs importiert bilder zum falschen Patient wenn die StudyID schon bei einem anderen Patient existiert

            try
            {
                dcmFile.Save(dicom_temp_file);
                LogEvent("saved to temp dmc file: " + dicom_temp_file);
                return dcmFile;
            }catch(Exception ex)
            {
                LogEvent("error writing dcm file: " + dicom_temp_file);
                LogEvent(ex.Message);
                return null;
            }
            
            
        }
        private String convertJPG(String _file,String dicom_temp_file, String dcmtkpath)
        {
            Process process = new Process();
            process.StartInfo.FileName = dcmtkpath + @"\img2dcm.exe";
            process.StartInfo.Arguments = _file + " " + dicom_temp_file + " -v"; // Note the /c command (*)
            //process.StartInfo.Arguments = @"C:\pc_inst\test.jpg C:\pc_inst\test_app.dcm -v"; // Note the /c command (*)
            //process.StartInfo.Arguments = @"/C C:\pc_inst\TestSCU\dcmtk\bin\img2dcm.exe C:\pc_inst\test.jpg C:\pc_inst\test_dcm.dcm -ll info"; // Note the /c command (*)
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            LogEvent("#--: " + output);
            string err = process.StandardError.ReadToEnd();
            LogEvent("E--: " + err);
            process.WaitForExit();

            if (err != null)
            {
                return err;

            }
            else
            {
                return output;
            }

        }
        /*
                public DicomDataset createJPGDataset(Image image)
                {
                    var studyUID = new DicomUID("1.2.826.0.1.3680043.8.498", "Scanned Document", DicomUidType.SOPInstance);
                    var generator = new DicomUIDGenerator();

                    int width = image.Width;
                    int height = image.Height;
                    byte[] b = ImageToByteArray(image);

                    var dataset = new DicomDataset{ };
                    dataset.AddOrUpdate(DicomTag.InstanceCreationDate, DateTime.Now);
                    dataset.AddOrUpdate(DicomTag.InstanceCreationDate, DateTime.Now);
                    dataset.AddOrUpdate(DicomTag.InstanceCreationTime, DateTime.Now);
                    dataset.AddOrUpdate(DicomTag.MediaStorageSOPClassUID, DicomUID.JPEGBaseline8Bit);
                    dataset.AddOrUpdate(DicomTag.TransferSyntaxUID, DicomTransferSyntax.JPEGProcess1);
                    dataset.AddOrUpdate(DicomTag.SOPClassUID, DicomUID.JPEG2000);
                    dataset.AddOrUpdate(DicomTag.SOPInstanceUID, generator.Generate(studyUID));
                    dataset.AddOrUpdate(DicomTag.SeriesInstanceUID, generator.Generate(studyUID));
                    dataset.AddOrUpdate(DicomTag.StudyInstanceUID, generator.Generate(studyUID));
                    dataset.AddOrUpdate(DicomTag.PixelData, b);
                    dataset.AddOrUpdate(DicomTag.SamplesPerPixel,(ushort) 3);
                    dataset.AddOrUpdate(DicomTag.BitsAllocated, (ushort)8);
                    dataset.AddOrUpdate(DicomTag.BitsStored, (ushort)8);
                    dataset.AddOrUpdate(DicomTag.HighBit, (ushort)7);
                    dataset.AddOrUpdate(DicomTag.LossyImageCompressionMethod, "ISO_10918_1");
                    //dataset.AddOrUpdate();
                    //dataset.AddOrUpdate();


                    DicomFile file = new DicomFile(dataset);
                    file.Save(@"c:\pc_inst\test.dcm");


                    return dataset;
                }
                public byte[] ImageToByteArray(Image img)
                {
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.ToArray();
                }*/

        #endregion
    }
}
