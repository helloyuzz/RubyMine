using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;
using RubyMine.Models.CustomModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Pages.Employees {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        public IList<RMUser> RMUser { get; set; }

        public async Task OnGetAsync() {
            List<User> db_User = await _context.Users.Where(x => x.Login.StartsWith("p_") == false && string.IsNullOrEmpty(x.Login) == false).ToListAsync();

            // 第二步：获取所有的自定义属性
            List<CustomField> db_CustomFields = await _context.CustomFields.Where(a => a.Type.Equals("UserCustomField") && a.Description.Contains("u")).OrderBy(x => x.Position).ToListAsync();

            // 查询和Project相关的自定义属性值，条件为所有自定义属性的Id作为查询条件
            List<int> ids = db_CustomFields.Select(x => x.Id).ToList();

            // 第三步：获取所有自定义属性值,可根据userid查询所有CustomValue，也就是获取某用户的自定义属性值
            List<CustomValue> db_CustomValues = await _context.CustomValues.Where(a => ids.Contains(a.CustomFieldId)).OrderBy(x => x.CustomizedId).ToListAsync();

            // 第四步：结构化调整：将CustomField转化为==>CustomField1
            RMUser = RMUtils.ParseToRMUser(db_User, db_CustomFields, db_CustomValues);// new List<RMUser>();         
        }
    }
}
