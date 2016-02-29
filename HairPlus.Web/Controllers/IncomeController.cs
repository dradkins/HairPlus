using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using HairPlus.Contract;
using HairPlus.Web.Models;
using System.Linq.Dynamic;

namespace HairPlus.Web.Controllers
{
    public class IncomeController : BaseController
    {
        public IncomeController(IUow uow)
        {
            _Uow = uow;
        }

        [HttpGet]
        [ActionName("GetAll")]
        public async Task<IHttpActionResult> GetAll(int page = 1, int itemsPerPage = 20, string sortBy = "amount", bool reverse = false, string search = null)
        {
            try
            {
                var incomeList = new List<IncomeListViewModel>();

                var incomes = _Uow._Income.GetAll();

                // searching
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    incomes = incomes.Where(x =>
                        x.Description.ToLower().Contains(search));
                }

                // sorting (done with the System.Linq.Dynamic library available on NuGet)
                incomes = incomes.OrderBy(sortBy + (reverse ? " descending" : ""));

                var totalIncomes = await incomes.CountAsync();

                // paging
                var incomesPaged = await incomes.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

                foreach (var item in incomesPaged)
                {
                    IncomeListViewModel model = new IncomeListViewModel();

                    model.IncomeDateTime = item.CreatedOn.GetValueOrDefault();
                    model.Amount = item.Amount;
                    model.Description = item.Description;
                    model.IncomeId = item.Id;

                    incomeList.Add(model);
                }

                // json result
                var json = new
                {
                    count = totalIncomes,
                    data = incomeList,
                };

                return Ok(json);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [ActionName("GetIncome")]
        public async Task<IHttpActionResult> GetIncome(int id)
        {
            try
            {
                var income = await _Uow._Income.GetAsync(x => x.Id == id);

                if (income == null)
                {
                    return NotFound();
                }

                var model = new IncomeListViewModel();

                model.Amount = income.Amount;
                model.Description = income.Description;
                model.IncomeId = income.Id;
                model.IncomeDateTime = income.CreatedOn.GetValueOrDefault();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("AddIncome")]
        public async Task<IHttpActionResult> AddIncome(AddUpdateIncomeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var income = new EF.Income
                {
                    Description = model.Description,
                    Amount = model.Amount,
                    CreatedOn = DateTime.Now,
                };

                _Uow._Income.Add(income);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("UpdateIncome")]
        public async Task<IHttpActionResult> UpdateIncome(AddUpdateIncomeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var income = await _Uow._Income.GetAsync(x => x.Id == model.IncomeId);

                if (income == null)
                {
                    return NotFound();
                }
                income.Amount = model.Amount;
                income.Description = model.Description;

                _Uow._Income.Update(income);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [ActionName("DeleteIncome")]
        public async Task<IHttpActionResult> DeleteIncome(int id)
        {
            try
            {
                var income = await _Uow._Income.GetAsync(x => x.Id == id);

                if (income == null)
                {
                    return NotFound();
                }

                _Uow._Income.Delete(income);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
