using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using JobHive.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace JobHive.Web.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    public class JobsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JobsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Applicant/Jobs (Multi-parameter search & filtering)
        public IActionResult Index(JobSearchFilterVM filter)
        {
            var result = _unitOfWork.Job.SearchJobs(filter);
            return View(result);
        }

        // GET: /Applicant/Jobs/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var job = _unitOfWork.Job.Get(u => u.Id == id, includeProperties: "Category");
            if (job == null)
            {
                return NotFound();
            }

            // Get related jobs in the same category
            ViewBag.RelatedJobs = _unitOfWork.Job
                .GetAll(j => j.CategoryId == job.CategoryId && j.Id != job.Id && j.IsActive, includeProperties: "Category")
                .Take(3)
                .ToList();

            return View(job);
        }

        // GET: /Applicant/Jobs/Apply/5
        public IActionResult Apply(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var job = _unitOfWork.Job.Get(u => u.Id == id, includeProperties: "Category");
            if (job == null)
            {
                return NotFound();
            }

            var currentEmail = HttpContext.Session.GetString(SD.Session_UserEmail) ?? "siddharth@example.com";
            var currentProfile = _unitOfWork.CandidateProfile.GetByEmail(currentEmail);

            var application = new JobApplication
            {
                JobId = job.Id,
                Job = job,
                ApplicantName = currentProfile?.FullName ?? "Siddharth Sharma",
                ApplicantEmail = currentProfile?.Email ?? currentEmail,
                ApplicantPhone = currentProfile?.Phone ?? "+91 98765 43210",
                ResumeFileName = currentProfile?.ResumeFileName,
                ResumePath = currentProfile?.ResumePath
            };

            return View(application);
        }

        // POST: /Applicant/Jobs/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(JobApplication application, IFormFile? resumeFile)
        {
            // Remove navigation property validation error
            ModelState.Remove("Job");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                // Enforce transactional data integrity using EF Core transaction
                using var transaction = _unitOfWork.BeginTransaction();
                try
                {
                    // Handle file upload if provided
                    if (resumeFile != null && resumeFile.Length > 0)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        string uploadsFolder = Path.Combine(wwwRootPath, "uploads", "resumes");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string fileExtension = Path.GetExtension(resumeFile.FileName).ToLowerInvariant();
                        string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(resumeFile.FileName)}";
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            resumeFile.CopyTo(fileStream);
                        }

                        application.ResumeFileName = resumeFile.FileName;
                        application.ResumePath = $"/uploads/resumes/{uniqueFileName}";
                    }

                    application.AppliedDate = DateTime.UtcNow;
                    application.Status = SD.Status_Applied;

                    _unitOfWork.JobApplication.Add(application);
                    _unitOfWork.CommitTransaction();

                    // Store candidate session email for subsequent tracking
                    HttpContext.Session.SetString(SD.Session_UserEmail, application.ApplicantEmail);
                    HttpContext.Session.SetString(SD.Session_UserName, application.ApplicantName);

                    TempData["success"] = "Application submitted successfully! You can track its status in 'My Applications'.";
                    return RedirectToAction(nameof(ApplySuccess), new { id = application.Id });
                }
                catch (Exception ex)
                {
                    _unitOfWork.RollbackTransaction();
                    ModelState.AddModelError("", $"Failed to submit application due to transactional error: {ex.Message}");
                }
            }

            // Reload job details on validation failure
            application.Job = _unitOfWork.Job.Get(u => u.Id == application.JobId, includeProperties: "Category");
            return View(application);
        }

        // GET: /Applicant/Jobs/ApplySuccess/5
        public IActionResult ApplySuccess(int id)
        {
            var application = _unitOfWork.JobApplication.Get(a => a.Id == id, includeProperties: "Job,Job.Category");
            if (application == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(application);
        }
    }
}

