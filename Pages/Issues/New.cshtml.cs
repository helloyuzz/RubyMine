using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RubyMine.CustomModels;
using RubyMine.Models;

namespace RubyMine.Pages.Issues {
    public class NewModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        public IList<Tracker> Tracker { get; set; }
        public IList<CustomField> CustomField { get; set; }
        public string CustomFieldProject { get; set; }
        public string CustomFieldTracker { get; set; }
        public string ProjectTracker { get; set; }
        public NewModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        public string Project_id { get; set; }
        public string Tracker_id { get; set; }
        public IList<CustomProject> DB_ProjectUser { get; set; }
        public IList<CustomVersion> DB_Version { get; set; }
        [BindProperty]
        public DateTime CurrentDateTime { get; set; }
        public IList<IssueStatus> DB_IssueStatus { get; set; }

        public async Task OnGetAsync() {
            Project_id = Request.Query["project_id"];
            Tracker_id = Request.Query["tracker_id"];
            Tracker = await _context.Trackers.OrderBy(x => x.Position).ToListAsync();

            CustomField = await _context.CustomFields.Where(t => t.Type.Equals("IssueCustomField") && t.Description.Equals("hide") == false).OrderBy(t => t.Position).ToListAsync();

            // 项目和自定义属性对应关系
            var db_CustomFieldProject = await _context.CustomFieldsProjects.OrderBy(t => t.ProjectId).ToListAsync();
            CustomFieldProject = JsonConvert.SerializeObject(db_CustomFieldProject);

            // 自定义属性和跟踪的对应关系
            var db_CustomFieldTracker = await _context.CustomFieldsTrackers.OrderBy(t => t.TrackerId).ToListAsync();
            CustomFieldTracker = JsonConvert.SerializeObject(db_CustomFieldTracker);

            var db_ProjectTracker = await _context.ProjectsTrackers.OrderBy(t => t.ProjectId).ToListAsync();
            ProjectTracker = JsonConvert.SerializeObject(db_ProjectTracker);

            DB_ProjectUser = await _context.Users.Where(x => x.Lastname.Equals("p")).Select(x => new CustomProject { Id = x.Id, Firstname = x.Firstname, pinyin = x.pinyin }).ToListAsync();
            DB_Version = await _context.Versions.Select(x => new CustomVersion { Id = x.Id, Name = x.Name, Project_id = x.ProjectId }).ToListAsync();

            DB_IssueStatus = await _context.IssueStatuses.OrderBy(t => t.Position).ToListAsync();
        }
    }
}