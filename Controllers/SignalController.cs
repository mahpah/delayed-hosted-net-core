using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private readonly SignalService _signalService;
        private readonly ILogger<SignalController> _logger;

        public SignalController(SignalService signalService, ILogger<SignalController> logger)
        {
            _signalService = signalService;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("warning/{id}")]
        public IActionResult Post(string id)
        {
            _logger.LogWarning("{0} {1} at risk", DateTime.Now, id);
            _signalService.AcceptSignal(id);
            return Accepted();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
