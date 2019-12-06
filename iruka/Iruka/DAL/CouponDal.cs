using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                couponDto.CouponTypeValue = coupon.CouponType.ToString();

                toReturn.Add(couponDto);
            }

            return toReturn;
        }
    }
}