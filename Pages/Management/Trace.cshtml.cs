using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;

namespace RubyMine.Pages.Management {
    public class TraceModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public TraceModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        [BindProperty]
        public IList<Tracker> Tracker { get; set; }
        public async Task<IActionResult> OnGetAsync() {
            Tracker = await _context.Trackers.Where(x => x.Id > 0).ToListAsync();
            return Page();
        }
    }
}