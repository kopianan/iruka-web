using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Iruka.DAL;
using Iruka.EF.Model;
using Iruka.Models;
using Microsoft.AspNet.Identity;

namespace Iruka.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View(db.Transactions.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = User.Identity.GetUserId(); return View(transaction);
        }

        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ServiceType,Description,IsActive,Point,CreatedDate")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.Id = Guid.NewGuid();
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = User.Identity.GetUserId(); return View(transaction);
        }

        public ActionResult GetUserInfo(string userId)
        {
            try
            {
                var getInvitationId = Guid.Parse(userId);
                //var getInvitation = Global.DB.Users.FirstOrDefault(item => item.Id == getInvitationId);

                var obj = new
                {
                    Temp = ""
                };
                Session["UserId"] = userId;
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            catch (NullReferenceException)
            {
                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }
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
