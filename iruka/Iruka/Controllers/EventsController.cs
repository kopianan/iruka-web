﻿using System;
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
using Microsoft.AspNet.Identity;

namespace Iruka.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: eventss
        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        // GET: eventss/Create
        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        // POST: eventss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(EventDTO eventDto)
        {
            if (ModelState.IsValid)
            {
                var events = Mapper.Map<EventDTO, Event>(eventDto);

                var getevents = db.Event.OrderByDescending(item => item.Priority).FirstOrDefault();
                events.Picture = "/Media/Event/" + events.Picture;

                if (getevents == null)
                {
                    events.Priority = 1;
                }
                else
                {
                    events.Priority = getevents.Priority + 1;
                }
                events.Id = Guid.NewGuid();
                events.isActive = true;

                db.Event.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = User.Identity.GetUserId(); return View(eventDto);
        }

        // GET: eventss/Edit/5
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event events = db.Event.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            var eventsDTO = Mapper.Map<Event, EventDTO>(events);

            ViewBag.UserId = User.Identity.GetUserId(); return View(eventsDTO);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,EventName,Description,Link,Picture,ScheduleDate,CreatedDate")] EventDTO events)
        {
            if (ModelState.IsValid)
            {
                Event eventsEdited = db.Event.Find(events.Id);
                var getPicture = events.Picture.Split('\\');
                eventsEdited.Picture = getPicture[getPicture.Length - 1];
                eventsEdited.Picture = "/Media/Event/" + eventsEdited.Picture;
                eventsEdited.EventName = events.EventName;
                eventsEdited.Description = events.Description;
                eventsEdited.Link = events.Link;
                eventsEdited.ScheduleDate = events.ScheduleDate;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = User.Identity.GetUserId(); return View(events);
        }

        public void UpdateRow(int fromPosition, int toPosition)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var eventList = ctx.Event.ToList();
                ctx.Event.First(c => c.Priority == fromPosition).Priority = toPosition;
                //ctx.Event.First(c => c.Priority == toPosition).Priority = fromPosition;
                ctx.SaveChanges();
            }
        }

        #region DELETE
        public ActionResult DeleteEvent(Guid id)
        {
            try
            {
                Event events = db.Event.Find(id);
                db.Event.Remove(events);
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
                var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Event");
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
