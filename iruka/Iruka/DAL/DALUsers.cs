using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Iruka.DAL
{
    public class DALUsers
    {
        public static List<UserDTO> GetAllUserBaseOnRole(string role)
        {
            var usersWithRoles = Global.DB.Users.ToList();
            List<UserDTO> listUser = new List<UserDTO>();

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var tempUser = um.Users.ToList();
            foreach (var item in tempUser)
            {
                if (um.IsInRole(item.Id, role))
                {
                    listUser.Add(new UserDTO { Id = item.Id, Name = item.Name, Email = item.Email,Certificate=item.Certificate,PhoneNumber=item.PhoneNumber,Address=item.Address,Description=item.Description });
                }
            }

            return listUser;
        }
    }
}