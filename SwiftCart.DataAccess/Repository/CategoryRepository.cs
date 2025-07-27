using JobHive.DataAccess.Data;
using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobHive.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository 
    {
        private ApplicationDbContext _db;        
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

       

        public void Update(Category obj)
        {
            _db.JobCategory.Update(obj);
        }
    }
}
