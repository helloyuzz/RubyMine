using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Modules {
    public class DetailsModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public DetailsModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        public Module Module { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == id);

            if (Module == null) {
                return NotFound();
            }
            return Page();
        }
    }
}