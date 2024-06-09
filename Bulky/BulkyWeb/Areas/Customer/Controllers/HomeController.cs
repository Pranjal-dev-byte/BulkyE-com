using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Products.GetAll(includeProperties: "Category") ;
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart Cart = new()
            {
                Product = _unitOfWork.Products.Get(u => u.Id == id, includeProperties: "Category"),
                Count = 1,
                ProductId = id
            };
            return View(Cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart cart) {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.ApplicationUserId = userId;
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId == userId && u.ProductId == cart.ProductId);
            if (cartFromDb!=null)
            {
                cartFromDb.Count += cart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                cart.Id = 0;
                _unitOfWork.ShoppingCart.Add(cart);
            }
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";   
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
