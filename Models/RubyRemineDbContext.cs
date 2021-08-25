using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RubyMine.Models;
using Version = RubyMine.Models.Version;

#nullable disable

namespace RubyMine.DbContexts {
    public partial class RubyRemineDbContext : bitnami_redmineplusagileContext {
        public RubyRemineDbContext() {
        }

        public RubyRemineDbContext(DbContextOptions<bitnami_redmineplusagileContext> options)
            : base(options) {
        }
    }
}