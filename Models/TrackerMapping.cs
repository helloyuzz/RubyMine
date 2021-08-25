using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models {
    public partial class TrackerMapping {
        public int Id { get; set; }
        public string TrackerKey { get; set; }
        public string TrackerName { get; set; }
        public int? TrackerId { get; set; }
    }
}