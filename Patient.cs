using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scan2Dicom
{
    internal class Patient
    {
        private String patientName;        
        private String patientID;
        private String patientSex;
        private String patientBirthDate;

        public string PatientName { get => patientName; set => patientName = value; }        
        public string PatientBirthDate { get => patientBirthDate; set => patientBirthDate = value; }
        public string PatientSex { get => patientSex; set => patientSex = value; }
        public string PatientID { get => patientID; set => patientID = value; }
    }
}

