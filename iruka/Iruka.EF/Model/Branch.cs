using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Branch : BaseEntity<Guid>
    {
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string PhoneAreaCode { get; set; }
        public string Phone { get; set; }
        public string MobilePhone { get; set; }
        public string Whatsapp { get; set; }
        public decimal PointRate { get; set; }
    }
}
