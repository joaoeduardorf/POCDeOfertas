using Descontos.Consumer.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descontos.Consumer.Data
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Desconto> _descontoCollection;

        public MongoDBService()
        {
            //MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            //IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            //_precoCollection = database.GetCollection<Preco>(mongoDBSettings.Value.CollectionName);
            MongoClient client = new MongoClient("mongodb://root:example@127.0.0.1:27017/");
            IMongoDatabase database = client.GetDatabase("DescontoDB");
            _descontoCollection = database.GetCollection<Desconto>("Descontos");

        }

        public async Task CreateAsync(Desconto desconto)
        {
            await _descontoCollection.InsertOneAsync(desconto);
        }

        public async Task UpdateAsync(Desconto desconto)
        {
            FilterDefinition<Desconto> filter = Builders<Desconto>.Filter.Where(w => w.ProdutoId == desconto.ProdutoId && w.TipoDePagamento == desconto.TipoDePagamento);//Eq(f => f.ProdutoId, desconto.ProdutoId);
            //await _precoCollection.ReplaceOneAsync<Preco>(f=> f.ProdutoId == preco.ProdutoId, preco);
            UpdateDefinition<Desconto> update = Builders<Desconto>.Update.Set<decimal>(s => s.Percentual, desconto.Percentual);
            //UpdateDefinition<Preco> update2 = Builders<Preco>.Update.AddToSet<DateTime>("DataEHora", DateTime.UtcNow);
            await _descontoCollection.UpdateOneAsync(filter, update);
            UpdateDefinition<Desconto> update1 = Builders<Desconto>.Update.Set<DateTime>(s => s.DataEHora, DateTime.UtcNow);
            await _descontoCollection.UpdateOneAsync(filter, update1);
            //await _precoCollection.UpdateOneAsync(filter, update2);
            return;
        }

        public async Task DeleteAsync(int descontoId)
        {
            FilterDefinition<Desconto> filter = Builders<Desconto>.Filter.Where(w => w.DescontoId == descontoId);
            await _descontoCollection.DeleteOneAsync(filter);
            return;

        }
    }
}
