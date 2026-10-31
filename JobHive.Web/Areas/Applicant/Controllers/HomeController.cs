using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using JobHive.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace JobHive.Web.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUnitOfWork unitOfWork, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index(string? category)
        {
            var categories = _unitOfWork.Category.GetAll().OrderBy(c => c.DisplayOrder).ToList();
            var allJobs = _unitOfWork.Job.GetAll(j => j.IsActive, includeProperties: "Category").ToList();

            // Background pastel colors for category cards matching the screenshot
            var bgClasses = new[] { "bg-primary-subtle", "bg-warning-subtle", "bg-info-subtle", "bg-success-subtle", "bg-danger-subtle", "bg-secondary-subtle" };
            var textClasses = new[] { "text-primary", "text-warning", "text-info", "text-success", "text-danger", "text-secondary" };

            var categoryCards = categories.Select((c, idx) => new CategoryCardVM
            {
                Id = c.Id,
                Name = c.Name,
                Icon = !string.IsNullOrEmpty(c.Icon) ? c.Icon : "bi-briefcase",
                Description = c.Description,
                JobCount = allJobs.Count(j => j.CategoryId == c.Id),
                BackgroundColorClass = bgClasses[idx % bgClasses.Length],
                IconColorClass = textClasses[idx % textClasses.Length]
            }).ToList();

            var featuredJobs = string.IsNullOrWhiteSpace(category)
                ? allJobs.Take(6).ToList()
                : allJobs.Where(j => j.Category != null && j.Category.Name.Equals(category, StringComparison.OrdinalIgnoreCase)).Take(6).ToList();

            var viewModel = new HomeViewModel
            {
                PopularCategories = categoryCards,
                FeaturedJobs = featuredJobs,
                TotalActiveJobs = allJobs.Count,
                TotalCompanies = allJobs.Select(j => j.CompanyName).Distinct().Count(),
                TotalApplicants = _unitOfWork.JobApplication.GetAll().Count(),
                SelectedCategory = category
            };

            return View(viewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SwitchRole(string role)
        {
            if (role == SD.Role_Admin || role == SD.Role_Employer || role == SD.Role_Applicant)
            {
                HttpContext.Session.SetString(SD.Session_Role, role);
                TempData["success"] = $"Switched view to {role} mode.";

                if (role == SD.Role_Employer)
                {
                    return RedirectToAction("Index", "Job", new { area = "Employer" });
                }
                else if (role == SD.Role_Admin)
                {
                    return RedirectToAction("Index", "Category", new { area = "Admin" });
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
