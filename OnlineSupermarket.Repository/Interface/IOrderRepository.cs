using OnlineSupermarket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSupermarket.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order GetOrderDetails(BaseEntity model);

    }
}
