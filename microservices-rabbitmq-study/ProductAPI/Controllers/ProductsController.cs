using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.WebApi.Repository.Interface;

namespace Product.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Model.Product>>> GetAll()
        {
            try
            {
                List<Model.Product> Products = await _productRepository.GetAll();

                if (Products == null) { return NotFound(); }

                return Ok(Products);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model.Product>> GetById(int id)
        {
            try
            {
                Model.Product Product = await _productRepository.GetById(id);

                if (Product == null) { return NotFound(); }

                return Ok(Product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Model.Product Product)
        {
            try
            {
                if (Product == null)
                {
                    return BadRequest();
                }

                await _productRepository.Create(Product);

                return Created("", Product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Model.Product Product)
        {
            try
            {
                if (Product == null)
                {
                    return BadRequest();
                }

                await _productRepository.Update(Product);

                return Ok(Product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Model.Product Product = await _productRepository.GetById(id);

                if (Product == null) { return NotFound(); }

                await _productRepository.Delete(Product);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
