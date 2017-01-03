using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
namespace MusicStore.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<Order> ListOrder()
        {
            return base.Get();
        }
        public void InsertOrder(Order order)
        {
            base.Insert(order);
        }
        public void UpdateOrder(Order order)
        {
            base.Update(order);
        }
    }
}