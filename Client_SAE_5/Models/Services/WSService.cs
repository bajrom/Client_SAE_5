using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client_SAE_5.Models.Services
{
    public class WSService<T> : IService<T>
    {
        private readonly HttpClient _httpClient;
        public WSService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api-ovhiutannecy-sae5-gwbxhugwhjb2aqdu.canadacentral-01.azurewebsites.net/api/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<T>> GetAllTAsync(string nomControleur)
        {
            var response = await _httpClient.GetAsync(nomControleur);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<T>>();
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

        public async Task<T> PostTAsync(string nomControleur, T table)
        {
            var response = await _httpClient.PostAsJsonAsync(nomControleur, table);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PutTAsync(string nomControleur, T table)
        {
            var response = await _httpClient.PutAsJsonAsync(nomControleur, table);

            // Assurez-vous que la réponse est réussie
            response.EnsureSuccessStatusCode();

            // Vérifie si la réponse a un contenu
            if (response.Content.Headers.ContentLength == 0)
            {
                // Si le contenu est vide, retourner null ou une valeur par défaut
                return default;
            }

            try
            {
                // Lire et désérialiser le contenu de la réponse si elle contient des données
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (JsonException jsonEx)
            {
                // Gérer une erreur JSON et retourner une exception plus descriptive
                throw new InvalidOperationException("Erreur lors de la désérialisation de la réponse JSON.", jsonEx);
            }
        }


        public async Task DeleteTAsync(string nomControleur, int id)
        {
            var response = await _httpClient.DeleteAsync($"{nomControleur}/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDoubleTAsync(string nomControleur, int id1, int id2)
        {
            var response = await _httpClient.DeleteAsync($"{nomControleur}/{id1}-{id2}");
            response.EnsureSuccessStatusCode();
        }
    }
}
