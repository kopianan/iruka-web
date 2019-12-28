using System;
using System.Collections.Generic;

namespace Iruka.Models
{
    public class TransactionDto : BaseEntityDto
    {
        public Guid? CustomerId { get; set; }
        public Guid? CouponId { get; set; }
        public string CouponValue { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        public string Notes { get; set; }
        public int EarnedPoint { get; set; }
        public string CustomerName { get; set; }
        public string TransactionType { get; set; }
        public List<string> TransactionTypeOptions { get; set; }
    }
}