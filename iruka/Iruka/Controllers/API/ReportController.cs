using AutoMapper;
using Iruka.DAL;
using Iruka.EF.Model;
using Iruka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Iruka.Controllers.API
{
    public class ReportController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public IHttpActionResult GetFilterableTransactionHistory([FromUri] string startDate, string endDate)
        {
            var startFilterDate = DateTime.MinValue;
            var endFilterDate = DateTime.MaxValue;
            var listToReturn = new List<TransactionDto>();

            if (startDate != "day-month-year")
            {
                startFilterDate = Global.ParseStringToDate(startDate);
            }

            if (endDate != "day-month-year")
            {
                endFilterDate = Global.ParseStringToDate(endDate).AddDays(1);
            }

            var transactionHistories = db.Transactions
                .Where(x
                => x.IsActive
                && x.CreatedDate >= startFilterDate
                && x.CreatedDate < endFilterDate)
                .OrderByDescending(x => x.CreatedDate)
                .ToList();

            foreach (var history in transactionHistories)
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
                historyDto.CustomerName = Global.GetUserNameById(history.CustomerId.ToString());
                listToReturn.Add(historyDto);
            }

            return Ok(listToReturn);
        }
    }
}
