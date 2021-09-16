using Microsoft.EntityFrameworkCore;
using OnlineSupermarket.Domain.DomainModels;
using OnlineSupermarket.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSupermarket.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> GetAllOrders()
        {
            return entities
                .Include(z => z.Products)
                .Include(z => z.User)
                .Include("Products.SelectedProduct")
                .ToListAsync().Result; ;
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return entities
               .Include(z => z.Products)
               .Include(z => z.User)
               .Include("Products.SelectedProduct")
               .SingleOrDefaultAsync(z => z.Id == model.Id).Result;
        }
    }
}
