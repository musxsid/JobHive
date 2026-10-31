using JobHive.Models;
using System.Collections.Generic;

namespace JobHive.DataAccess.Repository.IRepository
{
    public interface IJobApplicationRepository : IRepository<JobApplication>
    {
        void Update(JobApplication obj);
        void UpdateStatus(int applicationId, string status, string? notes = null);
        IEnumerable<JobApplication> GetApplicationsByJob(int jobId);
        IEnumerable<JobApplication> GetApplicationsByApplicantEmail(string email);
    }
}
