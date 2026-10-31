using JobHive.DataAccess.Repository.IRepository;
using JobHive.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace JobHive.Web.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    public class ApplicationsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Applicant/Applications/MyApplications
        public IActionResult MyApplications()
        {
            var email = HttpContext.Session.GetString(SD.Session_UserEmail) ?? "siddharth@example.com";
            var applications = _unitOfWork.JobApplication.GetApplicationsByApplicantEmail(email).ToList();

            // If no applications under session email, show all candidate applications for demonstration
            if (!applications.Any())
            {
                applications = _unitOfWork.JobApplication.GetAll(includeProperties: "Job,Job.Category").ToList();
            }

            return View(applications);
        }
    }
}

