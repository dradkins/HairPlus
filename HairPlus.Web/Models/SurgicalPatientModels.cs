using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HairPlus.Web.Models
{
    public class SurgicalPatientAddUpdateViewModel
    {
        public int PatientId { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int Advance { get; set; }
        public DateTime TreatmentDateTime { get; set; }
    }

    public class SurgicalPatientsListViewModel
    {
        public int PatientId { get; set; }
        public string DefaultPicture { get; set; }
        public string PatientName { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int Advance { get; set; }
        public DateTime TreatmentDateTime { get; set; }
    }

    public class SurgicalPatientStatusUpdateViewModel
    {
        public int PatientId { get; set; }
        public int Status { get; set; }
    }
}