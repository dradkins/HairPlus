using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HairPlus.Web.Models
{
    public class ExpenseListViewModel
    {
        public int ExpenseId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDateTime { get; set; }
    }
}