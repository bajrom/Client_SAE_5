using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class TypeSalleViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<TypeSalle> _serviceTypeSalle;

        public TypeSalleViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceTypeSalle = new WSService<TypeSalle>();
        }
        public List<TypeSalle> TypeSalles { get; private set; } = new List<TypeSalle>();

        public TypeSalle NewTypeSalle { get; private set; } = new TypeSalle();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                TypeSalles = await _serviceTypeSalle.GetAllTAsync("TypeSalles");
                Console.WriteLine("Recharger correctement");
                foreach (var typesalle in TypeSalles)
                {
                    typesalle.IsEditable = false; // initialement non modifiable
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
                var typesalle = new TypeSalle
                {
                    NomTypeSalle = NewTypeSalle.NomTypeSalle
                };

                try
                {
                    await _serviceTypeSalle.PostTAsync("TypeSalles", typesalle);
                    await LoadAsync();
                }
                catch (JsonException jsonEx)
                {
                    ErrorMessage = $"Erreur JSON : {jsonEx.Message}";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur dans AddTypeSalle : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
            }
        }

        private bool IsValidNew()
        {
            return !string.IsNullOrWhiteSpace(NewTypeSalle.NomTypeSalle);
        }

        public void Edit(TypeSalle typeSalle)
        {
            typeSalle.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(TypeSalle typeSalle)
        {
            await UpdateAsync(typeSalle);
            typeSalle.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(TypeSalle typeSalle)
        {
            try
            {
                await _serviceTypeSalle.PutTAsync($"TypeSalles/{typeSalle.IdTypeSalle}", typeSalle);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateTypeSalle : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceTypeSalle.DeleteTAsync($"TypeSalles", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de la type salle : {ex.Message}";
            }
        }
    }
}
