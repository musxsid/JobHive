using JobHive.Models;
using JobHive.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobHive.DataAccess.Repository.IRepository
{
public interface ICategoryRepository : IRepository<Models.Category>
    {
        void Update(Category obj);
    }
}
