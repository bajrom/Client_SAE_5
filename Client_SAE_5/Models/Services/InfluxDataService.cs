using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
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
                return GetLastValue(responseString);
            }

            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erreur lors de la désérialisation du JSON (GetTNowAsync) : {ex.Message}", ex);
            }
        }

        public async Task<List<InfluxDataReturn>> GetTInTimeIntervalAsync(string nomCapteur, DateTime startDate, DateTime endDate)
        {
            try
            {
                var test = $"{controllerName}?capteur={nomCapteur}&startDate={HttpUtility.UrlEncode(startDate.ToString("o"))}&endDate={HttpUtility.UrlEncode(endDate.ToString("o"))}";
                var response = await _httpClient.GetAsync($"{controllerName}?capteur={nomCapteur}&startDate={HttpUtility.UrlEncode(startDate.ToString("o"))}&endDate={HttpUtility.UrlEncode(endDate.ToString("o"))}");
                string responseString = await response.Content.ReadAsStringAsync();
                return GetAllValues(responseString);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Erreur lors de la désérialisation du JSON (GetTInTimeIntervalAsync) : {ex.Message}", ex);
            }
        }

        private double GetLastValue(string jsonString)
        {
            int closingBraceIndex = jsonString.IndexOf('}');
            int startCutIndex = 0;
            int actualIndex = closingBraceIndex;

            while (actualIndex > 0)
            {
                actualIndex--;
                if (jsonString[actualIndex] == ':')
                {
                    startCutIndex = actualIndex + 1;
                    break;
                }
            }

            string valueString = jsonString.Substring(startCutIndex, (closingBraceIndex - startCutIndex));

            return double.Parse(valueString, CultureInfo.InvariantCulture);
        }

        private List<InfluxDataReturn> GetAllValues(string jsonString)
        {
            int actualIndex = 0;
            int startCutIndex = 0;
            int endCutIndex = 0;

            List<InfluxDataReturn> values = new List<InfluxDataReturn>();

            while(jsonString.IndexOf("value", endCutIndex) != -1) //-1 si pas trouvé donc que il ne reste plus de value à récupérer
            {
                startCutIndex = jsonString.IndexOf("time", endCutIndex) + 7;
                endCutIndex = jsonString.IndexOf(",", startCutIndex) -1;

                string dateString = jsonString.Substring(startCutIndex, (endCutIndex - startCutIndex));
                DateTime date = DateTime.Parse(dateString);

                startCutIndex = jsonString.IndexOf("value", endCutIndex) + 7;
                endCutIndex = jsonString.IndexOf("}", startCutIndex);

                string valueString = jsonString.Substring(startCutIndex, (endCutIndex - startCutIndex));
                double value = double.Parse(valueString, CultureInfo.InvariantCulture);

                values.Add(new InfluxDataReturn(date, value));
            }

            return values;
        }
    }
}
