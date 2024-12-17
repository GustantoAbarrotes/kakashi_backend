using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Version;
using System;


namespace Api.Controllers.Version
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        private readonly VersionService _service;
        public VersionController(VersionService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_service.GetVersion());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
