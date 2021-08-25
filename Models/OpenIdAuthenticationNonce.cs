using System;
using System.Collections.Generic;

#nullable disable

namespace RubyMine.Models
{
    public partial class OpenIdAuthenticationNonce
    {
        public int Id { get; set; }
        public int Timestamp { get; set; }
        public string ServerUrl { get; set; }
        public string Salt { get; set; }
    }
}
