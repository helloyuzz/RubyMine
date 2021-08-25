using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models {
    public partial class Setting {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
