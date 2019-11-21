using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iruka.ModelAPI
{
    public class LoginModelRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginModelRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}