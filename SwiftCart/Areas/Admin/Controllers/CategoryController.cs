using Microsoft.AspNetCore.Mvc;
using JobHive.DataAccess.Data;
using JobHive.DataAccess.Repository.IRepository;
using JobHive.Models;

namespace SwiftCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;    
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
       
        public IActionResult Create (){
            return View();
        }
        [HttpPost] //Http POST 
        public IActionResult Create(Category obj)
        {
            
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot be exactly same as Name.");
            }

            if (ModelState.IsValid) {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Created Sucessfully";
                return RedirectToAction("Index");
            }
                 return View();
            
        }
        //HTTP GET Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _unitOfWork.Category.Get(u=>u.Id==id);
            //Category? CategoryFromDb1 = _db.JobCategory.FirstOrDefault(u=>u.Id==id);
            //Category? CategoryFromDb2= _db.JobCategory.Where(u=>u.Id==id).FirstOrDefault();

            if (CategoryFromDb == null) {
                return NotFound();
            }
                return View(CategoryFromDb);
        }

        [HttpPost] //HTTP POST Edit
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid) {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Sucessfully";
                return RedirectToAction("Index");
            }
            
            return View();
        }

        //HTTP GET Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? CategoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (CategoryFromDb == null) { 
                return NotFound();
            }
            return View(CategoryFromDb);
        
        }
        [HttpPost,ActionName("Delete")] //HTTP POST Delete
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"]= "Category Deleted Sucessfully";
            return RedirectToAction("Index");
        
        }
        
    }


}
