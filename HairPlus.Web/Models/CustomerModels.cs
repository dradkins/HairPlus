using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HairPlus.Web.Models
{
    public class CustomersListViewModel
    {
        public CustomersListViewModel()
        {
            Treatments = new List<HairLossSolutionViewModel>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public int EstimatedAmount { get; set; }
        public DateTime DateVisit { get; set; }
        public List<HairLossSolutionViewModel> Treatments { get; set; }
    }

    public class CustomerAddUpdateViewModel
    {
        public CustomerAddUpdateViewModel()
        {
            SolutionsWant = new List<int>();
        }

        public int CustomerId { get; set; }
        public DateTime DateVisited { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string Occupation { get; set; }
        public int YearsExpHairLoss { get; set; }
        public string Address { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string SourceClinicInfo { get; set; }
        public int HairLossStage { get; set; }
        public string Comment { get; set; }
        public int EstimatedPrice { get; set; }
        public int Priority { get; set; }
        public List<int> SolutionsWant { get; set; }
    }

    public class PhotosListViewModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public bool IsDefault { get; set; }
        public int OrderNo { get; set; }
    }

    public class PhotosUploadViewModel
    {
        public int CustomerId { get; set; }
    }
}