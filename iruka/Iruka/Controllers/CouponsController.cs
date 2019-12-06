using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using Microsoft.AspNet.Identity;

namespace Iruka.Controllers
{
    public class CouponsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Coupons
        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var coupon = Mapper.Map<CouponDto, Coupon>(couponDto);
                coupon.NewCreatedData(userId);
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = User.Identity.GetUserId(); return View(couponDto);
        }

        // GET: Coupons/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }

            var couponDto = Mapper.Map<Coupon, CouponDto>(coupon);

            ViewBag.UserId = User.Identity.GetUserId(); return View(couponDto);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var coupon = db.Coupons.SingleOrDefault(x => x.Id == couponDto.Id);
                coupon = Mapper.Map(couponDto, coupon);
                coupon.SetModifiedData(userId);
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = User.Identity.GetUserId(); return View(couponDto);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                Coupon coupon = db.Coupons.Find(id);
                coupon.SetIsActive(false, User.Identity.GetUserId());
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
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
