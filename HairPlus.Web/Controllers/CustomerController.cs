using HairPlus.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using System.Linq.Dynamic;
using HairPlus.Web.Models;
using Microsoft.AspNet.Identity;

namespace HairPlus.Web.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(IUow uow)
        {
            _Uow = uow;
        }

        public async Task<IHttpActionResult> GetAll(int page = 1, int itemsPerPage = 20, string sortBy = "name", bool reverse = false, string search = null)
        {
            try
            {
                var customersList = new List<CustomersListViewModel>();

                var customers = _Uow._Customer.GetAll(x => x.Active == true && x.CustomerType == null)
                    .Include(x => x.CutomerHairLossSolutions)
                    .Include(x => x.CutomerHairLossSolutions.Select(y => y.HairLossSolution));

                // searching
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    customers = customers.Where(x =>
                        x.Name.ToLower().Contains(search) ||
                        x.Occupation.ToLower().Contains(search) ||
                        x.Address.ToLower().Contains(search) ||
                        x.EmailAddress.ToLower().Contains(search) ||
                        x.PhoneNo.ToLower().Contains(search) ||
                        x.MobileNo.ToLower().Contains(search));
                }

                // sorting (done with the System.Linq.Dynamic library available on NuGet)
                customers = customers.OrderBy(sortBy + (reverse ? " descending" : ""));

                var totalCustomers = await customers.CountAsync();

                // paging
                var customersPaged = await customers.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

                foreach (var item in customersPaged)
                {
                    CustomersListViewModel model = new CustomersListViewModel();

                    model.CustomerId = item.Id;
                    model.CustomerName = item.Name;
                    model.DateVisit = item.VisitDateTime.GetValueOrDefault();
                    model.EstimatedAmount = item.EstimatedAmount.GetValueOrDefault();
                    model.Gender = Enum.GetName(typeof(GenderEnum), item.Gender);
                    model.Occupation = item.Occupation;

                    foreach (var treatment in item.CutomerHairLossSolutions)
                    {
                        model.Treatments.Add(new HairLossSolutionViewModel
                        {
                            Id = treatment.HairLossSolutionId.GetValueOrDefault(),
                            Name = treatment.HairLossSolution.Name
                        });
                    }

                    customersList.Add(model);
                }

                // json result
                var json = new
                {
                    count = totalCustomers,
                    data = customersList,
                };

                return Ok(json);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
    }
}
