using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Precos.API.Models;

namespace Precos.API.Data
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Preco> _precoCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _precoCollection = database.GetCollection<Preco>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<Preco>> GetAllAsync()
        {
            return await _precoCollection.FindSync(new BsonDocument()).ToListAsync();
        }

        public async Task<Preco> GetAsync(int ProdutoId)
        {
            //FilterDefinition<Preco> filter = Builders<Preco>.Filter.Eq("ProdutoId", ProdutoId);
            var result = await _precoCollection.FindAsync<Preco>(w => w.ProdutoId == ProdutoId);
            return await result.FirstOrDefaultAsync();
        }
    }
}
