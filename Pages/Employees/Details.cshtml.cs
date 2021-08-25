using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;
using RubyMine.Models.CustomModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Pages.Employees {
    public class DetailsModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public DetailsModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        public RMUser User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            User = new RMUser();
            // 系统约定：所有项目以p_开头
            // 第一步：获取所有的项目基本信息
            User db_User = _context.Users.FirstOrDefault(x => x.Id == id);
            if (db_User != null) {
                RMUtils.CloneObject(db_User, User);
                // 第二步：获取所有的自定义属性
                List<CustomField> db_CustomFields = await _context.CustomFields.Where(a => a.Type.Equals("UserCustomField") && a.Description.Contains("u")).OrderBy(x => x.Position).ToListAsync();

                // 查询和Project相关的自定义属性值，条件为所有自定义属性的Id作为查询条件
                List<int> ids = db_CustomFields.Select(x => x.Id).ToList();

                // 第三步：获取所有自定义属性值,可根据userid查询所有CustomValue，也就是获取某用户的自定义属性值
                List<CustomValue> db_CustomValues = await _context.CustomValues.Where(a => a.CustomizedId == db_User.Id && ids.Contains(a.CustomFieldId)).OrderBy(x => x.CustomizedId).ToListAsync();
                User = RMUtils.ParseToRMUser(db_User, db_CustomFields, db_CustomValues, User);
            }


            if (User == null) {
                return NotFound();
            }
            return Page();
        }
    }
}
