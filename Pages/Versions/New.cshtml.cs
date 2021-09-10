using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.Models;

namespace RubyMine.Pages.Versions {
    public class NewModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public NewModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        public string ShowFormTitle = "添加版本";
        public string ReadonlyClass = "";
        [BindProperty]
        public IList<Project> Project { get; set; }
        [BindProperty]
        public RubyMine.Models.Version Version { get; set; }

        public IActionResult OnGet() {
            var id = Request.Query["id"];
            var action = Request.Query["action"];

            if (string.IsNullOrEmpty(id) == false) {
                Version = _context.Versions.FirstOrDefault(t => t.Id.Equals(RMUtils.ParseInt(id)));
                if (Version != null) {
                    ShowFormTitle = "编辑版本";
                }
            }
            if ("view".Equals(action)) {
                ShowFormTitle = "查看版本";
                ReadonlyClass = "border-0";
            }
            Project = _context.Projects.Where(t => t.IsPublic.Value).ToList();
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                ModelState.ClearValidationState(nameof(Version));
                if (!TryValidateModel(Version, nameof(Version))) {
                    return Page();
                }
            }

            if (Version.Id > 0) {  // 修改
                Version.UpdatedOn = DateTime.Now;
                //_context.Versions.Update(Version);
                _context.Attach(Version).State = EntityState.Modified;
            } else {    // 新增
                Version.CreatedOn = DateTime.Now;
                Version.Sharing = "none";
                _context.Versions.Add(Version);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("/Versions/Index");
        }
    }
}
