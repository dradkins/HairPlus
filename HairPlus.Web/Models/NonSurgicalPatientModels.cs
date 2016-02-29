using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HairPlus.Web.Models
{
    public class NonSurgicalPatientAddUpdateViewModel
    {
        public int PatientId { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int Advance { get; set; }
        public int MaintainanceCharges { get; set; }
        public string Style { get; set; }
        public string HumanFiber { get; set; }
        public string syntheticFiber { get; set; }
        public string GrayFiber { get; set; }
        public string Color { get; set; }
        public int Size { get; set; }
        public int Length { get; set; }
        public int Curly { get; set; }
        public int Density { get; set; }
        public string ColorInstructions { get; set; }
        public string BaseInstructions { get; set; }
        public DateTime TreatmentDateTime { get; set; }
    }

    public class NonSurgicalPatientsListViewModel
    {
        public int PatientId { get; set; }
        public string DefaultPicture { get; set; }
        public string PatientName { get; set; }
        public int TotalAmount { get; set; }
        public int Discount { get; set; }
        public int Advance { get; set; }
        public DateTime TreatmentDateTime { get; set; }
    }

    public class NonSurgicalPatientStatusUpdateViewModel
    {
        public int PatientId { get; set; }
        public int Status { get; set; }
    }
}