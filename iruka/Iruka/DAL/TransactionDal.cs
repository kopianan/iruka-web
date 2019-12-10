using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iruka.DAL
{
    public class TransactionDal
    {
        public static int CalculateEarnedPointByTransanction(ApplicationDbContext db, double transactionTotal)
        {
            var branchPointRate = db.Branches.ToList().FirstOrDefault().PointRate;

            return (int)Math.Floor(transactionTotal / branchPointRate);
        }

        public static void DeductCustomerPointByCoupon(ApplicationDbContext db, string customerId, Guid? couponId)
        {
            var targetCustomer = db.Users.SingleOrDefault(x => x.Id == customerId);
            var targetCouponPointPrice = db.Coupons.SingleOrDefault(x => x.Id == couponId).PointPrice;

            targetCustomer.Points -= targetCouponPointPrice;
        }

        public static void AddPointToCustomer(ApplicationDbContext db, string customerId, int pointAmount)
        {
            var targetCustomer = db.Users.SingleOrDefault(x => x.Id == customerId);

            targetCustomer.Points += pointAmount;
        }

        public static List<TransactionDto> GetCustomerTransactionHistory(ApplicationDbContext db, Guid customerId)
        {
            var customerTransactionHistory = db.Transactions
                                .Where(x => x.IsActive && x.CustomerId == customerId)
                                .OrderByDescending(x => x.CreatedDate)
                                .ToList();
            var listToReturn = new List<TransactionDto>();

            foreach (var history in customerTransactionHistory)
            {
                var historyDto = Mapper.Map<Transaction, TransactionDto>(history);

                if (historyDto.CouponId != null)
                {
                    historyDto.CouponValue = CouponDal.GetCouponValue(db.Coupons.SingleOrDefault(x => x.Id == historyDto.CouponId));
                }
                else
                {
                    historyDto.CouponValue = "-";
                }

                historyDto.CreatedDate = Global.DateToString(history.CreatedDate);
                listToReturn.Add(historyDto);
            }

            return listToReturn;
        }

        public class CustomerDataDto
        {
            public string Id { get; set; }
            public string Picture { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Description { get; set; }
            public int Points { get; set; }
            public List<TransactionDto> TransactionHistory { get; set; }
            public List<CouponDto> PurchaseableCoupons { get; set; }
        }
    }
}