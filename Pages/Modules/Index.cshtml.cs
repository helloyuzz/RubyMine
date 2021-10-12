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
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        public IList<Module> Module { get; set; }

        public async Task OnGetAsync() {
            Module = await _context.Modules.ToListAsync();
        }
    }
}