using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http.Cors;
using System.Web.Http;
using System.Web.Http.Description;
using MusicStore.Models;
using MusicStore.Models.ViewModels;
using MusicStore.Repository;
using MusicStore.Service;
using System.Net.Http;
using System.Net.Http.Headers;
namespace MusicStore.WebAPI.Controllers
{
    [EnableCors(origins: "http://localhost:6225", headers: "*", methods: "*")]
    [AllowAnonymous]
    public class CartController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ICartService _cartService;
        private IAlbumService _albumService;

        public CartController(ICartService cartService, IAlbumService albumService)
        {
            _cartService = cartService;
            _albumService = albumService;
        }

        public CartController()
        {
            _cartService = new CartService(new UnitOfWork());
            _albumService = new AlbumService(new UnitOfWork(), null);
        }


        [HttpGet]
        [Route("api/Cart/{sessionId}")]
        public IHttpActionResult GetCartItems(string sessionId)
        {
            var cart = _cartService.GetCartService(sessionId);
            // Set up our ViewModel
            var shoppingcart = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = (decimal)cart.GetCartItems().Count()
            };

            // Return the view
            return Ok(shoppingcart);
        }


        [HttpPost]
        [Route("api/Cart/AddtoCart")]
        public IHttpActionResult AddtoCart(AddCartItem aci)
        {
            int AlbumId = aci.AlbumId;
            var addedAlbum = _albumService.ListAlbum().Single(album => album.AlbumId == AlbumId);

            if (User.Identity.IsAuthenticated)
            {
                aci.SessionId = User.Identity.Name;
            }
            else
            {
                if (aci.SessionId == null)
                {
                    aci.SessionId = System.Guid.NewGuid().ToString();
                }
             }

            ICartService cart = _cartService.GetCartService(aci.SessionId);
            cart.AddToCart(addedAlbum);
            return Ok(aci.SessionId);
        }


        [HttpDelete]
        [Route("api/Cart/{id}/{sessionId}")]
        public IHttpActionResult DeleteCart(int id, string sessionId)
        {
            var cart = _cartService.GetCartService(sessionId);
            // Get the name of the Album to display confirmation
            string AlbumName = _cartService.ListCartItems().Single(item => item.RecordId == id && item.CartId == sessionId).Album.Title;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = HttpUtility.HtmlEncode(AlbumName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cartService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return _cartService.ListCartItems().Count(e =>e.RecordId == id) > 0;
        }
    }
}