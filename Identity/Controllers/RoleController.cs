using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Repositories;
using Microsoft.AspNetCore.Identity;
using Identity.ViewModels.Role;
using Microsoft.Extensions.Caching.Memory;

namespace Identity.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUtilities _utilities;
        private readonly IMemoryCache _memoryCache;

        public RoleController(RoleManager<IdentityRole> roleManager, IUtilities utilities, IMemoryCache memoryCache)
        {
            _roleManager = roleManager;
            _utilities = utilities;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new List<IndexViewModel>();
            foreach (var role in roles)
            {
                model.Add(new IndexViewModel()
                {
                    RoleName = role.Name,
                    RoleId = role.Id
                });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
           // var allMvcName = _utilities.AreaAndActionAndControllerNamesList();
           var allMvcName = _memoryCache.GetOrCreate("AreaAndActionAndControllerNamesList", p =>
           {
               p.AbsoluteExpiration = DateTimeOffset.MaxValue;
               return _utilities.AreaAndActionAndControllerNamesList();
           });
           var model = new CreateRoleViewModel()
           {
               ActionAndControllerNames = allMvcName
           };
           return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(model.RoleName);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    var requestRoles =
                        model.ActionAndControllerNames.Where(c => c.IsSelected).ToList();
                    foreach (var requestRole in requestRoles)
                    {
                        var areaName = (string.IsNullOrEmpty(requestRole.AreaName)) ?
                            "NoArea" : requestRole.AreaName;

                        await _roleManager.AddClaimAsync(role,
                            new Claim($"{areaName}|{requestRole.ControllerName}|{requestRole.ActionName}",
                                true.ToString()));
                    }


                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}
