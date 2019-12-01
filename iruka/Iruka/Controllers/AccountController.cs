using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Iruka.Models;
using System.Reflection;
using Microsoft.AspNet.Identity.EntityFramework;
using Iruka.DAL;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using System.Data.Entity;
using static Iruka.EF.Model.Enum;

namespace Iruka.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Dashboard()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        public ActionResult InternalUserRegister()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InternalUserRegister(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var picturePath = string.IsNullOrWhiteSpace(userDTO.Picture) ? null : "/Media/UserPicture/" + userDTO.Picture;
                var certificatePath = string.IsNullOrWhiteSpace(userDTO.Certificate) ? null : "/Media/Certificate/" + userDTO.Certificate;
                var newUser = new ApplicationUser
                {
                    Picture = picturePath,
                    Certificate = certificatePath,
                    Name = userDTO.Name,
                    PhoneNumber = userDTO.PhoneNumber ?? "",
                    Address = userDTO.Address ?? "",
                    Email = userDTO.Email,
                    UserName = userDTO.Email,
                    Description = userDTO.Description ?? "",
                    CreatedDate = DateTime.Now
                };
                var result = await UserManager.CreateAsync(newUser, userDTO.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(userDTO.Picture))
                    {
                        var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/UserPicture");
                        Global.SaveBase64DataUrlFile(userDTO.Base64URL, userDTO.Picture, savePath);
                    }
                    //await SignInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("CoRegisterViewModelsnfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    if (!await roleManager.RoleExistsAsync(Enum.GetName(typeof(InternalRoleEnum), Enum.Parse(typeof(InternalRoleEnum), userDTO.InternalRoleEnum.ToString()))))
                    {
                        await roleManager.CreateAsync(new IdentityRole(Enum.GetName(typeof(InternalRoleEnum), Enum.Parse(typeof(InternalRoleEnum), userDTO.InternalRoleEnum.ToString()))));
                        await UserManager.AddToRoleAsync(newUser.Id, Enum.GetName(typeof(InternalRoleEnum), Enum.Parse(typeof(InternalRoleEnum), userDTO.InternalRoleEnum.ToString())));
                    }
                    else
                    {
                        await UserManager.AddToRoleAsync(newUser.Id, Enum.GetName(typeof(InternalRoleEnum), Enum.Parse(typeof(InternalRoleEnum), userDTO.InternalRoleEnum.ToString())));
                    }

                    ViewBag.UserId = User.Identity.GetUserId(); db.SaveChanges();
                    return RedirectToAction("InternalUserRegister");
                }

                AddErrors(result);
            }

            ViewBag.AccountExisted = "Email is already taken!";
            ViewBag.UserId = User.Identity.GetUserId(); return View(userDTO);
        }

        public ActionResult EndUserRegister()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EndUserRegister(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var picturePath = string.IsNullOrWhiteSpace(userDTO.Picture) ? null : "/Media/UserPicture/" + userDTO.Picture;
                var certificatePath = string.IsNullOrWhiteSpace(userDTO.Certificate) ? null : "/Media/Certificate/" + userDTO.Certificate;
                var newUser = new ApplicationUser
                {
                    Picture = picturePath,
                    Certificate = certificatePath,
                    Name = userDTO.Name,
                    PhoneNumber = userDTO.PhoneNumber ?? "",
                    Address = userDTO.Address ?? "",
                    Email = userDTO.Email,
                    UserName = userDTO.Email,
                    Description = userDTO.Description ?? "",
                    CreatedDate = DateTime.Now
                };
                var result = await UserManager.CreateAsync(newUser, userDTO.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(userDTO.Picture))
                    {
                        var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/UserPicture");
                        Global.SaveBase64DataUrlFile(userDTO.Base64URL, userDTO.Picture, savePath);
                    }
                    if (userDTO.EndClientEnum == EndClientEnum.Groomer)
                    {
                        if (!string.IsNullOrWhiteSpace(userDTO.Certificate))
                        {
                            var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Certificate");
                            Global.SaveBase64DataUrlFile(userDTO.Base64URLCertificate, userDTO.Certificate, savePath);
                        }
                    }
                    //await SignInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("CoRegisterViewModelsnfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    if (!await roleManager.RoleExistsAsync(Enum.GetName(typeof(EndClientEnum), Enum.Parse(typeof(EndClientEnum), userDTO.EndClientEnum.ToString()))))
                    {
                        await roleManager.CreateAsync(new IdentityRole(Enum.GetName(typeof(EndClientEnum), Enum.Parse(typeof(EndClientEnum), userDTO.EndClientEnum.ToString()))));
                        await UserManager.AddToRoleAsync(newUser.Id, Enum.GetName(typeof(EndClientEnum), Enum.Parse(typeof(EndClientEnum), userDTO.EndClientEnum.ToString())));
                    }
                    else
                    {
                        await UserManager.AddToRoleAsync(newUser.Id, Enum.GetName(typeof(EndClientEnum), Enum.Parse(typeof(EndClientEnum), userDTO.EndClientEnum.ToString())));
                    }

                    ViewBag.UserId = User.Identity.GetUserId(); db.SaveChanges();
                    return RedirectToAction("EndUserRegister");
                }

                AddErrors(result);
            }

            ViewBag.AccountExisted = "Email is already taken!";
            ViewBag.UserId = User.Identity.GetUserId(); return View(userDTO);
        }

        private async Task ClearAllRolesFromUser(string id)
        {
            var roles = await UserManager.GetRolesAsync(id);
            await UserManager.RemoveFromRolesAsync(id, roles.ToArray());
        }

        public ActionResult InternalUserEdit(string id)
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

            var userDTO = Mapper.Map<ApplicationUser, UserDTO>(user);
            userDTO.InternalRoleEnum = DALUsers.GetInternalUserRoleEnum(userDTO.Id);

            ViewBag.UserId = User.Identity.GetUserId(); return View(userDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InternalUserEdit(UserDTO userDTO)
        {
            var targetUser = db.Users.Find(userDTO.Id);

            targetUser.Name = userDTO.Name;
            targetUser.PhoneNumber = userDTO.PhoneNumber;
            targetUser.Address = userDTO.Address;
            targetUser.Description = userDTO.Description;

            if (targetUser.Picture != userDTO.Picture)
            {
                targetUser.Picture = string.IsNullOrWhiteSpace(userDTO.Picture) ? "" : "/Media/UserPicture/" + userDTO.Picture;
                var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/UserPicture");
                Global.SaveBase64DataUrlFile(userDTO.Base64URL, userDTO.Picture, savePath);
            }

            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            await ClearAllRolesFromUser(targetUser.Id);
            db.Entry(targetUser).State = EntityState.Modified;
            if (!await roleManager.RoleExistsAsync(userDTO.InternalRoleEnum.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(userDTO.InternalRoleEnum.ToString()));
                await UserManager.AddToRoleAsync(targetUser.Id, userDTO.InternalRoleEnum.ToString());
            }
            else
            {
                await UserManager.AddToRoleAsync(targetUser.Id, userDTO.InternalRoleEnum.ToString());
            }

            db.SaveChanges();

            return RedirectToAction("InternalUserRegister");
        }

        public ActionResult EndUserEdit(string id)
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

            var userDTO = Mapper.Map<ApplicationUser, UserDTO>(user);
            userDTO.EndClientEnum = DALUsers.GetEndUserRoleEnum(userDTO.Id);

            ViewBag.UserId = User.Identity.GetUserId(); return View(userDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EndUserEdit(UserDTO userDTO)
        {
            var targetUser = db.Users.Find(userDTO.Id);

            targetUser.Name = userDTO.Name;
            targetUser.PhoneNumber = userDTO.PhoneNumber;
            targetUser.Address = userDTO.Address;
            targetUser.Description = userDTO.Description;

            if (targetUser.Picture != userDTO.Picture)
            {
                targetUser.Picture = string.IsNullOrWhiteSpace(userDTO.Picture) ? "" : "/Media/UserPicture/" + userDTO.Picture;
                var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/UserPicture");
                Global.SaveBase64DataUrlFile(userDTO.Base64URL, userDTO.Picture, savePath);
            }

            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            await ClearAllRolesFromUser(targetUser.Id);
            db.Entry(targetUser).State = EntityState.Modified;
            if (!await roleManager.RoleExistsAsync(userDTO.EndClientEnum.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(userDTO.EndClientEnum.ToString()));
                await UserManager.AddToRoleAsync(targetUser.Id, userDTO.EndClientEnum.ToString());
            }
            else
            {
                await UserManager.AddToRoleAsync(targetUser.Id, userDTO.EndClientEnum.ToString());
            }

            if (userDTO.EndClientEnum == EndClientEnum.Groomer)
            {
                if (targetUser.Certificate != userDTO.Certificate)
                {
                    targetUser.Certificate = string.IsNullOrWhiteSpace(userDTO.Certificate) ? "" : "/Media/Certificate/" + userDTO.Certificate;
                    var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Certificate");
                    Global.SaveBase64DataUrlFile(userDTO.Base64URLCertificate, userDTO.Certificate, savePath);
                }
            }
            else
            {
                targetUser.Certificate = "";
            }

            db.SaveChanges();

            return RedirectToAction("EndUserRegister");
        }

        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            try
            {
                ApplicationUser targetUser = db.Users.Find(id);
                targetUser.IsActive = false;
                db.Entry(targetUser).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.GetUserId() == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var targetUser = db.Users.SingleOrDefault(x => x.Email == model.Email);

                    if (targetUser.IsActive)
                    {
                        return RedirectToLocal(returnUrl);
                    }

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    ModelState.AddModelError("", "Your account is inactive, please contact Administrator!");
                    return View(model);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}