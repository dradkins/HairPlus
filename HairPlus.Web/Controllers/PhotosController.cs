using HairPlus.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Threading.Tasks;
using HairPlus.Web.Models;
using System.IO;
using Newtonsoft.Json;

namespace HairPlus.Web.Controllers
{
    public class PhotosController : BaseController
    {
        public PhotosController(IUow uow)
        {
            _Uow = uow;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostStuff()
        {
            List<string> files = new List<string>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var tempDir = System.Web.HttpContext.Current.Server.MapPath("~/Images/Temp");
            Directory.CreateDirectory(tempDir);
            var provider = new CustomMultipartFormDataStreamProvider(tempDir);
            var result = await Request.Content.ReadAsMultipartAsync(provider);



            if (result.FormData["model"] == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var data = GetFormData<PhotosUploadViewModel>(result);
            if (data != null)
            {
                var photosUploadViewModel = (PhotosUploadViewModel)data;
                var customer = await _Uow._Customer.GetByIdAsync(photosUploadViewModel.CustomerId);
                if (customer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                var cd = "~/Images/" + customer.Name + "_" + customer.Id;
                var customerDirectory = System.Web.HttpContext.Current.Server.MapPath(cd);
                Directory.CreateDirectory(customerDirectory);
                foreach (var item in result.FileData)
                {
                    var tempFile = Path.Combine(tempDir, item.LocalFileName);
                    var cFile = DateTime.Now.Ticks + customer.Name + Path.GetExtension(item.LocalFileName);
                    var customerFile = Path.Combine(customerDirectory, cFile);
                    if (File.Exists(tempFile))
                    {
                        File.Move(tempFile, customerFile);
                        File.Delete(tempFile);
                    }
                    var cPhoto = new EF.CustomerPhoto
                    {
                        CreatedBy = User.Identity.GetUserId(),
                        CreatedOn = DateTime.Now,
                        CustomerId = customer.Id,
                        Path = "../../" + cd.Substring(2) + "/" + cFile,
                        IsDefault = false
                    };
                    _Uow._CustomerPhoto.Add(cPhoto);
                    files.Add(cPhoto.Path);
                }
                await _Uow.CommitAsync();
                if (!_Uow._CustomerPhoto.GetAll(x => x.CustomerId == customer.Id).Any(x => x.IsDefault))
                {
                    var firstPhoto = await _Uow._CustomerPhoto.GetAll().FirstOrDefaultAsync();
                    if (firstPhoto != null)
                    {
                        firstPhoto.IsDefault = true;
                        _Uow._CustomerPhoto.Update(firstPhoto);
                        await _Uow.CommitAsync();
                    }
                }

            }

            return Request.CreateResponse(HttpStatusCode.OK, files);
        }

        [HttpDelete]
        [ActionName("DeletePicture")]
        public async Task<IHttpActionResult> DeletePicture(int id)
        {
            try
            {
                var pic = await _Uow._CustomerPhoto.GetAll(x => x.Id == id).Include(x => x.Customer).FirstOrDefaultAsync();
                if (pic == null)
                {
                    return NotFound();
                }

                var serverPath = "~" + pic.Path.Substring(5);
                if (File.Exists(serverPath))
                {
                    File.Delete(serverPath);
                }
                if (pic.IsDefault)
                {
                    var nextPic = await _Uow._CustomerPhoto.GetAll(x => x.Id != id).FirstOrDefaultAsync();
                    if (nextPic != null)
                    {
                        nextPic.IsDefault = true;
                        _Uow._CustomerPhoto.Update(nextPic);
                    }
                }
                _Uow._CustomerPhoto.Delete(pic);
                await _Uow.CommitAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [ActionName("SetAsDefault")]
        public async Task<IHttpActionResult> SetAsDefault(SetDefaultPictureViewModel model)
        {
            try
            {
                var pics = await _Uow._CustomerPhoto.GetAll(x => x.CustomerId == model.CustomerId).ToListAsync();
                if (pics == null)
                {
                    return NotFound();
                }
                var defaultPic = pics.FirstOrDefault(x => x.IsDefault == true);
                if (defaultPic != null)
                {
                    defaultPic.IsDefault = false;
                    _Uow._CustomerPhoto.Update(defaultPic);
                }
                var currentDefault = pics.FirstOrDefault(x => x.Id == model.PicId);
                if (currentDefault != null)
                {
                    currentDefault.IsDefault = true;
                    _Uow._CustomerPhoto.Update(currentDefault);
                }
                await _Uow.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData
                    .GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            return null;
        }

    }



    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            return DateTime.Now.Ticks + "_" + headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
    }

    public class SetDefaultPictureViewModel
    {
        public int CustomerId { get; set; }
        public int PicId { get; set; }
    }
}
