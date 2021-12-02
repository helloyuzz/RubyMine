using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RubyMine.Customs.Models;
using RubyMine.DbContexts;
using RubyMine.Models;

namespace RubyMine.Pages.Platform {
    [Authorize]
    public class IndexModel : PageModel {
        private readonly RubyRemineDbContext _context;
        private readonly IConfiguration _config;

        public IndexModel(RubyMine.DbContexts.RubyRemineDbContext context,IConfiguration _configuration) {
            _context = context;
            _config = _configuration;
        }
        public IList<DisplayIssue> DisplayIssues { get; set; }
        public IList<RubyMine.Models.Module> Modules { get; set; }
        public Module CurrentModule { get; set; }
        public IList<CustomValue> CustomValues { get; set; }
        public int Module_id { get; set; }
        public string Admin_Role_id { get; set; }
        public string Prev_Url { get; set; }
        public string Download_Url { get; set; }

        public async Task OnGet() {
            // 可编辑权限
            Admin_Role_id = _config["AppSettings:Admin_Role_id"];        

            Prev_Url = _config["AppSettings:Attachment_PreviewUrl"];
            Download_Url = _config["AppSettings:Attachment_DownloadUrl"];

            int current_module_id = RMUtils.QueryInt(Request, "id");
            string action = Request.Query["action"];
            string queryUrl = Request.QueryString.Value;
            Module_id = RMUtils.QueryInt(Request, "module_id");
            
            User cua = CookieUtils.Get(HttpContext.User.Claims.ToList());

            ActiveNodes nodes = GlobalCache.ActiveNodes.FirstOrDefault(t => t.Key == cua.Id).Value;
            if (nodes == null) {
                nodes = new ActiveNodes();
                GlobalCache.ActiveNodes.Add(cua.Id, nodes);
            }

            switch (action) {
                case "up":
                case "down":
                    if (current_module_id > 0) {
                        Module tempModule = _context.Modules.FirstOrDefault(t => t.Id == current_module_id);
                        int cacheIndex = tempModule.Index.Value;
                        int pid = tempModule.PId.Value;
                        int targetIndex = -1;

                        if (action.Equals("up")) {
                            targetIndex = cacheIndex - 1;
                            tempModule.Index--;
                        } else {
                            targetIndex = cacheIndex + 1;
                            tempModule.Index++;
                        }
                        _context.Database.ExecuteSqlRaw("update `modules` set `index`=" + cacheIndex + " where `index`=" + targetIndex + " and `p_id`=" + pid);

                        _context.SaveChanges();
                    }
                    break;
                case "load":
                    break;
                default:
                    break;
            }

            // 查询所有模块列表
            Modules = await _context.Modules.ToListAsync();

            // 根据模块Id查询所有Issue
            var predicateBuilder = PredicateBuilder.New<CustomValue>(false);
            predicateBuilder = predicateBuilder.And(x => x.CustomFieldId == 54);

            // CustomizedId = 对应Issue的Id
            // CustomFieldId = 模块序号（54）
            DisplayIssues = new List<DisplayIssue>();
            if (Module_id > 0) {    // 组合查询条件
                CurrentModule = _context.Modules.FirstOrDefault(t => t.Id == Module_id);

                predicateBuilder.And(x => x.Value.Equals(Module_id.ToString()));
                predicateBuilder.And(x => x.CustomFieldId == 54);
                predicateBuilder.And(x => x.CustomizedType.Equals("Issue"));
                CustomValues = await _context.CustomValues.Where(predicateBuilder).OrderBy(t => t.Position).ToListAsync();

                var quireFieldIds = CustomValues.Select(t => t.CustomizedId).ToList();

                // 查询该模块下的所有Issue
                var db_issues = await _context.Issues.Where(t => quireFieldIds.Contains(t.Id))
                    .Include(t => t.Tracker)
                    .Include(t => t.Project)
                    .Include(t => t.Status)
                    .Include(t => t.Author)
                    .Select(t => new Issue {
                        Id = t.Id,
                        TrackerId = t.TrackerId,
                        Tracker = t.Tracker,
                        ProjectId = t.ProjectId,
                        Project = t.Project,
                        Subject = t.Subject,
                        StatusId = t.StatusId,
                        Status = t.Status,
                        AssignedToId = t.AssignedToId,                        
                        PriorityId = t.PriorityId,
                        Priority = t.Priority,
                        FixedVersionId = t.FixedVersionId,
                        AuthorId = t.AuthorId,
                        Author=t.Author,
                        LockVersion = t.LockVersion,
                        CreatedOn = t.CreatedOn,
                        UpdatedOn = t.UpdatedOn,
                        StartDate = t.StartDate,
                        DoneRatio = t.DoneRatio,
                        EstimatedHours = t.EstimatedHours,
                        ParentId = t.ParentId,
                        RootId = t.RootId,
                        IsPrivate = t.IsPrivate
                    })
                    .ToListAsync();

                foreach (Issue issue in db_issues) {
                    DisplayIssue displayIssue = new DisplayIssue(issue);
                    displayIssue.Position = CustomValues.FirstOrDefault(t => t.CustomizedId == issue.Id).Position.Value;
                    DisplayIssues.Add(displayIssue);
                }
            }
        }
    }
}