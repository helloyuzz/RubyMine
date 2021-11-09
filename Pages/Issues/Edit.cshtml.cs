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

namespace RubyMine.Pages.Issues
{
    public class EditModel : PageModel
    {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public EditModel(RubyMine.DbContexts.RubyRemineDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Issue Issue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue = await _context.Issues
                .Include(i => i.AssignedTo)
                .Include(i => i.Author)
                .Include(i => i.Priority)
                .Include(i => i.Project)
                .Include(i => i.Status)
                .Include(i => i.Tracker).FirstOrDefaultAsync(m => m.Id == id);

            if (Issue == null)
            {
                return NotFound();
            }
           ViewData["AssignedToId"] = new SelectList(_context.Users, "Id", "Firstname");
           ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Firstname");
           ViewData["PriorityId"] = new SelectList(_context.Enumerations, "Id", "Name");
           ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
           ViewData["StatusId"] = new SelectList(_context.IssueStatuses, "Id", "Name");
           ViewData["TrackerId"] = new SelectList(_context.Trackers, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(Issue.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IssueExists(int id)
        {
            return _context.Issues.Any(e => e.Id == id);
        }
    }
}
