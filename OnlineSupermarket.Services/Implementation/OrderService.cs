using Microsoft.Extensions.Logging;
using OnlineSupermarket.Domain.DomainModels;
using OnlineSupermarket.Repository.Interface;
using OnlineSupermarket.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSupermarket.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        
        public OrderService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
            
        }
        public List<Order> GetAllOrders()
        {
            return this._orderRepository.GetAllOrders();
           
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return this._orderRepository.GetOrderDetails(model);
        }
    }
}
