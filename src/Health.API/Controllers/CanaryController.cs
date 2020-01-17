using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Health.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanaryController : ControllerBase
    {

        [HttpGet]
        //[Route("Notification")]
        public IActionResult Index()
        {
            return Ok("Hello Tim");
        }
    }
}