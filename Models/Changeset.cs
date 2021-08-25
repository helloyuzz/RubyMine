using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class Changeset
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public string Revision { get; set; }
        public string Committer { get; set; }
        public DateTime CommittedOn { get; set; }
        public string Comments { get; set; }
        public DateTime? CommitDate { get; set; }
        public string Scmid { get; set; }
        public int? UserId { get; set; }
    }
}
