using System.Net.Http.Json;
using System.Text.Json;

namespace Client_SAE_5.Models.Services
{
    namespace Client_SAE_5.Models.Services
    {
        public class InfluxService
        {
            private readonly HttpClient _httpClient;
            private const string BaseUrl = "http://10.103.101.128:5173/api/";

            public InfluxService()
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(BaseUrl)
                };
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }

            // Récupérer toutes les données des capteurs
            public async Task<List<InfluxData>> GetAllCapteurDataAsync()
            {
                try
                {
                    var response = await _httpClient.GetAsync("InfluxData/data/capteurs");
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadFromJsonAsync<List<InfluxData>>() ?? new List<InfluxData>();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erreur HTTP: {ex.Message}");
                    throw;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Erreur de désérialisation: {ex.Message}");
                    throw;
                }
            }

            // Récupérer les données d'un capteur spécifique
            public async Task<List<InfluxData>> GetCapteurDataByIdAsync(string capteurId)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"InfluxData/data/capteurs/{capteurId}");
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadFromJsonAsync<List<InfluxData>>() ?? new List<InfluxData>();
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Erreur HTTP pour le capteur {capteurId}: {ex.Message}");
                    throw;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Erreur de désérialisation pour le capteur {capteurId}: {ex.Message}");
                    throw;
                }
            }

        }

        // Classe pour les statistiques des capteurs
        public class CapteurStats
        {
            public double Average { get; set; }
            public double Minimum { get; set; }
            public double Maximum { get; set; }
            public int Count { get; set; }
        }
    }
}
