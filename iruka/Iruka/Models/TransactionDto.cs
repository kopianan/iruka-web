using Iruka.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iruka.Models
{
    public class TransactionDto : BaseEntityDto
    {
        public Guid CustomerId { get; set; }
        public Guid? CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        public string Description { get; set; }
        public int EarnedPoint { get; set; }
    }
}