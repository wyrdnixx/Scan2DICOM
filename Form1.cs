using Dicom;
using Dicom.Imaging;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Scan2Dicom
{

    public delegate void EventHandler(string str);
    public delegate void ImgEventHandler(Image img);


    public partial class Form1 : Form
    {

        public DicomFunctions dicomFunctions;
        public TwainScanner twainScanner;

        private String DICOMServer;
        private int DICOMQueryPort;
        private int DICOMStoragePort;
        private String DICOMLocalAET;
        private String DICOMRemoteQueryAET;
        private String DICOMRemoteStoreAET;
        private String dcmtkPath;
        private string dicom_temp_file;
        //private List<Image> ScannedImages = new List<Image>();
        private List<ScannedPicture> ImageQueue = new List<ScannedPicture>();
        private int prevImageNumber;
        private int pbPreviewCurrentImage;

        CultureInfo provider = CultureInfo.InvariantCulture; // benoetigt fuer datumsumwandlung

        public Form1()
        {
            InitializeComponent();
            dicomFunctions = new DicomFunctions();
            // Log Event handler
            dicomFunctions.LogEvent += LogEvent;

            this.dgConfig.Columns.Add("Key", "Key");
            this.dgConfig.Columns.Add("Value", "Value");
            this.dicom_temp_file = System.IO.Path.GetTempPath() + @"\Scan2Dicom_Temp.dcm";

            dpDokDate.CustomFormat = "dd.MM.yyyy";
            dpDokDate.Format = DateTimePickerFormat.Custom;

           

            reLoadConfig();

           // ToDo: kann evtl. wieder rein genommen werden.
           // wenn nicht, muss im Programm der Scanner ausgewählt werden.
           //SetupScanner();
/*
            Image bg = Image.FromFile(@"c:\pc_inst\dark.png");
            tabPage1.BackgroundImage = bg;
            tabPage1.BackgroundImageLayout = ImageLayout.Stretch;
*/
        }

        


        #region Scanner


        private void SetupScanner()
        {

            twainScanner = new TwainScanner();
            twainScanner.LogEvent += LogEvent;
            twainScanner.ImageEvent += ImgEvent;
            twainScanner.SetupTwain();

            // Test Scanner
            List<string> ScannerList = twainScanner.ReloadSourceList();
            cbScannerList.Items.Clear();
            foreach (var s in ScannerList)
            {
                cbScannerList.Items.Add(s);
            }
            if (cbScannerList.Items.Count >0)
            {
                cbScannerList.SelectedIndex = 0;
                twainScanner.selectTwainScanner();
            }


        }

        private void ImgEvent(Image img)
        {
            pbPreview.Image = img;
            
            ImageQueue.Add(new ScannedPicture( img,false) );
            lblPageCount.Text = ImageQueue.Count.ToString() + "/" + ImageQueue.Count.ToString();
            prevImageNumber = ImageQueue.Count- 1;
            btnPrevImage.Enabled = true;
            
        }
        private void btnNextImage_Click(object sender, EventArgs e)
        {
            LogEvent("Images: " + ImageQueue.Count());
                if(ImageQueue.Count > 0 && prevImageNumber < ImageQueue.Count-1)
                {
                    prevImageNumber++;
                lblPageCount.Text = (prevImageNumber+1).ToString() + "/"+ ImageQueue.Count;
                    pbPreview.Image = ImageQueue[prevImageNumber].Img;
                btnPrevImage.Enabled = true;
                    
                }
                if (prevImageNumber == 0)
                    btnPrevImage.Enabled = false;
                if(prevImageNumber == ImageQueue.Count - 1)
                    btnNextImage.Enabled = false;
            
        }
        private void btnPrevImage_Click(object sender, EventArgs e)
        {
            if (prevImageNumber > 0) { 
                prevImageNumber--;
                lblPageCount.Text = (prevImageNumber + 1).ToString() + "/" + ImageQueue.Count;
                btnNextImage.Enabled = true;
                pbPreview.Image = ImageQueue[prevImageNumber].Img;
                if (prevImageNumber == 0)
                btnPrevImage.Enabled = false;
            }

        }

        private void btnDelPage_Click(object sender, EventArgs e)
        {
            //ToDo : prevent index out of range on delete
            ImageQueue.RemoveAt(prevImageNumber);
            if (ImageQueue.Count > 0)
            {
                pbPreview.Image = ImageQueue[prevImageNumber].Img;
            }else
            {
                pbPreview.Image = null;
            }
                

        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            ImageQueue.Clear();
            lblPageCount.Text = "0";
            btnNextImage.Enabled = false;
            btnPrevImage.Enabled = false;
            pbPreview.Image = null;
        }

        #endregion


        private void reLoadConfig()
        {

            ConfigurationManager.RefreshSection("appSettings");
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            DICOMServer = ConfigurationManager.AppSettings.Get("DICOMServer");
            DICOMQueryPort = Int32.Parse(ConfigurationManager.AppSettings.Get("DICOMQueryPort"));
            DICOMStoragePort = Int32.Parse(ConfigurationManager.AppSettings.Get("DICOMStoragePort"));
            DICOMLocalAET = ConfigurationManager.AppSettings.Get("DICOMLocalAET");
            DICOMRemoteQueryAET = ConfigurationManager.AppSettings.Get("DICOMRemoteQueryAET");
            DICOMRemoteStoreAET = ConfigurationManager.AppSettings.Get("DICOMRemoteStoreAET");
            dcmtkPath = ConfigurationManager.AppSettings.Get("dcmtkPath");

            this.dgConfig.Rows.Clear();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                this.dgConfig.Rows.Add(key, ConfigurationManager.AppSettings[key]);
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            foreach (DataGridViewRow r in this.dgConfig.Rows)
            {
                //Console.WriteLine(r.Cells[0].Value + " : " + r.Cells[1].Value);
                config.AppSettings.Settings[r.Cells[0].Value.ToString()].Value = r.Cells[1].Value.ToString();
            }

            config.AppSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            reLoadConfig();
        }

        private async void btnDICOMEcho_Click(object sender, EventArgs e)
        {
            reLoadConfig();

            lblDICOMEchoQueryResult.Text = "Echo Query-Port... ";
            lblDICOMEchoQueryResult.Text += await dicomFunctions.DicomEchoAsync(DICOMServer, DICOMQueryPort, DICOMLocalAET, DICOMRemoteQueryAET);
            lblDICOMEchoStorageResult.Text = "Echo Storage-Port... ";
            lblDICOMEchoStorageResult.Text += await dicomFunctions.DicomEchoAsync(DICOMServer, DICOMStoragePort, DICOMLocalAET, DICOMRemoteStoreAET);
        }



        private void btnTestSearch_Click(object sender, EventArgs e)
        {
            RunSearch();
        }

        private async void RunSearch()
        {
            lvResults.Items.Clear();
            List<DicomDataset> results = await dicomFunctions.CFind(DICOMServer, DICOMQueryPort, DICOMLocalAET, DICOMRemoteQueryAET, "*" + tbSearchPatId.Text + "*", "*" + tbSearchPatName.Text + "*");

            //ToDo : Exception if patient without name
            foreach (DicomDataset d in results)
            {


                Patient p = new Patient();
                try
                {
                    

                    p.PatientID = d.GetValue<String>(Dicom.DicomTag.PatientID, 0);
                    p.PatientName = d.GetValue<String>(Dicom.DicomTag.PatientName, 0);
                    p.PatientSex = d.GetValue<String>(Dicom.DicomTag.PatientSex, 0);
                    p.PatientBirthDate = d.GetValue<String>(Dicom.DicomTag.PatientBirthDate, 0);

                    d.TryGetValue(Dicom.DicomTag.PatientID, 0, out String PatientID);
                    p.PatientID = PatientID;
                    d.TryGetValue(Dicom.DicomTag.PatientName, 0, out String PatientName);
                    p.PatientName = PatientName;
                    d.TryGetValue(Dicom.DicomTag.PatientSex, 0, out String PatientSex);
                    p.PatientSex = PatientSex;

                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.Message);
                }

                ListViewItem item = null;
                try
                {
                    String PatientId = d.Get<String>(Dicom.DicomTag.PatientID);
                    item = new ListViewItem(PatientId);
                }
                catch (DicomException ex)
                {

                }
                try
                {
                    String xPatientName = d.Get<String>(Dicom.DicomTag.PatientName);
                    item.SubItems.Add(xPatientName);
                }
                catch (DicomException ex)
                {

                }
                try
                {
                    String PatientBirthDate = d.Get<String>(Dicom.DicomTag.PatientBirthDate);
                    item.SubItems.Add(PatientBirthDate);
                }
                catch (DicomException ex)
                {

                }
                try
                {
                    String xPatientSex = d.Get<String>(Dicom.DicomTag.PatientSex);
                    item.SubItems.Add(xPatientSex);
                }
                catch (DicomException ex)
                {

                }
                lvResults.Items.Add(item);
            }
        }

        // Eingaben prüfen
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RunSearch();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbPatId.Clear();
            tbPatName.Clear();
            tbPatGebDat.Clear();

            if (lvResults.SelectedItems != null)
            {
                try
                {

                    tbPatId.Text = lvResults.SelectedItems[0].SubItems[0].Text;
                    tbPatName.Text = lvResults.SelectedItems[0].SubItems[1].Text;


                    DateTime patGebDat;
                    bool res = DateTime.TryParseExact(lvResults.SelectedItems[0].SubItems[2].Text, "yyyyMMdd", provider, DateTimeStyles.None, out patGebDat);
                    if (res != false)
                    {
                        tbPatGebDat.Text = patGebDat.ToString();
                    }


                }
                catch (Exception ex)
                {
                    // nothing if date is not present or could not be parsed
                };

            }
        }

        private void btnImportPDF_Click(object sender, EventArgs e)
        {
            // only if something is selected
            if (lvResults.SelectedItems.Count > 0)
            {
                OpenFileDialog FilePicker = new OpenFileDialog();
                //FilePicker.Filter = "PDF Files (*.pdf)|*.pdf";
                //FilePicker.Filter = "ImportFile |*.jpg;*.pdf";
                FilePicker.Filter = "ImportFile |*.jpg";

                if (FilePicker.ShowDialog() != DialogResult.OK)
                {
                    // MessageBox.Show("no file ");
                }
                else
                {
                    if (FilePicker.FileName.ToString().Contains(".jpg"))
                    {
                        //importJPGFile(FilePicker.FileName, dicom_temp_file);
                        importJPGFile(FilePicker.FileName);
                    }
                    else if (FilePicker.FileName.ToString().Contains(".pdf"))
                    {
                        EncapsulatePDF(FilePicker.FileName,dicom_temp_file);
                    }
                }
            }
            else
            {
                LogEvent("Kein Patient ausgewählt");
            }
                

         }
        
    //public void importJPGFile(String FileName, String dicom_temp_file)
        public void importJPGFile(String FileName)
        {
         ImgEvent(Image.FromFile(FileName));
                //ScannedImages.Add(new ScannedPicture(img, false));
        
    }

        private void processJPGFile(ScannedPicture sp, int InstanceNumber, DicomUID uid)
        {
            Image img = sp.Img;

        String PatientID = "";
        String PatientName = "";
        String PatientBirthDate = "";
        String PatientSex = "";

        if (lvResults.SelectedItems.Count > 0)
        {
                try
                {
                    if (lvResults.SelectedItems[0].SubItems[0].Text != null)
                        PatientID = lvResults.SelectedItems[0].SubItems[0].Text;
                }
                catch (Exception ex) { PatientID = ""; }
                try
                {
                    if (lvResults.SelectedItems[0].SubItems[1].Text != null)
                        PatientName = lvResults.SelectedItems[0].SubItems[1].Text;
                }
                catch (Exception ex) { PatientName = ""; }
                try
                {
                    if (lvResults.SelectedItems[0].SubItems[2].Text != null)
                        PatientBirthDate = lvResults.SelectedItems[0].SubItems[2].Text;
                }
                catch (Exception ex) { PatientBirthDate = ""; }
                try
                {

                    if (lvResults.SelectedItems[0].SubItems[3].Text != null)
                        PatientSex = lvResults.SelectedItems[0].SubItems[3].Text;
                }
                catch (Exception ex) { PatientSex = ""; }

   


               
                DicomFile dcm =  dicomFunctions.createJPGDataset(uid, InstanceNumber, img, dicom_temp_file, dcmtkPath, PatientID, PatientName, PatientBirthDate, PatientSex, tbDokCreator.Text, tbDokTitle.Text, dpDokDate.Value.ToString("yyyyMMdd"));
                
                sp.DcmFile= dcm;
                LogEvent("jpg processed");
        }
        }

        /// <summary>
        /// save pdf file in dcm dicom file
        /// </summary>
        /// <param name="Filename"></param>
        public void EncapsulatePDF(String Filename,String dicom_temp_file)
        {
            LogEvent("converting " + Filename + " to " + dicom_temp_file);
            //string dicom_file = @"C:\pc_inst\test.dcm";
            string pdf_file = Filename;

            byte[] pdf = System.IO.File.ReadAllBytes(pdf_file);

            var studyUID = new DicomUID("1.2.826.0.1.3680043.8.498", "Encapsulated PDF Storage", DicomUidType.SOPInstance);
            var generator = new DicomUIDGenerator();

            String PatientID = "";
            String PatientName = "";
            String PatientBirthDate = "";
            String PatientSex = "";

            if (lvResults.SelectedItems.Count > 0) { 
            if (lvResults.SelectedItems[0].SubItems[0].Text != null)
                PatientID = lvResults.SelectedItems[0].SubItems[0].Text;
            if (lvResults.SelectedItems[0].SubItems[1].Text != null)
                PatientName = lvResults.SelectedItems[0].SubItems[1].Text;
            if (lvResults.SelectedItems[0].SubItems[2].Text != null)            
                PatientBirthDate = lvResults.SelectedItems[0].SubItems[2].Text;
            if (lvResults.SelectedItems[0].SubItems[3].Text != null)
                PatientSex = lvResults.SelectedItems[0].SubItems[3].Text;


                var dataset = dicomFunctions.createPDFDataset(PatientID,PatientName,PatientBirthDate,PatientSex,tbDokCreator.Text,tbDokTitle.Text, dpDokDate.Value.ToString("yyyyMMdd"),pdf);              

                try
                {
                    DicomFile file = new DicomFile();
                    file.Dataset.Add(dataset);
                    file.FileMetaInfo.TransferSyntax = DicomTransferSyntax.ImplicitVRLittleEndian; //Specify transfer syntax
                    file.Save(dicom_temp_file);
                    LogEvent("Datei gespeichert: " + dicom_temp_file);
                    // Preview bei PDF geht nicht
                    //PreviewDicomPicture();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    LogEvent("Fehler: " + ex.Message);
                }



            }
            else
            {
                LogEvent("Kein Patient ausgewählt");

            }
            

        }
 

        private void btnSendToPacs_Click(object sender, EventArgs e)
        {
            DicomUID uid = dicomFunctions.genStudyId();

            int InstanceNumber = 0;
            foreach(ScannedPicture sp in ImageQueue)
            {
                InstanceNumber++;
                processJPGFile(sp, InstanceNumber, uid);
            }

            foreach (ScannedPicture sp in ImageQueue)
            {

                // ToDO : geht das auch bei gescannten files? evtl. nur bei imporiterten
                 StoreSCU(sp.DcmFile);
            }

            // erste Tests
            /*
                        // ToDo - testet noch nicht ob es vielleicht eine alte Datei ist....
                        // ToDo - unterscheiden zwischen einzelner import-Datei oder Scan mit evtl. mehreren Seiten

                        // ToDo testjohe - nur temp switch zwischen import und picturebox
                        bool testjohe = true;
                        if (File.Exists(dicom_temp_file) && !testjohe)
                        {
                            StoreSCU(dicom_temp_file);
                        }
                        else
                        {
                            LogEvent("DICOM temp Datei nicht gefunden...");
                        }


                            // test
                            *//*ScannedImages.Add(Image.FromFile(@"C:\pc_inst\testJPG\ground1.png"));
                            ScannedImages.Add(Image.FromFile(@"C:\pc_inst\testJPG\idle.png"));
                            ScannedImages.Add(Image.FromFile(@"C:\pc_inst\testJPG\jump.png"));*//*


                            if (ScannedImages.Count ==0)
                            {
                                LogEvent("Keine Bilder vorhanden. Bitte erst scannen");
                            }
                        */


        }
        //private async void StoreSCU(String _dicomFile)
        private async void StoreSCU(DicomFile _dicomFile)
        {

            try
            {
                //create DICOM store SCU client with handlers
                //var client =  dicomFunctions.CreateDicomStoreClient(_dicomFile);
                var client = dicomFunctions.CreateDicomStoreClient(_dicomFile);
                


                //send the verification request to the remote DICOM server
                 client.Send(DICOMServer, DICOMStoragePort, false, DICOMLocalAET, DICOMRemoteStoreAET);
                LogEvent("Our DICOM CStore operation was successfully completed." );
            }
            catch (Exception e)
            {
                LogEvent("Error occured during DICOM verification request ->" + System.Environment.NewLine + e.Message );
            }


        }

        // delegate for calling from other thread
        delegate void StringParameterDelegate(string value);
        private void LogEvent(string str)
        {
            // Invoke if called from other thread
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(LogEvent), new object[] { str });
                return;
            }
            tbLogging.AppendText(str + System.Environment.NewLine);
        }

        private void btnScanStart_Click(object sender, EventArgs e)
        {
            twainScanner.StartScan(this.Handle);
        }

        private void cbScannerList_DropDown(object sender, EventArgs e)
        {
            SetupScanner();
        }

 
    }
    
}