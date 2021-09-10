using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;

namespace RubyMine.Pages.Versions.temp
{
    public class IndexModel : PageModel
    {
        private readonly RubyMine.Models.bitnami_redmineplusagileContext _context;

        public IndexModel(RubyMine.Models.bitnami_redmineplusagileContext context)
        {
            _context = context;
        }

        public IList<RubyMine.Models.Version> Version { get;set; }

        public async Task OnGetAsync()
        {
            Version = await _context.Versions
                .Include(v => v.Project).ToListAsync();
        }
    }
}
