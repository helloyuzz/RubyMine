using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Watcher
    {
        public int Id { get; set; }
        public string WatchableType { get; set; }
        public int WatchableId { get; set; }
        public int? UserId { get; set; }
    }
}
