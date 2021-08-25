using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Management {
    public class RoleModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public RoleModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        [BindProperty]
        public IList<Role> Roles { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            Roles = await _context.Roles.OrderBy(x => x.Position).ToListAsync();
            return Page();
        }
    }
}