using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ofertas.API.Data;
using Ofertas.API.Models;

namespace Ofertas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertasController : ControllerBase
    {

        private readonly MongoDBService _mongoDBService;

        public OfertasController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        // GET: api/<PrecosController>
        [HttpGet]
        public async Task<List<Oferta>> GetAsync()
        {
            return await _mongoDBService.GetAllAsync();
        }

        // GET api/<PrecosController>/5
        [HttpGet("{produtoId}")]
        public async Task<List<Oferta>> Get(int produtoId)
        {
            return await _mongoDBService.GetAsync(produtoId);
        }
    }
}
