// See https://aka.ms/new-console-template for more information
using CalculoDeOfertas.Consumer.Data;
using CalculoDeOfertas.Consumer.Domain;
using CalculoDeOfertas.Consumer.Mappers;
using CalculoDeOfertas.Consumer.Models;
using CalculoDeOfertas.Consumer.Repositories;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Text.Json;

Console.WriteLine("Calculo de oferta consumer ");

string topicoPrecoCriar = "Preco.Criar";
string topicoPrecoAtualizar = "Preco.Atualizar";
string topicoPrecoApagar = "Preco.Apagar";
string topicoDescontoCriar = "Desconto.Criar";
string topicoDescontoAtualizar = "Desconto.Atualizar";
string topicoDescontoApagar = "Desconto.Apagar";

MongoDBService mongoDBService = new MongoDBService();
var config = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "CalculoOfertaConsumer1",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

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
        topicos.Add(topicoDescontoCriar);
        topicos.Add(topicoDescontoAtualizar);
        topicos.Add(topicoDescontoApagar);

        consumer.Subscribe(topicos);

        try
        {
            while (true)
            {
                var cr = consumer.Consume(cts.Token);
                if (cr.Topic.Equals(topicoPrecoCriar))
                {
                    Preco preco = JsonSerializer.Deserialize<Preco>(cr.Message.Value);
                    DescontoRepository descontoRepository = new DescontoRepository();
                    var descontos = await descontoRepository.DescontosDeProdutoAsync(preco.ProdutoId);
                    CalcularOfertas calcularOfertas = new CalcularOfertas(preco, descontos.Mapper());
                    //var ofertas = calcularOfertas.ObterOfertas();
                    var oferta = calcularOfertas.CalcularOfertaBase();
                    await mongoDBService.CreateAsync(oferta);
                }
                if (cr.Topic.Equals(topicoPrecoAtualizar))
                {
                    Preco preco = JsonSerializer.Deserialize<Preco>(cr.Message.Value);
                    DescontoRepository descontoRepository = new DescontoRepository();
                    var descontos = await descontoRepository.DescontosDeProdutoAsync(preco.ProdutoId);
                    CalcularOfertas calcularOfertas = new CalcularOfertas(preco, descontos.Mapper());
                    var ofertas = calcularOfertas.ObterOfertas();
                    foreach (var oferta in ofertas)
                    {
                        await mongoDBService.UpdateAsync(oferta);
                    }
                }
                if (cr.Topic.Equals(topicoPrecoApagar))
                {
                    Preco preco = JsonSerializer.Deserialize<Preco>(cr.Message.Value);
                    DescontoRepository descontoRepository = new DescontoRepository();
                    descontoRepository.DescontosAsync();
                    //mongoDBService.DeleteAsync(preco.ProdutoId);
                }
                if (cr.Topic.Equals(topicoDescontoCriar))
                {
                    Desconto desconto = JsonSerializer.Deserialize<Desconto>(cr.Message.Value);
                    PrecoRepository precoRepository = new PrecoRepository();
                    PrecoAPI precoAPI  = await precoRepository.PrecosDeProdutoAsync(desconto.ProdutoId);
                    Preco preco = new Preco { PrecoId = precoAPI.precoId, ProdutoId = precoAPI.produtoId, Valor = precoAPI.valor };
                    CalcularOfertas calcularOfertas = new CalcularOfertas(preco, null);
                    var oferta = calcularOfertas.CalcularOferta(desconto);
                    mongoDBService.CreateAsync(oferta);
                }

                if(cr.Topic.Equals(topicoDescontoAtualizar))
                {
                    Desconto desconto = JsonSerializer.Deserialize<Desconto>(cr.Message.Value);
                    PrecoRepository precoRepository = new PrecoRepository();
                    PrecoAPI precoAPI = await precoRepository.PrecosDeProdutoAsync(desconto.ProdutoId);
                    Preco preco = new Preco { PrecoId = precoAPI.precoId, ProdutoId = precoAPI.produtoId, Valor = precoAPI.valor };
                    CalcularOfertas calcularOfertas = new CalcularOfertas(preco, null);
                    var oferta = calcularOfertas.CalcularOferta(desconto);
                    mongoDBService.UpdateAsync(oferta);
                }
                if (cr.Topic.Equals(topicoDescontoApagar))
                {
                    Preco preco = JsonSerializer.Deserialize<Preco>(cr.Message.Value);
                    DescontoRepository descontoRepository = new DescontoRepository();
                    descontoRepository.DescontosAsync();
                    //mongoDBService.DeleteAsync(preco.ProdutoId);
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