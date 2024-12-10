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
            _httpClient.BaseAddress = new Uri("https://localhost:7162/api/");
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
            Console.WriteLine("caca");
            var response = await _httpClient.GetFromJsonAsync<T>($"{nomControleur}/GetById/{id}");
/*            Console.WriteLine("caca");
            response.EnsureSuccessStatusCode();
            Console.WriteLine("caca");
            var jsonString = await response.Content.ReadFromJsonAsync<T>();*/
            Console.WriteLine(response); // Pour voir la réponse JSON brute

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

        Task<T> IService<T>.DeleteTAsync(string nomControleur, int id)
        {
            throw new NotImplementedException();
        }
    }
}
