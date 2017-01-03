using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
using MusicStore.Repository;
namespace MusicStore.Service
{
    public interface ICartService
    {
        IEnumerable<Cart> ListCartItems();
        void AddToCart(Cart cart);
        void UpdateCart(Cart cart);
        void DeleteCart(int recordId);
        ICartService GetCartService(HttpContextBase context);
        ICartService GetCartService(System.Web.Mvc.Controller controller);
        ICartService GetCartService(string sessionId);
        void AddToCart(Album Album);
        int RemoveFromCart(int id);
        void EmptyCart();
        List<Cart> GetCartItems();
        int GetCount();
        decimal GetTotal();
        string GetCartId(HttpContextBase context);
        void MigrateCart(string userName);
        void Dispose();
    }
}
