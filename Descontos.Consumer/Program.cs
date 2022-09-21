// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Descontos.Consumer.Data;
using Descontos.Consumer.Models;
using System.Text.Json;

Console.WriteLine("Consumer de Desconto");

MongoDBService mongoDBService = new MongoDBService();
var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "DescontoConsumer",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

string topicoCriarDesconto = "Desconto.Criar";
string topicoAtualizarDesconto = "Desconto.Atualizar";
string topicoApagarDesconto = "Desconto.Apagar";

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
        await CreateTopicMaybe(topicoCriarDesconto, 1, 1, config);
        await CreateTopicMaybe(topicoAtualizarDesconto, 1, 1, config);
        await CreateTopicMaybe(topicoApagarDesconto, 1, 1, config);
        var topicos = new List<string>();
        topicos.Add(topicoCriarDesconto);
        topicos.Add(topicoAtualizarDesconto);
        topicos.Add(topicoApagarDesconto);

        consumer.Subscribe(topicos);

        try
        {
            while (true)
            {
                var cr = consumer.Consume(cts.Token);
                Desconto desconto = JsonSerializer.Deserialize<Desconto>(cr.Message.Value);
                if (desconto != null)
                {
                    if (cr.Topic.Equals(topicoCriarDesconto))
                    {
                        mongoDBService.CreateAsync(desconto);
                    }
                    if (cr.Topic.Equals(topicoAtualizarDesconto))
                    {
                        mongoDBService.UpdateAsync(desconto);
                    }
                    if (cr.Topic.Equals(topicoApagarDesconto))
                    {
                        mongoDBService.DeleteAsync(desconto.DescontoId);
                    }
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