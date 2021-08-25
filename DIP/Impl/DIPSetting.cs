using RubyMine.DbContexts;
using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RubyMine.DIP.Impl {
    public class DIPSetting : IDIPSetting {
        RubyRemineDbContext _context;
        public DIPSetting(RubyRemineDbContext context) {
            _context = context;
        }
        public Setting GetPerPageOptions() {
            return _context.Settings.FirstOrDefault(t => t.Name.Equals("per_page_options"));
        }

        List<CustomFieldsMapping> IDIPSetting.CustomFieldsMappings() {
            if (RMGlobal.CustomFieldsMappings == null) {
                RMGlobal.CustomFieldsMappings = _context.CustomFieldsMappings.ToList();
            }
            return RMGlobal.CustomFieldsMappings;
        }

        List<TrackerMapping> IDIPSetting.TrackerMapping() {
            return _context.TrackerMappings.Where(t => t.Id >= 1).ToList();
        }
    }
}
