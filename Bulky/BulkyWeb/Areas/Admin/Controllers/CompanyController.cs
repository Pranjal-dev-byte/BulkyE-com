using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: CompanyController
        public ActionResult Index()
        {
            List<Company> companies = _unitOfWork.Companies.GetAll().ToList();
            return View(companies);
        }

        // GET: CompanyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompanyController/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            //Projections -:
            //IEnumerable<SelectListItem> CompanyList = _unitOfWork.Company.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString(),
            //});

           
            if (id == null || id == 0)
            {
                //Creates
                return View(new Company());

            }
            else
            {
                //Update
                Company company = _unitOfWork.Companies.Get(u => u.Id == id);
                return View(company);
            }
            //Viewbag -- Used to transfer dynamic data from controller to view
            //ViewBag.CompanyList = CompanyList;
            //ViewData["CompanyList"] = CompanyList;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company companyObj)
        {

            if (ModelState.IsValid)
            {

                if (companyObj.Id == 0)
                {
                    _unitOfWork.Companies.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Companies.Update(companyObj);

                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);

            }
        }

        #region API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> company = _unitOfWork.Companies.GetAll().ToList();
            return Json(new { data = company });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company? companyToBeDeleted = _unitOfWork.Companies.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Companies.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
