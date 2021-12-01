using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages {
    [AllowAnonymous]
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly RubyRemineDbContext _context;
        [BindProperty]
        [Display(Name = "登录帐号")]
        public string Login { get; set; }

        [BindProperty]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [BindProperty(SupportsGet =true)]
        public int AutoLogin { get; set; }

        [TempData]
        public string LoginMessage { get; set; }
        public IndexModel(ILogger<IndexModel> logger, RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync() {
            //HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (User.Identity.IsAuthenticated) {
                return RedirectToPage("/Platform/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync() {
            //returnUrl ??= Url.Content("~/");
            if (!ModelState.IsValid) {
                return Page();
            }
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)) {
                LoginMessage = "帐号或密码错误。";
                return Page();
            } else {
                var user = _context.Users.FirstOrDefault(x => x.Login.Equals(Login));
                if (user == null) {
                    LoginMessage = "帐号或密码错误。";
                    return Page();
                } else {
                    var encrypt_Password = RMUtils.SHA1(Password);
                    encrypt_Password = RMUtils.SHA1(user.Salt + encrypt_Password);

                    if (user.HashedPassword.Equals(encrypt_Password) == false) {
                        LoginMessage = "帐号或密码错误。";
                        return Page();
                    } else {
                        user.LastLoginOn = DateTime.Now;
                        _context.SaveChanges();

                        //SessionUtils.Set<User>(HttpContext.Session, "cua", user);

                        var claims = new List<Claim>() {
                            new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id)),
                            new Claim(ClaimTypes.Name,user.Login),
                            new Claim(ClaimTypes.Email,JsonConvert.SerializeObject(user)),
                            new Claim(ClaimTypes.Role,user.Salt),
                            new Claim("FavoriteDrink","Tea")
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { IsPersistent = true, ExpiresUtc = DateTime.Now.AddDays(AutoLogin) });
                        return RedirectToPage("/Platform/Index");
                    }
                }
            }
        }
    }
}