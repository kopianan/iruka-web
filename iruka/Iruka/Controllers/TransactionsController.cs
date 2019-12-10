using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Iruka.DAL;
using Iruka.EF.Model;
using Iruka.Models;
using Microsoft.AspNet.Identity;
using static Iruka.DAL.TransactionDal;

namespace Iruka.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View(db.Transactions.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionDto transactionDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var transaction = Mapper.Map<TransactionDto, Transaction>(transactionDto);
                transaction.NewCreatedData(userId);
                transaction.EarnedPoint = TransactionDal.CalculateEarnedPointByTransanction(db, transaction.Total);
                CouponDal.AddCouponPurchaseCount(db, transaction.CouponId);
                TransactionDal.DeductCustomerPointByCoupon(db, transaction.CustomerId.ToString(), transaction.CouponId);
                TransactionDal.AddPointToCustomer(db, transaction.CustomerId.ToString(), transaction.EarnedPoint);
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.UserId = User.Identity.GetUserId(); return View(transactionDto);
        }

        public ActionResult GetCustomerData(string userId)
        {
            try
            {
                var targetCustomer = db.Users.SingleOrDefault(x => x.Id == userId);
                var customerDataDto = Mapper.Map<ApplicationUser, CustomerDataDto>(targetCustomer);
                customerDataDto.Picture = customerDataDto.Picture ?? "/Media/avatar-placeholder.png";
                customerDataDto.TransactionHistory = new List<TransactionDto>();
                customerDataDto.PurchaseableCoupons = new List<CouponDto>();

                if (Global.CheckIfUserInRole(
                    Global.GetUserRole(targetCustomer.Id), new List<string> { RoleList.Customer, RoleList.Groomer, RoleList.Owner }))
                {
                    var guidCustomerId = Guid.Parse(targetCustomer.Id);
                    var purchaseableCoupons = db.Coupons.Where(x
                        => x.IsActive
                        && x.Purchased < x.Amount
                        && x.PointPrice <= customerDataDto.Points)
                        .OrderBy(x => x.PointPrice)
                        .ToList();

                    customerDataDto.TransactionHistory = TransactionDal.GetCustomerTransactionHistory(db, guidCustomerId);

                    foreach (var coupon in purchaseableCoupons)
                    {
                        var couponDto = Mapper.Map<Coupon, CouponDto>(coupon);
                        couponDto.CouponValue = CouponDal.GetCouponValue(coupon);
                        couponDto.CouponTypeValue = coupon.CouponType.ToString();
                        customerDataDto.PurchaseableCoupons.Add(couponDto);
                    }

                    return Json(new { success = customerDataDto }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            catch (NullReferenceException)
            {
                return Json(new { error = "null" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
