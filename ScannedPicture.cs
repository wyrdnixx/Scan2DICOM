using Dicom;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scan2Dicom
{
    internal class ScannedPicture
    {

        private String id;
        private Image img;
        private bool successfullSend;
        private DicomFile dcmFile;

        public ScannedPicture( Image img, bool successfullSend)
        {
            Id = Guid.NewGuid().ToString();
            Img = img;
            SuccessfullSend = successfullSend;
            Id = id;
            Img = img;
            SuccessfullSend = successfullSend;
        }

        public String Id { get => id; set => id = value; }
        public Image Img { get => img; set => img = value; }
        public bool SuccessfullSend { get => successfullSend; set => successfullSend = value; }
        public DicomFile DcmFile { get => dcmFile; set => dcmFile = value; }
    }
}
