using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Coupon : BaseEntity<Guid>
    {
        public string ServiceType { get; set; }
        public int MaxPoint { get; set; }
        public int Point { get; set; }
        public string Bonus { get; set; }
    }
}
