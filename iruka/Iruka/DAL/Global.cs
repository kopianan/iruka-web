using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using static Iruka.EF.Model.Enum;

namespace Iruka.DAL
{
    public class Global
    {
        private static readonly string AccessKey = "d78c1a5c-ccbe-4c26-ac08-43ed66c8afb9";
        public static readonly string Message_ErrorMessage = "Error processing your request, Please contact support!";
        public static readonly string Message_WrongPassword = "Wrong Username or Password";
        public static readonly string Message_WrongAccessKey = "Wrong access key given, please update your software.";
        public static readonly string Message_SuccessMessage = "Process finished succesfully";
        public static IList<string> GetCurrentMenuList(string userId)
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
        public static ApplicationDbContext DB
        {
            get { return new ApplicationDbContext(); }
        }
    }
}