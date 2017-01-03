using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.KendoSPA.Controllers
{
    public class CartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShoppingCart()
        {
        
            return View();
        }

        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult Complete()
        {
            return View();
        }
    }
}