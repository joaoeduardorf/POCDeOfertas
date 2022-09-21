using Confluent.Kafka;
using Descontos.Admin.API.Models;
using System.Text.Json;

namespace Descontos.Admin.API.Data
{
    public class DescontosAdminAPIKafka
    {
        private readonly ProducerConfig _config;
        public DescontosAdminAPIKafka()
        {
            _config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

        }

        public async Task Produzir(string topico, Desconto desconto)
        {


            using (var producer = new ProducerBuilder<string, string>(_config).Build())
            {
                _ = await producer.ProduceAsync(topico, new Message<string, string> { Key = JsonSerializer.Serialize(new { desconto.ProdutoId }), Value = JsonSerializer.Serialize(desconto) });
            }
        }
    }
}
