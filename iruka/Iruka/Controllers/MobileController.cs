using AutoMapper;
using Iruka.DAL;
using Iruka.ModelAPI;
using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static Iruka.EF.Model.Enum;

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
                        var User = Mapper.Map<ApplicationUser, MobileUserViewModel>(getUser);
                        User.Role = role;

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
                var listUser = new List<MobileUserViewModel>();
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var targetUsers = um.Users.Where(x => x.IsActive).ToList();

                foreach (var user in targetUsers)
                {
                    if (um.IsInRole(user.Id, request.Role))
                    {
                        listUser.Add(Mapper.Map<ApplicationUser, MobileUserViewModel>(user));
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { listUser }, MediaTypeHeaderValue.Parse("application/json"));
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Global.Message_ErrorMessage);
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
                var newUserDto = new MobileUserDto();
                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var passwordHasher = new PasswordHasher();
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
                    foreach (var value in provider.FormData.GetValues(key))
                    {
                        if (key.Equals("accessKey"))
                        {
                            if (!Global.CheckAccessKey(value))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Global.Message_WrongAccessKey);
                            }
                        }

                        foreach (PropertyInfo propertyInfo in newUserDto.GetType().GetProperties())
                        {
                            if (key.Equals(propertyInfo.Name))
                            {
                                var propType = newUserDto.GetType().GetProperty(propertyInfo.Name).PropertyType;
                                var converter = TypeDescriptor.GetConverter(propType);
                                var convertedObject = converter.ConvertFromString(value);

                                newUserDto.GetType().GetProperty(propertyInfo.Name).SetValue(newUserDto, convertedObject);
                            }
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

                var pathUrl = provider.FileData.Count() == 0 ? null : Global.GetServerPathFromAUploadPath(sb.ToString(), 3);

                var user = new ApplicationUser
                {
                    CreatedDate = DateTime.Now,
                    PasswordHash = passwordHasher.HashPassword(newUserDto.Password),
                    Name = newUserDto.Name,
                    UserName = newUserDto.Email,
                    Email = newUserDto.Email,
                    PhoneNumber = newUserDto.PhoneNumber,
                    Address = newUserDto.Address,
                    Description = newUserDto.Description,
                    Picture = pathUrl,
                    PIC = newUserDto.PIC,
                    KeyFeatures = newUserDto.KeyFeatures,
                    CoverageArea = newUserDto.CoverageArea,
                    YearsOfExperience = newUserDto.YearsOfExperience,
                    Availability = newUserDto.Availability,
                    Styling = newUserDto.Styling,
                    Clipping = newUserDto.Styling,
                    TrainingYears = newUserDto.TrainingYears,
                    TrainingCourses = newUserDto.TrainingCourses
                };

                try
                {
                    user.TrainingStartDate = Global.ParseStringToDate(newUserDto.TrainingStartDate);
                }
                catch (FormatException)
                {
                }
                catch (ArgumentNullException)
                {
                }

                db.Users.Add(user);

                IdentityUserRole userRole = new IdentityUserRole();
                userRole.UserId = user.Id;
                userRole.RoleId = newUserDto.Role;
                db.UserRoles.Add(userRole);
                db.SaveChanges();

                var getUser = db.Users.SingleOrDefault(item => item.Email == newUserDto.Email);
                var roleUser = um.GetRoles(getUser.Id).FirstOrDefault();
                var User = Mapper.Map<ApplicationUser, MobileUserViewModel>(getUser);
                User.Role = roleUser;

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

        [HttpPost]
        public async Task<HttpResponseMessage> EditUserMobile()
        {
            try
            {
                var db = Global.DB;
                var root = HttpContext.Current.Server.MapPath("~/Media/");
                var startingPosition = root.Length - 6;
                var editUserDto = new MobileUserDto();
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
                    foreach (var value in provider.FormData.GetValues(key))
                    {
                        if (key.Equals("accessKey"))
                        {
                            if (!Global.CheckAccessKey(value))
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Global.Message_WrongAccessKey);
                            }
                        }
                        foreach (PropertyInfo propertyInfo in editUserDto.GetType().GetProperties())
                        {
                            if (key.Equals(propertyInfo.Name))
                            {
                                var propType = editUserDto.GetType().GetProperty(propertyInfo.Name).PropertyType;
                                var converter = TypeDescriptor.GetConverter(propType);
                                var convertedObject = converter.ConvertFromString(value);

                                editUserDto.GetType().GetProperty(propertyInfo.Name).SetValue(editUserDto, convertedObject);
                            }
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

                var targetUser = db.Users.SingleOrDefault(x => x.Id == editUserDto.Id);
                targetUser.Name = editUserDto.Name;
                targetUser.PhoneNumber = editUserDto.PhoneNumber;
                targetUser.Address = editUserDto.Address;
                targetUser.Description = editUserDto.Description;
                targetUser.PIC = editUserDto.PIC;
                targetUser.Show = editUserDto.Show;
                targetUser.KeyFeatures = editUserDto.KeyFeatures;
                targetUser.CoverageArea = editUserDto.CoverageArea;
                targetUser.YearsOfExperience = editUserDto.YearsOfExperience;
                targetUser.Availability = editUserDto.Availability;
                targetUser.Styling = editUserDto.Styling;
                targetUser.Clipping = editUserDto.Styling;
                targetUser.TrainingYears = editUserDto.TrainingYears;
                targetUser.TrainingCourses = editUserDto.TrainingCourses;

                try
                {
                    targetUser.TrainingStartDate = Global.ParseStringToDate(editUserDto.TrainingStartDate);
                }
                catch (FormatException)
                {
                }
                catch (ArgumentNullException)
                {
                }

                if (provider.FileData.Count() > 0)
                {
                    targetUser.Picture = Global.GetServerPathFromAUploadPath(sb.ToString(), 3);
                }

                db.SaveChanges();

                var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var roleUser = um.GetRoles(targetUser.Id).FirstOrDefault();
                var User = Mapper.Map<ApplicationUser, MobileUserViewModel>(targetUser);

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

        [HttpPut]
        public IHttpActionResult ChangeGroomerShowStatus([FromUri] string userId, bool status)
        {
            try
            {
                if (Global.CheckAccessKey(Global.GetAccessKeyFromHeader(Request)))
                {
                    var targetUser = db.Users.SingleOrDefault(x => x.Id == userId);
                    targetUser.Show = status;
                    db.SaveChanges();

                    return Ok("Action successful!");
                }
                else
                {
                    return BadRequest(Global.Message_WrongAccessKey);
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest(Global.Message_NoData);
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

        public class MobileUserViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string Picture { get; set; }
            public DateTime CreatedDate { get; set; }
            public string PIC { get; set; }
            public string KeyFeatures { get; set; }
            public string CoverageArea { get; set; }
            public int YearsOfExperience { get; set; }
            public bool Availability { get; set; }
            public int Styling { get; set; }
            public int Clipping { get; set; }
            public DateTime? TrainingStartDate { get; set; }
            public int TrainingYears { get; set; }
            public string TrainingCourses { get; set; }
            public bool Show { get; set; }
            public string Certificate { get; set; }
            public string Role { get; set; }
            public bool IsActive { get; set; }
        }

        public class MobileUserDto
        {
            public string Id { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string Picture { get; set; }
            public DateTime CreatedDate { get; set; }
            public string PIC { get; set; }
            public string KeyFeatures { get; set; }
            public string CoverageArea { get; set; }
            public int YearsOfExperience { get; set; }
            public bool Availability { get; set; }
            public GroomerRating Styling { get; set; }
            public GroomerRating Clipping { get; set; }
            public string TrainingStartDate { get; set; }
            public int TrainingYears { get; set; }
            public string TrainingCourses { get; set; }
            public bool Show { get; set; }
            public string Certificate { get; set; }
            public string Role { get; set; }
        }
    }
}