using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Management {
    public class CustomFieldModel : PageModel {
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public CustomFieldModel(RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
        }

        public IList<CustomFieldsMapping> CustomFieldsMapping { get; set; }
        public IList<CustomField> CustomField { get; set; }
        public IList<string> CustomType { get; set; }

        public async Task OnGetAsync() {
            CustomFieldsMapping = await _context.CustomFieldsMappings.ToListAsync();
            CustomField = await _context.CustomFields.ToListAsync();

            CustomType = CustomField.Select(x => x.Type).Distinct().ToList();
        }
    }
}