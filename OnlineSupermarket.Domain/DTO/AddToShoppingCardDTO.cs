using OnlineSupermarket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Domain.DTO
{
    public class AddToShoppingCardDTO
    {
        public Product SelectedProduct { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
