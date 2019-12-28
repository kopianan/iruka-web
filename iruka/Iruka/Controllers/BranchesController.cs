using AutoMapper;
using Iruka.EF.Model;
using Iruka.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace Iruka.Controllers
{
    public class BranchesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult PointRateSetup()
        {
            var targetBranch = db.Branches.ToList().FirstOrDefault();
            var branchDto = Mapper.Map<Branch, BranchDto>(targetBranch);

            ViewBag.UserId = User.Identity.GetUserId(); return View(branchDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PointRateSetup(BranchDto branchDto)
        {
            var targetBranch = db.Branches.ToList().FirstOrDefault();
            targetBranch.PointRate = branchDto.PointRate;
            targetBranch.SetModifiedData(User.Identity.GetUserId());
            db.SaveChanges();

            ViewBag.UserId = User.Identity.GetUserId(); return RedirectToAction("PointRateSetup");
        }

        public ActionResult ResetPoint()
        {
            return View();
        }
    }
}