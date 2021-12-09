using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dx29.Documents.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutController : Controller
    {
        [HttpGet("version")]
        public IActionResult Version()
        {
            return Redirect("/api/v1/About/version");
        }

        [HttpGet("/api/v1/[controller]/version")]
        public IActionResult VersionV1()
        {
            return Ok(Startup.VERSION);
        }
    }
}
