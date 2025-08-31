using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.WebApi.Repository.Interfaces;

namespace Order.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrderRepository _productRepository;
        public OrdersController(IOrderRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Model.Order>>> GetAll()
        {
            try
            {
                List<Model.Order> Orders = await _productRepository.GetAll();

                if (Orders == null) { return NotFound(); }

                return Ok(Orders);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model.Order>> GetById(int id)
        {
            try
            {
                Model.Order Order = await _productRepository.GetById(id);

                if (Order == null) { return NotFound(); }

                return Ok(Order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Model.Order Order)
        {
            try
            {
                if (Order == null)
                {
                    return BadRequest();
                }

                await _productRepository.Create(Order);

                return Created("", Order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Model.Order Order)
        {
            try
            {
                if (Order == null)
                {
                    return BadRequest();
                }

                await _productRepository.Update(Order);

                return Ok(Order);
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
                Model.Order Order = await _productRepository.GetById(id);

                if (Order == null) { return NotFound(); }

                await _productRepository.Delete(Order);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
