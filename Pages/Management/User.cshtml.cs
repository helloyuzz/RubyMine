using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;

namespace RubyMine.Pages.Management {
    public class UserModel : PageModel {

        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public UserModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        public IList<User> User { get; set; }
        public async Task<IActionResult> OnGetAsync() {
            User = await _context.Users.Where(x => x.Login.StartsWith("p_") == false).ToListAsync();
            return Page();
        }
    }
}