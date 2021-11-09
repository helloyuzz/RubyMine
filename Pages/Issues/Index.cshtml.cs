using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Issues
{
    public class IndexModel : PageModel
    {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context)
        {
            _context = context;
        }

        public IList<Issue> Issue { get;set; }

        public async Task OnGetAsync()
        {
            Issue = await _context.Issues
                .Include(i => i.AssignedTo)
                .Include(i => i.Author)
                .Include(i => i.Priority)
                .Include(i => i.Project)
                .Include(i => i.Status)
                .Include(i => i.Tracker).ToListAsync();
        }
    }
}
