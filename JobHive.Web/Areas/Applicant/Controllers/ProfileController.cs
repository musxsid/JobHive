using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using JobHive.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace JobHive.Web.Areas.Applicant.Controllers
{
    [Area("Applicant")]
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Applicant/Profile/Resume
        public IActionResult Resume()
        {
            var email = HttpContext.Session.GetString(SD.Session_UserEmail) ?? "siddharth@example.com";
            var profile = _unitOfWork.CandidateProfile.GetByEmail(email);

            if (profile == null)
            {
                profile = new CandidateProfile
                {
                    FullName = "Siddharth Sharma",
                    Email = email,
                    Phone = "+91 98765 43210",
                    Headline = "Software Developer (.NET / C#)",
                    Location = "Bangalore, India",
                    Skills = "C#, ASP.NET Core MVC, Entity Framework Core, SQL Server, HTML5, Bootstrap 5",
                    ExperienceYears = 2,
                    Bio = "Developer with hands-on internship experience at CodeArrest Softwares, building scalable backend services and compound queries."
                };
            }

            return View(profile);
        }

        // POST: /Applicant/Profile/Resume
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Resume(CandidateProfile profile, IFormFile? resumeFile)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (resumeFile != null && resumeFile.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string uploadsFolder = Path.Combine(wwwRootPath, "uploads", "resumes");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(resumeFile.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        resumeFile.CopyTo(fileStream);
                    }

                    profile.ResumeFileName = resumeFile.FileName;
                    profile.ResumePath = $"/uploads/resumes/{uniqueFileName}";
                }

                if (profile.Id == 0)
                {
                    _unitOfWork.CandidateProfile.Add(profile);
                }
                else
                {
                    _unitOfWork.CandidateProfile.Update(profile);
                }

                _unitOfWork.Save();

                HttpContext.Session.SetString(SD.Session_UserEmail, profile.Email);
                HttpContext.Session.SetString(SD.Session_UserName, profile.FullName);

                TempData["success"] = "Candidate profile and resume updated successfully!";
                return RedirectToAction(nameof(Resume));
            }

            return View(profile);
        }
    }
}
