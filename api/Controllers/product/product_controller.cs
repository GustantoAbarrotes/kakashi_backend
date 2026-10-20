using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Product;
using System;
using System.Threading.Tasks;


namespace Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;
        public ProductController(ProductService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var prods = await _service.GetAll();
                if (prods.Count < 1)
                    return StatusCode(204, ""); // 204 No Content 
                return Ok(prods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //500 Internal Server Error
            }
        }

        [AllowAnonymous]
        [HttpGet("get-by-id/{barCode}")]
        public async Task<IActionResult> GetByBarCode(string barCode)
        {
            try
            {
                var prod = await _service.GetByBarCode(barCode);
                if (prod is null)
                    return StatusCode(204, ""); // 204 No Content 
                return Ok(prod);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //500 Internal Server Error
            }
        }

        [AllowAnonymous]
        [HttpGet("find-by-text/{text}/{page}/{sizePage}")]
        public async Task<IActionResult> FindByText(string text, int page, int sizePage)
        {
            try
            {
                var prods = await _service.FindByText(text, page, sizePage);
                if (prods is null)
                    return StatusCode(204, ""); // 204 No Content 
                return Ok(prods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); //500 Internal Server Error
            }
        }
    }
}
