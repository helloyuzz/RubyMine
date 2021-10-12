using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Modules {
    public class CreateModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public CreateModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }
        [BindProperty(SupportsGet =true)]
        public int Pid { get; set; }
        public IActionResult OnGet() {
            var pid = Request.Query["pid"];
            if (Module == null) {
                Module = new Module();
            }

            Module.PId = RMUtils.ParseInt(pid);

            var dbCount = _context.Modules.Count(t => t.PId == Module.PId);
            Module.Index = dbCount + 1;
            return Page();
        }

        [BindProperty]
        public Module Module { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }
            saveChange();
            return RedirectToPage("/Creation/Index");
        }
        public async Task<IActionResult> OnPostCreateAndNew() {
            if (!ModelState.IsValid) {
                return Page();
            }
            saveChange(true);
            return RedirectToPage("/Modules/Create", new { pid = Module.PId });
        }

        private async void saveChange(bool newAction = false) {
            PageResult pageResult = new PageResult();

            _context.Modules.Add(Module);
            await _context.SaveChangesAsync();
        }
    }
}