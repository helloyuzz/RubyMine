using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine {
    public class RMGlobal{
        public static List<CustomFieldsMapping> CustomFieldsMappings { get; set; }

        static string _RemineUrl = "";
        public static string RemineUrl {
            get {
                if (string.IsNullOrEmpty(_RemineUrl)) { 
                
                }
                return _RemineUrl;
            }
        }
    }
}
