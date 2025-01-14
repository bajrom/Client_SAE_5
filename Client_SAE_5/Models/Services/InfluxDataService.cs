using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Client_SAE_5.Models.InfluxDB;

namespace Client_SAE_5.Models.Services
{
    public class InfluxDataService
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

        // renvoie les dernières mesures (prendre 1ère pour avoir la dernière mesure prise)
        public async Task<double> GetTNowAsync(string nomCapteur)
        {

            try
            {
                var response = await _httpClient.GetAsync($"{controllerName}_Now?capteur={nomCapteur}");
                string responseString = await response.Content.ReadAsStringAsync();
                int closingBraceIndex = responseString.IndexOf('}');
                int startCutIndex = 0;
                int actualIndex = closingBraceIndex;
                
                while(actualIndex > 0)
                {
                    actualIndex--;
                    var caca = responseString[actualIndex];
                    if (responseString[actualIndex] == ':')
                    {
                        startCutIndex = actualIndex + 1;
                        break;
                    }
                }

                string valueString = responseString.Substring(startCutIndex, (closingBraceIndex-startCutIndex));

                return double.Parse(valueString, CultureInfo.InvariantCulture);
            }

            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erreur lors de la désérialisation du JSON (GetTNowAsync) : {ex.Message}", ex);
            }
        }

        public async Task<List<decimal>> GetTInTimeIntervalAsync(string nomCapteur, DateTime startDate, DateTime endDate)
        {
            var response = await _httpClient.GetFromJsonAsync<List<decimal>>($"{controllerName}?capteur={nomCapteur}&startDate={startDate}&endDAte={endDate}");

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
