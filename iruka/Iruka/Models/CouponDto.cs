using System;
using System.ComponentModel.DataAnnotations;
using static Iruka.EF.Model.Enum;

namespace Iruka.Models
{
    public class CouponDto : BaseEntityDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Coupon Type")]
        public CouponType CouponType { get; set; }
        public string CouponTypeValue { get; set; }

        [Required]
        [Display(Name = "Price in Point")]
        public int PointPrice { get; set; }

        [Required]
        [Display(Name = "Coupon Amount")]
        public int Amount { get; set; }
        public int Purchased { get; set; }
        public FeeType DiscountType { get; set; }

        [Display(Name = "Discount")]
        public double DiscountValue { get; set; }

        [Display(Name = "Free Product")]
        public string FreeProduct { get; set; }

        public string CouponValue { get; set; }
    }
}