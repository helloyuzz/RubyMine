using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RubyMine.Models;

namespace RubyMine.Pages.Versions.temp
{
    public class CreateModel : PageModel
    {
        private readonly RubyMine.Models.bitnami_redmineplusagileContext _context;

        public CreateModel(RubyMine.Models.bitnami_redmineplusagileContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public RubyMine.Models.Version Version { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Versions.Add(Version);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
