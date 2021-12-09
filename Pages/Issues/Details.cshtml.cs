using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Issues {
    public class DetailsModel : PageModel {
        private readonly RubyRemineDbContext _context;
        private readonly IConfiguration _config;

        public DetailsModel(RubyMine.DbContexts.RubyRemineDbContext context, IConfiguration _configuration) {
            _context = context;
            _config = _configuration;
        }

        public Issue Issue { get; set; }
        public Module Module { get; set; }
        public List<Issue> AssociatedIssue { get; set; }
        public string Admin_Role_id { get; set; }
        public int? prev_issue_id { get; set; }
        public int? next_issue_id { get; set; }
        public async Task<IActionResult> OnGetAsync(int? issue_id, int? module_id) {
            if (issue_id == null) {
                return NotFound();
            }
            Admin_Role_id = _config["AppSettings:Admin_Role_id"];

            Issue = await _context.Issues
                .Include(i => i.AssignedTo)
                .Include(i => i.Author)
                .Include(i => i.Priority)
                .Include(i => i.Project)
                .Include(i => i.Status)
                .Include(i => i.Tracker).FirstOrDefaultAsync(m => m.Id == issue_id);

            Module = _context.Modules.FirstOrDefault(t => t.Id == module_id);

            var customValues = await _context.CustomValues.Where(t => t.CustomFieldId == 54 && t.Value.Equals(module_id.ToString())).OrderBy(t => t.Position).Select(t => t.CustomizedId).ToListAsync();

            AssociatedIssue = await _context.Issues.Where(t => customValues.Contains(t.Id)).Select(t => new Issue { Id = t.Id, Subject = t.Subject }).ToListAsync();
            var index = AssociatedIssue.IndexOf(AssociatedIssue.FirstOrDefault(t => t.Id == issue_id));
            if (AssociatedIssue.Count > 1) {
                if (index >= 1) {
                    prev_issue_id = AssociatedIssue[index - 1].Id;
                }
                if (index < AssociatedIssue.Count -1) {
                    next_issue_id = AssociatedIssue[index + 1].Id;
                }

            }
            if (Issue == null) {
                return NotFound();
            }
            return Page();
        }
    }
}