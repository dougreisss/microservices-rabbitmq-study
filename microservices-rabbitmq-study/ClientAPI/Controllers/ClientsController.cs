using Client.WebApi.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _productRepository;
        public ClientsController(IClientRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Model.Client>>> GetAll()
        {
            try
            {
                List<Model.Client> clients = await _productRepository.GetAll();

                if (clients == null) { return NotFound(); }

                return Ok(clients);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model.Client>> GetById(int id)
        {
            try
            {
                Model.Client client = await _productRepository.GetById(id);

                if (client == null) { return NotFound(); }

                return Ok(client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Model.Client client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest();
                }

                await _productRepository.Create(client);

                return Created("", client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Model.Client client)
        {
            try
            {
                if (client == null)
                {
                    return BadRequest();
                }

                await _productRepository.Update(client);

                return Ok(client);
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
                Model.Client client = await _productRepository.GetById(id);

                if (client == null) { return NotFound(); } 

                await _productRepository.Delete(client);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
