using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Filters;
using WebApp.Model;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View(new CredentialViewModel());
        }

        [HttpPost]
        [EnsureValidModelStateFilter]
        public async Task<IActionResult> Login(CredentialViewModel credential)
        {

            if (credential.EmailAddress == "admin@company.com" &&  credential.Password == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim("Name", credential.EmailAddress),
                    new Claim("Role", "Admin")
                };

                var identity = new ClaimsIdentity(claims, "CookieScheme");
                var userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieScheme", userPrincipal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Error", "Not able to login.");
            }

            return View(credential);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieScheme");
            return RedirectToAction("Login", "Account");
        }
    }
}
