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
    public class DetailsModel : PageModel
    {
        private readonly RubyMine.Models.bitnami_redmineplusagileContext _context;

        public DetailsModel(RubyMine.Models.bitnami_redmineplusagileContext context)
        {
            _context = context;
        }

        public RubyMine.Models.Version Version { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Version = await _context.Versions
                .Include(v => v.Project).FirstOrDefaultAsync(m => m.Id == id);

            if (Version == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
