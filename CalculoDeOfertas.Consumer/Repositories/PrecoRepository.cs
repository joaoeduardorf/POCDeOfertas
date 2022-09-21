
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
    public class PrecoRepository
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<PrecoAPI>> PrecosAsync()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://localhost:7147/api/Precos");

            return await JsonSerializer.DeserializeAsync<List<PrecoAPI>>(await streamTask);
        }

        public async Task<PrecoAPI> PrecosDeProdutoAsync(int produtoId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://localhost:7147/api/Precos/" + produtoId);

            return await JsonSerializer.DeserializeAsync<PrecoAPI>(await streamTask);
        }
    }
}
