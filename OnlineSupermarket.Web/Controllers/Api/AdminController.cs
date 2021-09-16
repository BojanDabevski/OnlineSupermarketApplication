using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSupermarket.Domain.DomainModels;
using OnlineSupermarket.Domain.Identity;
using OnlineSupermarket.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineSupermarket.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> userManager;
        public AdminController(IOrderService orderService , UserManager<ApplicationUser> userManager)
        {
            this._orderService = orderService;
            this.userManager = userManager;

        }

       [HttpGet("[action]")]
        public List<Order> GetAllActiveOrders()
        {
            return this._orderService.GetAllOrders();
        }
        [HttpPost("[action]")]
        public Order GetDetailsForOrder(BaseEntity model)
        {
            return this._orderService.GetOrderDetails(model);
        }
        [HttpPost("[action]")]
        public bool ImportAllUsers(List<UserRegistrationDto> model)
        {
            bool status = true;
            foreach (var item in model)
            {
                var userCheck = userManager.FindByEmailAsync(item.Email).Result;
                if (userCheck == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        Role = item.Role,
                        PhoneNumberConfirmed = true,
                        UserCart = new ShoppingCart()
                    };
                    var result = userManager.CreateAsync(user, item.Password).Result;
                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }
            return status;
        }
    }
}
