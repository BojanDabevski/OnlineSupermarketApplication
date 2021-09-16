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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IRepository<ProductInOrder> _productInOrderRepositorty;
        private readonly ILogger<ProductService> _logger;
        private readonly IRepository<EmailMessage> _mailRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<Order> orderRepositorty, IRepository<ProductInOrder> productInOrderRepositorty, ILogger<ProductService> logger, IRepository<EmailMessage> mailRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
           _productInOrderRepositorty = productInOrderRepositorty;
            _logger = logger;
            _mailRepository = mailRepository;

        }

        public bool deleteProductFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.ProductInShoppingCarts.Where(z => z.ProductId.Equals(id)).FirstOrDefault();

                userShoppingCart.ProductInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);
                _logger.LogInformation("Product From Shopping cart was deleted");
                return true;
            }
            _logger.LogInformation("Product From Shopping cart was not deleted");
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var AllProducts = userShoppingCart.ProductInShoppingCarts.ToList();

            var allProductPrice = AllProducts.Select(z => new
            {
                ProductPrice = z.Product.ProductPrice,
                Quanitity = z.Quantity
            }).ToList();

            var totalPrice = 0;


            foreach (var item in allProductPrice)
            {
                totalPrice += item.Quanitity * item.ProductPrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                ProductInCart = AllProducts,
                TotalPrice = totalPrice
            };

            _logger.LogInformation("GetShoppingCartInfo was called");
            return scDto;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage message = new EmailMessage();
                message.MailTo = loggedInUser.Email;
                message.Subject = "Successfully created order";
                message.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepositorty.Insert(order);

                List<ProductInOrder> productInOrders = new List<ProductInOrder>();

                var result = userShoppingCart.ProductInShoppingCarts.Select(z => new ProductInOrder
                {
                    Id = Guid.NewGuid(),
                    ProductId = z.Product.Id,
                    SelectedProduct = z.Product,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your order is completed. The order contains: ");
                var totalPrice = 0.0;
                for (int i = 0; i < result.Count(); i++)
                {
                    var item = result[i];
                    totalPrice += item.Quantity * item.SelectedProduct.ProductPrice;
                    sb.AppendLine(i.ToString() + "." + item.SelectedProduct.ProductName + " with price of:" + item.SelectedProduct.ProductPrice + " and quantity of: " + item.Quantity);
                }
                sb.AppendLine("Total price: " + totalPrice.ToString());
                message.Content = sb.ToString();

                productInOrders.AddRange(result);

                foreach (var item in productInOrders)
                {
                    this._productInOrderRepositorty.Insert(item);
                }
                _logger.LogInformation("Ticket was successfully ordered");
                loggedInUser.UserCart.ProductInShoppingCarts.Clear();
                this._mailRepository.Insert(message);

                this._userRepository.Update(loggedInUser);


                return true;
            }
            _logger.LogInformation("Something was wrong. Find the Problem and fix it");
            return false;
        }

    }
    
}
