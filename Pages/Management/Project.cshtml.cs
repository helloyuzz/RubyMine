using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Management {
    public class ProjectModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public ProjectModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        [BindProperty]
        public IList<Project> Projects { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            Projects = await _context.Projects.Where(t => t.Id > 0).ToListAsync();
            return Page();
        }

  
        private bool CustomFieldsMappingExists(int id) {
            return _context.CustomFieldsMappings.Any(e => e.Id == id);
        }
    }
}