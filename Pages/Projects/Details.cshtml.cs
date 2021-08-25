using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.DIP;
using RubyMine.Models;
using RubyMine.Models.CustomModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Pages.Projects {
    public class DetailsModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        private readonly RubyMine.DIP.IDIPSetting _config;

        public DetailsModel(RubyMine.DbContexts.RubyRemineDbContext context, IDIPSetting config) {
            _context = context;
            _config = config;
        }

        public RMUser User { get; set; }
        public IList<RMIssue> Issue { get; set; }
        public List<CustomField> db_CustomFields { get; set; }
        public int PageSize = 30;
        public int PageIndex = 1;
        public List<TrackerMapping> db_Tracker { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            User = new RMUser();
            // 系统约定：所有项目以p_开头
            // 第一步：获取所有的项目基本信息
            User db_User = _context.Users.FirstOrDefault(x => x.Id == id);
            if (db_User == null) {
                return NotFound();
            }
          
            RMUtils.CloneObject(db_User, User);
            // 第二步：获取所有的自定义属性
            db_CustomFields = await _context.CustomFields.Where(a => a.Type.Equals("IssueCustomField") && a.Description.Contains("p")).OrderBy(x => x.Position).ToListAsync();

            // 查询和Project相关的自定义属性值，条件为所有自定义属性的Id作为查询条件
            List<int> db_CustomIds = db_CustomFields.Select(x => x.Id).ToList();

            // 第三步：获取所有自定义属性值,可根据userid查询所有CustomValue，也就是获取某用户的自定义属性值
            List<CustomValue> db_CustomValues = await _context.CustomValues.Where(a => db_CustomIds.Contains(a.CustomFieldId)).OrderBy(x => x.CustomizedId).ToListAsync();
            User = RMUtils.ParseToRMUser(db_User, db_CustomFields, db_CustomValues, User);

            db_Tracker = await _context.TrackerMappings.Where(t => t.Id >= 1).ToListAsync();

            // 第四步获取Issueid
            List<int> db_IssueIds = db_CustomValues.Where(t=>db_User.Id.ToString().Equals(t.Value)).Select(t => t.CustomizedId).Distinct().ToList();

            // 第五步获取分页设置
            Setting setting = _config.GetPerPageOptions();
            if (setting != null) {
                PageSize = RMUtils.ParseInt(setting.Value);
            }
            int skipRecordCount = (PageIndex - 1) * PageSize;
            List<Issue> db_Issues = await _context.Issues.Where(t=> db_IssueIds.Contains(t.Id)).Include(t => t.Tracker)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.AssignedTo)
                .Include(t => t.Priority)
                .Include(t => t.Author).Skip(skipRecordCount).Take(PageSize).ToListAsync();

            
            Issue = RMUtils.ParseToRMIssues(db_Issues, db_CustomFields, db_CustomValues);
            return Page();
        }
    }
}
