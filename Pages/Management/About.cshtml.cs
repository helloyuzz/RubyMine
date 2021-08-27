using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RubyMine.Pages.Management {
    public class AboutModel : PageModel {
        public void OnGet() {
        }
        public void OnPost() {
            HttpContext.Session.Clear();
            //HttpContext.Response.Redirect("/");
        }
    }
}