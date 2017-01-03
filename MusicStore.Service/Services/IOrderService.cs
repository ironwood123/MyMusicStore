using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Repository;
using MusicStore.Models;
namespace MusicStore.Service
{
    public interface IOrderService
    {
        IEnumerable<Order> ListOrder();
        void AddOrder(Order order);
        void UpdateOrder(Order order);
    }
}
