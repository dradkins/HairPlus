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
    public class ExpenseController : BaseController
    {
        public ExpenseController(IUow uow)
        {
            _Uow = uow;
        }

        [HttpGet]
        [ActionName("GetAll")]
        public async Task<IHttpActionResult> GetAll(int page = 1, int itemsPerPage = 20, string sortBy = "amount", bool reverse = false, string search = null)
        {
            try
            {
                var expenseList = new List<ExpenseListViewModel>();

                var expenses = _Uow._Expense.GetAll(x => x.Active == true);

                // searching
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    expenses = expenses.Where(x =>
                        x.Description.ToLower().Contains(search));
                }

                // sorting (done with the System.Linq.Dynamic library available on NuGet)
                expenses = expenses.OrderBy(sortBy + (reverse ? " descending" : ""));

                var totalExpenses = await expenses.CountAsync();

                // paging
                var expensesPaged = await expenses.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

                foreach (var item in expensesPaged)
                {
                    ExpenseListViewModel model = new ExpenseListViewModel();

                    model.ExpenseDateTime = item.CreatedOn;
                    model.Amount = item.Amount;
                    model.Description = item.Description;
                    model.ExpenseId = item.Id;

                    expenseList.Add(model);
                }

                // json result
                var json = new
                {
                    count = expensesPaged,
                    data = expenseList,
                };

                return Ok(json);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [ActionName("GetExpense")]
        public async Task<IHttpActionResult> GetExpense(int id)
        {
            try
            {
                var expense = await _Uow._Expense.GetAsync(x => x.Id == id && x.Active == true);

                if (expense == null)
                {
                    return NotFound();
                }

                var model = new ExpenseListViewModel();

                model.Amount = expense.Amount;
                model.Description = expense.Description;
                model.ExpenseId = expense.Id;
                model.ExpenseDateTime = expense.CreatedOn;

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("AddExpense")]
        public async Task<IHttpActionResult> AddExpense(ExpenseListViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var expense = new EF.Expense
                {
                    Active = true,
                    Description = model.Description,
                    Amount = model.Amount,
                    CreatedBy = User.Identity.GetUserId(),
                    CreatedOn = model.ExpenseDateTime,
                };

                _Uow._Expense.Add(expense);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("UpdateExpense")]
        public async Task<IHttpActionResult> UpdateExpense(ExpenseListViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var expense = await _Uow._Expense.GetAsync(x => x.Id == model.ExpenseId && x.Active == true);

                if (expense == null)
                {
                    return NotFound();
                }
                expense.Amount = model.Amount;
                expense.Description = model.Description;
                expense.UpdatedBy = User.Identity.GetUserId();
                expense.UpdatedOn = DateTime.Now;

                _Uow._Expense.Update(expense);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [ActionName("DeleteExpense")]
        public async Task<IHttpActionResult> DeleteExpense(int id)
        {
            try
            {
                var expense = await _Uow._Expense.GetAsync(x => x.Id == id && x.Active == true);

                if (expense == null)
                {
                    return NotFound();
                }

                expense.Active = false;
                _Uow._Expense.Update(expense);
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
