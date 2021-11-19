using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public ActionResult<SoapResult> Post([FromBody] Issue value) {
            // 注意，此方法中：
            // value.PriorityId当作前一记录的Issue.Id使用
            SoapResult result = new SoapResult();
            if (value == null) {
                result.Result = "value is null!";
            } else {
                if (value.Id > 0) { // 修改
                    var issue = _context.Issues.FirstOrDefault(t => t.Id == value.Id);
                    issue.Subject = value.Subject;
                    issue.UpdatedOn = DateTime.Now;
                    _context.SaveChanges();
                } else {    // 新增
                    if (value.CategoryId <= 0) {
                        result.Result = "module_id is null";
                    } else {
                        int module_id = value.CategoryId.Value;
                        int prev_issue_id = value.PriorityId;
                        bool emptyModule = value.PriorityId <= 0;
                        Issue issue = null;
                        int before_position = 0;

                        // 新增Issue
                        value.PriorityId = 2;
                        value.CreatedOn = DateTime.Now;
                        value.UpdatedOn = DateTime.Now;
                        value.CategoryId = 0;
                        value.StatusId = 1;
                        value.TrackerId = 2;    // 需求
                        value.ProjectId = 5;    // 3.0项目
                        value.Lft = 1;
                        value.Rgt = 2;
                        _context.Issues.Add(value);
                        _context.SaveChanges();

                        // 模块下不为空时才调整次序
                        if (emptyModule == false) {
                            issue = _context.Issues.FirstOrDefault(t => t.Id == prev_issue_id);
                            before_position = _context.CustomValues.FirstOrDefault(t => t.CustomFieldId == 54 && t.Value.Equals(module_id.ToString()) && t.CustomizedId == issue.Id).Position;

                            // 更新原有的排序position
                            _context.CustomValues.Where(t => t.CustomFieldId == 54 && t.Value.Equals(module_id.ToString()) && t.Position > before_position).Update(t => new CustomValue { Position = t.Position + 1 });
                        }

                        // 新增custom_value，并占用原有的排序position=before_position
                        var new_custom_value = new CustomValue();
                        new_custom_value.CustomFieldId = 54;
                        new_custom_value.CustomizedId = value.Id;
                        new_custom_value.Value = module_id.ToString();
                        new_custom_value.Position = before_position + 1;
                        _context.CustomValues.Add(new_custom_value);
                        _context.SaveChanges();

                        if (emptyModule == false) { // 第一次新增，无需获取，客户端直接window.location.refresh();
                            issue = _context.Issues.Where(t => t.Id == value.Id).Include(t => t.Tracker).Include(t => t.Project).Include(t => t.Status).Include(t => t.Author).FirstOrDefault();
                            issue.Description = issue.UpdatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            result.Value = JsonConvert.SerializeObject(value);
                        }
                    }
                }
                result.Result = "OK";
            }
            return result;
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
