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
            return new JsonResult("ok");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> SendCredentials([FromQuery] User user)
        {
            string userId = LoginUser(user.Email, user.Password);
            if (userId != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
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

        private string LoginUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == username);
            if(user != null && user.Password == password)
            {
                return user.Id;
            }
            else
            {
                return null;
            }
        }

    }
}