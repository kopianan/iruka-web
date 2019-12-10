using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iruka.EF.Model
{
    public class Transaction : BaseEntity<Guid>
    {
        public Guid CustomerId { get; set; }
        public Guid? CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        public string Notes { get; set; }
        public int EarnedPoint { get; set; }
    }
}