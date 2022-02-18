using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyMine.DbContexts;
using RubyMine.Models;
using System;
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
        public int Module_id { get; set; }
        [BindProperty(SupportsGet = true)]
        public Issue Issue { get; set; }
        [TempData]
        public int Issue_result_id { get; set; }
        [TempData]
        public string Issue_result_name { get; set; }
        [TempData]
        public string Issue_result_type { get; set; }
        public void OnGet() {
            Issue_id = RMUtils.QueryInt(Request, "issue_id");
            Admin_Role_id = _config["AppSettings:Admin_Role_id"];   // 可编辑权限

            if (Issue_id > 0) {
                Issue = db.Issues.Include(t => t.Author).Include(t => t.Status).Select(t => new Issue() { Id = t.Id, Subject = t.Subject, Description = t.Description, Author = t.Author,AuthorId=t.AuthorId, UpdatedOn = t.UpdatedOn, Status = t.Status }).FirstOrDefault(t => t.Id == Issue_id);
            } else {
                Issue = new Issue();
                Issue.Status = new IssueStatus();
                Issue.Author = new User();
                if (Issue_id < 0) {
                    Issue.Subject = "[请输入需求标题]";
                } else {
                    Issue.Subject = "请指定正确的编号";
                }
            }
        }
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }
            if (Issue.Id < 1) {
                /* ==========================================================================================
                * 新增程序逻辑说明
                * cache_position = 查询前一条记录.Position
                * 更新Position = Position + 1，条件为.Position > cacha_position && module_id = value.module_id
                * 当前记录.Position = cache_position + 1
                * 新增当前记录 db.savechanges()
                */
                int position = db.CustomValues.Count(t => t.CustomFieldId == 54 && t.CustomizedType.Equals("Issue") && t.Value.Equals(Module_id.ToString())) + 1;

                // 新增Issue
                Issue.PriorityId = 2;
                Issue.CreatedOn = DateTime.Now;
                Issue.UpdatedOn = DateTime.Now;
                Issue.CategoryId = 0;
                Issue.StatusId = 1;
                Issue.TrackerId = 2;      // 需求
                Issue.ProjectId = 5;      // 3.0项目
                Issue.Lft = 1;
                Issue.Rgt = 2;
                db.Issues.Add(Issue);
                db.SaveChanges();         // 先保存到数据库，获取Issue.id

                // 新增custom_value，排序=before_position+1
                var new_custom_value = new CustomValue();
                new_custom_value.CustomFieldId = 54;
                new_custom_value.CustomizedType = "Issue";
                new_custom_value.CustomizedId = Issue.Id;
                new_custom_value.Value = Module_id.ToString();
                new_custom_value.Position = position;

                db.CustomValues.Add(new_custom_value);
                db.SaveChanges();
                Issue_result_id = Issue.Id;
                Issue_result_name = Issue.Subject;
                Issue_result_type = "add";
            } else {
                string user_id = User.Claims.FirstOrDefault(t => t.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")).Value;
                var item = db.Issues.FirstOrDefault(t => t.Id == Issue.Id);
                if (user_id.Equals(item.AuthorId.ToString())) {
                    item.Subject = Issue.Subject;
                    item.UpdatedOn = DateTime.Now;
                    item.Description = Issue.Description;
                    await db.SaveChangesAsync();
                    Issue_result_type = "update";
                } else {
                    Issue_result_type = "decline";
                }
                Issue_result_id = item.Id;
                Issue_result_name = item.Subject;
            }

            return RedirectToPage("./ViewIssue", new { issue_id = Issue.Id });
        }
    }
}