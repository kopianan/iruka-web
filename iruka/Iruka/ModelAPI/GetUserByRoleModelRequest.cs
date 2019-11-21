using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iruka.ModelAPI
{
    public class GetUserByRoleModelRequest
    {
        public string Role { get; set; }

        public GetUserByRoleModelRequest(string role)
        {
            Role = role;
        }
    }
}