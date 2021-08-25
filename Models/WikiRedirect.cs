using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class WikiRedirect
    {
        public int Id { get; set; }
        public int WikiId { get; set; }
        public string Title { get; set; }
        public string RedirectsTo { get; set; }
        public DateTime CreatedOn { get; set; }
        public int RedirectsToWikiId { get; set; }
    }
}
