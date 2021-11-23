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
            // 可编辑
            Chanpins = new List<int>();
            Chanpins.Add(8);
            Chanpins.Add(135);
            
            int current_module_id = RMUtils.QueryInt(Request, "id");
            string action = Request.Query["action"];
            string queryUrl = Request.QueryString.Value;
            bool isRefresh = false; // 判断是否刷新操作
            Module_id = RMUtils.QueryInt(Request, "module_id");

            string url = SessionUtils.Get<string>(HttpContext.Session, "QueryUrl");
            if (queryUrl.Equals(url)) {
                isRefresh = true;
            } else {
                SessionUtils.Set<string>(HttpContext.Session, "QueryUrl", queryUrl);
            }
            ActiveNodes nodes = SessionUtils.Get<ActiveNodes>(HttpContext.Session, "ActiveNodes");
            if (nodes == null) {
                nodes = new ActiveNodes();
            }
            if (Module_id > 0 && isRefresh == false) {
                if (nodes.Contains(Module_id)) {
                    nodes.Remove(Module_id);
                } else {
                    nodes.Add(Module_id);
                }
            }
            SessionUtils.Set(HttpContext.Session, "ActiveNodes", nodes);

            switch (action) {
                case "up":
                case "down":
                    if (current_module_id > 0) {
                        Module tempModule = _context.Modules.FirstOrDefault(t => t.Id == current_module_id);
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