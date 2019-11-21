using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using Iruka.DAL;
using System.IO;

namespace Iruka.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public async Task<ActionResult> UserRegistration(string name, string email, string password, string phone, string address, string picture, string desc, string role, string certificate)
        {
            try
            {
                picture = "/Media/UserPicture/" + picture;
                certificate = "/Media/Certificate/" + certificate;
                var user = new ApplicationUser { Name = name, Description = desc, PhoneNumber = phone, UserName = email, Email = email, CreatedDate = DateTime.Now, Address = address, Picture = picture, Certificate = certificate };
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("CoRegisterViewModelsnfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    if (!await roleManager.RoleExistsAsync(Enum.GetName(typeof(RoleMenuList), Enum.Parse(typeof(RoleMenuList), role))))
                    {
                        await roleManager.CreateAsync(new IdentityRole(Enum.GetName(typeof(RoleMenuList), Enum.Parse(typeof(RoleMenuList), role))));
                        await UserManager.AddToRoleAsync(user.Id, Enum.GetName(typeof(RoleMenuList), Enum.Parse(typeof(RoleMenuList), role)));
                    }
                    else
                    {
                        await UserManager.AddToRoleAsync(user.Id, Enum.GetName(typeof(RoleMenuList), Enum.Parse(typeof(RoleMenuList), role)));
                    }
                    db.SaveChanges();
                    return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region INDEX
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult RegisterFinance()
        {
            return View();
        }
        public ActionResult RegisterAdmin()
        {
            return View();
        }
        public ActionResult RegisterGroomer()
        {
            return View();
        }
        public ActionResult RegisterOwner()
        {
            return View();
        }
        public ActionResult RegisterCustomer()
        {
            return View();
        }
        public ActionResult RegisterSuperAdmin()
        {
            return View();
        }
        public ActionResult RegisterContentManager()
        {
            return View();
        }
        #endregion

        #region EDIT
        public ActionResult EditContentManager(string id)
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

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContentManager([Bind(Include = "Id,Picture,Name,Description,Address,Email,PhoneNumber")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userEdited = db.Users.Find(user.Id);
                var getPicture = user.Picture.Split('\\');
                userEdited.Picture = getPicture[getPicture.Length - 1];
                userEdited.Name = user.Name;
                userEdited.Description = user.Description;
                userEdited.Address = user.Address;
                userEdited.Email = user.Email;
                userEdited.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
                return RedirectToAction("RegisterContentManager");
            }
            return View(user);
        }
        public ActionResult EditFinance(string id)
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

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFinance([Bind(Include = "Id,Picture,Name,Description,Address,Email,PhoneNumber")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userEdited = db.Users.Find(user.Id);
                var getPicture = user.Picture.Split('\\');
                userEdited.Picture = getPicture[getPicture.Length - 1];
                userEdited.Name = user.Name;
                userEdited.Description = user.Description;
                userEdited.Address = user.Address;
                userEdited.Email = user.Email;
                userEdited.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
                return RedirectToAction("RegisterFinance");
            }
            return View(user);
        }
        public ActionResult EditSuperAdmin(string id)
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

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSuperAdmin([Bind(Include = "Id,Picture,Name,Description,Address,Email,PhoneNumber")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userEdited = db.Users.Find(user.Id);
                var getPicture = user.Picture.Split('\\');
                userEdited.Picture = getPicture[getPicture.Length - 1];
                userEdited.Name = user.Name;
                userEdited.Description = user.Description;
                userEdited.Address = user.Address;
                userEdited.Email = user.Email;
                userEdited.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
                return RedirectToAction("RegisterSuperAdmin");
            }
            return View(user);
        }
        public ActionResult EditOwner(string id)
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

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOwner([Bind(Include = "Id,Picture,Name,Description,Address,Email,PhoneNumber")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userEdited = db.Users.Find(user.Id);
                var getPicture = user.Picture.Split('\\');
                userEdited.Picture = getPicture[getPicture.Length - 1];
                userEdited.Name = user.Name;
                userEdited.Description = user.Description;
                userEdited.Address = user.Address;
                userEdited.Email = user.Email;
                userEdited.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
                return RedirectToAction("RegisterOwner");
            }
            return View(user);
        }
        public ActionResult EditCustomer(string id)
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

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCustomer([Bind(Include = "Id,Picture,Name,Description,Address,Email,PhoneNumber")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userEdited = db.Users.Find(user.Id);
                var getPicture = user.Picture.Split('\\');
                userEdited.Picture = getPicture[getPicture.Length - 1];
                userEdited.Name = user.Name;
                userEdited.Description = user.Description;
                userEdited.Address = user.Address;
                userEdited.Email = user.Email;
                userEdited.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
                return RedirectToAction("RegisterCustomer");
            }
            return View(user);
        }

        public ActionResult EditAdmin(string id)
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

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin([Bind(Include = "Id,Picture,Name,Description,Address,Email,PhoneNumber")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userEdited = db.Users.Find(user.Id);
                var getPicture = user.Picture.Split('\\');
                userEdited.Picture = getPicture[getPicture.Length - 1];
                userEdited.Name = user.Name;
                userEdited.Description = user.Description;
                userEdited.Address = user.Address;
                userEdited.Email = user.Email;
                userEdited.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
                return RedirectToAction("RegisterAdmin");
            }
            return View(user);
        }

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

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        #endregion

        #region DELETE
        public ActionResult DeleteUser(string id)
        {
            try
            {
                ApplicationUser user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

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

        #region ETC
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        #endregion

        #region RELOAD TABLE
        public ActionResult ReloadRegisterContentManagerTable()
        {
            return PartialView("/Views/Partial/Partial_RegisterContentManager.cshtml");
        }
        public ActionResult ReloadRegisterFinanceTable()
        {
            return PartialView("/Views/Partial/Partial_RegisterFinance.cshtml");
        }
        public ActionResult ReloadRegisterAdminTable()
        {
            return PartialView("/Views/Partial/Partial_RegisterAdmin.cshtml");
        }
        public ActionResult ReloadRegisterGroomerTable()
        {
            return PartialView("/Views/Partial/Partial_RegisterGroomer.cshtml");
        }
        public ActionResult ReloadRegisterCustomerTable()
        {
            return PartialView("/Views/Partial/Partial_RegisterCustomer.cshtml");
        }
        public ActionResult ReloadRegisterOwnerTable()
        {
            return PartialView("/Views/Partial/Partial_RegisterOwner.cshtml");
        }
        public ActionResult ReloadRegisterSuperAdminTable()
        {
            return PartialView("/Views/Partial/Partial_RegisterSuperAdmin.cshtml");
        }
        #endregion
    }
}
