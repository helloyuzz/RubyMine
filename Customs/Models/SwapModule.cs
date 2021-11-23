using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Customs.Models {
    public class SwapModule {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Parent_id { get; set; }
        public string Action { get; set; }
    }
}
