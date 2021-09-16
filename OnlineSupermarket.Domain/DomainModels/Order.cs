using OnlineSupermarket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Domain.DomainModels
{
    public class Order :BaseEntity
    {
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public virtual ICollection<ProductInOrder> Products { get; set; }
    }
}
