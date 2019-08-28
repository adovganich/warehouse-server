using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseServer.Database;
using WarehouseServer.Model;

namespace WarehouseServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly WarehouseContext _context;

        public AuthController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return new JsonResult(new User() { Email = "test", Password = "test" });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> SendCredentials([FromQuery] User user)
        {
            if (LoginUser(user.Email, user.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(
                  claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity),
                  authProperties);
                return new JsonResult(user);
            }
            else
            {
                return Unauthorized();
            }
        }

        private bool LoginUser(string username, string password)
        {
            var user = _context.Users.Single(x => x.Email == username);
            return user != null && user.Password == password;
        }

    }
}