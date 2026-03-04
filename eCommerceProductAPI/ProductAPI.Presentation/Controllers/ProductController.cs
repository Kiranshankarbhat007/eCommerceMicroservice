using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.App.DTOs;
using ProductAPI.App.DTOs.Conversions;
using ProductAPI.App.Interfaces;

namespace ProductAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetALLProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (products == null)
                return NotFound("No products found");

            var (_, list) = ProductConversions.FromEntity(null!, (IEnumerable<ProductDTO>)products);
            return list!.Any() ? Ok(list) : NotFound("No products found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await productInterface.GetByIdAsync(id);
            if (product == null)
                return NotFound($"No product found with id {id}");

            var (dto, _) = ProductConversions.FromEntity(product, null!);
            return dto == null ? NotFound($"No product found with id {id}") : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);

            return response.Flag ? Ok(response) : BadRequest(response); 
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getEntity = ProductConversions.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);

            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
