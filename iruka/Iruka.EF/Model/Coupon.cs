using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Iruka.EF.Model.Enum;

namespace Iruka.EF.Model
{
    public class Coupon : BaseEntity<Guid>
    {
        public int PointPrice { get; set; }
        public int Amount { get; set; }
        public int Purchased { get; set; }
        public FeeType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal DiscountTotal { get; set; }
    }
}