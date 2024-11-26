using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;

namespace Client_SAE_5.ViewModel
{
    public class MurViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly WSService<Direction> _serviceDirection;
        private readonly WSService<Salle> _serviceSalle;
        private readonly WSService<Mur> _serviceMur;

        public MurViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _serviceDirection = new WSService<Direction>();
            _serviceSalle = new WSService<Salle>();
            _serviceMur = new WSService<Mur>();
        }

        public List<Mur> Murs { get; private set; } = new List<Mur>();
        public List<Direction> Directions { get; private set; } = new List<Direction>();
        public List<Salle> Salles { get; private set; } = new List<Salle>();

        public Mur NewMur { get; private set; } = new Mur();

        public string ErrorMessage { get; private set; } // Pour stocker les messages d'erreur

        public async Task LoadAsync()
        {
            try
            {
                Directions = await _serviceDirection.GetAllTAsync("Directions");
                Salles = await _serviceSalle.GetAllTAsync("Salles");
                Murs = await _serviceMur.GetAllTAsync("Murs");
                Console.WriteLine("Recharger correctement");
                foreach (var mur in Murs)
                {
                    mur.IsEditable = false; // initialement non modifiable
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
                var mur = new Mur
                {
                    Longueur = NewMur.Longueur,
                    Hauteur = NewMur.Hauteur,
                    Orientation = NewMur.Orientation,
                    IdSalle = NewMur.IdSalle,
                    IdDirection = NewMur.IdDirection
                };

                try
                {
                    await _serviceMur.PostTAsync("Murs", mur);
                    await LoadAsync();
                }
                catch (JsonException jsonEx)
                {
                    ErrorMessage = $"Erreur JSON : {jsonEx.Message}";
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur dans AddMur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs requis.";
            }
        }

        private bool IsValidNew()
        {
            return NewMur.Longueur > 0 && NewMur.Hauteur > 0 && NewMur.Orientation >= 0 && NewMur.Orientation <= 360;
        }

        public void Edit(Mur mur)
        {
            mur.IsEditable = true; // Active le mode d'édition
        }

        public async Task SaveAsync(Mur mur)
        {
            await UpdateAsync(mur);
            mur.IsEditable = false; // Désactive le mode d'édition
        }

        private async Task UpdateAsync(Mur mur)
        {
            try
            {
                await _serviceMur.PutTAsync($"Murs/{mur.IdMur}", mur);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur dans UpdateMur : {ex.Message}";
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _serviceMur.DeleteTAsync($"Murs", id);
                await LoadAsync();
                Console.WriteLine("Supprimer correctement");

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du mur : {ex.Message}";
            }
        }
    }
}
