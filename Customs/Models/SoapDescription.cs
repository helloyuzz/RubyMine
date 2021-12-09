using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine.Customs.Models {
    public class SoapDescription {        
        public SoapDescription(string value) {
            result = value;
        }
        public string result { get; set; }
        public int issue_id { get; set; }
        public string description { get; set; }
        public List<Journal> Journals { get; set; }
        public List<JournalDetail> JournalDetails { get; set; }
        public string notes_tr { get; set; }
        public string history_tr { get; set; }
        public string properties_tr { get; set; }
        public string attachment_tr { get; set; }
        public int notes_length { get; set; }
        public int history_length { get; set; }
        public int property_length { get; set; }
        public int attachment_length { get; set; }
    }
}
