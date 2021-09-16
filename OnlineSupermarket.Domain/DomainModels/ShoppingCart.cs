using OnlineSupermarket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
       
        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
    }
}
