using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class ChangesetParent
    {
        public int ChangesetId { get; set; }
        public int ParentId { get; set; }
    }
}
