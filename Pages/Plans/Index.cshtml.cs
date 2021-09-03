using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Pages.Plans {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        public IList<RubyMine.Models.Version> Version { get; set; }
        public IList<Project> Project { get; set; }
        public async Task OnGetAsync() {
            Version = await _context.Versions.Where(t => t.Status.Equals("closed") == false).OrderByDescending(t => t.Status).ThenByDescending(t => t.Name).ToListAsync();
            Project = await _context.Projects.Where(t => t.IsPublic == true).ToListAsync();
        }
    }
}