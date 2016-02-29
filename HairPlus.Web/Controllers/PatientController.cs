using HairPlus.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using HairPlus.Web.Models;
using System.Data.Entity;
using System.Linq.Dynamic;

namespace HairPlus.Web.Controllers
{
    [Authorize]
    public class PatientController : BaseController
    {
        public PatientController(IUow uow)
        {
            _Uow = uow;
        }

        [HttpGet]
        [ActionName("GetPatientPhotos")]
        public async Task<IHttpActionResult> GetPatientPhotos(int id)
        {
            try
            {
                var customer = await _Uow._Customer.GetAll(x => x.Active == true && x.Id == id)
                    .Include(x => x.CustomerPhotoes)
                    .FirstOrDefaultAsync();

                if (customer == null)
                {
                    return NotFound();
                }

                List<PhotosListViewModel> model = new List<PhotosListViewModel>();

                foreach (var item in customer.CustomerPhotoes)
                {
                    model.Add(new PhotosListViewModel
                    {
                        Id = item.Id,
                        IsDefault = item.IsDefault,
                        OrderNo = item.OrderNo,
                        Path = item.Path,
                    });
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [ActionName("GetPatient")]
        public async Task<IHttpActionResult> GetPatient(int id)
        {
            try
            {
                var patient = await _Uow._Customer.GetAll(x => x.Id == id && x.Active == true)
                    .Include(x => x.CutomerHairLossSolutions)
                    .FirstOrDefaultAsync();

                if (patient == null)
                {
                    return NotFound();
                }

                CustomerAddUpdateViewModel model = new CustomerAddUpdateViewModel();

                model.Address = patient.Address;
                model.Age = patient.Age;
                model.Comment = patient.Comment;
                model.EmailAddress = patient.EmailAddress;
                model.EstimatedPrice = patient.EstimatedAmount.GetValueOrDefault();
                model.Gender = patient.Gender;
                model.SourceClinicInfo = patient.HowYouKnowUs;
                model.MobileNo = patient.MobileNo;
                model.Name = patient.Name;
                model.Occupation = patient.Occupation;
                model.TelephoneNo = patient.PhoneNo;
                model.Priority = patient.Priority;
                model.HairLossStage = patient.Stage;
                model.YearsExpHairLoss = patient.TotalYearsOfHairLoss.GetValueOrDefault();
                model.DateVisited = patient.VisitDateTime.GetValueOrDefault();

                foreach (var item in patient.CutomerHairLossSolutions)
                {
                    model.SolutionsWant.Add(item.HairLossSolutionId.GetValueOrDefault());
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ActionName("AddPatient")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPatient(CustomerAddUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var customer = new EF.Customer
                {
                    Active = true,
                    Address = model.Address,
                    Age = model.Age,
                    Comment = model.Comment,
                    CreatedBy = User.Identity.GetUserId(),
                    CreatedOn = DateTime.Now,
                    EmailAddress = model.EmailAddress,
                    EstimatedAmount = model.EstimatedPrice,
                    Gender = model.Gender,
                    HowYouKnowUs = model.SourceClinicInfo,
                    MobileNo = model.MobileNo,
                    Name = model.Name,
                    Occupation = model.Occupation,
                    PhoneNo = model.TelephoneNo,
                    Priority = model.Priority,
                    Stage = model.HairLossStage,
                    TotalYearsOfHairLoss = model.YearsExpHairLoss,
                    VisitDateTime = model.DateVisited
                };

                foreach (var item in model.SolutionsWant)
                {
                    customer.CutomerHairLossSolutions.Add(new EF.CutomerHairLossSolution
                    {
                        HairLossSolutionId = item
                    });
                }

                _Uow._Customer.Add(customer);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [ActionName("UpdatePatient")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdatePatient(CustomerAddUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var customer = await _Uow._Customer.GetAll(x => x.Id == model.CustomerId && x.Active == true).Include(x => x.CutomerHairLossSolutions).FirstOrDefaultAsync();

                if (customer == null)
                {
                    return NotFound();
                }
                customer.Address = model.Address;
                customer.Age = model.Age;
                customer.Comment = model.Comment;
                customer.EmailAddress = model.EmailAddress;
                customer.EstimatedAmount = model.EstimatedPrice;
                customer.Gender = model.Gender;
                customer.HowYouKnowUs = model.SourceClinicInfo;
                customer.MobileNo = model.MobileNo;
                customer.Name = model.Name;
                customer.Occupation = model.Occupation;
                customer.PhoneNo = model.TelephoneNo;
                customer.Priority = model.Priority;
                customer.Stage = model.HairLossStage;
                customer.TotalYearsOfHairLoss = model.YearsExpHairLoss;
                customer.VisitDateTime = model.DateVisited;
                customer.UpdatedBy = User.Identity.GetUserId();
                customer.UpdatedOn = DateTime.Now;

                foreach (var item in customer.CutomerHairLossSolutions.ToList())
                {
                    _Uow._CutomerHairLossSolution.Delete(item);
                }

                await _Uow.CommitAsync();

                foreach (var item in model.SolutionsWant)
                {
                    customer.CutomerHairLossSolutions.Add(new EF.CutomerHairLossSolution
                    {
                        HairLossSolutionId = item
                    });
                }

                _Uow._Customer.Update(customer);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [ActionName("DeletePatient")]
        public async Task<IHttpActionResult> DeletePatient(int id)
        {
            try
            {
                var patient = await _Uow._Customer.GetAll(x => x.Active == true)
                    .Include(x => x.Patient)
                    .Include(x => x.Patient.SurgicalPatient)
                    .Include(x => x.Patient.NonSurgicalPatient)
                    .FirstOrDefaultAsync();

                if (patient == null)
                {
                    return NotFound();
                }

                var userId = User.Identity.GetUserId();

                if (patient.Patient.NonSurgicalPatient != null)
                {
                    patient.Patient.NonSurgicalPatient.Active = false;
                    patient.Patient.NonSurgicalPatient.UpdatedBy = userId;
                    patient.Patient.NonSurgicalPatient.UpdatedOn = DateTime.Now;
                    _Uow._NonSurgicalPatient.Update(patient.Patient.NonSurgicalPatient);
                }

                if (patient.Patient.SurgicalPatient != null)
                {
                    patient.Patient.SurgicalPatient.Active = false;
                    patient.Patient.SurgicalPatient.UpdatedBy = userId;
                    patient.Patient.SurgicalPatient.UpdatedOn = DateTime.Now;
                    _Uow._SurgicalPatient.Update(patient.Patient.SurgicalPatient);
                }

                if (patient.Patient != null)
                {
                    patient.Patient.Active = false;
                    patient.Patient.UpdatedBy = userId;
                    patient.Patient.UpdatedOn = DateTime.Now;
                    _Uow._Patient.Update(patient.Patient);
                }

                patient.Active = false;
                patient.UpdatedBy = userId;
                patient.UpdatedOn = DateTime.Now;
                _Uow._Customer.Update(patient);

                await _Uow.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /************ Surgical Patient Section *****************/

        [HttpGet]
        [ActionName("GetSurgicalPatient")]
        public async Task<IHttpActionResult> GetSurgicalPatient(int id)
        {
            try
            {
                var patient = await _Uow._Patient.GetAll(x => x.Id == id && x.Active == true)
                    .Include(x => x.SurgicalPatient)
                    .FirstOrDefaultAsync();

                if (patient == null)
                {
                    return NotFound();
                }

                SurgicalPatientAddUpdateViewModel model = new SurgicalPatientAddUpdateViewModel();

                model.Advance = patient.AdvanceAmount;
                model.TreatmentDateTime = patient.TreatmentDateTime;
                model.TreatmentDateTime = patient.TreatmentDateTime;
                model.Discount = patient.DiscountAmount;
                model.PatientId = patient.Id;
                model.TotalAmount = patient.TotalAmount;

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("AddSurgicalPatient")]
        public async Task<IHttpActionResult> AddSurgicalPatient(SurgicalPatientAddUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var customer = await _Uow._Customer.GetByIdAsync(model.PatientId);

                if (customer == null)
                {
                    return NotFound();
                }

                customer.CustomerType = (int)CustomerTypeEnum.Surgical;

                var patient = new EF.Patient
                {
                    Active = true,
                    BookingDateTime = DateTime.Now,
                    AdvanceAmount = model.Advance,
                    CreatedBy = User.Identity.GetUserId(),
                    CreatedOn = DateTime.Now,
                    DiscountAmount = model.Discount,
                    Id = customer.Id,
                    TotalAmount = model.TotalAmount,
                    TreatmentDateTime = model.TreatmentDateTime,
                };

                var surgicaPatient = new EF.SurgicalPatient
                {
                    Id = customer.Id,
                    Active = true,
                    CreatedBy = User.Identity.GetUserId(),
                    Status = Enum.GetName(typeof(StatusEnum), StatusEnum.In_Process),
                    CreatedOn = DateTime.Now
                };

                patient.SurgicalPatient = surgicaPatient;

                _Uow._Patient.Add(patient);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("UpdateSurgicalPatient")]
        public async Task<IHttpActionResult> UpdateSurgicalPatient(SurgicalPatientAddUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var sPatient = await _Uow._Patient.GetAll(x => x.Id == model.PatientId && x.Active == true).Include(x => x.SurgicalPatient).FirstOrDefaultAsync();

                if (sPatient == null)
                {
                    return NotFound();
                }

                sPatient.AdvanceAmount = model.Advance;
                sPatient.UpdatedBy = User.Identity.GetUserId();
                sPatient.UpdatedOn = DateTime.Now;
                sPatient.DiscountAmount = model.Discount;
                sPatient.Id = model.PatientId;
                sPatient.TotalAmount = model.TotalAmount;
                sPatient.TreatmentDateTime = model.TreatmentDateTime;

                _Uow._Patient.Update(sPatient);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [ActionName("GetAllSurgicalPatients")]
        public async Task<IHttpActionResult> GetAllSurgicalPatients(int page = 1, int itemsPerPage = 20, string sortBy = "createdOn", bool reverse = false, string search = null, int patientStatus = 2)
        {
            try
            {
                var surgicalPatientsList = new List<SurgicalPatientsListViewModel>();

                var patientStatusName = Enum.GetName(typeof(StatusEnum), patientStatus);

                var surgicalPatients = _Uow._SurgicalPatient.GetAll(x => x.Active == true && x.Patient.SurgicalPatient.Status == patientStatusName)
                    .Include(x => x.Patient)
                    .Include(x => x.Patient.Customer)
                    .Include(x => x.Patient.Customer.CustomerPhotoes);

                //var surgicalPatients = _Uow._Customer.GetAll(x => x.Active == true)
                //    .Include(x => x.Patient)
                //    .Include(x => x.Patient.SurgicalPatient)
                //    .Include(x => x.CustomerPhotoes)
                //    .Where(x => x.Patient.SurgicalPatient.Status == patientStatusName);

                // searching
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    surgicalPatients = surgicalPatients.Where(x =>
                        x.Patient.Customer.Name.ToLower().Contains(search) ||
                        x.Patient.Customer.Occupation.ToLower().Contains(search) ||
                        x.Patient.Customer.Address.ToLower().Contains(search) ||
                        x.Patient.Customer.EmailAddress.ToLower().Contains(search) ||
                        x.Patient.Customer.PhoneNo.ToLower().Contains(search) ||
                        x.Patient.Customer.MobileNo.ToLower().Contains(search));
                }

                // sorting (done with the System.Linq.Dynamic library available on NuGet)
                surgicalPatients = surgicalPatients.OrderBy(sortBy + (reverse ? " descending" : ""));

                var totalPatients = await surgicalPatients.CountAsync();

                // paging
                var patientsPaged = await surgicalPatients.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

                foreach (var item in patientsPaged)
                {
                    SurgicalPatientsListViewModel model = new SurgicalPatientsListViewModel();
                    model.PatientId = item.Id;
                    var picture = item.Patient.Customer.CustomerPhotoes.FirstOrDefault(x => x.IsDefault == true);
                    //var picture=await _Uow._CustomerPhoto.GetAsync(x=>x.CustomerId==item.Id);
                    if (picture != null)
                    {
                        model.DefaultPicture = picture.Path;
                    }
                    model.PatientName = item.Patient.Customer.Name;
                    model.TotalAmount = item.Patient.TotalAmount;
                    model.Discount = item.Patient.DiscountAmount;
                    model.Advance = item.Patient.AdvanceAmount;
                    model.TreatmentDateTime = item.Patient.TreatmentDateTime;

                    surgicalPatientsList.Add(model);
                }

                // json result
                var json = new
                {
                    count = totalPatients,
                    data = surgicalPatientsList,
                };

                return Ok(json);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("UpdateSurgicalPatientStatus")]
        public async Task<IHttpActionResult> UpdateSurgicalPatientStatus(SurgicalPatientStatusUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var sPatient = await _Uow._SurgicalPatient.GetAll(x => x.Id == model.PatientId && x.Active == true).FirstOrDefaultAsync();

                if (sPatient == null)
                {
                    return NotFound();
                }

                sPatient.Status = Enum.GetName(typeof(StatusEnum), model.Status);
                _Uow._SurgicalPatient.Update(sPatient);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        /**************** Non Surgical Patient Section **********************/

        [HttpGet]
        [ActionName("GetNonSurgicalPatient")]
        public async Task<IHttpActionResult> GetNonSurgicalPatient(int id)
        {
            try
            {
                var patient = await _Uow._Patient.GetAll(x => x.Id == id && x.Active == true)
                    .Include(x => x.SurgicalPatient)
                    .FirstOrDefaultAsync();

                if (patient == null)
                {
                    return NotFound();
                }

                SurgicalPatientAddUpdateViewModel model = new SurgicalPatientAddUpdateViewModel();

                model.Advance = patient.AdvanceAmount;
                model.TreatmentDateTime = patient.TreatmentDateTime;
                model.TreatmentDateTime = patient.TreatmentDateTime;
                model.Discount = patient.DiscountAmount;
                model.PatientId = patient.Id;
                model.TotalAmount = patient.TotalAmount;

                return Ok(model);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("AddNonSurgicalPatient")]
        public async Task<IHttpActionResult> AddNonSurgicalPatient(NonSurgicalPatientAddUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var customer = await _Uow._Customer.GetByIdAsync(model.PatientId);

                if (customer == null)
                {
                    return NotFound();
                }

                customer.CustomerType = (int)CustomerTypeEnum.Surgical;

                var patient = new EF.Patient
                {
                    Active = true,
                    BookingDateTime = DateTime.Now,
                    AdvanceAmount = model.Advance,
                    CreatedBy = User.Identity.GetUserId(),
                    CreatedOn = DateTime.Now,
                    DiscountAmount = model.Discount,
                    Id = customer.Id,
                    TotalAmount = model.TotalAmount,
                    TreatmentDateTime = model.TreatmentDateTime,
                };

                var nonSurgicalPatient = new EF.NonSurgicalPatient
                {
                    Id = customer.Id,
                    Active = true,
                    CreatedBy = User.Identity.GetUserId(),
                    Status = Enum.GetName(typeof(StatusEnum), StatusEnum.In_Process),
                    CreatedOn = DateTime.Now,
                    BaseInstructions = model.BaseInstructions,
                    ColorInstructions = model.ColorInstructions,
                    Curly = model.Curly,
                    Density = model.Density,
                    FiberColor = model.Color,
                    FiberGrayHairPerc = model.GrayFiber,
                    FiberHumanPerc = model.HumanFiber,
                    FiberSyntheticPerc = model.syntheticFiber,
                    Length = model.Length,
                    Remarks = "",
                    Size = model.Size,
                    Style = model.Style,
                    MaintananceCharges=model.MaintainanceCharges
                    
                };

                patient.NonSurgicalPatient = nonSurgicalPatient;

                _Uow._Patient.Add(patient);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("UpdateNonSurgicalPatient")]
        public async Task<IHttpActionResult> UpdateNonSurgicalPatient(NonSurgicalPatientAddUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var nSPatient = await _Uow._Patient.GetAll(x => x.Id == model.PatientId && x.Active == true).Include(x => x.NonSurgicalPatient).FirstOrDefaultAsync();

                if (nSPatient == null)
                {
                    return NotFound();
                }

                nSPatient.AdvanceAmount = model.Advance;
                nSPatient.UpdatedBy = User.Identity.GetUserId();
                nSPatient.UpdatedOn = DateTime.Now;
                nSPatient.DiscountAmount = model.Discount;
                nSPatient.Id = model.PatientId;
                nSPatient.TotalAmount = model.TotalAmount;
                nSPatient.TreatmentDateTime = model.TreatmentDateTime;

                nSPatient.NonSurgicalPatient.BaseInstructions = model.BaseInstructions;
                nSPatient.NonSurgicalPatient.ColorInstructions = model.ColorInstructions;
                nSPatient.NonSurgicalPatient.Curly = model.Curly;
                nSPatient.NonSurgicalPatient.Density = model.Density;
                nSPatient.NonSurgicalPatient.FiberColor = model.Color;
                nSPatient.NonSurgicalPatient.FiberGrayHairPerc = model.GrayFiber;
                nSPatient.NonSurgicalPatient.FiberHumanPerc = model.HumanFiber;
                nSPatient.NonSurgicalPatient.FiberSyntheticPerc = model.syntheticFiber;
                nSPatient.NonSurgicalPatient.Length = model.Length;
                nSPatient.NonSurgicalPatient.Remarks = "";
                nSPatient.NonSurgicalPatient.Size = model.Size;
                nSPatient.NonSurgicalPatient.Style = model.Style;
                nSPatient.NonSurgicalPatient.MaintananceCharges = model.MaintainanceCharges;

                _Uow._Patient.Update(nSPatient);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //Done
        [HttpGet]
        [ActionName("GetAllNonSurgicalPatients")]
        public async Task<IHttpActionResult> GetAllNonSurgicalPatients(int page = 1, int itemsPerPage = 20, string sortBy = "createdOn", bool reverse = false, string search = null, int patientStatus = 2)
        {
            try
            {
                var nonSurgicalPatientsList = new List<NonSurgicalPatientsListViewModel>();

                var patientStatusName = Enum.GetName(typeof(StatusEnum), patientStatus);

                var nonSurgicalPatients = _Uow._NonSurgicalPatient.GetAll(x => x.Active == true && x.Patient.NonSurgicalPatient.Status == patientStatusName)
                    .Include(x => x.Patient)
                    .Include(x => x.Patient.Customer)
                    .Include(x => x.Patient.Customer.CustomerPhotoes);

                //var nonSurgicalPatients = _Uow._Customer.GetAll(x => x.Active == true)
                //    .Include(x => x.Patient)
                //    .Include(x => x.Patient.NonSurgicalPatient)
                //    .Include(x => x.CustomerPhotoes)
                //    .Where(x => x.Patient.NonSurgicalPatient.Status == patientStatusName);

                // searching
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.ToLower();
                    nonSurgicalPatients = nonSurgicalPatients.Where(x =>
                        x.Patient.Customer.Name.ToLower().Contains(search) ||
                        x.Patient.Customer.Occupation.ToLower().Contains(search) ||
                        x.Patient.Customer.Address.ToLower().Contains(search) ||
                        x.Patient.Customer.EmailAddress.ToLower().Contains(search) ||
                        x.Patient.Customer.PhoneNo.ToLower().Contains(search) ||
                        x.Patient.Customer.MobileNo.ToLower().Contains(search));
                }

                // sorting (done with the System.Linq.Dynamic library available on NuGet)
                nonSurgicalPatients = nonSurgicalPatients.OrderBy(sortBy + (reverse ? " descending" : ""));

                var totalPatients = await nonSurgicalPatients.CountAsync();

                // paging
                var patientsPaged = await nonSurgicalPatients.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();

                foreach (var item in patientsPaged)
                {
                    NonSurgicalPatientsListViewModel model = new NonSurgicalPatientsListViewModel();
                    model.PatientId = item.Id;
                    var picture = item.Patient.Customer.CustomerPhotoes.Where(x => x.IsDefault).FirstOrDefault();
                    if (picture != null)
                    {
                        model.DefaultPicture = picture.Path;
                    }
                    model.PatientName = item.Patient.Customer.Name;
                    model.TotalAmount = item.Patient.TotalAmount;
                    model.Discount = item.Patient.DiscountAmount;
                    model.Advance = item.Patient.AdvanceAmount;
                    model.TreatmentDateTime = item.Patient.TreatmentDateTime;

                    nonSurgicalPatientsList.Add(model);
                }

                // json result
                var json = new
                {
                    count = totalPatients,
                    data = nonSurgicalPatientsList,
                };

                return Ok(json);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        //Done
        [HttpPost]
        [ActionName("UpdateNonSurgicalPatientStatus")]
        public async Task<IHttpActionResult> UpdateNonSurgicalPatientStatus(NonSurgicalPatientStatusUpdateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Data not valid. Please enter valid data and try again");
                }

                var nSPatient = await _Uow._NonSurgicalPatient.GetAll(x => x.Id == model.PatientId && x.Active == true).FirstOrDefaultAsync();

                if (nSPatient == null)
                {
                    return NotFound();
                }

                nSPatient.Status = Enum.GetName(typeof(StatusEnum), model.Status);
                _Uow._NonSurgicalPatient.Update(nSPatient);
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
