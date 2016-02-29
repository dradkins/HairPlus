using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using HairPlus.Contract;
using System.Threading.Tasks;
using HairPlus.EF;
using Microsoft.AspNet.Identity;
using HairPlus.Web.Models;


namespace HairPlus.Web.Controllers
{
    public class InvoiceController : BaseController
    {
        public InvoiceController(IUow uow)
        {
            _Uow = uow;
        }

        [HttpGet]
        [ActionName("GetInvoice")]
        public async Task<IHttpActionResult> GetInvoice(int invoiceType, int customerId)
        {
            try
            {
                if (invoiceType == (int)InvoiceTypeEnum.Maintanance)
                {
                    return await PrintMaintainanceInvoice(customerId);
                }
                else if (invoiceType == (int)InvoiceTypeEnum.Surgical)
                {
                    return await PrintSurgicalInvoice(customerId);
                }
                else if (invoiceType == (int)InvoiceTypeEnum.NonSurgical)
                {
                    return await PrintNonSurgicalInvoice(customerId);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> PrintSurgicalInvoice(int id)
        {
            try
            {
                var surgicalPatient = await _Uow._Customer.GetAll(x => x.Id == id)
                    .Include(x => x.Patient)
                    .Include(x => x.Patient.SurgicalPatient)
                    .FirstOrDefaultAsync();

                if (surgicalPatient == null)
                {
                    return NotFound();
                }

                var model = new InvoiceViewModel();
                model.Advance = surgicalPatient.Patient.AdvanceAmount;
                model.CustomerAddress = surgicalPatient.Address;
                model.CustomerId = surgicalPatient.Id;
                model.CustomerMobileNo = surgicalPatient.MobileNo;
                model.CustomerName = surgicalPatient.Name;
                model.CustomerPhoneNo = surgicalPatient.PhoneNo;
                model.TotalAmount = surgicalPatient.Patient.TotalAmount;
                model.Discount = surgicalPatient.Patient.DiscountAmount;
                model.TreatmentDate = surgicalPatient.Patient.TreatmentDateTime;

                var storedInvoice = await _Uow._Invoice.GetAsync(x => x.CustomerId == model.CustomerId && x.InvoiceType == "Surgical");

                if (storedInvoice != null)
                {
                    model.InvoiceId = storedInvoice.Id;
                }
                else
                {
                    var invoice = new Invoice();
                    invoice.Amount = model.TotalAmount;
                    invoice.CreatedBy = User.Identity.GetUserId();
                    invoice.CreatedOn = DateTime.Now;
                    invoice.CustomerId = model.CustomerId;
                    invoice.GenerationTime = DateTime.Now;
                    invoice.InvoiceType = "Surgical";
                    invoice.Flag = 1;
                    invoice.RefId = 1;
                    invoice.RepairCharges = 0;
                    _Uow._Invoice.Add(invoice);
                    await _Uow.CommitAsync();

                    model.InvoiceId = invoice.Id;
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> PrintNonSurgicalInvoice(int id)
        {
            try
            {
                var nonSurgicalPatient = await _Uow._Customer.GetAll(x => x.Id == id)
                    .Include(x => x.Patient)
                    .Include(x => x.Patient.NonSurgicalPatient)
                    .FirstOrDefaultAsync();

                if (nonSurgicalPatient == null)
                {
                    return NotFound();
                }

                var model = new InvoiceViewModel();
                model.Advance = nonSurgicalPatient.Patient.AdvanceAmount;
                model.CustomerAddress = nonSurgicalPatient.Address;
                model.CustomerId = nonSurgicalPatient.Id;
                model.CustomerMobileNo = nonSurgicalPatient.MobileNo;
                model.CustomerName = nonSurgicalPatient.Name;
                model.CustomerPhoneNo = nonSurgicalPatient.PhoneNo;
                model.TotalAmount = nonSurgicalPatient.Patient.TotalAmount;
                model.Discount = nonSurgicalPatient.Patient.DiscountAmount;
                model.TreatmentDate = nonSurgicalPatient.Patient.TreatmentDateTime;

                var storedInvoice = await _Uow._Invoice.GetAsync(x => x.CustomerId == model.CustomerId && x.InvoiceType == "Non-Surgical");

                if (storedInvoice != null)
                {
                    model.InvoiceId = storedInvoice.Id;
                }
                else
                {
                    var invoice = new Invoice();
                    invoice.Amount = model.TotalAmount;
                    invoice.CreatedBy = User.Identity.GetUserId();
                    invoice.CreatedOn = DateTime.Now;
                    invoice.CustomerId = model.CustomerId;
                    invoice.GenerationTime = DateTime.Now;
                    invoice.InvoiceType = "Non-Surgical";
                    invoice.Flag = 2;
                    invoice.RefId = 2;
                    invoice.RepairCharges = 0;
                    _Uow._Invoice.Add(invoice);
                    await _Uow.CommitAsync();

                    model.InvoiceId = invoice.Id;
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> PrintMaintainanceInvoice(int id)
        {
            try
            {
                var nonSurgicalPatient = await _Uow._Customer.GetAll(x => x.Id == id)
                    .Include(x => x.Patient)
                    .Include(x => x.Patient.NonSurgicalPatient)
                    .FirstOrDefaultAsync();

                if (nonSurgicalPatient == null)
                {
                    return NotFound();
                }

                var model = new InvoiceViewModel();
                model.Advance = 0;
                model.CustomerAddress = nonSurgicalPatient.Address;
                model.CustomerId = nonSurgicalPatient.Id;
                model.CustomerMobileNo = nonSurgicalPatient.MobileNo;
                model.CustomerName = nonSurgicalPatient.Name;
                model.CustomerPhoneNo = nonSurgicalPatient.PhoneNo;
                model.TotalAmount = nonSurgicalPatient.Patient.NonSurgicalPatient.MaintananceCharges;
                model.Discount = 0;
                model.TreatmentDate = nonSurgicalPatient.Patient.TreatmentDateTime;

                var storedInvoice = await _Uow._Invoice.GetAsync(x => x.CustomerId == model.CustomerId && x.InvoiceType == "Maintanance-Surgical");

                if (storedInvoice != null)
                {
                    model.InvoiceId = storedInvoice.Id;
                }
                else
                {
                    var invoice = new Invoice();
                    invoice.Amount = model.TotalAmount;
                    invoice.CreatedBy = User.Identity.GetUserId();
                    invoice.CreatedOn = DateTime.Now;
                    invoice.CustomerId = model.CustomerId;
                    invoice.GenerationTime = DateTime.Now;
                    invoice.InvoiceType = "Maintanance-Surgical";
                    invoice.Flag = 1;
                    invoice.RefId = 1;
                    invoice.RepairCharges = nonSurgicalPatient.Patient.NonSurgicalPatient.MaintananceCharges;
                    _Uow._Invoice.Add(invoice);
                    await _Uow.CommitAsync();

                    model.InvoiceId = invoice.Id;
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string CustomerMobileNo { get; set; }
        public int TotalAmount { get; set; }
        public int Advance { get; set; }
        public int Discount { get; set; }
        public DateTime TreatmentDate { get; set; }
    }

}
