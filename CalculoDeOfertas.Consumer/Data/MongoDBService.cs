using CalculoDeOfertas.Consumer.Models;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Data
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Oferta> _ofertaCollection;

        public MongoDBService()
        {
            //MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            //IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            //_precoCollection = database.GetCollection<Preco>(mongoDBSettings.Value.CollectionName);
            MongoClient client = new MongoClient("mongodb://root:example@127.0.0.1:27017/");
            IMongoDatabase database = client.GetDatabase("OfertaDB");
            _ofertaCollection = database.GetCollection<Oferta>("Ofertas");

        }

        //public async Task<List<Oferta>> GetAllAsync()
        //{
        //    return await _ofertaCollection.FindSync(new BsonDocument()).ToListAsync();
        //}

        //public async Task<List<Oferta>> GetAsync(int ProdutoId)
        //{
        //    FilterDefinition<Oferta> filter = Builders<Oferta>.Filter.Eq("ProdutoId", ProdutoId);
        //    var result = await _ofertaCollection.FindAsync<Oferta>(filter, null);
        //    return await result.ToListAsync<Oferta>();
        //}

        public async Task CreateAsync(Oferta oferta)
        {
            await _ofertaCollection.InsertOneAsync(oferta);
        }

        public async Task CreateAsync(List<Oferta> ofertas)
        {
            await _ofertaCollection.InsertManyAsync(ofertas);
        }

        public async Task UpdateAsync(List<Oferta> ofertas)
        {
            {
                foreach (Oferta oferta in ofertas)
                {
                    //await _ofertaCollection.FindOneAndReplaceAsync<Oferta>(w => w.ProdutoId == oferta.ProdutoId && w.TipoDePagamento == oferta.TipoDePagamento, oferta);
                    await UpdateAsync(oferta);
                }

                return;
            }

        }
        public async Task UpdateAsync(Oferta oferta)
        {
            try
            {
                FilterDefinition<Oferta> filter = Builders<Oferta>.Filter.Where(w => w.ProdutoId == oferta.ProdutoId && w.TipoDePagamento == oferta.TipoDePagamento);
                UpdateDefinition<Oferta> update = Builders<Oferta>.Update.Set("PercentualDesconto", oferta.PercentualDesconto)
                                                                             .Set("ValorOferta", oferta.ValorOferta)
                                                                             .Set("ValorPreco", oferta.ValorPreco);

                await _ofertaCollection.UpdateOneAsync(filter, update);

                //await _ofertaCollection.FindOneAndReplaceAsync<Oferta>(w => w.ProdutoId == oferta.ProdutoId && w.TipoDePagamento == oferta.TipoDePagamento, oferta);
            }
            catch (Exception ex)
            {

                throw;
            }
            
            //await _precoCollection.ReplaceOneAsync<Preco>(f=> f.ProdutoId == preco.ProdutoId, preco);
            //UpdateDefinition<Desconto> update = Builders<Desconto>.Update.Set<decimal>(s => s.Percentual, desconto.Percentual);
            //UpdateDefinition<Preco> update2 = Builders<Preco>.Update.AddToSet<DateTime>("DataEHora", DateTime.UtcNow);
            //await _descontoCollection.UpdateOneAsync(filter, update);
            //UpdateDefinition<Desconto> update1 = Builders<Desconto>.Update.Set<DateTime>(s => s.DataEHora, DateTime.UtcNow);
            //
            //await _precoCollection.UpdateOneAsync(filter, update2);

            return;
        }

        //public async Task DeleteAsync(int descontoId)
        //{
        //    FilterDefinition<Desconto> filter = Builders<Desconto>.Filter.Where(w => w.DescontoId == descontoId);
        //    await _ofertaCollection.DeleteOneAsync(filter);
        //    return;
        //}
    }
}
