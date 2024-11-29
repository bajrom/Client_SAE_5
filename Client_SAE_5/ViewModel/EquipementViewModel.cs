using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class EquipementViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<TypeEquipement> _serviceTypeEquipement;
        private readonly WSService<Salle> _serviceSalle;
        private readonly WSService<Equipement> _serviceEquipement;

        public EquipementViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceTypeEquipement = new WSService<TypeEquipement>();
            _serviceSalle = new WSService<Salle>();
            _serviceEquipement = new WSService<Equipement>();
        }

        public List<Equipement> Equipements { get; private set; } = new List<Equipement>();
        public List<Salle> Salles { get; private set; } = new List<Salle>();
        public List<TypeEquipement> TypeEquipements { get; private set; } = new List<TypeEquipement>();

        public Equipement NewEquipement { get; private set; } = new Equipement();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                Salles = await _serviceSalle.GetAllTAsync("Salles");
                TypeEquipements = await _serviceTypeEquipement.GetAllTAsync("TypeEquipements");
                Equipements = await _serviceEquipement.GetAllTAsync("Equipements");

                foreach (var equipement in Equipements)
                {
                    equipement.IsEditable = false; // initialement non modifiable
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
                var equipement = new Equipement
                {
                    NomEquipement = NewEquipement.NomEquipement,
                    Longeur = NewEquipement.Longeur,
                    Largeur = NewEquipement.Largeur,
                    Hauteur = NewEquipement.Hauteur,
                    XEquipement = NewEquipement.XEquipement,
                    YEquipement = NewEquipement.YEquipement,
                    ZEquipement = NewEquipement.ZEquipement,
                    EstActif = NewEquipement.EstActif,
                    IdSalle = NewEquipement.IdSalle,
                    IdTypeEquipement = NewEquipement.IdTypeEquipement
                };

                try
                {
                    await _serviceEquipement.PostTAsync("Equipements", equipement);
                    await LoadAsync();
                }
                catch (JsonException jsonEx)
                {
                    ErrorMessage = $"Erreur JSON : {jsonEx.Message}";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur dans AddEquipement : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
            }
        }

        private bool IsValidNew()
        {
            return !string.IsNullOrWhiteSpace(NewEquipement.NomEquipement) &&
                   NewEquipement.Longeur > 0 && NewEquipement.Largeur > 0 && NewEquipement.Hauteur > 0 && NewEquipement.XEquipement > 0 && NewEquipement.YEquipement > 0 && NewEquipement.ZEquipement > 0 && (NewEquipement.EstActif == "OUI" || NewEquipement.EstActif == "NON" || NewEquipement.EstActif == "NSP");
        }

        public void Edit(Equipement equipement)
        {
            equipement.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(Equipement equipement)
        {
            await UpdateAsync(equipement);
            equipement.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(Equipement equipement)
        {
            try
            {
                await _serviceEquipement.PutTAsync($"Equipements/{equipement.IdSalle}", equipement);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateEquipement : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceEquipement.DeleteTAsync($"Equipements", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'équipement : {ex.Message}";
            }
        }
    }
}
