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
using Iruka.EF.Model;
using Iruka.Models;

namespace Iruka.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {
                var product = Mapper.Map<ProductDTO, Product>(productDto);
                var getProduct = db.Product.OrderByDescending(item => item.Priority).FirstOrDefault();
                product.Picture = "/Media/Product/" + product.Picture;

                if (getProduct==null)
                {
                    product.Priority = 1;
                }
                else
                {
                    product.Priority = getProduct.Priority + 1;
                }
                product.Id = Guid.NewGuid();
                product.isActive = true;

                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productDto);
        }

        // GET: Products/Edit/5
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
            return View(productDTO);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,ProductName,Description,Link,Picture,ScheduleDate,CreatedDate")] ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                Product productEdited = db.Product.Find(product.Id);
                var getPicture = product.Picture.Split('\\');
                productEdited.Picture = getPicture[getPicture.Length - 1];
                productEdited.Picture = "/Media/Product/" + productEdited.Picture;
                productEdited.ProductName = product.ProductName;
                productEdited.Description = product.Description;
                productEdited.Link = product.Link;
                //productEdited.ScheduleDate = product.ScheduleDate;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public void UpdateRow( int fromPosition, int toPosition)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var productList = ctx.Product.ToList();
                ctx.Product.First(c => c.Priority == fromPosition).Priority = toPosition;
                ctx.Product.First(c => c.Priority == toPosition).Priority = fromPosition;
                ctx.SaveChanges();
            }
        }

        #region DELETE
        public ActionResult DeleteProduct(Guid id)
        {
            try
            {
                Product product = db.Product.Find(id);
                db.Product.Remove(product);
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Upload
        [HttpPost]
        public ActionResult SavePicture(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var path = Save(files);
            return Json(new { success = path }, JsonRequestBehavior.AllowGet);
        }
        public static string Save(IEnumerable<HttpPostedFileBase> files)
        {
            var tempPhysicalPath = "";
            var fullPath = "";
            foreach (var file in files)
            {

                var fileName = Path.GetFileName(file.FileName);
                var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Product");
                var physicalPath = Path.Combine(filePath, fileName);
                file.SaveAs(physicalPath);
                tempPhysicalPath = fileName;
                fullPath = physicalPath;
            }

            Image image = ResizeImage(fullPath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                image.Save(fullPath);
            }

            return tempPhysicalPath;
        }

        public ActionResult RemovePicture(string[] fileNames)
        {
            RemovePicture(fileNames);
            return Json(new { error = "Insert Error : Inserted data not valid" }, JsonRequestBehavior.AllowGet);
        }

        public static Image ResizeImage(string pathName)
        {

            Bitmap original, resizedImage;


            using (FileStream fs = new System.IO.FileStream(pathName, System.IO.FileMode.Open))
            {
                original = new Bitmap(fs);
            }

            int rectHeight = 200;
            int rectWidth = 200;

            //if the image is squared set it's height and width to the smallest of the desired dimensions (our box). In the current example rectHeight<rectWidth
            if (original.Height == original.Width)
            {
                resizedImage = new Bitmap(original, rectHeight, rectHeight);
            }
            else
            {
                //calculate aspect ratio
                float aspect = original.Width / (float)original.Height;
                int newWidth, newHeight;

                //calculate new dimensions based on aspect ratio
                newWidth = (int)(rectWidth * aspect);
                newHeight = (int)(newWidth / aspect);

                //if one of the two dimensions exceed the box dimensions
                if (newWidth > rectWidth || newHeight > rectHeight)
                {
                    //depending on which of the two exceeds the box dimensions set it as the box dimension and calculate the other one based on the aspect ratio
                    if (newWidth > newHeight)
                    {
                        newWidth = rectWidth;
                        newHeight = (int)(newWidth / aspect);
                    }
                    else
                    {
                        newHeight = rectHeight;
                        newWidth = (int)(newHeight * aspect);
                    }
                }
                resizedImage = new Bitmap(original, newWidth, newHeight);
            }
            return resizedImage;

        }
        #endregion
    }
}
