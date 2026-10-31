using JobHive.Models;

namespace JobHive.DataAccess.Repository.IRepository
{
    public interface ICandidateProfileRepository : IRepository<CandidateProfile>
    {
        void Update(CandidateProfile obj);
        CandidateProfile? GetByEmail(string email);
    }
}
