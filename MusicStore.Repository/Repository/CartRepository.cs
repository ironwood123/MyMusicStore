using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
namespace MusicStore.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Cart> GetCartItems()
        {
            return base.Get(null, null, "album");
        }

        public void AddToCart(Cart cart)
        {
            base.Insert(cart);
        }

        public void UpdateCart(Cart cart)
        {
            base.Update(cart);
        }

        public void DeleteCart(int recordId)
        {
            base.Delete(recordId);
        }
    }
}