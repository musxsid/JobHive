using JobHive.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JobHive.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll().ToList();
            var jobs = _unitOfWork.Job.GetAll(includeProperties: "Category,Applications").ToList();
            var applications = _unitOfWork.JobApplication.GetAll(includeProperties: "Job").ToList();

            ViewBag.TotalCategories = categories.Count;
            ViewBag.TotalJobs = jobs.Count;
            ViewBag.ActiveJobs = jobs.Count(j => j.IsActive);
            ViewBag.TotalApplications = applications.Count;
            ViewBag.PlacedCandidates = applications.Count(a => a.Status == "Offered");
            ViewBag.RecentApplications = applications.OrderByDescending(a => a.AppliedDate).Take(6).ToList();

            return View(jobs);
        }
    }
}

