using RubyMine.Customs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubyMine {
    public class GlobalCache {
        static Dictionary<int, ActiveNodes> _ActiveNodes = null;
        public static Dictionary<int, ActiveNodes> ActiveNodes {
            get {
                if (_ActiveNodes == null) {
                    _ActiveNodes = new Dictionary<int, ActiveNodes>();
                }
                return _ActiveNodes;
            }
        }
    }
}
