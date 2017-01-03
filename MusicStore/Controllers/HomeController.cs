using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;
namespace MusicStore.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        public ActionResult Index()
        {
            var albums = GetTopSellingAlbums(5);

            return View(albums);

            // return "Hello from Home";
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your app description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}
