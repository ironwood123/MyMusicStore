using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Repository;
using MusicStore.Models;
namespace MusicStore.Service
{
    public class OrderService : IOrderService
    {
        protected IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Order> ListOrder()
        {
           return  _unitOfWork.OrderRepository.ListOrder();
        }
        public void AddOrder(Order order)
        {
            _unitOfWork.OrderRepository.InsertOrder(order);
            _unitOfWork.Save();
        }

        public void UpdateOrder(Order order)
        {
            _unitOfWork.OrderRepository.UpdateOrder(order);
            _unitOfWork.Save();
        }
    }
}