using RubyMine.DbContexts;
using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RubyMine.DIP.Impl {
    public class GlobalSetting : IGlobalSetting {
        RubyRemineDbContext _context;
        public GlobalSetting(RubyRemineDbContext context) {
            _context = context;
        }
        public Setting GetPerPageOptions() {
            return _context.Settings.FirstOrDefault(t => t.Name.Equals("per_page_options"));
        }

        List<CustomFieldsMapping> IGlobalSetting.CustomFieldsMappings() {
            if (RMGlobal.CustomFieldsMappings == null) {
                RMGlobal.CustomFieldsMappings = _context.CustomFieldsMappings.ToList();
            }
            return RMGlobal.CustomFieldsMappings;
        }

        List<TrackerMapping> IGlobalSetting.TrackerMapping() {
            return _context.TrackerMappings.Where(t => t.Id >= 1).ToList();
        }
    }
}
