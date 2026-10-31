using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using JobHive.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace JobHive.Web.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /Employer/Job
        public IActionResult Index()
        {
            var jobs = _unitOfWork.Job.GetAll(includeProperties: "Category,Applications").OrderByDescending(j => j.PostedDate).ToList();
            return View(jobs);
        }

        // GET: /Employer/Job/Create
        public IActionResult Create()
        {
            PopulateCategoriesDropdown();
            var job = new Job
            {
                PostedDate = DateTime.UtcNow,
                IsActive = true,
                SalaryMin = 800000,
                SalaryMax = 1500000,
                Location = "Bangalore, India",
                JobType = SD.JobType_FullTime,
                ExperienceLevel = SD.Exp_Mid
            };
            return View(job);
        }

        // POST: /Employer/Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Job job)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Applications");

            if (ModelState.IsValid)
            {
                job.PostedDate = DateTime.UtcNow;
                _unitOfWork.Job.Add(job);
                _unitOfWork.Save();

                TempData["success"] = "Vacancy posted successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateCategoriesDropdown(job.CategoryId);
            return View(job);
        }

        // GET: /Employer/Job/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var job = _unitOfWork.Job.Get(u => u.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            PopulateCategoriesDropdown(job.CategoryId);
            return View(job);
        }

        // POST: /Employer/Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Job job)
        {
            ModelState.Remove("Category");
            ModelState.Remove("Applications");

            if (ModelState.IsValid)
            {
                _unitOfWork.Job.Update(job);
                _unitOfWork.Save();

                TempData["success"] = "Job specifications updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            PopulateCategoriesDropdown(job.CategoryId);
            return View(job);
        }

        // POST: /Employer/Job/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var job = _unitOfWork.Job.Get(u => u.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            _unitOfWork.Job.Remove(job);
            _unitOfWork.Save();

            TempData["success"] = "Job listing removed successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Employer/Job/Applicants/5 (Review applicants & track recruitment progression)
        public IActionResult Applicants(int? jobId)
        {
            if (jobId == null || jobId == 0)
            {
                return NotFound();
            }

            var job = _unitOfWork.Job.Get(u => u.Id == jobId, includeProperties: "Category");
            if (job == null)
            {
                return NotFound();
            }

            var applications = _unitOfWork.JobApplication.GetApplicationsByJob(job.Id).ToList();

            ViewBag.Job = job;
            return View(applications);
        }

        // POST: /Employer/Job/UpdateApplicationStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateApplicationStatus(int applicationId, string status, string? notes, int jobId)
        {
            _unitOfWork.JobApplication.UpdateStatus(applicationId, status, notes);
            _unitOfWork.Save();

            TempData["success"] = $"Applicant stage updated to '{status}'.";
            return RedirectToAction(nameof(Applicants), new { jobId = jobId });
        }

        private void PopulateCategoriesDropdown(object? selectedCategory = null)
        {
            var categories = _unitOfWork.Category.GetAll().OrderBy(c => c.DisplayOrder);
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name", selectedCategory);
        }
    }
}
