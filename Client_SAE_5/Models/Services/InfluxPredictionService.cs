using System.Net.Http.Json;

namespace Client_SAE_5.Models.Services
{
    public class InfluxPredictionService
    {
        private readonly HttpClient _httpClient;
        public InfluxPredictionService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.103.101.128:5173/api/InfluxData/data/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<string>> GetCapteursAsync()
        {
            var response = await _httpClient.GetAsync("capteurs");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<string>>();
        }

        public async Task<bool> GetFenetreOuvertePredAsync(string nomCapteur)
        {
            var response = await _httpClient.GetAsync($"fenetre_ouverte?capteur={nomCapteur}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<string> GetNbPersonnePredAsync(string nomCapteur)
        {
            var response = await _httpClient.GetAsync($"nb_personne?capteur={nomCapteur}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<string>();
        }

        public async Task<string> GetInconfortPredAsync(string nomCapteur)
        {
            var response = await _httpClient.GetAsync($"inconfort?capteur={nomCapteur}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<string>();
        }

        public async Task<List<float>> GetTemperaturesPredAsync(string nomCapteur)
        {
            var response = await _httpClient.GetAsync($"temperature_futur?capteur={nomCapteur}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<float>>();
        }
    }
}
