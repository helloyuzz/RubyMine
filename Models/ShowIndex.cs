using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Models {
    public partial class ShowIndex {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ModuleId { get; set; }
        public int? CustomId { get; set; }
        public int? Position { get; set; }
    }
}
