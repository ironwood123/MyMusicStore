using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http.Cors;
using System.Web.Http;
using MusicStore.Models;
using MusicStore.Repository;
using MusicStore.Service;
namespace MusicStore.WebAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "http://localhost:6225", headers: "*", methods: "*")]
    public class CheckOutController : ApiController
    {
     //   private ApplicationDbContext db = new ApplicationDbContext();
        const string PromoCode = "FREE";
        IOrderService _orderService;
        IOrderDetailService _orderdetailService;
        ICartService _cardService;

        public CheckOutController()
        {
            _orderService = new OrderService(new UnitOfWork());
            _orderdetailService = new OrderDetailService(new UnitOfWork());
            _cardService = new CartService(new UnitOfWork());
        }
        public CheckOutController(IOrderService orderService, IOrderDetailService orderdetailService, ICartService cartService)
        {
            _orderService = orderService;
            _orderdetailService = orderdetailService;
            _cardService = cartService;
        }
        public IHttpActionResult  AddressAndPayment()
        {
            return Ok();
        }
        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        [Route("api/CheckOut")]
        public IHttpActionResult  AddressAndPayment(System.Net.Http.Formatting.FormDataCollection values)
        {
            var order = new Order();
            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return Ok("Wrong PromoCode");
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    order.Address = values["Address"];
                    order.FirstName = values["FirstName"];
                    order.LastName = values["LastName"];
                    order.City = values["City"];
                    order.State = values["State"];
                    order.Country = values["Country"];
                    order.PostalCode = values["PostalCode"];
                    order.Phone = values["Phone"];
                    order.Email = values["Email"];
                    _orderService.AddOrder(order);

                    //Process the order
                    var cid = CreateOrder(order, values["SessionId"]);
                    return  Ok(cid);

                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return Ok(order);
            }
        }
        private int CreateOrder(Order order,string sessionid)
        {
            decimal orderTotal = 0;
            var cart = _cardService.GetCartService(sessionid);
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

        public IHttpActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = _orderService.ListOrder().Any(order => order.OrderId == id && order.Username == User.Identity.Name);

            if (isValid)
            {
                return Ok(id);
            }
            else
            {
                return Ok("Error");
            }
        }

    }
}