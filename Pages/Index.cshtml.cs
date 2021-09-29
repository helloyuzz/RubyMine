using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RubyMine.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        [BindProperty]
        [Display(Name = "登录帐号")]
        public string Login { get; set; }
        [BindProperty]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [BindProperty]
        public bool SaveOnline { get; set; }
        [TempData]
        public string ShowLogin { get; set; } = "none";
        [TempData]
        public string ShowMessage { get; set; }
        [TempData]
        public string jsScript { get; set; }
        [TempData]
        public string TempLogin { get; set; }
        [TempData]
        public string TempPassword { get; set; }
        public IndexModel(ILogger<IndexModel> logger, RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
            _logger = logger;
        }

        public void OnGet() {
            Login = Request.Cookies["cache_Account"];
            TempLogin = Login;
            if (string.IsNullOrEmpty(Login) == false) {
                jsScript = "$('#Password').focus();";
            }
            if (string.IsNullOrEmpty(ShowMessage)) {
                ShowMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
            }
        }
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }
            ShowLogin = "block";
            TempLogin = Login;
            TempPassword = Password;
            if (string.IsNullOrEmpty(Login)) {
                ShowMessage = "登录帐号不能为空!";
                jsScript = "$('#Login').focus();";
            } else if (string.IsNullOrEmpty(Password)) {
                Response.Cookies.Append("cache_Account", Login);
                ShowMessage = "密码不能为空!";
                jsScript = "$('#Password').focus();";
            } else {
                var item = _context.Users.FirstOrDefault(x => x.Login.Equals(Login));
                if (item == null) {
                    ShowMessage = "帐号或密码错误!";
                    jsScript = "$('#Login').focus();";
                } else {
                    var encrypt_Password = RMUtils.SHA1(Password);
                    encrypt_Password = RMUtils.SHA1(item.Salt + encrypt_Password);

                    if (item.HashedPassword.Equals(encrypt_Password)) {
                        ShowMessage = "登陆成功";
                        ShowLogin = "";
                        SessionUtils.Set<User>(HttpContext.Session, "cua", item);
                        ShowMessage += "cua设置成功";
                    } else {
                        ShowMessage = "帐号或密码错误!";
                        jsScript = "$('#Password').focus();";
                    }
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
