using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Pages.Management {
    public class IssueStatusModel : PageModel {

        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public IssueStatusModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        [BindProperty]
        public IList<IssueStatus> IssueStatuses { get; set; }
        public async Task<IActionResult> OnGetAsync() {
            IssueStatuses = await _context.IssueStatuses.Where(x => x.Id > 0).ToListAsync();
            return Page();
        }
    }
}