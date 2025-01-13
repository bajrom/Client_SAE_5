using System.Net.Http.Json;
using System.Text.Json;

namespace Client_SAE_5.Models.Services
{
    public class InfluxDataService<T>
    {
        private readonly HttpClient _httpClient;
        public InfluxDataService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.103.101.128:5173/api/InfluxData/data/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetTAsync(string nomControleur, int id)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"{nomControleur}/GetById/{id}");

            try
            {
                return response;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erreur lors de la désérialisation du JSON : {ex.Message}", ex);
            }
        }
    }
}
