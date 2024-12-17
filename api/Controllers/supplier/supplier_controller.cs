using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ServiceLayer.Supplier;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers.Supplier
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _service;
        public SupplierController(SupplierService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromBody] JObject data)
        {
            try
            {
                var hascp = data.TryGetValue("cveProv", StringComparison.CurrentCulture, out JToken cveProv);
                var hasrfc = data.TryGetValue("rfc", StringComparison.CurrentCulture, out JToken rfc);
                var hastn = data.TryGetValue("tradeName", StringComparison.CurrentCulture, out JToken tradeName);
                var hassn = data.TryGetValue("supplName", StringComparison.CurrentCulture, out JToken supplName);
                var hase = data.TryGetValue("email", StringComparison.CurrentCulture, out JToken email);

                var cp = hascp ? cveProv.ToString() : string.Empty;
                var _rfc = hasrfc ? rfc.ToString() : string.Empty;
                var tn = hastn ? tradeName.ToString() : string.Empty;
                var sn = hassn ? supplName.ToString() : string.Empty;
                var e = hase ? email.ToString() : string.Empty;
                int.TryParse(cp, out int _cp);

                var suppliers = await _service.ApproveBalance(0, 0, _cp, _rfc, tn, sn, e);
                if (suppliers.Count < 1)
                    return StatusCode(204, ""); // 204 No Content 
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //500 Internal Server Error
            }
        }



        // GET: api/<supplier_Controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<supplier_Controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<supplier_Controller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<supplier_Controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<supplier_Controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
