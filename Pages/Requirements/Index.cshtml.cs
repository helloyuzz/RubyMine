using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RubyMine.DbContexts;
using RubyMine.DIP;
using RubyMine.Models;

namespace RubyMine.Pages.Requirements {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        private readonly RubyMine.DIP.IGlobalSetting _config;

        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context, IGlobalSetting config) {
            _context = context;
            _config = config;
        }

        public IList<Issue> Issue { get; set; }
        public IList<Tracker> Tracker { get; set; }
        public IList<Project> Project { get; set; }

        public IList<RubyMine.Models.Version> Version { get; set; }
        
        public string ProjectTracker { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 30;
        public int PageIndex = 1;

        public async Task OnGetAsync() {
            Tracker = await _context.Trackers.OrderBy(x => x.Position).ToListAsync();
            Project = await _context.Projects.Where(x => x.IsPublic == true).Select(x => new Project() { Id = x.Id, Name = x.Name }).ToListAsync();
            Version = await _context.Versions.Where(t => t.Status.Equals("closed") == false).OrderByDescending(t => t.Status).ThenByDescending(t => t.Name).ToListAsync();
            var db_ProjectTracker = await _context.ProjectsTrackers.Select(x => new ProjectsTracker() { ProjectId = x.ProjectId, TrackerId = x.TrackerId }).ToListAsync();
            ProjectTracker = JsonConvert.SerializeObject(db_ProjectTracker);
            Setting setting = _config.GetPerPageOptions();
            if (setting != null) {
                //PageSize = RMUtils.ParseInt(setting.Value);
            }
            int skipRecordCount = (PageIndex - 1) * PageSize;

            Issue = await _context.Issues.Include(t=>t.Tracker)
                .Include(t=>t.Project)
                .Include(t=>t.Status)
                .Include(t => t.AssignedTo)
                .Include(t => t.Priority)
                .Include(t=>t.Author).Skip(skipRecordCount).Take(PageSize).ToListAsync();                
        }
    }
}