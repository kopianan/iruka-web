using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Iruka.DAL;
using Iruka.EF.Model;
using Iruka.Models;
using Microsoft.AspNet.Identity;

namespace Iruka.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var product = Mapper.Map<ProductDTO, Product>(productDto);
                product.Picture = string.IsNullOrWhiteSpace(productDto.Picture) ? null : "/Media/Product/" + productDto.Picture;
                product.NewCreatedData(userId);

                db.Product.Add(product);
                var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Product");
                Global.SaveBase64DataUrlFile(productDto.Base64URL, productDto.Picture, savePath);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = User.Identity.GetUserId(); return View(productDto);
        }

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var productDTO = Mapper.Map<Product, ProductDTO>(product);
            productDTO.ScheduleDate = product.ScheduleDate.ToString("dd MMMM yyyy");

            ViewBag.UserId = User.Identity.GetUserId(); return View(productDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                Product product = db.Product.Find(productDTO.Id);
                product.ProductName = productDTO.ProductName;
                product.Description = productDTO.Description;
                product.Link = productDTO.Link;
                product.ScheduleDate = DateTime.Parse(productDTO.ScheduleDate);
                product.SetModifiedData(User.Identity.GetUserId());

                if (product.Picture != productDTO.Picture)
                {
                    product.Picture = string.IsNullOrWhiteSpace(productDTO.Picture) ? "" : "/Media/Product/" + productDTO.Picture;
                    var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Product");
                    Global.SaveBase64DataUrlFile(productDTO.Base64URL, productDTO.Picture, savePath);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = User.Identity.GetUserId(); return View(productDTO);
        }

        public ActionResult StartProduct(Guid id)
        {
            try
            {
                Product product = db.Product.Find(id);
                product.EventStatus = EventStatus.OnGoing;
                var lastPriorityOnGoingProduct = db.Product.Where(x => x.Priority != null).OrderByDescending(item => item.Priority).FirstOrDefault();

                if (lastPriorityOnGoingProduct == null)
                {
                    product.Priority = 1;
                }
                else
                {
                    product.Priority = lastPriorityOnGoingProduct.Priority + 1;
                }

                product.SetModifiedData(User.Identity.GetUserId());
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CompleteProduct(Guid id)
        {
            try
            {
                Product product = db.Product.Find(id);
                product.EventStatus = EventStatus.Finished;

                var onGoingProducts = db.Product.Where(x => x.Priority != null).OrderBy(item => item.Priority).ToList();
                for (int i = (int)product.Priority + 1; i <= onGoingProducts.Count(); i++)
                {
                    onGoingProducts.SingleOrDefault(x => x.Priority == i).Priority -= 1;
                }
                product.Priority = null;

                product.SetModifiedData(User.Identity.GetUserId());
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteProduct(Guid id)
        {
            try
            {
                Product product = db.Product.Find(id);
                product.SetIsActive(false, User.Identity.GetUserId());
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        public void UpdateRow(int fromPosition, int toPosition)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var productList = ctx.Product.ToList();
                ctx.Product.First(c => c.Priority == fromPosition).Priority = toPosition;
                ctx.Product.First(c => c.Priority == toPosition).Priority = fromPosition;
                ctx.SaveChanges();
            }
        }
    }
}
