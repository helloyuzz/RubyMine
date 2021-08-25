using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace RubyMine.Models {
    public partial class Issue {
        [Display(Name ="序号")]
        public int Id { get; set; }
        [Display(Name = "跟踪")]
        public int TrackerId { get; set; }
        [Display(Name = "项目")]
        public int ProjectId { get; set; }
        [Display(Name = "主题")]
        public string Subject { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        [Display(Name = "类别")]
        public int? CategoryId { get; set; }
        [Display(Name = "状态")]
        public int StatusId { get; set; }
        [Display(Name = "指派给")]
        public int? AssignedToId { get; set; }
        [Display(Name = "优先级")]
        public int PriorityId { get; set; }
        [Display(Name = "修复版本")]
        public int? FixedVersionId { get; set; }
        [Display(Name = "作者")]
        public int AuthorId { get; set; }
        [Display(Name = "锁定版本")]
        public int LockVersion { get; set; }
        [Display(Name = "创建于")]
        public DateTime? CreatedOn { get; set; }
        [Display(Name = "更新于")]
        public DateTime? UpdatedOn { get; set; }
        [Display(Name = "开始日期")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "完成比")]
        public int DoneRatio { get; set; }
        [Display(Name = "工时花费")]
        public float? EstimatedHours { get; set; }
        [Display(Name = "上级序号")]
        public int? ParentId { get; set; }
        [Display(Name = "根序号")]
        public int? RootId { get; set; }
        public int? Lft { get; set; }
        public int? Rgt { get; set; }
        [Display(Name = "私有")]
        public bool IsPrivate { get; set; }
        [Display(Name = "关闭")]
        public DateTime? ClosedOn { get; set; }
        public Tracker Tracker { get; set; }
        public Project Project { get; set; }
        public IssueStatus Status { get; set; }
        public User AssignedTo { get; set; }
        public Enumeration Priority { get; set; }
        public User Author { get; set; }
    }
}