using CalculoDeOfertas.Consumer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CalculoDeOfertas.Consumer.Repositories
{
    public class DescontoRepository
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<DescontoAPI>> DescontosAsync()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://localhost:7056/api/Descontos");

            return await JsonSerializer.DeserializeAsync<List<DescontoAPI>>(await streamTask);
            
        }

        public async Task<List<DescontoAPI>> DescontosDeProdutoAsync(int produtoId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://localhost:7056/api/Descontos/" + produtoId);

            return await JsonSerializer.DeserializeAsync<List<DescontoAPI>>(await streamTask);

        }
    }
}
