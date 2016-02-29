using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HairPlus.Web.Models
{
    public class IncomeListViewModel
    {
        public int IncomeId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncomeDateTime { get; set; }
    }

    public class AddUpdateIncomeViewModel
    {
        public int IncomeId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime IncomeDateTime { get; set; }
    }
}