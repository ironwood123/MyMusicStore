using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Models;
namespace MusicStore.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail> , IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void InsertOrderDetail(OrderDetail od)
        {
            base.Insert(od);
        }
        public void UpdateOrderDetail(OrderDetail orderdetail)
        {
            base.Update(orderdetail);
        }
    }
}