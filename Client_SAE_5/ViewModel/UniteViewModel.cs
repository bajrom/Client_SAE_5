using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class UniteViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<Unite> _serviceUnite;

        public UniteViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceUnite = new WSService<Unite>();
        }
        public List<Unite> Unites { get; private set; } = new List<Unite>();

        public Unite NewUnite { get; private set; } = new Unite();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                Unites = await _serviceUnite.GetAllTAsync("Unites");
                Console.WriteLine("Recharger correctement");
                foreach (var unite in Unites)
                {
                    unite.IsEditable = false; // initialement non modifiable
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
                var unite = new Unite
                {
                    NomUnite = NewUnite.NomUnite,
                    SigleUnite = NewUnite.SigleUnite
                };

                try
                {
                    await _serviceUnite.PostTAsync("Unites", unite);
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
            return !string.IsNullOrWhiteSpace(NewUnite.NomUnite);
        }

        public void Edit(Unite unite)
        {
            unite.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(Unite unite)
        {
            await UpdateAsync(unite);
            unite.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(Unite unite)
        {
            try
            {
                await _serviceUnite.PutTAsync($"Unites/{unite.IdUnite}", unite);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateUnite : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceUnite.DeleteTAsync($"Unites", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'unite : {ex.Message}";
            }
        }
    }
}
