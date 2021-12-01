using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.DIP {
    public interface IGlobalSetting {
        Setting GetPerPageOptions();
        List<TrackerMapping> TrackerMapping();
        List<CustomFieldsMapping> CustomFieldsMappings();
    }
}
