using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Modules {
    public class EditModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public EditModel(RubyMine.DbContexts.RubyRemineDbContext context) {
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(Module).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!ModuleExists(Module.Id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return RedirectToPage("/Creation/Index");
        }

        private bool ModuleExists(int id) {
            return _context.Modules.Any(e => e.Id == id);
        }
    }
}