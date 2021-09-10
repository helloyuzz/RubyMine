using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RubyMine.Pages.Versions {
    public class DeleteModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public DeleteModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        [BindProperty]
        public RubyMine.Models.Version Version { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Version = await _context.Versions.Include(t=>t.Project).FirstOrDefaultAsync(m => m.Id == id);

            if (Version == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Version = await _context.Versions.FindAsync(id);

            if (Version != null) {
                _context.Versions.Remove(Version);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Versions/Index");
        }
    }
}