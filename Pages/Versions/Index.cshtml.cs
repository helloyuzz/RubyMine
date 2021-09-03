using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RubyMine.Pages.Version {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        public IList<RubyMine.Models.Version> Version { get; set; }
        public async Task OnGet() {
            Version = await _context.Versions.Where(t => t.Status.Equals("closed") == false).ToListAsync();
        }
    }
}
