using JobHive.DataAccess.Data;
using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace JobHive.DataAccess.Repository
{
    public class JobApplicationRepository : Repository<JobApplication>, IJobApplicationRepository
    {
        private readonly ApplicationDbContext _db;

        public JobApplicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(JobApplication obj)
        {
            _db.JobApplications.Update(obj);
        }

        public void UpdateStatus(int applicationId, string status, string? notes = null)
        {
            var appFromDb = _db.JobApplications.FirstOrDefault(a => a.Id == applicationId);
            if (appFromDb != null)
            {
                appFromDb.Status = status;
                if (!string.IsNullOrWhiteSpace(notes))
                {
                    appFromDb.RecruiterNotes = notes;
                }
            }
        }

        public IEnumerable<JobApplication> GetApplicationsByJob(int jobId)
        {
            return _db.JobApplications
                .Include(a => a.Job)
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.AppliedDate)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<JobApplication> GetApplicationsByApplicantEmail(string email)
        {
            return _db.JobApplications
                .Include(a => a.Job)
                    .ThenInclude(j => j!.Category)
                .Where(a => a.ApplicantEmail == email)
                .OrderByDescending(a => a.AppliedDate)
                .AsNoTracking()
                .ToList();
        }
    }
}
