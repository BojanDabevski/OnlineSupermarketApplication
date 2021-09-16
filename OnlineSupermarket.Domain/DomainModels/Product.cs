using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductImage { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public int ProductPrice { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public string ExpirationDate { get; set; }
        [Required]
        public string Category { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual ICollection<ProductInOrder> Orders { get; set; }

        public Product() { }
    }
}
