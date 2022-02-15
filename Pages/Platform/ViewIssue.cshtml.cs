using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyMine.DbContexts;
using RubyMine.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Pages.Platform {
    public class ViewIssueModel : PageModel {
        private readonly RubyRemineDbContext db;
        private readonly IConfiguration _config;
        public ViewIssueModel(RubyRemineDbContext _db, IConfiguration _configuration) {
            db = _db;
            _config = _configuration;
        }
        public string Admin_Role_id { get; set; }
        public int Issue_id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Issue Issue { get; set; }
        public void OnGet() {
            Issue_id = RMUtils.QueryInt(Request, "issue_id");            
            Admin_Role_id = _config["AppSettings:Admin_Role_id"];   // 可编辑权限

            if (Issue_id > 0) {
                Issue = db.Issues.Include(t => t.Author).Include(t => t.Status).Select(t => new Issue() { Id = t.Id, Subject = t.Subject, Description = t.Description, Author = t.Author, UpdatedOn = t.UpdatedOn, Status = t.Status }).FirstOrDefault(t => t.Id == Issue_id);
            } else {
                Issue = new Issue();
                Issue.Status = new IssueStatus();
                Issue.Author = new User();
                Issue.Subject = "请指定正确的编号";
            }
        }
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var item = db.Issues.FirstOrDefault(t => t.Id == Issue.Id);
            item.Subject = Issue.Subject;
            item.Description = Issue.Description;
            await db.SaveChangesAsync();

            return RedirectToPage("./ViewIssue", new { issue_id = item.Id });
        }
    }
}