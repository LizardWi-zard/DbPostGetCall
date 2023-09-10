using DbPostGetCall;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace WebAppCaller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly ILogger<ResponseController> _logger;
        private readonly IResponseCall _caller;

        public ResponseController(ILogger<ResponseController> logger, IResponseCall caller)
        {
            _logger = logger;
            _caller = caller ?? throw new ArgumentNullException(nameof(caller));
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetCars()
        {
            var response = await _caller.GetCars();

            string output = (string)response.Data;

            return output;
        }

        [HttpPost]
        public async Task<ActionResult<HttpStatusCode>> AddCar()
        {
            var response = await _caller.AddCar();

            return response.Status;
        }
    }
}