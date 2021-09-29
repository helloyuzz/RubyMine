using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyMine.CustomModels;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.ThridParty {
    public class CustomModel : PageModel {
        private readonly RubyRemineDbContext _context;
        private readonly IConfiguration _configuration;
        public CustomModel(RubyRemineDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }
        public IList<User> User { get; set; }
        public IList<CustomField> CustomField { get; set; }
        public IList<Tracker> Tracker { get; set; }
        public IList<ViewIssuetemp> IssueTemp { get; set; }
        public string RedmineUrl { get; set; }
        public async Task OnGet() {
            User = await _context.Users.Where(t => t.Lastname.Equals("p") && t.Status == 1).ToListAsync();
            Tracker = await _context.Trackers.Where(t => t.Description.Equals("hide") == false).OrderBy(t => t.Position).Select(t => new Tracker { Id = t.Id, Name = t.Name }).ToListAsync();
            IssueTemp = await _context.ViewIssuetemps.ToListAsync();
            RedmineUrl = _configuration["AppSettings:RedmineUrl"];
        }
    }
}