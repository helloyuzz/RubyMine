using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyMine.Models;

namespace RubyMine.Pages.ThridParty {
    public class VersionModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        private readonly IConfiguration _configuration;
        public VersionModel(RubyMine.DbContexts.RubyRemineDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }
        public IList<RubyMine.Models.Version> Version { get; set; }
        public IList<RubyMine.Models.Project> Project { get; set; }
        public IList<Tracker> Tracker { get; set; }
        public IList<Issue> Issue { get; set; }
        public IList<CustomValue> CustomValue { get; set; }
        public string RedmineUrl { get; set; }
        public async Task OnGet() {
            Version = await _context.Versions.Where(t => t.Status.Equals("closed") == false).ToListAsync();
            Project = await _context.Projects.Where(t => t.IsPublic.Value).Select(t => new Project() { Id = t.Id, Name = t.Name }).ToListAsync();
            Tracker = await _context.Trackers.Where(t => t.Description.Equals("hide") == false).OrderBy(t => t.Position).Select(t => new Tracker { Id = t.Id, Name = t.Name }).ToListAsync();
            Issue = await _context.Issues.Select(t => new Issue() { Id = t.Id, TrackerId = t.TrackerId, FixedVersionId = t.FixedVersionId }).ToListAsync();

            // 查询版本的“类型”自定义属性
            CustomValue = await _context.CustomValues.Where(t => t.CustomizedType.Equals("Version") && t.CustomFieldId == 53).Select(t => new CustomValue() { CustomizedId = t.CustomizedId, Value = t.Value }).ToListAsync();
            RedmineUrl = _configuration["AppSettings:RedmineUrl"];
        }
    }
}