using Microsoft.Extensions.Logging;
using OnlineSupermarket.Domain.DomainModels;
using OnlineSupermarket.Domain.DTO;
using OnlineSupermarket.Repository.Interface;
using OnlineSupermarket.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineSupermarket.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ProductService> _logger;


        public ProductService(IRepository<Product> productRepository, IUserRepository userRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
            _logger = logger;
        }

        public bool AddToShoppingCart(AddToShoppingCardDTO item, string userID)
        {
            var user = this._userRepository.Get(userID);
            var userShoppingCard = user.UserCart;
            if (item.ProductId != null && userShoppingCard != null)
            {
                var product = this.GetDetailsForProduct(item.ProductId);
                if (product != null)
                {
                    ProductInShoppingCart itemToAdd = new ProductInShoppingCart
                    {   Id =Guid.NewGuid(),
                        Product = product,
                        ProductId = product.Id,
                        ShoppingCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };
                    this._productInShoppingCartRepository.Insert(itemToAdd);
                    _logger.LogInformation("Ticket was successfully added into ShoppingCart");
                    return true;
                }
                return false;

            }
            _logger.LogInformation("Something was wrong. ProductId or UserShoppingCard may be unavailable");
            return false;
        }

        public void CreateNewProduct(Product p)
        {
            _logger.LogInformation("New Product was created");
            this._productRepository.Insert(p); 
        }

        public void DeleteProduct(Guid id)
        {
            _logger.LogInformation(" Product was deleted");
            var product = this.GetDetailsForProduct(id);
            this._productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts was called");
            return this._productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            _logger.LogInformation("GetDetailForProduct was called");
            return this._productRepository.Get(id);
        }

        public AddToShoppingCardDTO GetShoppingCartInfo(Guid? id)
        {
            _logger.LogInformation("GetShoppingCartInfo was called");
            var product = this.GetDetailsForProduct(id);
            AddToShoppingCardDTO model = new AddToShoppingCardDTO
            {
                SelectedProduct = product,
                ProductId = product.Id,
                Quantity = 1
            };

            return model;
        }

        public void UpdeteExistingProduct(Product p)
        {
            _logger.LogInformation("Product was updated");
            this._productRepository.Update(p);
        }
    }
}
