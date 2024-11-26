using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class BatimentViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<Batiment> _serviceBatiment;

        public BatimentViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceBatiment = new WSService<Batiment>();
        }
        public List<Batiment> Batiments { get; private set; } = new List<Batiment>();

        public Batiment NewBatiment { get; private set; } = new Batiment();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                Batiments = await _serviceBatiment.GetAllTAsync("Batiments");
                Console.WriteLine("Recharger correctement");
                foreach (var batiment in Batiments)
                {
                    batiment.IsEditable = false; // initialement non modifiable
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement : {ex.Message}";
            }
        }

        public async Task AddAsync()
        {
            if (IsValidNew())
            {
                var batiment = new Batiment
                {
                    NomBatiment = NewBatiment.NomBatiment
                };

                try
                {
                    await _serviceBatiment.PostTAsync("Batiments", batiment);
                    await LoadAsync();
                }
                catch (JsonException jsonEx)
                {
                    ErrorMessage = $"Erreur JSON : {jsonEx.Message}";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur dans AddBatiment : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
            }
        }

        private bool IsValidNew()
        {
            return !string.IsNullOrWhiteSpace(NewBatiment.NomBatiment);
        }

        public void Edit(Batiment batiment)
        {
            batiment.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(Batiment batiment)
        {
            await UpdateAsync(batiment);
            batiment.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(Batiment batiment)
        {
            try
            {
                await _serviceBatiment.PutTAsync($"Batiments/{batiment.IdBatiment}", batiment);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateBatiment : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceBatiment.DeleteTAsync($"Batiments", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du batiment : {ex.Message}";
            }
        }
    }
}
