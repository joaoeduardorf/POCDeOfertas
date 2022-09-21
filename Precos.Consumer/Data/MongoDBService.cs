using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Precos.Consumer.Data;
using Precos.Consumer.Models;

namespace MongoExample.Services;

public class MongoDBService
{

    private readonly IMongoCollection<Preco> _precoCollection;

    public MongoDBService()
    {
        //MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        //IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        //_precoCollection = database.GetCollection<Preco>(mongoDBSettings.Value.CollectionName);
        MongoClient client = new MongoClient("mongodb://root:example@127.0.0.1:27017/");
        IMongoDatabase database = client.GetDatabase("PrecoDB");
        _precoCollection = database.GetCollection<Preco>("Precos");

    }

    public async Task CreateAsync(Preco preco) { await _precoCollection.InsertOneAsync(preco); }

    public async Task UpdateAsync(Preco preco)
    {
        FilterDefinition<Preco> filter = Builders<Preco>.Filter.Eq(f => f.ProdutoId, preco.ProdutoId);
        //await _precoCollection.ReplaceOneAsync<Preco>(f=> f.ProdutoId == preco.ProdutoId, preco);
        UpdateDefinition<Preco> update = Builders<Preco>.Update.Set<decimal>(s => s.Valor, preco.Valor);
        //UpdateDefinition<Preco> update2 = Builders<Preco>.Update.AddToSet<DateTime>("DataEHora", DateTime.UtcNow);
        await _precoCollection.UpdateOneAsync(filter, update);
        UpdateDefinition<Preco> update1 = Builders<Preco>.Update.Set<DateTime>(s => s.DataEHora, DateTime.UtcNow);
        await _precoCollection.UpdateOneAsync(filter, update1);
        //await _precoCollection.UpdateOneAsync(filter, update2);
        return;
    }

    public async Task DeleteAsync(int produtoId)
    {
        FilterDefinition<Preco> filter = Builders<Preco>.Filter.Eq("ProdutoId", produtoId);
        await _precoCollection.DeleteOneAsync(filter);
        return;

    }
}