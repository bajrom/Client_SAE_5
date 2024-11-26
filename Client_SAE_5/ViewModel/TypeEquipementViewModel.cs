using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class TypeEquipementViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<TypeEquipement> _serviceTypeEquipement;

        public TypeEquipementViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceTypeEquipement = new WSService<TypeEquipement>();
        }
        public List<TypeEquipement> TypeEquipements { get; private set; } = new List<TypeEquipement>();

        public TypeEquipement NewTypeEquipement { get; private set; } = new TypeEquipement();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                TypeEquipements = await _serviceTypeEquipement.GetAllTAsync("TypeEquipements");
                Console.WriteLine("Recharger correctement");
                foreach (var typeEquipement in TypeEquipements)
                {
                    typeEquipement.IsEditable = false; // initialement non modifiable
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
                var typeEquipement = new TypeEquipement
                {
                    NomTypeEquipement = NewTypeEquipement.NomTypeEquipement
                };

                try
                {
                    await _serviceTypeEquipement.PostTAsync("TypeEquipements", typeEquipement);
                    await LoadAsync();
                }
                catch (JsonException jsonEx)
                {
                    ErrorMessage = $"Erreur JSON : {jsonEx.Message}";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur dans AddTypeEquipement : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
            }
        }

        private bool IsValidNew()
        {
            return !string.IsNullOrWhiteSpace(NewTypeEquipement.NomTypeEquipement);
        }

        public void Edit(TypeEquipement typeEquipement)
        {
            typeEquipement.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(TypeEquipement typeEquipement)
        {
            await UpdateAsync(typeEquipement);
            typeEquipement.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(TypeEquipement typeEquipement)
        {
            try
            {
                await _serviceTypeEquipement.PutTAsync($"TypesEquipements/{typeEquipement.IdTypeEquipement}", typeEquipement);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateTypeEquipement : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceTypeEquipement.DeleteTAsync($"TypesEquipements", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du typeEquipement : {ex.Message}";
            }
        }
    }
}
