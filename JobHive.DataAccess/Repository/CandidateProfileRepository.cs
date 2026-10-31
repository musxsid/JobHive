using JobHive.DataAccess.Data;
using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using System;
using System.Linq;

namespace JobHive.DataAccess.Repository
{
    public class CandidateProfileRepository : Repository<CandidateProfile>, ICandidateProfileRepository
    {
        private readonly ApplicationDbContext _db;

        public CandidateProfileRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CandidateProfile obj)
        {
            var profileFromDb = _db.CandidateProfiles.FirstOrDefault(p => p.Id == obj.Id);
            if (profileFromDb != null)
            {
                profileFromDb.FullName = obj.FullName;
                profileFromDb.Email = obj.Email;
                profileFromDb.Phone = obj.Phone;
                profileFromDb.Headline = obj.Headline;
                profileFromDb.Location = obj.Location;
                profileFromDb.Skills = obj.Skills;
                profileFromDb.ExperienceYears = obj.ExperienceYears;
                profileFromDb.Bio = obj.Bio;
                if (!string.IsNullOrEmpty(obj.ResumeFileName))
                {
                    profileFromDb.ResumeFileName = obj.ResumeFileName;
                    profileFromDb.ResumePath = obj.ResumePath;
                }
                profileFromDb.UpdatedAt = DateTime.UtcNow;
            }
        }

        public CandidateProfile? GetByEmail(string email)
        {
            return _db.CandidateProfiles.FirstOrDefault(p => p.Email == email);
        }
    }
}

