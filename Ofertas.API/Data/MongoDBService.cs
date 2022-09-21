using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Ofertas.API.Models;

namespace Ofertas.API.Data
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Oferta> _precoCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _precoCollection = database.GetCollection<Oferta>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<Oferta>> GetAllAsync()
        {
            return await _precoCollection.FindSync(new BsonDocument()).ToListAsync();
        }

        public async Task<List<Oferta>> GetAsync(int ProdutoId)
        {
            FilterDefinition<Oferta> filter = Builders<Oferta>.Filter.Eq("ProdutoId", ProdutoId);
            var result = await _precoCollection.FindAsync<Oferta>(filter, null);
            return await result.ToListAsync<Oferta>();
        }
    }
}
