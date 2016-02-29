using HairPlus.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;

namespace HairPlus.Web.Controllers
{
    public class HairLossSolutionController : BaseController
    {
        public HairLossSolutionController(IUow uow)
        {
            _Uow = uow;
        }

        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var hairLossSolutions = await _Uow._HairLossSolution
                    .GetAll(x => x.Active == true)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToListAsync();

                return Ok(hairLossSolutions);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
