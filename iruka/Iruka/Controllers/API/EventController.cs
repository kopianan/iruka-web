using Iruka.EF.Model;
using Iruka.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Iruka.Controllers.API
{
    public class EventController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public IHttpActionResult UpdateRow([FromBody] UpdateRowRequest request)
        {
            var eventSequence = new List<Event>();
            var rowArray = new JavaScriptSerializer().Deserialize<List<RowData>>(request.RowArray);

            foreach (var row in rowArray)
            {
                eventSequence.Add(db.Event.Single(x => x.Priority == row.OldData));
            }

            for (int i = 0; i < eventSequence.Count(); i++)
            {
                var newRow = rowArray.Single(x => x.OldData == eventSequence[i].Priority).NewData;
                eventSequence[i].Priority = newRow;
            }

            db.SaveChanges();
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class UpdateRowRequest
    {
        public string UserId { get; set; }
        public string RowArray { get; set; }
    }

    public class RowData
    {
        public int OldData { get; set; }
        public int NewData { get; set; }
    }
}
