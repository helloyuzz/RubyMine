using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RubyMine.Customs.Models;
using RubyMine.Jwt;
using RubyMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RubyMine.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase {
        private readonly ILogger<ModuleController> _logger;
        private readonly RubyMine.DbContexts.RubyRemineDbContext _context;

        public ModuleController(ILogger<ModuleController> logger, RubyMine.DbContexts.RubyRemineDbContext context) {
            _context = context;
            _logger = logger;
        }
        //// GET: api/<ModuleController>
        //[HttpGet]
        //public IEnumerable<string> Get() {
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ModuleController>/5
        //[HttpGet("{id}")]
        //public string Get(int id) {
        //    return "value";
        //}

        // POST api/<ModuleController>
        [HttpPost]
        public ActionResult<string> Post([FromBody] SwapModule value) {
            string soapResult = "Invalid request.";
            Module module = null;
            if (value.Id > 0 && value.Parent_id > 0) {
                switch (value.Action) {
                    case "up":
                    case "down":
                        module = _context.Modules.FirstOrDefault(t => t.Id == value.Id);
                        if (module == null) {
                            soapResult = "Invalid module.";
                        } else {
                            int currentIndex = module.Index.Value;
                            Module toModule = null;
                            switch (value.Action) {
                                case "up":
                                    if (module.Index > 1) {
                                        toModule = _context.Modules.Where(t => t.PId == value.Parent_id).OrderBy(t => t.Index).FirstOrDefault(t => t.Index == currentIndex - 1);
                                        toModule.Index = currentIndex;

                                        module.Index--;
                                    } else {
                                        soapResult = "Current node is on top.";
                                    }
                                    break;
                                case "down":
                                    var count = _context.Modules.Count(t => t.PId == value.Parent_id);
                                    if (module.Index < count) {
                                        toModule = _context.Modules.Where(t => t.PId == value.Parent_id).OrderBy(t => t.Index).FirstOrDefault(t => t.Index == currentIndex + 1);
                                        toModule.Index = currentIndex;

                                        module.Index++;
                                    } else {
                                        soapResult = "Current node is on bottom.";
                                    }
                                    break;
                            }
                            _context.SaveChanges();
                            soapResult = "OK";
                        }
                        break;
                }
            }

            return soapResult;
        }

        //// PUT api/<ModuleController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value) {
        //}

        //// DELETE api/<ModuleController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id) {
        //}
    }
}
