using JobHive.Models;
using System.Collections.Generic;

namespace JobHive.DataAccess.Repository.IRepository
{
    public interface IJobRepository : IRepository<Job>
    {
        void Update(Job obj);
        JobSearchFilterVM SearchJobs(JobSearchFilterVM filter);
        IEnumerable<Job> GetFeaturedJobs(int count = 6);
    }
}

