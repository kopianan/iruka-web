using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

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
        public static ApplicationDbContext DB
        {
            get { return new ApplicationDbContext(); }
        }
    }
}