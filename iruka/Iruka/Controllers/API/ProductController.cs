using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Iruka.Controllers.API
{
    public class ProductController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public IHttpActionResult UpdateRow([FromBody] UpdateRowRequest request)
        {
            var productSequence = new List<Product>();
            var rowArray = new JavaScriptSerializer().Deserialize<List<RowData>>(request.RowArray);

            foreach (var row in rowArray)
            {
                productSequence.Add(db.Product.Single(x => x.Priority == row.OldData));
            }

            for (int i = 0; i < productSequence.Count(); i++)
            {
                var newRow = rowArray.Single(x => x.OldData == productSequence[i].Priority).NewData;
                productSequence[i].Priority = newRow;
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
}
