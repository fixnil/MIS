using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GISCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace WebApplication.Pages
{
    public class LoginModel : PageModel
    {
        private readonly GISContext _context;

        public LoginModel(GISContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User UserInfo { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var password = UserInfo.Password.Encryption(UserInfo.UserName);

            if (_context.Users.SingleOrDefault(x => x.UserName == UserInfo.UserName && x.Password == password) != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, UserInfo.UserName), new Claim(ClaimTypes.Role, "admin") };

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }

            return RedirectToPage("/Info");
        }
    }
}