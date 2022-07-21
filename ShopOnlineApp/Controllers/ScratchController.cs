using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnlineApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScratchController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IActionResult GetAllValue()
        {
            return Ok(new List<string>
            {
                "1",
                "2",
                "3"
            });
        }
    }
}
