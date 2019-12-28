using Iruka.DAL;
using System.Web.Http;

namespace Iruka.Controllers.API
{
    public class ExternalController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetAllCitiesOfIndonesia()
        {
            return Ok(Global.GetAllCitiesOfIndonesia());
        }
    }
}