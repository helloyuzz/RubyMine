using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RubyMine.Customs.Models;
using RubyMine.Jwt;
using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace RubyMine.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase {
        private readonly ILogger<IssueController> _logger;
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;
        public IssueController(ILogger<IssueController> logger, RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
            _logger = logger;
        }
        // GET: api/<JwtController>
        [HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/<IssueController>/5
        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }

        // POST api/<IssueController>
        [HttpPost]
        public ActionResult<SoapResult> Post([FromBody] SoapIssue value) {
            SoapResult result = new SoapResult("Invalid Request.");
            if (value != null) {
                switch (value.Action) {
                    case "create_issue": // 新增
                        if (value.module_id > 0) {
                            /* ==========================================================================================
                             * 新增程序逻辑说明
                             * cache_position = 查询前一条记录.Position
                             * 更新Position = Position + 1，条件为.Position > cacha_position && module_id = value.module_id
                             * 当前记录.Position = cache_position + 1
                             * 新增当前记录 db.savechanges()
                             */
                            Issue prevIssue = null;
                            int before_position = 0;

                            // 调整排序
                            if (value.prev_issue_id > 0) {
                                prevIssue = _context.Issues.FirstOrDefault(t => t.Id == value.prev_issue_id);
                                before_position = _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.Value.Equals(value.module_id.ToString()) && t.CustomizedId == prevIssue.Id).Position;

                                // 更新排序position+1
                                _context.CustomValues.Where(t => t.CustomFieldId == 54 && t.Value.Equals(value.module_id.ToString()) && t.Position > before_position).Update(t => new CustomValue { Position = t.Position + 1 });
                            }

                            // 新增Issue
                            value.Issue.PriorityId = 2;
                            value.Issue.CreatedOn = DateTime.Now;
                            value.Issue.UpdatedOn = DateTime.Now;
                            value.Issue.CategoryId = 0;
                            value.Issue.StatusId = 1;
                            value.Issue.TrackerId = 2;      // 需求
                            value.Issue.ProjectId = 5;      // 3.0项目
                            value.Issue.Lft = 1;
                            value.Issue.Rgt = 2;
                            _context.Issues.Add(value.Issue);
                            _context.SaveChanges();         // 先保存到数据库，获取Issue.id

                            // 新增custom_value，排序=before_position+1
                            var new_custom_value = new CustomValue();
                            new_custom_value.CustomFieldId = 54;
                            new_custom_value.CustomizedId = value.Issue.Id;
                            new_custom_value.Value = value.module_id.ToString();
                            new_custom_value.Position = before_position + 1;
                            _context.CustomValues.Add(new_custom_value);
                            _context.SaveChanges();

                            //if (value.prev_issue_id >0) { // 第一次新增，无需获取，客户端直接window.location.refresh();
                            //    prevIssue = _context.Issues.Where(t => t.Id == value.Issue.Id).Include(t => t.Tracker).Include(t => t.Project).Include(t => t.Status).Include(t => t.Author).FirstOrDefault();
                            //    prevIssue.Description = prevIssue.UpdatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            //    result.Value = JsonConvert.SerializeObject(value);
                            //}
                            result.Result = "OK";
                        } else {
                            result.Result = "module id is null.";
                        }
                        break;
                    case "edit_issue":   // 修改
                        var issue = _context.Issues.FirstOrDefault(t => t.Id == value.Issue.Id);
                        issue.Subject = value.Issue.Subject;
                        issue.UpdatedOn = DateTime.Now;
                        _context.SaveChanges();
                        result.Result = "OK";
                        result.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        break;
                    case "MoveUp":
                        break;
                    case "MoveDown":
                        break;
                }
            } else {
                result.Result = "SoapIssue is null!";
            }
            return Ok(result);
        }

        // PUT api/<IssueController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/<IssueController>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }

        //[HttpPost("authentication")]
        //public ActionResult<string> AddIssue(Issue issue) {
        //    return "aaa_";
        //}
        //[HttpPost("authentication")]
        //public ActionResult<string> UpdateIssue(int id,string subject) {
        //    return "aaa_" + id;
        //}

    }
}
