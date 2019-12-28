using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Iruka.EF.Model.Enum;

namespace Iruka.DAL
{
    public class CouponDal
    {
        public static List<CouponDto> GetAllActiveCoupons()
        {
            var coupons = Global.DB.Coupons
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();
            var toReturn = new List<CouponDto>();

            foreach (var coupon in coupons)
            {
                var couponDto = Mapper.Map<Coupon, CouponDto>(coupon);
                couponDto.CouponValue = GetCouponValue(coupon);
                couponDto.CouponTypeValue = coupon.CouponType.ToString();

                toReturn.Add(couponDto);
            }

            return toReturn;
        }

        public static void AddCouponPurchaseCount(ApplicationDbContext db, Guid? targetCouponId)
        {
            if (targetCouponId != null && targetCouponId != Guid.Empty)
            {
                var targetCoupon = db.Coupons.SingleOrDefault(x => x.Id == targetCouponId);

                targetCoupon.Purchased++;
            }
        }

        public static string GetCouponValue(Coupon coupon)
        {
            if (coupon.CouponType == CouponType.Discount)
            {
                if (coupon.DiscountType == FeeType.Percent)
                {
                    return "Discount " + coupon.DiscountValue + "%";
                }
                else
                {
                    return "Discount Rp. " + coupon.DiscountValue.ToString("N0");
                }
            }
            else
            {
                return coupon.FreeProduct;
            }
        }
    }
}