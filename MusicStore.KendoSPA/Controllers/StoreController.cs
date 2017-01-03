using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.KendoSPA.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlbumsByGenre()
        {
            return View();
        }

        public ActionResult AlbumDetail()
        {

            return View();
        }

        public ActionResult Catalog()
        {
            return View();
        }
    }
}