﻿using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Script.Serialization;
using static Iruka.EF.Model.Enum;

namespace Iruka.DAL
{
    public class Global
    {
        private static readonly string AccessKey = "d78c1a5c-ccbe-4c26-ac08-43ed66c8afb9";
        public static readonly string Message_ErrorMessage = "Error processing your request, please contact support!";
        public static readonly string Message_WrongPassword = "Wrong Username or Password";
        public static readonly string Message_NoAccessKey = "No access key given !";
        public static readonly string Message_WrongAccessKey = "Wrong access key given, please update your software.";
        public static readonly string Message_SuccessMessage = "Process finished succesfully";
        public static readonly string Message_NoData = "Can't update null data!";

        public static IList<string> GetUserRole(string userId)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var listRoles = um.GetRoles(userId);

            return listRoles;
        }

        public static bool CheckIfUserInRole(IList<string> userRoles, List<string> targetRoles)
        {
            foreach (var targetRole in targetRoles)
            {
                if (userRoles.Contains(targetRole))
                {
                    return true;
                }
            }

            return false;
        }
        internal static bool CheckAccessKey(string accessKey)
        {
            return accessKey == AccessKey;
        }
        internal static string GetServerPathFromAUploadPath(string uploadPathName, int subStringLength)
        {
            string[] splittedPath = uploadPathName.Split('\\');
            string serverPathName = "";
            for (int i = splittedPath.Length - subStringLength; i < splittedPath.Length; i++)
            {
                serverPathName += "/" + splittedPath[i];
            }

            return serverPathName;
        }
        internal static string GetAccessKeyFromHeader(HttpRequestMessage request)
        {
            var headers = request.Headers;

            if (headers.Contains("AccessKey"))
            {
                return headers.GetValues("AccessKey").First();
            }

            throw new AccessViolationException("No Access Key given!");
        }
        public static void SaveBase64DataUrlFile(string base64, string fileName, string savePath)
        {
            byte[] imgArr = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imgArr);
            Image image = Image.FromStream(ms);
            var physicalPath = Path.Combine(savePath, fileName);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
                image.Save(physicalPath);
            }
            else
            {
                image.Save(physicalPath);
            }
        }
        public static string GetLayoutUsername(string id)
        {
            return new ApplicationDbContext().Users.FirstOrDefault(x => x.Id == id).Name;
        }
        public static string GetLayoutEmail(string id)
        {
            return new ApplicationDbContext().Users.FirstOrDefault(x => x.Id == id).Email;
        }
        public static UserRoleEnum GetLayoutRole(string userId)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            foreach (var role in um.GetRoles(userId))
            {
                return (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), role);
            }

            return UserRoleEnum.None;
        }
        public static string GetLayoutPicture(string getUserId)
        {
            var db = new ApplicationDbContext();
            var targetUserPicture = db.Users.SingleOrDefault(x => x.Id == getUserId).Picture;
            var toReturn = (targetUserPicture != null && targetUserPicture != "") ? targetUserPicture : "/Media/avatar-placeholder.png";
            return toReturn;
        }
        public static string DateToString(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                return ((DateTime)dateTime).ToString("dd MMMM yyyy");
            }
            else
            {
                return "-";
            }
        }

        public static string DateTimeToPostableString(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                if (((DateTime)dateTime).TimeOfDay > new TimeSpan(0, 0, 0, 0))
                {
                    return ((DateTime)dateTime).ToString("dd-MM-yyyy HH:mm");
                }
                else
                {
                    return ((DateTime)dateTime).ToString("dd-MM-yyyy");
                }
            }
            else
            {
                return "day-month-year";
            }
        }

        public static string GetUserNameById(string customerId)
        {
            try
            {
                return DB.Users.SingleOrDefault(x => x.Id == customerId).Name;
            }
            catch (Exception)
            {
                return "No Data";
            }
        }

        public static DateTime ParseStringToDate(string dateInString, string formatDate = "dd-MM-yyyy") =>
            DateTime.ParseExact(dateInString, formatDate, CultureInfo.InvariantCulture);

        public static List<object> GetAllCitiesOfIndonesia()
        {
            var toReturn = new List<object>();
            var client = new RestClient("http://pro.rajaongkir.com/api/city");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("key", "d1534a4407fa84006019516410b55d5f");
            IRestResponse result = client.Execute(request);
            var response = new JavaScriptSerializer().Deserialize<LayerObject>(result.Content);
            var cityList = response.rajaongkir.results.ToList();

            foreach (var city in cityList)
            {
                toReturn.Add(new
                {
                    Value = city.type + " " + city.city_name + ", " + city.province
                });
            }

            return toReturn;
        }

        public static ApplicationDbContext DB
        {
            get { return new ApplicationDbContext(); }
        }

        public class LayerObject
        {
            public RootObject rajaongkir { get; set; }
        }

        public class RootObject
        {
            public object query { get; set; }
            public object status { get; set; }
            public List<CityObject> results { get; set; }
        }

        public class CityObject
        {
            public string type { get; set; }
            public string province { get; set; }
            public string city_name { get; set; }
        }
    }
}