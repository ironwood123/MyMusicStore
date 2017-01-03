using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
using MusicStore.Repository;
using System.Web.Http;
namespace MusicStore.Service
{
    public class CartService : ICartService
    {
        protected IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Cart> ListCartItems()
        {
            return _unitOfWork.CartRepository.GetCartItems();
        }
        public void AddToCart(Cart cart)
        {
            _unitOfWork.CartRepository.AddToCart(cart);
            _unitOfWork.Save();
        }

        public void UpdateCart(Cart cart)
        {
            _unitOfWork.CartRepository.UpdateCart(cart);
            _unitOfWork.Save();
        }

        public void DeleteCart(int recordId)
        {
            _unitOfWork.CartRepository.DeleteCart(recordId);
            _unitOfWork.Save();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public  ICartService GetCartService(HttpContextBase context)
        {
            var cart = new CartService(_unitOfWork);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public ICartService GetCartService(string sessionId)
        {
            var cart = new CartService(_unitOfWork);
            cart.ShoppingCartId = sessionId;
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public  ICartService GetCartService(System.Web.Mvc.Controller controller)
        {
            return GetCartService(controller.HttpContext);
        }

        public void AddToCart(Album Album)
        {
            var cartItem = ListCartItems().SingleOrDefault(c => c.CartId == ShoppingCartId && c.AlbumId == Album.AlbumId);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    AlbumId = Album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
               AddToCart(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
               UpdateCart(cartItem);
            }

        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = ListCartItems().Single(cart => cart.CartId == ShoppingCartId && cart.RecordId == id);
            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                    UpdateCart(cartItem);
                }
                else
                {
                    DeleteCart(cartItem.RecordId);
                }
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = ListCartItems().Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                DeleteCart(cartItem.RecordId);
            }
        }
        public List<Cart> GetCartItems()
        {
            return ListCartItems().Where(cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in ListCartItems().Where(cart => cart.CartId == ShoppingCartId) select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply Album price by count of that Album to get 
            // the current price for each of those Albums in the cart
            // sum all Album price totals to get the cart total
            decimal? total = (from cartItems in ListCartItems().Where(cart => cart.CartId == ShoppingCartId) select (int?)cartItems.Count * cartItems.Album.Price).Sum();

            return total ?? decimal.Zero;
        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    var tempCartId = Guid.NewGuid().ToString();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId;
                }
            }
            return context.Session[CartSessionKey].ToString();

        }


        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = ListCartItems().Where(cart => cart.CartId == ShoppingCartId);
            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
                UpdateCart(item);
            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    }
}