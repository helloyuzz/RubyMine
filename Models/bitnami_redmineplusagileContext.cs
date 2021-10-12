using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using RubyMine.Models;

#nullable disable

namespace RubyMine.DbContexts {
    public partial class RubyRemineDbContext : DbContext {
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        private readonly IConfiguration _configuration;
        public RubyRemineDbContext() { 
        }
        public RubyRemineDbContext(DbContextOptions<RubyRemineDbContext> options)
            : base(options) {
        }

        public virtual DbSet<ViewIssuetemp> ViewIssuetemps { get; set; }
        public virtual DbSet<AgileColor> AgileColors { get; set; }
        public virtual DbSet<AgileDatum> AgileData { get; set; }
        public virtual DbSet<AgileSprint> AgileSprints { get; set; }
        public virtual DbSet<ArInternalMetadatum> ArInternalMetadata { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AuthSource> AuthSources { get; set; }
        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<Change> Changes { get; set; }
        public virtual DbSet<Changeset> Changesets { get; set; }
        public virtual DbSet<ChangesetParent> ChangesetParents { get; set; }
        public virtual DbSet<ChangesetsIssue> ChangesetsIssues { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CustomField> CustomFields { get; set; }
        public virtual DbSet<CustomFieldEnumeration> CustomFieldEnumerations { get; set; }
        public virtual DbSet<CustomFieldsMapping> CustomFieldsMappings { get; set; }
        public virtual DbSet<CustomFieldsProject> CustomFieldsProjects { get; set; }
        public virtual DbSet<CustomFieldsRole> CustomFieldsRoles { get; set; }
        public virtual DbSet<CustomFieldsTracker> CustomFieldsTrackers { get; set; }
        public virtual DbSet<CustomValue> CustomValues { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<EmailAddress> EmailAddresses { get; set; }
        public virtual DbSet<EnabledModule> EnabledModules { get; set; }
        public virtual DbSet<Enumeration> Enumerations { get; set; }
        public virtual DbSet<GroupsUser> GroupsUsers { get; set; }
        public virtual DbSet<Import> Imports { get; set; }
        public virtual DbSet<ImportItem> ImportItems { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueCategory> IssueCategories { get; set; }
        public virtual DbSet<IssueRelation> IssueRelations { get; set; }
        public virtual DbSet<IssueStatus> IssueStatuses { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<JournalDetail> JournalDetails { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberRole> MemberRoles { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<OpenIdAuthenticationAssociation> OpenIdAuthenticationAssociations { get; set; }
        public virtual DbSet<OpenIdAuthenticationNonce> OpenIdAuthenticationNonces { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectsTracker> ProjectsTrackers { get; set; }
        public virtual DbSet<QueriesRole> QueriesRoles { get; set; }
        public virtual DbSet<Query> Queries { get; set; }
        public virtual DbSet<Repository> Repositories { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolesManagedRole> RolesManagedRoles { get; set; }
        public virtual DbSet<SchemaMigration> SchemaMigrations { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TimeEntry> TimeEntries { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<Tracker> Trackers { get; set; }
        public virtual DbSet<TrackerMapping> TrackerMappings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPreference> UserPreferences { get; set; }
        public virtual DbSet<RubyMine.Models.Version> Versions { get; set; }
        public virtual DbSet<Watcher> Watchers { get; set; }
        public virtual DbSet<Wiki> Wikis { get; set; }
        public virtual DbSet<WikiContent> WikiContents { get; set; }
        public virtual DbSet<WikiContentVersion> WikiContentVersions { get; set; }
        public virtual DbSet<WikiPage> WikiPages { get; set; }
        public virtual DbSet<WikiRedirect> WikiRedirects { get; set; }
        public virtual DbSet<Workflow> Workflows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging();
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseMySql("server=localhost;port=3306;database=bitnami_redmineplusagile;uid=bitnami;pwd=e32a9b7bd9;sslmode=None;allowpublickeyretrieval=true", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.32-mysql"));
                string db_connectionString = _configuration["ConnectionStrings:MySQL_product"];
                string db_version = _configuration["ConnectionStrings:Version"];
                optionsBuilder.UseMySql(db_connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse(db_version));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_unicode_ci");

            //modelBuilder.Ignore<IssusTemp>();
            //modelBuilder.Entity<IssusTemp>().ToView("view_issuetemp").HasKey(p => new { p.Id, p.Issue_id, p.cf_22, p.Tracker_id });

            modelBuilder.Entity<ViewIssuetemp>(entity => {
                entity.HasNoKey();

                entity.ToView("view_issuetemp");

                entity.Property(e => e.Cf22).HasColumnName("cf_22");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.IssueId)
                    .HasColumnType("int(11)")
                    .HasColumnName("issue_id");

                entity.Property(e => e.TrackerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("tracker_id");
            });

            modelBuilder.Entity<AgileColor>(entity => {
                entity.ToTable("agile_colors");

                entity.HasIndex(e => e.ContainerId, "index_agile_colors_on_container_id");

                entity.HasIndex(e => e.ContainerType, "index_agile_colors_on_container_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Color)
                    .HasMaxLength(255)
                    .HasColumnName("color");

                entity.Property(e => e.ContainerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("container_id");

                entity.Property(e => e.ContainerType).HasColumnName("container_type");
            });

            modelBuilder.Entity<AgileDatum>(entity => {
                entity.ToTable("agile_data");

                entity.HasIndex(e => e.IssueId, "index_agile_data_on_issue_id");

                entity.HasIndex(e => e.Position, "index_agile_data_on_position");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AgileSprintId)
                    .HasColumnType("int(11)")
                    .HasColumnName("agile_sprint_id");

                entity.Property(e => e.IssueId)
                    .HasColumnType("int(11)")
                    .HasColumnName("issue_id");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");

                entity.Property(e => e.StoryPoints)
                    .HasColumnType("int(11)")
                    .HasColumnName("story_points");
            });

            modelBuilder.Entity<AgileSprint>(entity => {
                entity.ToTable("agile_sprints");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("end_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasColumnName("status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");
            });

            modelBuilder.Entity<ArInternalMetadatum>(entity => {
                entity.HasKey(e => e.Key)
                    .HasName("PRIMARY");

                entity.ToTable("ar_internal_metadata");

                entity.Property(e => e.Key)
                    .HasColumnName("key")
                    .UseCollation("utf8_unicode_ci")
                    .HasCharSet("utf8");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Value)
                    .HasMaxLength(255)
                    .HasColumnName("value");
            });

            modelBuilder.Entity<Attachment>(entity => {
                entity.ToTable("attachments");

                entity.HasIndex(e => e.AuthorId, "index_attachments_on_author_id");

                entity.HasIndex(e => new { e.ContainerId, e.ContainerType }, "index_attachments_on_container_id_and_container_type");

                entity.HasIndex(e => e.CreatedOn, "index_attachments_on_created_on");

                entity.HasIndex(e => e.DiskFilename, "index_attachments_on_disk_filename");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.ContainerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("container_id");

                entity.Property(e => e.ContainerType)
                    .HasMaxLength(30)
                    .HasColumnName("container_type");

                entity.Property(e => e.ContentType)
                    .HasMaxLength(255)
                    .HasColumnName("content_type")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Digest)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("digest")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.DiskDirectory)
                    .HasMaxLength(255)
                    .HasColumnName("disk_directory");

                entity.Property(e => e.DiskFilename)
                    .IsRequired()
                    .HasColumnName("disk_filename")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Downloads)
                    .HasColumnType("int(11)")
                    .HasColumnName("downloads");

                entity.Property(e => e.Filename)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("filename")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Filesize)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("filesize");
            });

            modelBuilder.Entity<AuthSource>(entity => {
                entity.ToTable("auth_sources");

                entity.HasIndex(e => new { e.Id, e.Type }, "index_auth_sources_on_id_and_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Account)
                    .HasMaxLength(255)
                    .HasColumnName("account");

                entity.Property(e => e.AccountPassword)
                    .HasMaxLength(255)
                    .HasColumnName("account_password")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.AttrFirstname)
                    .HasMaxLength(30)
                    .HasColumnName("attr_firstname");

                entity.Property(e => e.AttrLastname)
                    .HasMaxLength(30)
                    .HasColumnName("attr_lastname");

                entity.Property(e => e.AttrLogin)
                    .HasMaxLength(30)
                    .HasColumnName("attr_login");

                entity.Property(e => e.AttrMail)
                    .HasMaxLength(30)
                    .HasColumnName("attr_mail");

                entity.Property(e => e.BaseDn)
                    .HasMaxLength(255)
                    .HasColumnName("base_dn");

                entity.Property(e => e.Filter)
                    .HasColumnType("text")
                    .HasColumnName("filter");

                entity.Property(e => e.Host)
                    .HasMaxLength(60)
                    .HasColumnName("host");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.OntheflyRegister).HasColumnName("onthefly_register");

                entity.Property(e => e.Port)
                    .HasColumnType("int(11)")
                    .HasColumnName("port");

                entity.Property(e => e.Timeout)
                    .HasColumnType("int(11)")
                    .HasColumnName("timeout");

                entity.Property(e => e.Tls).HasColumnName("tls");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("type")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.VerifyPeer)
                    .IsRequired()
                    .HasColumnName("verify_peer")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<Board>(entity => {
                entity.ToTable("boards");

                entity.HasIndex(e => e.ProjectId, "boards_project_id");

                entity.HasIndex(e => e.LastMessageId, "index_boards_on_last_message_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.LastMessageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("last_message_id");

                entity.Property(e => e.MessagesCount)
                    .HasColumnType("int(11)")
                    .HasColumnName("messages_count");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.TopicsCount)
                    .HasColumnType("int(11)")
                    .HasColumnName("topics_count");
            });

            modelBuilder.Entity<Change>(entity => {
                entity.ToTable("changes");

                entity.HasIndex(e => e.ChangesetId, "changesets_changeset_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("action")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Branch)
                    .HasMaxLength(255)
                    .HasColumnName("branch");

                entity.Property(e => e.ChangesetId)
                    .HasColumnType("int(11)")
                    .HasColumnName("changeset_id");

                entity.Property(e => e.FromPath)
                    .HasColumnType("text")
                    .HasColumnName("from_path");

                entity.Property(e => e.FromRevision)
                    .HasMaxLength(255)
                    .HasColumnName("from_revision");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("path");

                entity.Property(e => e.Revision)
                    .HasMaxLength(255)
                    .HasColumnName("revision");
            });

            modelBuilder.Entity<Changeset>(entity => {
                entity.ToTable("changesets");

                entity.HasIndex(e => new { e.RepositoryId, e.Revision }, "changesets_repos_rev")
                    .IsUnique();

                entity.HasIndex(e => new { e.RepositoryId, e.Scmid }, "changesets_repos_scmid");

                entity.HasIndex(e => e.CommittedOn, "index_changesets_on_committed_on");

                entity.HasIndex(e => e.RepositoryId, "index_changesets_on_repository_id");

                entity.HasIndex(e => e.UserId, "index_changesets_on_user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Comments).HasColumnName("comments");

                entity.Property(e => e.CommitDate)
                    .HasColumnType("date")
                    .HasColumnName("commit_date");

                entity.Property(e => e.CommittedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("committed_on");

                entity.Property(e => e.Committer)
                    .HasMaxLength(255)
                    .HasColumnName("committer");

                entity.Property(e => e.RepositoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("repository_id");

                entity.Property(e => e.Revision)
                    .IsRequired()
                    .HasColumnName("revision");

                entity.Property(e => e.Scmid).HasColumnName("scmid");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<ChangesetParent>(entity => {
                entity.HasNoKey();

                entity.ToTable("changeset_parents");

                entity.HasIndex(e => e.ChangesetId, "changeset_parents_changeset_ids");

                entity.HasIndex(e => e.ParentId, "changeset_parents_parent_ids");

                entity.Property(e => e.ChangesetId)
                    .HasColumnType("int(11)")
                    .HasColumnName("changeset_id");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");
            });

            modelBuilder.Entity<ChangesetsIssue>(entity => {
                entity.HasNoKey();

                entity.ToTable("changesets_issues");

                entity.HasIndex(e => new { e.ChangesetId, e.IssueId }, "changesets_issues_ids")
                    .IsUnique();

                entity.HasIndex(e => e.IssueId, "index_changesets_issues_on_issue_id");

                entity.Property(e => e.ChangesetId)
                    .HasColumnType("int(11)")
                    .HasColumnName("changeset_id");

                entity.Property(e => e.IssueId)
                    .HasColumnType("int(11)")
                    .HasColumnName("issue_id");
            });

            modelBuilder.Entity<Comment>(entity => {
                entity.ToTable("comments");

                entity.HasIndex(e => e.AuthorId, "index_comments_on_author_id");

                entity.HasIndex(e => new { e.CommentedId, e.CommentedType }, "index_comments_on_commented_id_and_commented_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.CommentedId)
                    .HasColumnType("int(11)")
                    .HasColumnName("commented_id");

                entity.Property(e => e.CommentedType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("commented_type")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_on");
            });

            modelBuilder.Entity<CustomField>(entity => {
                entity.ToTable("custom_fields");

                entity.HasIndex(e => new { e.Id, e.Type }, "index_custom_fields_on_id_and_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.DefaultValue)
                    .HasColumnType("text")
                    .HasColumnName("default_value");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Editable)
                    .HasColumnName("editable")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.FieldFormat)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("field_format")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.FormatStore)
                    .HasColumnType("text")
                    .HasColumnName("format_store");

                entity.Property(e => e.IsFilter).HasColumnName("is_filter");

                entity.Property(e => e.IsForAll).HasColumnName("is_for_all");

                entity.Property(e => e.IsRequired).HasColumnName("is_required");

                entity.Property(e => e.MaxLength)
                    .HasColumnType("int(11)")
                    .HasColumnName("max_length");

                entity.Property(e => e.MinLength)
                    .HasColumnType("int(11)")
                    .HasColumnName("min_length");

                entity.Property(e => e.Multiple)
                    .HasColumnName("multiple")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");

                entity.Property(e => e.PossibleValues)
                    .HasColumnType("text")
                    .HasColumnName("possible_values");

                entity.Property(e => e.Regexp)
                    .HasMaxLength(255)
                    .HasColumnName("regexp")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Searchable)
                    .HasColumnName("searchable")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("type")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Visible)
                    .IsRequired()
                    .HasColumnName("visible")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<CustomFieldEnumeration>(entity => {
                entity.ToTable("custom_field_enumerations");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.CustomFieldId)
                    .HasColumnType("int(11)")
                    .HasColumnName("custom_field_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<CustomFieldsMapping>(entity => {
                entity.ToTable("custom_fields_mapping");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CustomFieldId)
                    .HasColumnType("int(11)")
                    .HasColumnName("custom_field_id");

                entity.Property(e => e.CustomKey)
                    .HasMaxLength(255)
                    .HasColumnName("custom_key");

                entity.Property(e => e.CustomName)
                    .HasMaxLength(255)
                    .HasColumnName("custom_name");
            });

            modelBuilder.Entity<CustomFieldsProject>(entity => {
                entity.HasNoKey();

                entity.ToTable("custom_fields_projects");

                entity.HasIndex(e => new { e.CustomFieldId, e.ProjectId }, "index_custom_fields_projects_on_custom_field_id_and_project_id")
                    .IsUnique();

                entity.Property(e => e.CustomFieldId)
                    .HasColumnType("int(11)")
                    .HasColumnName("custom_field_id");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");
            });

            modelBuilder.Entity<CustomFieldsRole>(entity => {
                entity.HasNoKey();

                entity.ToTable("custom_fields_roles");

                entity.HasIndex(e => new { e.CustomFieldId, e.RoleId }, "custom_fields_roles_ids")
                    .IsUnique();

                entity.Property(e => e.CustomFieldId)
                    .HasColumnType("int(11)")
                    .HasColumnName("custom_field_id");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("role_id");
            });

            modelBuilder.Entity<CustomFieldsTracker>(entity => {
                entity.HasNoKey();

                entity.ToTable("custom_fields_trackers");

                entity.HasIndex(e => new { e.CustomFieldId, e.TrackerId }, "index_custom_fields_trackers_on_custom_field_id_and_tracker_id")
                    .IsUnique();

                entity.Property(e => e.CustomFieldId)
                    .HasColumnType("int(11)")
                    .HasColumnName("custom_field_id");

                entity.Property(e => e.TrackerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("tracker_id");
            });

            modelBuilder.Entity<CustomValue>(entity => {
                entity.ToTable("custom_values");

                entity.HasIndex(e => new { e.CustomizedType, e.CustomizedId }, "custom_values_customized");

                entity.HasIndex(e => e.CustomFieldId, "index_custom_values_on_custom_field_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CustomFieldId)
                    .HasColumnType("int(11)")
                    .HasColumnName("custom_field_id");

                entity.Property(e => e.CustomizedId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customized_id");

                entity.Property(e => e.CustomizedType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("customized_type")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<Document>(entity => {
                entity.ToTable("documents");

                entity.HasIndex(e => e.ProjectId, "documents_project_id");

                entity.HasIndex(e => e.CategoryId, "index_documents_on_category_id");

                entity.HasIndex(e => e.CreatedOn, "index_documents_on_created_on");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("category_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("title")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<EmailAddress>(entity => {
                entity.ToTable("email_addresses");

                entity.HasIndex(e => e.UserId, "index_email_addresses_on_user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Notify)
                    .IsRequired()
                    .HasColumnName("notify")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_on");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<EnabledModule>(entity => {
                entity.ToTable("enabled_modules");

                entity.HasIndex(e => e.ProjectId, "enabled_modules_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");
            });

            modelBuilder.Entity<Enumeration>(entity => {
                entity.ToTable("enumerations");

                entity.HasIndex(e => new { e.Id, e.Type }, "index_enumerations_on_id_and_type");

                entity.HasIndex(e => e.ProjectId, "index_enumerations_on_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.IsDefault).HasColumnName("is_default");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");

                entity.Property(e => e.PositionName)
                    .HasMaxLength(30)
                    .HasColumnName("position_name");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<GroupsUser>(entity => {
                entity.HasNoKey();

                entity.ToTable("groups_users");

                entity.HasIndex(e => new { e.GroupId, e.UserId }, "groups_users_ids")
                    .IsUnique();

                entity.Property(e => e.GroupId)
                    .HasColumnType("int(11)")
                    .HasColumnName("group_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<Import>(entity => {
                entity.ToTable("imports");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Filename)
                    .HasMaxLength(255)
                    .HasColumnName("filename");

                entity.Property(e => e.Finished).HasColumnName("finished");

                entity.Property(e => e.Settings)
                    .HasColumnType("text")
                    .HasColumnName("settings");

                entity.Property(e => e.TotalItems)
                    .HasColumnType("int(11)")
                    .HasColumnName("total_items");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<ImportItem>(entity => {
                entity.ToTable("import_items");

                entity.HasIndex(e => new { e.ImportId, e.UniqueId }, "index_import_items_on_import_id_and_unique_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ImportId)
                    .HasColumnType("int(11)")
                    .HasColumnName("import_id");

                entity.Property(e => e.Message)
                    .HasColumnType("text")
                    .HasColumnName("message");

                entity.Property(e => e.ObjId)
                    .HasColumnType("int(11)")
                    .HasColumnName("obj_id");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");

                entity.Property(e => e.UniqueId).HasColumnName("unique_id");
            });

            modelBuilder.Entity<Issue>(entity => {
                entity.ToTable("issues");

                entity.HasIndex(e => e.AssignedToId, "index_issues_on_assigned_to_id");

                entity.HasIndex(e => e.AuthorId, "index_issues_on_author_id");

                entity.HasIndex(e => e.CategoryId, "index_issues_on_category_id");

                entity.HasIndex(e => e.CreatedOn, "index_issues_on_created_on");

                entity.HasIndex(e => e.FixedVersionId, "index_issues_on_fixed_version_id");

                entity.HasIndex(e => e.ParentId, "index_issues_on_parent_id");

                entity.HasIndex(e => e.PriorityId, "index_issues_on_priority_id");

                entity.HasIndex(e => new { e.RootId, e.Lft, e.Rgt }, "index_issues_on_root_id_and_lft_and_rgt");

                entity.HasIndex(e => e.StatusId, "index_issues_on_status_id");

                entity.HasIndex(e => e.TrackerId, "index_issues_on_tracker_id");

                entity.HasIndex(e => e.ProjectId, "issues_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AssignedToId)
                    .HasColumnType("int(11)")
                    .HasColumnName("assigned_to_id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.CategoryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("category_id");

                entity.Property(e => e.ClosedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("closed_on");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DoneRatio)
                    .HasColumnType("int(11)")
                    .HasColumnName("done_ratio");

                entity.Property(e => e.DueDate)
                    .HasColumnType("date")
                    .HasColumnName("due_date");

                entity.Property(e => e.EstimatedHours).HasColumnName("estimated_hours");

                entity.Property(e => e.FixedVersionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("fixed_version_id");

                entity.Property(e => e.IsPrivate).HasColumnName("is_private");

                entity.Property(e => e.Lft)
                    .HasColumnType("int(11)")
                    .HasColumnName("lft");

                entity.Property(e => e.LockVersion)
                    .HasColumnType("int(11)")
                    .HasColumnName("lock_version");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.PriorityId)
                    .HasColumnType("int(11)")
                    .HasColumnName("priority_id");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.Rgt)
                    .HasColumnType("int(11)")
                    .HasColumnName("rgt");

                entity.Property(e => e.RootId)
                    .HasColumnType("int(11)")
                    .HasColumnName("root_id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("start_date");

                entity.Property(e => e.StatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("status_id");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("subject")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.TrackerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("tracker_id");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("updated_on");
            });

            modelBuilder.Entity<IssueCategory>(entity => {
                entity.ToTable("issue_categories");

                entity.HasIndex(e => e.AssignedToId, "index_issue_categories_on_assigned_to_id");

                entity.HasIndex(e => e.ProjectId, "issue_categories_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AssignedToId)
                    .HasColumnType("int(11)")
                    .HasColumnName("assigned_to_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");
            });

            modelBuilder.Entity<IssueRelation>(entity => {
                entity.ToTable("issue_relations");

                entity.HasIndex(e => e.IssueFromId, "index_issue_relations_on_issue_from_id");

                entity.HasIndex(e => new { e.IssueFromId, e.IssueToId }, "index_issue_relations_on_issue_from_id_and_issue_to_id")
                    .IsUnique();

                entity.HasIndex(e => e.IssueToId, "index_issue_relations_on_issue_to_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Delay)
                    .HasColumnType("int(11)")
                    .HasColumnName("delay");

                entity.Property(e => e.IssueFromId)
                    .HasColumnType("int(11)")
                    .HasColumnName("issue_from_id");

                entity.Property(e => e.IssueToId)
                    .HasColumnType("int(11)")
                    .HasColumnName("issue_to_id");

                entity.Property(e => e.RelationType)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("relation_type")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<IssueStatus>(entity => {
                entity.ToTable("issue_statuses");

                entity.HasIndex(e => e.IsClosed, "index_issue_statuses_on_is_closed");

                entity.HasIndex(e => e.Position, "index_issue_statuses_on_position");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.DefaultDoneRatio)
                    .HasColumnType("int(11)")
                    .HasColumnName("default_done_ratio");

                entity.Property(e => e.IsClosed).HasColumnName("is_closed");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");
            });

            modelBuilder.Entity<Journal>(entity => {
                entity.ToTable("journals");

                entity.HasIndex(e => e.CreatedOn, "index_journals_on_created_on");

                entity.HasIndex(e => e.JournalizedId, "index_journals_on_journalized_id");

                entity.HasIndex(e => e.UserId, "index_journals_on_user_id");

                entity.HasIndex(e => new { e.JournalizedId, e.JournalizedType }, "journals_journalized_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.JournalizedId)
                    .HasColumnType("int(11)")
                    .HasColumnName("journalized_id");

                entity.Property(e => e.JournalizedType)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("journalized_type")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.PrivateNotes).HasColumnName("private_notes");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<JournalDetail>(entity => {
                entity.ToTable("journal_details");

                entity.HasIndex(e => e.JournalId, "journal_details_journal_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.JournalId)
                    .HasColumnType("int(11)")
                    .HasColumnName("journal_id");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.PropKey)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("prop_key")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Property)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("property")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<Member>(entity => {
                entity.ToTable("members");

                entity.HasIndex(e => e.ProjectId, "index_members_on_project_id");

                entity.HasIndex(e => e.UserId, "index_members_on_user_id");

                entity.HasIndex(e => new { e.UserId, e.ProjectId }, "index_members_on_user_id_and_project_id")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.MailNotification).HasColumnName("mail_notification");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<MemberRole>(entity => {
                entity.ToTable("member_roles");

                entity.HasIndex(e => e.InheritedFrom, "index_member_roles_on_inherited_from");

                entity.HasIndex(e => e.MemberId, "index_member_roles_on_member_id");

                entity.HasIndex(e => e.RoleId, "index_member_roles_on_role_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.InheritedFrom)
                    .HasColumnType("int(11)")
                    .HasColumnName("inherited_from");

                entity.Property(e => e.MemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("member_id");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("role_id");
            });

            modelBuilder.Entity<Message>(entity => {
                entity.ToTable("messages");

                entity.HasIndex(e => e.AuthorId, "index_messages_on_author_id");

                entity.HasIndex(e => e.CreatedOn, "index_messages_on_created_on");

                entity.HasIndex(e => e.LastReplyId, "index_messages_on_last_reply_id");

                entity.HasIndex(e => e.BoardId, "messages_board_id");

                entity.HasIndex(e => e.ParentId, "messages_parent_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.BoardId)
                    .HasColumnType("int(11)")
                    .HasColumnName("board_id");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.LastReplyId)
                    .HasColumnType("int(11)")
                    .HasColumnName("last_reply_id");

                entity.Property(e => e.Locked)
                    .HasColumnName("locked")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.RepliesCount)
                    .HasColumnType("int(11)")
                    .HasColumnName("replies_count");

                entity.Property(e => e.Sticky)
                    .HasColumnType("int(11)")
                    .HasColumnName("sticky")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("subject")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_on");
            });

            modelBuilder.Entity<Module>(entity => {
                entity.ToTable("modules");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Index)
                    .HasColumnType("int(11)")
                    .HasColumnName("index");

                entity.Property(e => e.Name)
                    .HasMaxLength(60)
                    .HasColumnName("name");

                entity.Property(e => e.Pinyin)
                    .HasMaxLength(100)
                    .HasColumnName("pinyin");

                entity.Property(e => e.Disabled)
                    .HasColumnType("bit")
                    .HasColumnName("disabled");

                entity.Property(e => e.PId)
                    .HasColumnType("int(11)")
                    .HasColumnName("p_id");
            });

            modelBuilder.Entity<News>(entity => {
                entity.ToTable("news");

                entity.HasIndex(e => e.AuthorId, "index_news_on_author_id");

                entity.HasIndex(e => e.CreatedOn, "index_news_on_created_on");

                entity.HasIndex(e => e.ProjectId, "news_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.CommentsCount)
                    .HasColumnType("int(11)")
                    .HasColumnName("comments_count");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.Summary)
                    .HasMaxLength(255)
                    .HasColumnName("summary")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("title")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<OpenIdAuthenticationAssociation>(entity => {
                entity.ToTable("open_id_authentication_associations");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AssocType)
                    .HasMaxLength(255)
                    .HasColumnName("assoc_type");

                entity.Property(e => e.Handle)
                    .HasMaxLength(255)
                    .HasColumnName("handle");

                entity.Property(e => e.Issued)
                    .HasColumnType("int(11)")
                    .HasColumnName("issued");

                entity.Property(e => e.Lifetime)
                    .HasColumnType("int(11)")
                    .HasColumnName("lifetime");

                entity.Property(e => e.Secret)
                    .HasColumnType("blob")
                    .HasColumnName("secret");

                entity.Property(e => e.ServerUrl)
                    .HasColumnType("blob")
                    .HasColumnName("server_url");
            });

            modelBuilder.Entity<OpenIdAuthenticationNonce>(entity => {
                entity.ToTable("open_id_authentication_nonces");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("salt");

                entity.Property(e => e.ServerUrl)
                    .HasMaxLength(255)
                    .HasColumnName("server_url");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("int(11)")
                    .HasColumnName("timestamp");
            });

            modelBuilder.Entity<Project>(entity => {
                entity.ToTable("projects");

                entity.HasIndex(e => e.Lft, "index_projects_on_lft");

                entity.HasIndex(e => e.Rgt, "index_projects_on_rgt");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.DefaultAssignedToId)
                    .HasColumnType("int(11)")
                    .HasColumnName("default_assigned_to_id");

                entity.Property(e => e.DefaultVersionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("default_version_id");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Homepage)
                    .HasMaxLength(255)
                    .HasColumnName("homepage")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(255)
                    .HasColumnName("identifier");

                entity.Property(e => e.InheritMembers).HasColumnName("inherit_members");

                entity.Property(e => e.IsPublic)
                    .IsRequired()
                    .HasColumnName("is_public")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Lft)
                    .HasColumnType("int(11)")
                    .HasColumnName("lft");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.Rgt)
                    .HasColumnType("int(11)")
                    .HasColumnName("rgt");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("updated_on");
            });

            modelBuilder.Entity<ProjectsTracker>(entity => {
                entity.HasNoKey();

                entity.ToTable("projects_trackers");

                entity.HasIndex(e => e.ProjectId, "projects_trackers_project_id");

                entity.HasIndex(e => new { e.ProjectId, e.TrackerId }, "projects_trackers_unique")
                    .IsUnique();

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.TrackerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("tracker_id");
            });

            modelBuilder.Entity<QueriesRole>(entity => {
                entity.HasNoKey();

                entity.ToTable("queries_roles");

                entity.HasIndex(e => new { e.QueryId, e.RoleId }, "queries_roles_ids")
                    .IsUnique();

                entity.Property(e => e.QueryId)
                    .HasColumnType("int(11)")
                    .HasColumnName("query_id");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("role_id");
            });

            modelBuilder.Entity<Query>(entity => {
                entity.ToTable("queries");

                entity.HasIndex(e => e.ProjectId, "index_queries_on_project_id");

                entity.HasIndex(e => e.UserId, "index_queries_on_user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ColumnNames)
                    .HasColumnType("text")
                    .HasColumnName("column_names");

                entity.Property(e => e.Filters)
                    .HasColumnType("text")
                    .HasColumnName("filters");

                entity.Property(e => e.GroupBy)
                    .HasMaxLength(255)
                    .HasColumnName("group_by");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Options)
                    .HasColumnType("text")
                    .HasColumnName("options");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.SortCriteria)
                    .HasColumnType("text")
                    .HasColumnName("sort_criteria");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.Property(e => e.Visibility)
                    .HasColumnType("int(11)")
                    .HasColumnName("visibility")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Repository>(entity => {
                entity.ToTable("repositories");

                entity.HasIndex(e => e.ProjectId, "index_repositories_on_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.ExtraInfo).HasColumnName("extra_info");

                entity.Property(e => e.Identifier)
                    .HasMaxLength(255)
                    .HasColumnName("identifier");

                entity.Property(e => e.IsDefault)
                    .HasColumnName("is_default")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LogEncoding)
                    .HasMaxLength(64)
                    .HasColumnName("log_encoding");

                entity.Property(e => e.Login)
                    .HasMaxLength(60)
                    .HasColumnName("login")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PathEncoding)
                    .HasMaxLength(64)
                    .HasColumnName("path_encoding");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.RootUrl)
                    .HasMaxLength(255)
                    .HasColumnName("root_url")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Type)
                    .HasMaxLength(255)
                    .HasColumnName("type");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("url")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<Role>(entity => {
                entity.ToTable("roles");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AllRolesManaged)
                    .IsRequired()
                    .HasColumnName("all_roles_managed")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Assignable)
                    .HasColumnName("assignable")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Builtin)
                    .HasColumnType("int(11)")
                    .HasColumnName("builtin");

                entity.Property(e => e.IssuesVisibility)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("issues_visibility")
                    .HasDefaultValueSql("'default'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Permissions)
                    .HasColumnType("text")
                    .HasColumnName("permissions");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");

                entity.Property(e => e.Settings)
                    .HasColumnType("text")
                    .HasColumnName("settings");

                entity.Property(e => e.TimeEntriesVisibility)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("time_entries_visibility")
                    .HasDefaultValueSql("'all'");

                entity.Property(e => e.UsersVisibility)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("users_visibility")
                    .HasDefaultValueSql("'all'");
            });

            modelBuilder.Entity<RolesManagedRole>(entity => {
                entity.HasNoKey();

                entity.ToTable("roles_managed_roles");

                entity.HasIndex(e => new { e.RoleId, e.ManagedRoleId }, "index_roles_managed_roles_on_role_id_and_managed_role_id")
                    .IsUnique();

                entity.Property(e => e.ManagedRoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("managed_role_id");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("role_id");
            });

            modelBuilder.Entity<SchemaMigration>(entity => {
                entity.HasKey(e => e.Version)
                    .HasName("PRIMARY");

                entity.ToTable("schema_migrations");

                entity.Property(e => e.Version)
                    .HasColumnName("version")
                    .UseCollation("utf8_unicode_ci")
                    .HasCharSet("utf8");
            });

            modelBuilder.Entity<Setting>(entity => {
                entity.ToTable("settings");

                entity.HasIndex(e => e.Name, "index_settings_on_name");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("updated_on");

                entity.Property(e => e.Value)
                    .HasColumnType("text")
                    .HasColumnName("value");
            });

            modelBuilder.Entity<TimeEntry>(entity => {
                entity.ToTable("time_entries");

                entity.HasIndex(e => e.ActivityId, "index_time_entries_on_activity_id");

                entity.HasIndex(e => e.CreatedOn, "index_time_entries_on_created_on");

                entity.HasIndex(e => e.UserId, "index_time_entries_on_user_id");

                entity.HasIndex(e => e.IssueId, "time_entries_issue_id");

                entity.HasIndex(e => e.ProjectId, "time_entries_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ActivityId)
                    .HasColumnType("int(11)")
                    .HasColumnName("activity_id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.Comments)
                    .HasMaxLength(1024)
                    .HasColumnName("comments");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.Hours).HasColumnName("hours");

                entity.Property(e => e.IssueId)
                    .HasColumnType("int(11)")
                    .HasColumnName("issue_id");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.SpentOn)
                    .HasColumnType("date")
                    .HasColumnName("spent_on");

                entity.Property(e => e.Tmonth)
                    .HasColumnType("int(11)")
                    .HasColumnName("tmonth");

                entity.Property(e => e.Tweek)
                    .HasColumnType("int(11)")
                    .HasColumnName("tweek");

                entity.Property(e => e.Tyear)
                    .HasColumnType("int(11)")
                    .HasColumnName("tyear");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_on");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<Token>(entity => {
                entity.ToTable("tokens");

                entity.HasIndex(e => e.UserId, "index_tokens_on_user_id");

                entity.HasIndex(e => e.Value, "tokens_value")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("action")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("updated_on");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("value")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<Tracker>(entity => {
                entity.ToTable("trackers");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.DefaultStatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("default_status_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.FieldsBits)
                    .HasColumnType("int(11)")
                    .HasColumnName("fields_bits")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsInChlog).HasColumnName("is_in_chlog");

                entity.Property(e => e.IsInRoadmap)
                    .IsRequired()
                    .HasColumnName("is_in_roadmap")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Position)
                    .HasColumnType("int(11)")
                    .HasColumnName("position");
            });

            modelBuilder.Entity<TrackerMapping>(entity => {
                entity.ToTable("tracker_mapping");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.TrackerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("tracker_id");

                entity.Property(e => e.TrackerKey)
                    .HasMaxLength(255)
                    .HasColumnName("tracker_key");

                entity.Property(e => e.TrackerName)
                    .HasMaxLength(255)
                    .HasColumnName("tracker_name");
            });

            modelBuilder.Entity<User>(entity => {
                entity.ToTable("users");

                entity.HasIndex(e => e.AuthSourceId, "index_users_on_auth_source_id");

                entity.HasIndex(e => new { e.Id, e.Type }, "index_users_on_id_and_type");

                entity.HasIndex(e => e.Type, "index_users_on_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Admin).HasColumnName("admin");

                entity.Property(e => e.AuthSourceId)
                    .HasColumnType("int(11)")
                    .HasColumnName("auth_source_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("firstname")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.HashedPassword)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("hashed_password")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.IdentityUrl)
                    .HasMaxLength(255)
                    .HasColumnName("identity_url");

                entity.Property(e => e.Language)
                    .HasMaxLength(5)
                    .HasColumnName("language")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.LastLoginOn)
                    .HasColumnType("datetime")
                    .HasColumnName("last_login_on");

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("lastname")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("login")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.MailNotification)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("mail_notification")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.MustChangePasswd).HasColumnName("must_change_passwd");

                entity.Property(e => e.PasswdChangedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("passwd_changed_on");

                entity.Property(e => e.Salt)
                    .HasMaxLength(64)
                    .HasColumnName("salt");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("updated_on");
            });

            modelBuilder.Entity<UserPreference>(entity => {
                entity.ToTable("user_preferences");

                entity.HasIndex(e => e.UserId, "index_user_preferences_on_user_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.HideMail)
                    .HasColumnName("hide_mail")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Others)
                    .HasColumnType("text")
                    .HasColumnName("others");

                entity.Property(e => e.TimeZone)
                    .HasMaxLength(255)
                    .HasColumnName("time_zone");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");
            });

            modelBuilder.Entity<RubyMine.Models.Version>(entity => {
                entity.ToTable("versions");

                entity.HasIndex(e => e.Sharing, "index_versions_on_sharing");

                entity.HasIndex(e => e.ProjectId, "versions_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_on");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("date")
                    .HasColumnName("effective_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("name")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.Sharing)
                    .IsRequired()
                    .HasColumnName("sharing")
                    .HasDefaultValueSql("'none'");

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .HasColumnName("status")
                    .HasDefaultValueSql("'open'");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("timestamp")
                    .HasColumnName("updated_on");

                entity.Property(e => e.WikiPageTitle)
                    .HasMaxLength(255)
                    .HasColumnName("wiki_page_title");
            });

            modelBuilder.Entity<Watcher>(entity => {
                entity.ToTable("watchers");

                entity.HasIndex(e => e.UserId, "index_watchers_on_user_id");

                entity.HasIndex(e => new { e.WatchableId, e.WatchableType }, "index_watchers_on_watchable_id_and_watchable_type");

                entity.HasIndex(e => new { e.UserId, e.WatchableType }, "watchers_user_id_type");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.UserId)
                    .HasColumnType("int(11)")
                    .HasColumnName("user_id");

                entity.Property(e => e.WatchableId)
                    .HasColumnType("int(11)")
                    .HasColumnName("watchable_id");

                entity.Property(e => e.WatchableType)
                    .IsRequired()
                    .HasColumnName("watchable_type")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<Wiki>(entity => {
                entity.ToTable("wikis");

                entity.HasIndex(e => e.ProjectId, "wikis_project_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("int(11)")
                    .HasColumnName("project_id");

                entity.Property(e => e.StartPage)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("start_page");

                entity.Property(e => e.Status)
                    .HasColumnType("int(11)")
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");
            });

            modelBuilder.Entity<WikiContent>(entity => {
                entity.ToTable("wiki_contents");

                entity.HasIndex(e => e.AuthorId, "index_wiki_contents_on_author_id");

                entity.HasIndex(e => e.PageId, "wiki_contents_page_id");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.Comments)
                    .HasMaxLength(1024)
                    .HasColumnName("comments")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.PageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("page_id");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_on");

                entity.Property(e => e.Version)
                    .HasColumnType("int(11)")
                    .HasColumnName("version");
            });

            modelBuilder.Entity<WikiContentVersion>(entity => {
                entity.ToTable("wiki_content_versions");

                entity.HasIndex(e => e.UpdatedOn, "index_wiki_content_versions_on_updated_on");

                entity.HasIndex(e => e.WikiContentId, "wiki_content_versions_wcid");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("author_id");

                entity.Property(e => e.Comments)
                    .HasMaxLength(1024)
                    .HasColumnName("comments")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Compression)
                    .HasMaxLength(6)
                    .HasColumnName("compression")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.PageId)
                    .HasColumnType("int(11)")
                    .HasColumnName("page_id");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_on");

                entity.Property(e => e.Version)
                    .HasColumnType("int(11)")
                    .HasColumnName("version");

                entity.Property(e => e.WikiContentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("wiki_content_id");
            });

            modelBuilder.Entity<WikiPage>(entity => {
                entity.ToTable("wiki_pages");

                entity.HasIndex(e => e.ParentId, "index_wiki_pages_on_parent_id");

                entity.HasIndex(e => e.WikiId, "index_wiki_pages_on_wiki_id");

                entity.HasIndex(e => new { e.WikiId, e.Title }, "wiki_pages_wiki_id_title");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.ParentId)
                    .HasColumnType("int(11)")
                    .HasColumnName("parent_id");

                entity.Property(e => e.Protected).HasColumnName("protected");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title");

                entity.Property(e => e.WikiId)
                    .HasColumnType("int(11)")
                    .HasColumnName("wiki_id");
            });

            modelBuilder.Entity<WikiRedirect>(entity => {
                entity.ToTable("wiki_redirects");

                entity.HasIndex(e => e.WikiId, "index_wiki_redirects_on_wiki_id");

                entity.HasIndex(e => new { e.WikiId, e.Title }, "wiki_redirects_wiki_id_title");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.RedirectsTo)
                    .HasMaxLength(255)
                    .HasColumnName("redirects_to");

                entity.Property(e => e.RedirectsToWikiId)
                    .HasColumnType("int(11)")
                    .HasColumnName("redirects_to_wiki_id");

                entity.Property(e => e.Title).HasColumnName("title");

                entity.Property(e => e.WikiId)
                    .HasColumnType("int(11)")
                    .HasColumnName("wiki_id");
            });

            modelBuilder.Entity<Workflow>(entity => {
                entity.ToTable("workflows");

                entity.HasIndex(e => e.NewStatusId, "index_workflows_on_new_status_id");

                entity.HasIndex(e => e.OldStatusId, "index_workflows_on_old_status_id");

                entity.HasIndex(e => e.RoleId, "index_workflows_on_role_id");

                entity.HasIndex(e => e.TrackerId, "index_workflows_on_tracker_id");

                entity.HasIndex(e => new { e.RoleId, e.TrackerId, e.OldStatusId }, "wkfs_role_tracker_old_status");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Assignee).HasColumnName("assignee");

                entity.Property(e => e.Author).HasColumnName("author");

                entity.Property(e => e.FieldName)
                    .HasMaxLength(30)
                    .HasColumnName("field_name");

                entity.Property(e => e.NewStatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("new_status_id");

                entity.Property(e => e.OldStatusId)
                    .HasColumnType("int(11)")
                    .HasColumnName("old_status_id");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int(11)")
                    .HasColumnName("role_id");

                entity.Property(e => e.Rule)
                    .HasMaxLength(30)
                    .HasColumnName("rule");

                entity.Property(e => e.TrackerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("tracker_id");

                entity.Property(e => e.Type)
                    .HasMaxLength(30)
                    .HasColumnName("type");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}