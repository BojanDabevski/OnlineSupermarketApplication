using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineSupermarket.Domain;
using OnlineSupermarket.Domain.Identity;
using OnlineSupermarket.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OnlineSupermarket.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager, IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {

            this._userRepository = userRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        //CREATE ROLE
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ManageUserRoles", "Role");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        //MANAGE USER ROLE
        [HttpGet]
        public IActionResult ManageUserRoles()
        {

            var allUsers = this._userRepository.GetAll().ToList();

            return View(allUsers);
        }


        //GRANT ADMIN
        public async Task<IActionResult> GrantAdminAsync(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = this._userRepository.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            if (await userManager.IsInRoleAsync(user, user.Role))
            {
                IdentityResult result = await userManager.AddToRoleAsync(user, "Admin");

                if (result != null)
                {
                    var secondResult = await userManager.RemoveFromRoleAsync(user, "User");

                    if (secondResult != null)
                    {
                        user.Role = "Admin";
                        _userRepository.Update(user);

                        return RedirectToAction("ManageUserRoles", "Role");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction("ManageUserRoles", "Role");

        }

        //REMOVE ADMIN
        public async Task<IActionResult> RemoveAdminAsync(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = this._userRepository.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            if (await userManager.IsInRoleAsync(user, user.Role))
            {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, user.Role);

                if (result != null)
                {
                    var secondResult = await userManager.AddToRoleAsync(user, "User");

                    if (secondResult != null)
                    {
                        user.Role = "User";
                        _userRepository.Update(user);

                        return RedirectToAction("ManageUserRoles", "Role");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return RedirectToAction("ManageUserRoles", "Role");

        }

    }
}
