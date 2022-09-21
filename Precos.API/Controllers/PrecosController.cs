using Microsoft.AspNetCore.Mvc;
using Precos.API.Data;
using Precos.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Precos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrecosController : ControllerBase
    {

        private readonly MongoDBService _mongoDBService;

        public PrecosController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // GET: api/<PrecosController>
        [HttpGet]
        public async Task<List<Preco>> GetAsync()
        {
            return await _mongoDBService.GetAllAsync();
        }

        // GET api/<PrecosController>/5
        [HttpGet("{produtoId}")]
        public async Task<Preco> Get(int produtoId)
        {
            return await _mongoDBService.GetAsync(produtoId);
        }

    }
}
