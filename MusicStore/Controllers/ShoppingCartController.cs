using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models.ViewModels;
using MusicStore.Models;
using MusicStore.Repository;
using MusicStore.Service;
namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
       // ApplicationDbContext storeDB = new ApplicationDbContext();
        ICartService _cartService;
        IAlbumService _albumService;
        public ShoppingCartController(ICartService cartService, IAlbumService albumService)
        {
            _cartService = cartService;
            _albumService = albumService;
        }

        public ShoppingCartController()
        {
            _cartService = new CartService(new UnitOfWork());
            _albumService = new AlbumService(new UnitOfWork(), null);
        }
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = _cartService.GetCartService(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the Album from the database
            var addedAlbum = _albumService.ListAlbum().Single(album => album.AlbumId == id);
            // Add it to the shopping cart
            var cart = _cartService.GetCartService(this.HttpContext);
            cart.AddToCart(addedAlbum);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = _cartService.GetCartService(this.HttpContext);
            // Get the name of the Album to display confirmation
            string AlbumName = _cartService.ListCartItems().Single(item => item.RecordId == id).Album.Title;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(AlbumName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = _cartService.GetCartService(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}