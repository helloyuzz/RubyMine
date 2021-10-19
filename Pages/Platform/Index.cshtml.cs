using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;

namespace RubyMine.Pages.Platform {
    public class IndexModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        public IList<RubyMine.Models.Issue> Issue { get; set; }
        public IList<RubyMine.Models.Module> Module { get; set; }
        public async Task OnGet() {
            //Issue = await _context.Issues.Where(t => t.TrackerId == 8).Select(t => new Models.Issue { Id = t.Id, Subject = t.Subject, ParentId = t.ParentId, RootId = t.RootId, Lft = t.Lft, Rgt = t.Rgt }).ToListAsync();
            string action = Request.Query["action"];
            int id = RMUtils.QueryInt(Request, "id");
            int module_id = RMUtils.QueryInt(Request, "module_id");

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
            Module = await _context.Modules.ToListAsync();

            Issue = await _context.Issues.Take(30).ToListAsync();
        }
    }
}