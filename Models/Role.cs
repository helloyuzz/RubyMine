#nullable disable
using System.ComponentModel.DataAnnotations;

namespace RubyMine.Models {
    public partial class Role {
        [Display(Name = "序号")]
        public int Id { get; set; }
        [Display(Name = "角色名称")]
        public string Name { get; set; }
        [Display(Name = "显示排序")]
        public int? Position { get; set; }
        public bool? Assignable { get; set; }
        public int Builtin { get; set; }
        public string Permissions { get; set; }
        public string IssuesVisibility { get; set; }
        public string UsersVisibility { get; set; }
        public string TimeEntriesVisibility { get; set; }
        public bool? AllRolesManaged { get; set; }
        public string Settings { get; set; }
    }
}
