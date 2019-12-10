using AutoMapper;
using Iruka.DAL;
using Iruka.EF.Model;
using Iruka.ModelAPI;
using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Iruka.Controllers
{
    public class MobileController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public IHttpActionResult GetSpecificCustomerTransactionHistory(string id)
        {
            try
            {
                if (Global.CheckAccessKey(Global.GetAccessKeyFromHeader(Request)))
                {
                    var guidCustomerId = Guid.Parse(id);

                    return Ok(TransactionDal.GetCustomerTransactionHistory(db, guidCustomerId));
                }
                else
                {
                    return BadRequest(Global.Message_WrongAccessKey);
                }
            }
            catch (AccessViolationException)
            {
                return BadRequest(Global.Message_NoAccessKey);
            }
            catch (Exception)
            {
                return BadRequest(Global.Message_ErrorMessage);
            }
        }

        [HttpGet]
        public IHttpActionResult GetSpecificCustomerLastTransaction(string id)
        {
            try
            {
                if (Global.CheckAccessKey(Global.GetAccessKeyFromHeader(Request)))
                {
                    var guidCustomerId = Guid.Parse(id);
                    var lastTransaction = TransactionDal.GetCustomerTransactionHistory(db, guidCustomerId).FirstOrDefault();
                    var customerPoints = db.Users.SingleOrDefault(x => x.Id == id).Points;

                    return Ok(new
                    {
                        lastTransaction = lastTransaction == null ? new TransactionDto() : lastTransaction,
                        customerPoints
                    });
                }
                else
                {
                    return BadRequest(Global.Message_WrongAccessKey);
                }
            }
            catch (AccessViolationException)
            {
                return BadRequest(Global.Message_NoAccessKey);
            }
            catch (Exception)
            {
                return BadRequest(Global.Message_ErrorMessage);
            }
        }

        [HttpPost]
        public HttpResponseMessage Login(LoginModelRequest request)
        {
            var accessKey = Global.GetAccessKeyFromHeader(Request);

            if (Global.CheckAccessKey(accessKey))
            {
                var db = Global.DB;

                try
                {
                    var result = new AccountController();
                    var getUser = db.Users.FirstOrDefault(item => item.UserName == request.Username);
                    var passwordHasher = new PasswordHasher();
                    if (passwordHasher.VerifyHashedPassword(getUser.PasswordHash, request.Password) == PasswordVerificationResult.Success)
                    {
                        var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                        var role = um.GetRoles(getUser.Id).FirstOrDefault();
                        var User = new
                        {
                            getUser.Id,
                            getUser.Name,
                            getUser.Certificate,
                            getUser.Description,
                            getUser.Address,
                            getUser.Picture,
                            getUser.IsActive,
                            getUser.CreatedDate,
                            getUser.Email,
                            getUser.PhoneNumber,
                            getUser.UserName,
                            role
                        };

                        if (role == "Groomer" || role == "Customer" || role == "Owner")
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { User }, MediaTypeHeaderValue.Parse("application/json"));
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_WrongPassword);
                        }
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_WrongPassword);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_ErrorMessage);
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_WrongAccessKey);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetRole()
        {
            try
            {
                var getRoleList = db.Roles.ToList();
                List<object> RoleList = new List<object>();
                foreach (var item in getRoleList)
                {
                    if (item.Name == "Groomer" || item.Name == "Customer" || item.Name == "Owner")
                    {
                        var obj = new { item.Id, item.Name };

                        RoleList.Add(obj);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new { RoleList }, MediaTypeHeaderValue.Parse("application/json"));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_WrongAccessKey);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllProduct()
        {
            try
            {
                var productList = db.Product.ToList();

                return Request.CreateResponse(HttpStatusCode.OK, new { productList }, MediaTypeHeaderValue.Parse("application/json"));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_WrongAccessKey);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllEvent()
        {
            try
            {
                var eventList = db.Event.ToList();

                return Request.CreateResponse(HttpStatusCode.OK, new { eventList }, MediaTypeHeaderValue.Parse("application/json"));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_WrongAccessKey);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetUserByRole(GetUserByRoleModelRequest request)
        {
            try
            {
                var usersWithRoles = Global.DB.Users.ToList();
                List<UserDTO> listUser = new List<UserDTO>();

                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var tempUser = um.Users.ToList();
                foreach (var item in tempUser)
                {
                    if (um.IsInRole(item.Id, request.Role))
                    {
                        listUser.Add(new UserDTO { Id = item.Id, Name = item.Name, Email = item.Email, Certificate = item.Certificate, PhoneNumber = item.PhoneNumber, Address = item.Address, Description = item.Description, Picture = item.Picture });
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { listUser }, MediaTypeHeaderValue.Parse("application/json"));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_WrongAccessKey);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> RegisterUserMobile()
        {
            try
            {
                var db = Global.DB;
                var root = HttpContext.Current.Server.MapPath("~/Media/");
                var startingPosition = root.Length - 6;
                var role = "";
                var name = "";
                var email = "";
                var password = "";
                var phonenumber = "";
                var address = "";
                var description = "";

                var provider = new CustomMultipartFormDataStreamProvider(root);
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                StringBuilder sb = new StringBuilder(); // Holds the response body
                                                        // Read the form data and return an async task.
                await Request.Content.ReadAsMultipartAsync(provider);
                // This illustrates how to get the form data.
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        if (key.Equals("accessKey"))
                        {
                            if (!Global.CheckAccessKey(val))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Wrong access key given, please contact support");
                            }
                        }
                        else if (key.Equals("Name"))
                        {
                            name = val;
                        }
                        else if (key.Equals("Email"))
                        {
                            email = val;
                        }
                        else if (key.Equals("Password"))
                        {
                            password = val;
                        }
                        else if (key.Equals("Phonenumber"))
                        {
                            phonenumber = val;
                        }
                        else if (key.Equals("Address"))
                        {
                            address = val;
                        }
                        else if (key.Equals("Description"))
                        {
                            description = val;
                        }
                        else if (key.Equals("Role"))
                        {
                            role = val;
                        }
                        else
                        {
                            sb.Append(string.Format("{0}: {1}\n", key, val));
                        }
                    }
                }

                // This illustrates how to get the file names for uploaded files.
                foreach (var file in provider.FileData)
                {
                    var splitted = file.LocalFileName.Split('\\');
                    root += "UserPicture\\" + splitted[splitted.Length - 1];

                    try
                    {
                        if (File.Exists(root))
                        {
                            File.Delete(root);
                        }

                        File.Move(file.LocalFileName, root);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        new FileInfo(root).Directory.Create();
                        File.Move(file.LocalFileName, root);
                    }

                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    sb.Append(string.Format("{0}", root));
                }

                var pathUrl = Global.GetServerPathFromAUploadPath(sb.ToString(), 3);
                var passwordHasher = new PasswordHasher();

                var user = new ApplicationUser { Name = name, UserName = email, Email = email, CreatedDate = DateTime.Now, PasswordHash = passwordHasher.HashPassword(password), PhoneNumber = phonenumber, Address = address, Description = description, Picture = pathUrl };
                db.Users.Add(user);

                IdentityUserRole userRole = new IdentityUserRole();
                userRole.UserId = user.Id;
                userRole.RoleId = role;
                db.UserRoles.Add(userRole);
                db.SaveChanges();

                var getUser = db.Users.FirstOrDefault(item => item.UserName == email);

                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var roleUser = um.GetRoles(getUser.Id).FirstOrDefault();

                var User = new
                {
                    getUser.Id,
                    getUser.Name,
                    getUser.Certificate,
                    getUser.Description,
                    getUser.Address,
                    getUser.Picture,
                    getUser.IsActive,
                    getUser.CreatedDate,
                    getUser.Email,
                    getUser.PhoneNumber,
                    getUser.UserName,
                    roleUser
                };

                return Request.CreateResponse(HttpStatusCode.OK, new { User }, MediaTypeHeaderValue.Parse("application/json"));

            }
            catch (NullReferenceException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_ErrorMessage);
            }
            catch (ArgumentOutOfRangeException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Global.Message_ErrorMessage);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, Global.Message_ErrorMessage);
            }
        }
    }
}