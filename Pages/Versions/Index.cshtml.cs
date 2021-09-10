using Abp.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace RubyMine.Pages.Version {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        [BindProperty(SupportsGet = true)]
        public int project_id { get; set; }
        [BindProperty(SupportsGet = true)]
        public string status { get; set; }
        public IList<RubyMine.Models.Version> Version { get; set; }
        public IList<Project> Project { get; set; }
        public async Task OnGet() {
            var pr = PredicateBuilder.New<RubyMine.Models.Version>(true);
            if (project_id > 0) {
                pr = pr.And(x => x.ProjectId == project_id);
            }
            if (string.IsNullOrEmpty(status) == false) {
                pr = pr.And(x => x.Status.Equals(status));
            } else {
                pr.And(x => x.Status.Equals("closed") == false);
            }
            Project = await _context.Projects.Where(t => t.IsPublic.Value).Select(t => new Project { Id = t.Id, Name = t.Name }).ToListAsync();
            Version = await _context.Versions.Where(pr).OrderBy("status desc,EffectiveDate desc").Include(t => t.Project).ToListAsync();
        }
    }
}
