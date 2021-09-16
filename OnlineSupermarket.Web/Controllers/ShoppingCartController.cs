using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineSupermarket.Domain.DomainModels;
using OnlineSupermarket.Domain.DTO;
using OnlineSupermarket.Domain.Identity;
using OnlineSupermarket.Repository;
using OnlineSupermarket.Services.Interface;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineSupermarket.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            //var allProducts = userShoppingCart.Products.Select(z => z.Product).ToList();


            return View(this._shoppingCartService.getShoppingCartInfo(userId));
        }
        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.deleteProductFromShoppingCart(userId, id);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

        }
        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = this._shoppingCartService.getShoppingCartInfo(userId);
            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(order.TotalPrice) * 100),
                Description = "OnlineSupermarketApplication Payment",
                Currency = "usd",
                Customer = customer.Id
            });
            if (charge.Status == "succeeded")
            {
                var result = this.OrderNow();
                if (result)
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
                else
                {
                    return RedirectToAction("Index", "ShoppingCart");
                }
            }
            return RedirectToAction("Index", "ShoppingCart");
            
        }
        private Boolean OrderNow()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._shoppingCartService.orderNow(userId);

            return result;
        }
    }
}
