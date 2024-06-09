using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }    
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM orderVM = new()
            {
                orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View();
        }

        #region API calls
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            switch (status)
            {
                case "pending": orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusPending); break;
                case "inprocess": orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess); break;
                case "completed": orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped); break;
                case "approved": orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved); ; break;
                default: break;
            }

            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
