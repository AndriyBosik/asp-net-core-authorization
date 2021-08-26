using System.Collections.Generic;
using System.Net.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationExample.Data;
using AuthorizationExample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationExample.Controllers
{
    public class AccountController: Controller
    {
        private ApplicationDbContext _dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == model.Username);
                if (user == null)
                {
                    user = new User
                    {
                        Username = model.Username,
                        Password = model.Password
                    };
                    Role userRole = await _dbContext.Roles.FirstOrDefaultAsync(role => role.Title == "user");
                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Show", "Inventory");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username and(or) password");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _dbContext.Users
                    .Include(user => user.Role)
                    .FirstOrDefaultAsync(user => user.Username == model.Username && user.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Show", "Inventory");
                }
                ModelState.AddModelError("", "Invalid username and(or) password");
            }

            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Title)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}