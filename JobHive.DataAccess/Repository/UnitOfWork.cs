using JobHive.DataAccess.Data;
using JobHive.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace JobHive.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private IDbContextTransaction? _transaction;

        public ICategoryRepository Category { get; private set; }
        public IJobRepository Job { get; private set; }
        public IJobApplicationRepository JobApplication { get; private set; }
        public ICandidateProfileRepository CandidateProfile { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Job = new JobRepository(_db);
            JobApplication = new JobApplicationRepository(_db);
            CandidateProfile = new CandidateProfileRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _db.Database.BeginTransaction();
            return _transaction;
        }

        public void CommitTransaction()
        {
            try
            {
                _db.SaveChanges();
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _db.Dispose();
        }
    }
}
