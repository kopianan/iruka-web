using Iruka.DAL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Iruka.Controllers.API
{
    public class ExternalController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllCitiesOfIndonesia()
        {
            return Content(HttpStatusCode.OK, Global.GetAllCitiesOfIndonesia());
        }
    }
}