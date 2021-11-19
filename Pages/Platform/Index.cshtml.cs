using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Customs.Models;
using RubyMine.Models;

namespace RubyMine.Pages.Platform {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        public IList<DisplayIssue> DisplayIssues { get; set; }
        public IList<RubyMine.Models.Module> Modules { get; set; }
        public Module CurrentModule { get; set; }
        public IList<CustomValue> CustomValues { get; set; }
        public int Module_id { get; set; }
        public IList<int> Chanpins { get; set; }
        public async Task OnGet() {
            Chanpins = new List<int>();
            Chanpins.Add(8);
            Chanpins.Add(135);
            //Issue = await _context.Issues.Where(t => t.TrackerId == 8).Select(t => new Models.Issue { Id = t.Id, Subject = t.Subject, ParentId = t.ParentId, RootId = t.RootId, Lft = t.Lft, Rgt = t.Rgt }).ToListAsync();
            string action = Request.Query["action"];
            int id = RMUtils.QueryInt(Request, "id");
            Module_id = RMUtils.QueryInt(Request, "module_id");

            switch (action) {
                case "up":
                case "down":
                    if (id > 0) {
                        Module tempModule = _context.Modules.FirstOrDefault(t => t.Id == id);
                        int cacheIndex = tempModule.Index.Value;
                        int pid = tempModule.PId.Value;
                        int targetIndex = -1;

                        if (action.Equals("up")) {
                            targetIndex = cacheIndex - 1;
                            tempModule.Index--;
                        } else {
                            targetIndex = cacheIndex + 1;
                            tempModule.Index++;
                        }
                        _context.Database.ExecuteSqlRaw("update `modules` set `index`=" + cacheIndex + " where `index`=" + targetIndex + " and `p_id`=" + pid);

                        _context.SaveChanges();
                    }
                    break;
                case "load":
                    break;
                default:
                    break;
            }

            // 查询所有模块列表
            Modules = await _context.Modules.ToListAsync();

            // 根据模块Id查询所有Issue
            var predicateBuilder = PredicateBuilder.New<CustomValue>(false);
            predicateBuilder = predicateBuilder.And(x => x.CustomFieldId == 54);

            // CustomizedId = 对应Issue的Id
            // CustomFieldId = 模块序号（54）
            DisplayIssues = new List<DisplayIssue>();
            if (Module_id > 0) {    // 组合查询条件
                CurrentModule = _context.Modules.FirstOrDefault(t => t.Id == Module_id);

                predicateBuilder = predicateBuilder.And(x => x.Value.Equals(Module_id.ToString()));
                CustomValues = await _context.CustomValues.Where(predicateBuilder).OrderBy(t => t.Position).ToListAsync();

                var quireFieldIds = CustomValues.Select(t => t.CustomizedId).ToList();

                // 查询该模块下的所有Issue
                var db_issues = await _context.Issues.Where(t => quireFieldIds.Contains(t.Id)).Include(t => t.Tracker).Include(t => t.Project).Include(t => t.Status).Include(t => t.Author).ToListAsync();

                foreach (Issue issue in db_issues) {
                    DisplayIssue displayIssue = new DisplayIssue(issue);
                    displayIssue.Position = CustomValues.FirstOrDefault(t => t.CustomizedId == issue.Id).Position;
                    DisplayIssues.Add(displayIssue);
                }
            }
        }
    }
}