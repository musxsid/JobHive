using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace JobHive.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IJobRepository Job { get; }
        IJobApplicationRepository JobApplication { get; }
        ICandidateProfileRepository CandidateProfile { get; }

        void Save();
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
