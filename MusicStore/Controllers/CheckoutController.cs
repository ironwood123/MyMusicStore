using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;
using MusicStore.Models.ViewModels;
using MusicStore.Repository;
using MusicStore.Service;
namespace MusicStore.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {
        const string PromoCode = "FREE";
        IOrderService _orderService;
        IOrderDetailService _orderdetailService;
        ICartService _cardService;
        public CheckoutController()
        {
            _orderService = new OrderService(new UnitOfWork());
            _orderdetailService = new OrderDetailService(new UnitOfWork());
            _cardService = new CartService(new UnitOfWork());
        }
        public CheckoutController(IOrderService orderService, IOrderDetailService orderdetailService, ICartService cartService)
        {
            _orderService = orderService;
            _orderdetailService = orderdetailService;
            _cardService = cartService;
        }
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }
        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    _orderService.AddOrder(order);

                    //Process the order
                    var cid = CreateOrder(order);
                    return RedirectToAction("Complete",
                        new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        private int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cart = _cardService.GetCartService(this.HttpContext);
            var cartItems = cart.GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);
                _orderdetailService.AddOrderDetail(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            _orderService.UpdateOrder(order);
            // Save the order

            // Empty the shopping cart
            cart.EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = _orderService.ListOrder().Any(order => order.OrderId == id && order.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}