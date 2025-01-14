using System.Net.Http.Json;
using System.Text.Json;

namespace Client_SAE_5.Models.Services
{
    public class InfluxDataService<T>
    {
        private readonly HttpClient _httpClient;
        private readonly string controllerName;

        public InfluxDataService(string controllerName)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://10.103.101.128:5173/api/InfluxData/data/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.controllerName = controllerName;
        }

        public async Task<T> GetTNowAsync(string nomCapteur)
        {
            var response = await _httpClient.GetFromJsonAsync<T>($"{controllerName}_Now?capteur={nomCapteur}");

            try
            {
                return response;
            }

            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erreur lors de la désérialisation du JSON (GetTNowAsync) : {ex.Message}", ex);
            }
        }

        public async Task<List<T>> GetTInTimeIntervalAsync(string nomCapteur, DateTime startDate, DateTime endDate)
        {
            var response = await _httpClient.GetFromJsonAsync<List<T>>($"{controllerName}?capteur={nomCapteur}&startDate={startDate}&endDAte={endDate}");

            try
            {
                return response;
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erreur lors de la désérialisation du JSON (GetTInTimeIntervalAsync) : {ex.Message}", ex);
            }
        }
    }
}
