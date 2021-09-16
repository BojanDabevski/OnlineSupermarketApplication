using OnlineSupermarket.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<ProductInShoppingCart> ProductInCart { get; set; }
        public double TotalPrice { get; set; }
    }
}
