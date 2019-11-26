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
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.UserId = User.Identity.GetUserId(); return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EventDTO eventDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var @event = Mapper.Map<EventDTO, Event>(eventDto);
                @event.Picture = string.IsNullOrWhiteSpace(eventDto.Picture) ? null : "/Media/Event/" + eventDto.Picture;
                @event.NewCreatedData(userId);

                db.Event.Add(@event);
                var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Event");
                Global.SaveBase64DataUrlFile(eventDto.Base64URL, eventDto.Picture, savePath);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = User.Identity.GetUserId(); return View(eventDto);
        }

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Event.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            var eventDto = Mapper.Map<Event, EventDTO>(@event);
            eventDto.ScheduleDate = Global.DateToString(@event.ScheduleDate);

            ViewBag.UserId = User.Identity.GetUserId(); return View(eventDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventDTO eventDTO)
        {
            if (ModelState.IsValid)
            {
                Event @event = db.Event.Find(eventDTO.Id);
                @event.EventName = eventDTO.EventName;
                @event.Link = eventDTO.Link;
                @event.Description = eventDTO.Description;
                @event.ScheduleDate = DateTime.Parse(eventDTO.ScheduleDate);
                @event.SetModifiedData(User.Identity.GetUserId());
                if (@event.Picture != eventDTO.Picture)
                {
                    @event.Picture = string.IsNullOrWhiteSpace(eventDTO.Picture) ? "" : "/Media/Event/" + eventDTO.Picture;
                    var savePath = System.Web.HttpContext.Current.Server.MapPath("~/Media/Event");
                    Global.SaveBase64DataUrlFile(eventDTO.Base64URL, eventDTO.Picture, savePath);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = User.Identity.GetUserId(); return View(eventDTO);
        }

        public ActionResult StartEvent(Guid id)
        {
            try
            {
                Event @event = db.Event.Find(id);
                @event.EventStatus = EventStatus.OnGoing;
                var lastPriorityOnGoingEvent = db.Event.Where(x => x.Priority != null).OrderByDescending(item => item.Priority).FirstOrDefault();

                if (lastPriorityOnGoingEvent == null)
                {
                    @event.Priority = 1;
                }
                else
                {
                    @event.Priority = lastPriorityOnGoingEvent.Priority + 1;
                }

                @event.SetModifiedData(User.Identity.GetUserId());
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CompleteEvent(Guid id)
        {
            try
            {
                Event @event = db.Event.Find(id);
                @event.EventStatus = EventStatus.Finished;

                var onGoingEvents = db.Event.Where(x => x.Priority != null).OrderBy(item => item.Priority).ToList();
                for (int i = (int)@event.Priority + 1; i <= onGoingEvents.Count(); i++)
                {
                    onGoingEvents.SingleOrDefault(x => x.Priority == i).Priority -= 1;
                }
                @event.Priority = null;

                @event.SetModifiedData(User.Identity.GetUserId());
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteEvent(Guid id)
        {
            try
            {
                Event @event = db.Event.Find(id);
                @event.SetIsActive(false, User.Identity.GetUserId());
                db.SaveChanges();
                return Json(new { success = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { error = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

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
