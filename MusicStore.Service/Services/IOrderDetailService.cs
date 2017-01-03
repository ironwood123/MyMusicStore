using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Repository;
using MusicStore.Models;
namespace MusicStore.Service
{
    public interface IOrderDetailService
    {
        void AddOrderDetail(OrderDetail orderdetail);
        void UpdateOrderDetail(OrderDetail orderdetail);
    }
}
