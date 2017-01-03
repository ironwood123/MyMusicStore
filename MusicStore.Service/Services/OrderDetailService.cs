using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicStore.Repository;
using MusicStore.Models;
namespace MusicStore.Service
{
    public class OrderDetailService : IOrderDetailService
    {
        protected IUnitOfWork _unitOfWork;
        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddOrderDetail(OrderDetail orderdetail)
        {
            _unitOfWork.OrderDetailRepository.InsertOrderDetail(orderdetail);
            _unitOfWork.Save();
        }

        public void UpdateOrderDetail(OrderDetail orderdetail)
        {
            _unitOfWork.OrderDetailRepository.InsertOrderDetail(orderdetail);
            _unitOfWork.Save();
        }
    }
}