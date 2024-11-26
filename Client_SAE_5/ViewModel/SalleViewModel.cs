using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class SalleViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<Batiment> _serviceBatiment;
        private readonly WSService<TypeSalle> _serviceTypeSalle;
        private readonly WSService<Salle> _serviceSalle;

        public SalleViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceBatiment = new WSService<Batiment>();
            _serviceTypeSalle = new WSService<TypeSalle>();
            _serviceSalle = new WSService<Salle>();
        }

        public List<Salle> Salles { get; private set; } = new List<Salle>();
        public List<Batiment> Batiments { get; private set; } = new List<Batiment>();
        public List<TypeSalle> TypeSalles { get; private set; } = new List<TypeSalle>();

        public Salle NewSalle { get; private set; } = new Salle();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                Batiments = await _serviceBatiment.GetAllTAsync("Batiments");
                TypeSalles = await _serviceTypeSalle.GetAllTAsync("TypeSalles");
                Salles = await _serviceSalle.GetAllTAsync("Salles");
                Console.WriteLine("Recharger correctement");
                foreach (var salle in Salles)
                {
                    salle.IsEditable = false; // initialement non modifiable
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
                var salle = new Salle
                {
                    NomSalle = NewSalle.NomSalle,
                    SuperficieSalle = NewSalle.SuperficieSalle,
                    IdBatiment = NewSalle.IdBatiment,
                    IdTypeSalle = NewSalle.IdTypeSalle
                };

                try
                {
                    await _serviceSalle.PostTAsync("Salles", salle);
                    await LoadAsync();
                }
                catch (JsonException jsonEx)
                {
                    ErrorMessage = $"Erreur JSON : {jsonEx.Message}";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur dans AddSalle : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
            }
        }

        private bool IsValidNew()
        {
            return !string.IsNullOrWhiteSpace(NewSalle.NomSalle) &&
                   NewSalle.SuperficieSalle > 0;
        }

        public void Edit(Salle salle)
        {
            salle.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(Salle salle)
        {
            await UpdateAsync(salle);
            salle.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(Salle salle)
        {
            try
            {
                await _serviceSalle.PutTAsync($"Salles/{salle.IdSalle}", salle);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateSalle : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceSalle.DeleteTAsync($"Salles", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de la salle : {ex.Message}";
            }
        }
    }
}
