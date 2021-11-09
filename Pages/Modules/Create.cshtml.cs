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
        [BindProperty(SupportsGet = true)]
        public int Pid { get; set; }
        [BindProperty(SupportsGet = true)]
        public string UrlReferer { get; set; }
        public IActionResult OnGet() {
            var pid = Request.Query["pid"];
            UrlReferer = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(UrlReferer)) {
                UrlReferer = "/Platform";
            }
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
        public async Task<IActionResult> OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            _context.Modules.Add(Module);
            await _context.SaveChangesAsync();


            string urlReferer = Request.Form["UrlReferer"];

            if (string.IsNullOrEmpty(urlReferer) == false) {
                return Redirect(urlReferer);
            } else {
                return RedirectToPage("/Platform/Index");
            }            
        }
        public async Task<IActionResult> OnPostCreateAndNew() {
            if (!ModelState.IsValid) {
                return Page();
            }
            _context.Modules.Add(Module);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Modules/Create", new { pid = Module.PId });
        }
    }
}