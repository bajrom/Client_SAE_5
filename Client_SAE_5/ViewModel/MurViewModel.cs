using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages;
using Client_SAE_5.Utils.Singleton;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client_SAE_5.ViewModel
{
    public class MurViewModel
    {
        private readonly WSService<MurDTO> _murService;
        private readonly WSService<MurDetailDTO> _murDetailService;
        private readonly WSService<MurSansNavigationDTO> _murSansNavigationService;
        private readonly WSService<DirectionSansNavigationDTO> _directionService;
        private readonly WSService<SalleDTO> _salleService;
        public DataStorage DBData;

        public MurViewModel(DataStorage data)
        {
            _murService = new WSService<MurDTO>();
            _murDetailService = new WSService<MurDetailDTO>();
            _murSansNavigationService = new WSService<MurSansNavigationDTO>();
            _directionService = new WSService<DirectionSansNavigationDTO>();
            _salleService = new WSService<SalleDTO>();
            this.DBData = data;
        }

        public MurDetailDTO SelectedMurDetails { get; private set; }

        public MurDetailDTO MurInEdition { get; set; }

        public string ErrorMessage { get; private set; }

        public async Task LoadMursAsync()
        {
            try
            {
                DBData.Murs = await _murService.GetAllTAsync("Murs");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des murs : {ex.Message}";
            }
        }

        public async Task LoadMurDetailsAsync(int idMur)
        {
            try
            {
                SelectedMurDetails = await _murDetailService.GetTAsync("Murs", idMur);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails du mur : {ex.Message}";
            }
        }

        public async Task LoadDirectionsAsync()
        {
            try
            {
                DBData.Directions = await _directionService.GetAllTAsync("Direction");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des directions : {ex.Message}";
            }
        }

        public async Task LoadSallesAsync()
        {
            try
            {
                DBData.Salles = await _salleService.GetAllTAsync("Salles");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des salles : {ex.Message}";
            }
        }

        public async Task SetupMurEdition(int idMur)
        {
            MurDetailDTO temp = await _murDetailService.GetTAsync("Murs", idMur);

            if (DBData.Salles == null || DBData.Salles.Count == 0)
            {
                await LoadSallesAsync();
            }

            if (DBData.Directions == null || DBData.Directions.Count == 0)
            {
                await LoadDirectionsAsync();
            }

            MurInEdition = temp;
        }

        public async Task SetupNewMur()
        {
            if (DBData.Salles == null || DBData.Salles.Count == 0)
            {
                await LoadSallesAsync();
            }

            if (DBData.Directions == null || DBData.Directions.Count == 0)
            {
                await LoadDirectionsAsync();
            }

            MurInEdition = new MurDetailDTO();
        }

        public async Task AddMurAsync()
        {
            MurSansNavigationDTO newMur = new MurSansNavigationDTO
            {
                IdMur = MurInEdition.IdMur,
                IdDirection = MurInEdition.IdDirection,
                IdSalle = MurInEdition.IdSalle,
                Longueur = MurInEdition.Longueur,
                Hauteur = MurInEdition.Hauteur,
                Orientation = MurInEdition.Orientation
            };

            if (IsValidMur(newMur))
            {
                try
                {
                    await _murSansNavigationService.PostTAsync("Murs", newMur);
                    await LoadMursAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout du mur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task UpdateMurAsync()
        {
            MurSansNavigationDTO newMur = new MurSansNavigationDTO
            {
                IdMur = MurInEdition.IdMur,
                IdDirection = MurInEdition.IdDirection,
                IdSalle = MurInEdition.IdSalle,
                Longueur = MurInEdition.Longueur,
                Hauteur = MurInEdition.Hauteur,
                Orientation = MurInEdition.Orientation
            };

            if (IsValidMur(newMur))
            {
                try
                {
                    await _murSansNavigationService.PutTAsync($"Murs/{newMur.IdMur}", newMur);
                    await LoadMursAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour du mur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task DeleteMurAsync(int idMur)
        {
            try
            {
                await _murService.DeleteTAsync("Murs", idMur);
                await LoadMursAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du mur : {ex.Message}";
            }
        }

        private bool IsValidMur(MurSansNavigationDTO mur)
        {
            return mur.IdDirection > 0 &&
                   mur.IdSalle > 0 &&
                   mur.Hauteur > 0 &&
                   mur.Longueur > 0 &&
                   mur.Orientation >= 0 && mur.Orientation < 360;
        }

        public void ResetError()
        {
            ErrorMessage = "";
        }
    }
}
