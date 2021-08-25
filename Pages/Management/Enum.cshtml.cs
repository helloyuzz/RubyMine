using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;

namespace RubyMine.Pages.Management {
    public class EnumModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public EnumModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        [BindProperty]
        public IList<Enumeration> Enumerations { get; set; }
        public async Task<IActionResult> OnGetAsync() {
            Enumerations = await _context.Enumerations.OrderBy(x => x.Type).ThenBy(x => x.Position).ToListAsync();
            return Page();
        }
    }
}