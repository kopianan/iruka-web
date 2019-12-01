using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Iruka.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using static Iruka.EF.Model.Enum;

namespace Iruka.DAL
{
    public class DALUsers
    {
        public static List<UserDTO> GetAllUserBaseOnRole(List<string> roles)
        {
            var activeUsers = Global.DB.Users.Where(x => x.IsActive).OrderByDescending(x => x.CreatedDate).ToList();
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            List<UserDTO> toReturn = new List<UserDTO>();

            foreach (var user in activeUsers)
            {
                foreach (var role in roles)
                {
                    if (um.IsInRole(user.Id, role))
                    {
                        toReturn.Add(new UserDTO
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Email = user.Email,
                            Picture = user.Picture,                            
                            Certificate = user.Certificate,
                            PhoneNumber = user.PhoneNumber,
                            Address = user.Address,
                            Description = user.Description,
                            Role = role
                        });
                    }
                }
            }

            return toReturn;
        }

        public static InternalRoleEnum GetInternalUserRoleEnum(string userId)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            foreach (var role in um.GetRoles(userId))
            {
                return (InternalRoleEnum)Enum.Parse(typeof(InternalRoleEnum), role);
            }

            return InternalRoleEnum.Admin;
        }

        public static EndClientEnum GetEndUserRoleEnum(string userId)
        {
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            foreach (var role in um.GetRoles(userId))
            {
                return (EndClientEnum)Enum.Parse(typeof(EndClientEnum), role);
            }

            return EndClientEnum.Groomer;
        }
    }
}