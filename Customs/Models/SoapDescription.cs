using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Customs.Models {
    public class SoapDescription {        
        public SoapDescription(string value) {
            Result = value;
        }
        public string Result { get; set; }
        public int Issue_id { get; set; }
        public string Description { get; set; }
        public List<Journal> Journals { get; set; }
        public List<JournalDetail> JournalDetails { get; set; }
        public string notes_tr { get; set; }
        public string history_tr { get; set; }
        public string properties_tr { get; set; }
        public string attachment_tr { get; set; }
    }
}
