using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Iruka.Models
{
    public class BranchDto : BaseEntityDto
    {
        public Guid Id { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string PhoneAreaCode { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Whatsapp { get; set; }

        [Display(Name = "Point Rate")]
        public double PointRate { get; set; }
    }
}