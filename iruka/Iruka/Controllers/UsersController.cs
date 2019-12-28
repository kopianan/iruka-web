using Iruka.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Iruka.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult EditGroomer(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroomer([Bind(Include = "Id,Picture,Name,Description,Address,Email,PhoneNumber,Certificate")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userEdited = db.Users.Find(user.Id);
                var getPicture = user.Picture.Split('\\');
                var getCertificate = user.Certificate.Split('\\');
                userEdited.Picture = getPicture[getPicture.Length - 1];
                userEdited.Name = user.Name;
                userEdited.Description = user.Description;
                userEdited.Address = user.Address;
                userEdited.Email = user.Email;
                userEdited.PhoneNumber = user.PhoneNumber;
                userEdited.Certificate = getCertificate[getCertificate.Length - 1];
                db.SaveChanges();
                return RedirectToAction("RegisterGroomer");
            }
            return View(user);
        }

        #region UPLOAD

        [HttpPost]
        public ActionResult SavePicture(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var path = Save(files);
            return Json(new { success = path }, JsonRequestBehavior.AllowGet);
        }
        public static string Save(IEnumerable<HttpPostedFileBase> files)
        {
            var tempPhysicalPath = "";

            foreach (var file in files)
            {

                var fileName = Path.GetFileName(file.FileName);
                var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/UserPicture");
                var physicalPath = Path.Combine(filePath, fileName);
                file.SaveAs(physicalPath);
                tempPhysicalPath = physicalPath;
            }

            return tempPhysicalPath;
        }
        public ActionResult RemovePicture(string[] fileNames)
        {
            RemovePicture(fileNames);
            return Json(new { error = "Insert Error : Inserted data not valid" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SavingCertificate(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var path = SaveCertificate(files);
            return Json(new { success = path }, JsonRequestBehavior.AllowGet);
        }
        public static string SaveCertificate(IEnumerable<HttpPostedFileBase> files)
        {
            var tempPhysicalPath = "";

            foreach (var file in files)
            {

                var fileName = Path.GetFileName(file.FileName);
                var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Certificate");
                var physicalPath = Path.Combine(filePath, fileName);
                file.SaveAs(physicalPath);
                tempPhysicalPath = physicalPath;
            }

            return tempPhysicalPath;
        }
        public ActionResult RemoveCertificate(string[] fileNames)
        {
            RemoveCertificate(fileNames);
            return Json(new { error = "Insert Error : Inserted data not valid" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}