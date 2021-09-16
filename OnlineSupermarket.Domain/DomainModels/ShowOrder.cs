using OnlineSupermarket.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSupermarket.Domain.DomainModels
{
    public class ShowOrder
    {
        public string UserId { get; set; }
        public EShopApplicationUser User { get; set; }

        public ICollection<ProductInOrder> Products { get; set; }
    }
}
