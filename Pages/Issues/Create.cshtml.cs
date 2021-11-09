using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Issues {
    public class CreateModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public CreateModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        public IActionResult OnGet() {
            ViewData["AssignedToId"] = new SelectList(_context.Users.Where(t=>t.Lastname.Equals("u")), "Id", "Firstname");
            ViewData["AuthorId"] = new SelectList(_context.Users.Where(t => t.Lastname.Equals("u")), "Id", "Firstname");
            ViewData["PriorityId"] = new SelectList(_context.Enumerations, "Id", "Name");
            ViewData["ProjectId"] = new SelectList(_context.Projects.Where(t => t.Description.Contains("3.0")), "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.IssueStatuses, "Id", "Name");
            ViewData["TrackerId"] = new SelectList(_context.Trackers.Where(t => t.Description.Contains("3.0")), "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Issue Issue { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Issues.Add(Issue);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}