// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MongoExample.Services;
using Precos.Consumer.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

Console.WriteLine("Consumer de Preço");

string topicoPrecoCriar = "Preco.Criar";
string topicoPrecoAtualizar = "Preco.Atualizar";
string topicoPrecoApagar = "Preco.Apagar";

MongoDBService mongoDBService = new MongoDBService();
var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "PrecoAlteradoConsumer",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

await CreateTopicMaybe(topicoPrecoCriar, 1, 1, config);
await CreateTopicMaybe(topicoPrecoAtualizar, 1, 1, config);
await CreateTopicMaybe(topicoPrecoApagar, 1, 1, config);

CancellationTokenSource cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

try
{
    using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
    {

        var topicos = new List<string>();
        topicos.Add(topicoPrecoCriar);
        topicos.Add(topicoPrecoAtualizar);
        topicos.Add(topicoPrecoApagar);

        consumer.Subscribe(topicos);

        try
        {
            while (true)
            {
                var cr = consumer.Consume(cts.Token);
                if (cr.Topic.Equals(topicoPrecoCriar))
                {
                    Preco preco = JsonSerializer.Deserialize<Preco>(cr.Message.Value);
                    mongoDBService.CreateAsync(preco);
                }
                if (cr.Topic.Equals(topicoPrecoAtualizar))
                {
                    Preco preco = JsonSerializer.Deserialize<Preco>(cr.Message.Value);
                    mongoDBService.UpdateAsync(preco);
                }
                if (cr.Topic.Equals(topicoPrecoApagar))
                {
                    Preco preco = JsonSerializer.Deserialize<Preco>(cr.Message.Value);
                    mongoDBService.DeleteAsync(preco.ProdutoId);
                }
                Console.WriteLine("Topico: " + cr.Topic + " - Mensagem: " + cr.Message.Value);
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
            Console.WriteLine("Cancelada a execução do Consumer...");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exceção: {ex.GetType().FullName} | " +
                 $"Mensagem: {ex.Message}");
}

static async Task CreateTopicMaybe(string name, int numPartitions, short replicationFactor, ClientConfig cloudConfig)
{
    using (var adminClient = new AdminClientBuilder(cloudConfig).Build())
    {
        try
        {
            await adminClient.CreateTopicsAsync(new List<TopicSpecification> {
                        new TopicSpecification { Name = name, NumPartitions = numPartitions, ReplicationFactor = replicationFactor } });
        }
        catch (CreateTopicsException e)
        {
            if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
            {
                Console.WriteLine($"An error occured creating topic {name}: {e.Results[0].Error.Reason}");
            }
            else
            {
                Console.WriteLine("Topic already exists");
            }
        }
    }
}