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

        /// <summary>
        /// Permet de charger tous les murs de façon asynchrone.
        /// </summary>
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

        /// <summary>
        /// Permet de charger les détails d'un mur de façon asynchrone
        /// </summary>
        /// <param name="idMur">ID du Mur à charger</param>
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

        /// <summary>
        /// permet de charger toutes les directions pour faire les clé étrangères
        /// </summary>
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

        /// <summary>
        /// permet de charger toutes les salles pour faire les clé étrangères
        /// </summary>
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

        /// <summary>
        /// Permet d'initialiser un mur et ses détails pour son édition
        /// </summary>
        /// <param name="idMur"></param>
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

        /// <summary>
        /// Permet de reset l'édition du mur utilisé quand appui sur annuler
        /// </summary>
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

        /// <summary>
        /// Permet de rajouter un mur de façon asynchrone
        /// </summary>
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
        }

        /// <summary>
        /// Permet de mettre à jour un mur de façon asynchrone
        /// </summary>
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
        }

        /// <summary>
        /// Permet de supprimer un mur de façon asynchrone
        /// </summary>
        /// <param name="idMur">ID du mur à supprimer</param>
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

        /// <summary>
        /// Vérifie si les propriétés d'un myr sont valides
        /// </summary>
        /// <param name="mur">mur dont les propriétés sont à vérifier</param>
        /// <returns>true s'il est correct, false sinon</returns>
        private bool IsValidMur(MurSansNavigationDTO mur)
        {
            if (mur.IdDirection <= 0)
            {
                ErrorMessage = "Veuillez sélectionner une direction";
                return false;
            }
            else if (mur.IdSalle <= 0)
            {
                ErrorMessage = "Veuillez sélectionner une salle";
                return false;
            }
            else if (mur.Hauteur < 1)
            {
                ErrorMessage = "Veuillez mettre une hauteur valide (>= 1)";
                return false;
            }
            else if (mur.Longueur < 1)
            {
                ErrorMessage = "Veuillez mettre une longueur valide (>= 1)";
                return false;
            }
            else if (mur.Orientation < 0 || mur.Orientation >= 360)
            {
                ErrorMessage = "Veuillez mettre une orientation valide (entre 0 et <360)";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Permet de réinitialiser le message d'erreur
        /// </summary>
        public void ResetError()
        {
            ErrorMessage = "";
        }
    }
}
