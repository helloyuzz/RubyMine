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
    public class CommonModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public CommonModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        [BindProperty]
        public IList<Setting> Setting { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id) {
            Setting = await _context.Settings.OrderBy(x => x.Id).ToListAsync();
            return Page();
        }
    }
}