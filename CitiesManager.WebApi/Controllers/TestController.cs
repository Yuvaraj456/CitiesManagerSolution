using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {

            _logger = logger;

        }

        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }
    }
}
