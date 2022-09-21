using Descontos.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Descontos.API.Data
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Desconto> _descontoCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _descontoCollection = database.GetCollection<Desconto>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<List<Desconto>> GetAllAsync()
        {
            return await _descontoCollection.FindSync(new BsonDocument()).ToListAsync();
        }

        public async Task<List<Desconto>> GetAsync(int produtoId)
        {
            FilterDefinition<Desconto> filter = Builders<Desconto>.Filter.Where(w => w.ProdutoId == produtoId);
            return await _descontoCollection.FindSync(filter).ToListAsync();
            //return await result.FirstOrDefaultAsync();
        }
    }
}
