using Descontos.API.Data;
using Descontos.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Descontos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DescontosController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public DescontosController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }
        // GET: api/<DescontosController>
        [HttpGet]
        public async Task<List<Desconto>> Get()
        {
            return await _mongoDBService.GetAllAsync();
        }

        // GET api/<DescontosController>/5
        [HttpGet("{produtoId}")]
        public async Task<List<Desconto>> Get(int produtoId)
        {
            return await _mongoDBService.GetAsync(produtoId);
        }
    }
}
