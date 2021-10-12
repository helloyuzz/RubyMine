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
    public class DeleteModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public DeleteModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Module = await _context.Modules.FindAsync(id);

            if (Module != null) {
                //_context.Modules.Remove(Module);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Creation/Index");
        }
    }
}