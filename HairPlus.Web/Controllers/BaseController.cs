﻿using HairPlus.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HairPlus.Web.Controllers
{
    public class BaseController : ApiController
    {
        public IUow _Uow { get; set; }
    }
}
