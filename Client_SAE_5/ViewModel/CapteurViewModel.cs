using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class CapteurViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<Salle> _serviceSalle;
        private readonly WSService<Capteur> _serviceCapteur;

        public CapteurViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceSalle = new WSService<Salle>();
            _serviceCapteur = new WSService<Capteur>();
        }

        public List<Capteur> Capteurs { get; private set; } = new List<Capteur>();
        public List<Salle> Salles { get; private set; } = new List<Salle>();

        public Capteur NewCapteur { get; private set; } = new Capteur();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                Salles = await _serviceSalle.GetAllTAsync("Salles");
                Capteurs = await _serviceCapteur.GetAllTAsync("Capteurs");
                Console.WriteLine("Recharger correctement");
                foreach (var capteur in Capteurs)
                {
                    capteur.IsEditable = false; // initialement non modifiable
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
                var capteur = new Capteur
                {
                    NomTypeCapteur = NewCapteur.NomTypeCapteur,
                    EstActif = NewCapteur.EstActif,
                    XCapteur = NewCapteur.XCapteur,
                    YCapteur = NewCapteur.YCapteur,
                    ZCapteur = NewCapteur.ZCapteur,
                    IdSalle = NewCapteur.IdSalle,
                };

                try
                {
                    await _serviceCapteur.PostTAsync("Capteurs", capteur);
                    await LoadAsync();
                }
                catch (JsonException jsonEx)
                {
                    ErrorMessage = $"Erreur JSON : {jsonEx.Message}";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur dans AddCapteur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
            }
        }

        private bool IsValidNew()
        {
            return !string.IsNullOrWhiteSpace(NewCapteur.NomTypeCapteur) && NewCapteur.XCapteur > 0 && NewCapteur.YCapteur > 0 && NewCapteur.ZCapteur > 0 &&
                   (NewCapteur.EstActif == "OUI" || NewCapteur.EstActif == "NON" || NewCapteur.EstActif == "NSP");
        }

        public void Edit(Capteur capteur)
        {
            capteur.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(Capteur capteur)
        {
            await UpdateAsync(capteur);
            capteur.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(Capteur capteur)
        {
            try
            {
                await _serviceCapteur.PutTAsync($"Capteurs/{capteur.IdCapteur}", capteur);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateCapteur : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceCapteur.DeleteTAsync($"Capteurs", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du capteur : {ex.Message}";
            }
        }
    }
}
