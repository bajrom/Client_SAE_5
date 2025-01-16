using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages.CRUD.Mur;
using System.Globalization;
using System.Text.RegularExpressions;
using Client_SAE_5.Utils.Singleton;
using Client_SAE_5.Pages.CRUD.Capteur;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client_SAE_5.ViewModel
{
    public class EquipementViewModel
    {
        private readonly WSService<EquipementDTO> _equipementService;
        private readonly WSService<EquipementDetailDTO> _equipementDetailService;
        private readonly WSService<EquipementSansNavigationDTO> _equipementSansNavigationService;
        private readonly WSService<SalleDTO> _salleService;
        private readonly WSService<MurDTO> _murService;
        private readonly WSService<MurDetailDTO> _murDetailService;
        private readonly WSService<TypeEquipementDTO> _typeequipementService;
        public DataStorage DBData;

        public EquipementViewModel(DataStorage data)
        {
            _equipementService = new WSService<EquipementDTO>();
            _equipementDetailService = new WSService<EquipementDetailDTO>();
            _equipementSansNavigationService = new WSService<EquipementSansNavigationDTO>();
            _salleService = new WSService<SalleDTO>();
            _murService = new WSService<MurDTO>();
            _murDetailService = new WSService<MurDetailDTO>();
            _typeequipementService = new WSService<TypeEquipementDTO>();
            this.DBData = data;
        }

        public EquipementDetailDTO SelectedEquipementDetails { get; private set; }

        public EquipementDetailDTO EquipementInEdition { get; private set; }

        public int EquipementInEditionOldTypeEquipementId;
        public int EquipementInEditionOldMurId;

        public List<MurDTO> Murs { get; private set; } = new List<MurDTO>();

        public List<string> NomSalles { get; private set; } = new List<string>();
        public string EquipementInEditionNomSalleSelected { get; set; } = "";

        public List<TypeEquipementDTO> TypesEquipement { get; private set; } = new List<TypeEquipementDTO>();

        private MurDetailDTO murSelected { get; set; } = new MurDetailDTO();
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Permet de charger tous les équipements de façon asynchrone.
        /// </summary>
        public async Task LoadEquipementsAsync()
        {
            try
            {
                DBData.Equipements = await _equipementService.GetAllTAsync("Equipement");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des équipements : {ex.Message}";
            }
        }

        /// <summary>
        /// Permet de charger les détails d'un équipement de façon asynchrone
        /// </summary>
        /// <param name="idEquipement">ID de l'équipement à charger</param>
        public async Task LoadEquipementDetailsAsync(int idEquipement)
        {
            try
            {
                SelectedEquipementDetails = await _equipementDetailService.GetTAsync("Equipement", idEquipement);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails de l'équipement : {ex.Message}";
            }
        }

        /// <summary>
        /// Permet d'obtenir la liste unique de tous les nom des salles à partir des mur
        /// </summary>
        public void LoadNomSallesAsync()
        {
            NomSalles = DBData.Murs
            .Where(mur => !string.IsNullOrEmpty(mur.NomSalle)) // Filtre les noms non nulls
            .Select(mur => mur.NomSalle)
            .Distinct() // Supprime les doublons
            .ToList();
        }

        /// <summary>
        /// permet de charger tous les murs pour faire les clé étrangères
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
        /// permet de charger tous les type équipements pour faire les clé étrangères
        /// </summary>
        public async Task LoadTypeEquipementAsync()
        {
            try
            {
                DBData.TypesEquipement = await _typeequipementService.GetAllTAsync("TypeEquipements");

                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des types d'équipement : {ex.Message}";
            }
        }

        /// <summary>
        /// Permet d'initialiser l'équipement et ses détails pour son édition
        /// </summary>
        /// <param name="idEquipement"></param>
        public async Task SetupEquipementEdition(int idEquipement)
        {
            EquipementDetailDTO temp = await _equipementDetailService.GetTAsync("Equipement", idEquipement);

            murSelected = new MurDetailDTO
            {
                IdMur = temp.Mur.IdMur,
                IdSalle = temp.Mur.IdSalle,
                IdDirection = temp.Mur.IdDirection,
                Longueur = temp.Mur.Longueur,
                Hauteur = temp.Mur.Hauteur,
                Orientation = temp.Mur.Orientation,
            };

            if (DBData.Murs == null || DBData.Murs.Count == 0)
            {
                await LoadMursAsync();
            }

            if (DBData.TypesEquipement == null || DBData.TypesEquipement.Count == 0)
            {
                await LoadTypeEquipementAsync();
            }

            LoadNomSallesAsync();

            EquipementInEdition = temp;
            EquipementInEditionNomSalleSelected = EquipementInEdition.Salle.NomSalle;
            EquipementInEditionOldMurId = EquipementInEdition.Mur.IdMur;
            EquipementInEditionOldTypeEquipementId = EquipementInEdition.TypeEquipement.IdTypeEquipement;
        }

        /// <summary>
        /// Permet de reset l'édition de l'équipement utilisé quand appui sur annuler
        /// </summary>
        public async Task SetupNewEquipement()
        {
            if (DBData.Murs == null || DBData.Murs.Count == 0)
            {
                await LoadMursAsync();
            }

            if (DBData.TypesEquipement == null || DBData.TypesEquipement.Count == 0)
            {
                await LoadTypeEquipementAsync();
            }

            LoadNomSallesAsync();
            EquipementInEdition = new EquipementDetailDTO();
        }

        /// <summary>
        /// Permet de rajouter un équipement de façon asynchrone
        /// </summary>
        public async Task AddEquipementAsync()
        {
            EquipementSansNavigationDTO newEquipement = new EquipementSansNavigationDTO
            {
                IdEquipement = EquipementInEdition.IdEquipement,
                NomEquipement = EquipementInEdition.NomEquipement,
                EstActif = EquipementInEdition.EstActif,
                Longueur = EquipementInEdition.Longueur,
                Largeur = EquipementInEdition.Largeur,
                Hauteur = EquipementInEdition.Hauteur,
                XEquipement = EquipementInEdition.PositionX,
                YEquipement = EquipementInEdition.PositionY,
                ZEquipement = EquipementInEdition.PositionZ,
                IdMur = EquipementInEdition.Mur.IdMur,
                IdTypeEquipement = EquipementInEdition.TypeEquipement.IdTypeEquipement,
            };
            if (IsValidEquipement(newEquipement))
            {
                try
                {
                    newEquipement = await _equipementSansNavigationService.PostTAsync("Equipement", newEquipement);
                    await LoadEquipementsAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout de l'équipement : {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Permet de mettre à jour un équipement de façon asynchrone
        /// </summary>
        public async Task UpdateEquipementAsync()
        {
            EquipementSansNavigationDTO newEquipement = new EquipementSansNavigationDTO
            {
                IdEquipement = EquipementInEdition.IdEquipement,
                NomEquipement = EquipementInEdition.NomEquipement,
                EstActif = EquipementInEdition.EstActif,
                Longueur = EquipementInEdition.Longueur,
                Largeur = EquipementInEdition.Largeur,
                Hauteur = EquipementInEdition.Hauteur,
                XEquipement = EquipementInEdition.PositionX,
                YEquipement = EquipementInEdition.PositionY,
                ZEquipement = EquipementInEdition.PositionZ,
                IdMur = EquipementInEdition.Mur.IdMur,
                IdTypeEquipement = EquipementInEdition.TypeEquipement.IdTypeEquipement,
            };
            if (IsValidEquipement(newEquipement))
            {
                try
                {
                    newEquipement = await _equipementSansNavigationService.PutTAsync($"Equipement/{newEquipement.IdEquipement}", newEquipement);
                    await LoadEquipementsAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour de l'équipement : {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Permet de supprimer un équipement de façon asynchrone
        /// </summary>
        /// <param name="idEquipement">ID de l'équipement à supprimer</param>
        public async Task DeleteEquipementAsync(int idEquipement)
        {
            try
            {
                await _equipementService.DeleteTAsync("Equipement", idEquipement);
                DBData.Equipements.Remove(DBData.Equipements.Single(c => c.IdEquipement == idEquipement));
                await LoadEquipementsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'équipement : {ex.Message}";
            }
        }

        /// <summary>
        /// Vérifie si les propriétés d'un équipement sont valides
        /// </summary>
        /// <param name="equipement">Équipement dont les propriétés sont à vérifier</param>
        /// <returns>true s'il est correct, false sinon</returns>
        private bool IsValidEquipement(EquipementSansNavigationDTO equipement)
        {
            if (string.IsNullOrWhiteSpace(equipement.NomEquipement))
            {
                ErrorMessage = "Veuillez donner un nom";
                return false;
            }
            else if (equipement.IdMur <= 0)
            {
                ErrorMessage = "Veuillez sélectionner un mur";
                return false;
            }
            else if (equipement.IdTypeEquipement <= 0)
            {
                ErrorMessage = "Veuillez sélectionner un type d'équipement";
                return false;
            }
            else if (equipement.XEquipement < 0 || equipement.XEquipement > murSelected.Longueur)
            {
                ErrorMessage = $"X en dehors des limites (Longueur du mur: {murSelected.Longueur} cm)";
                return false;
            }
            else if (equipement.YEquipement < 0 || equipement.YEquipement > murSelected.Hauteur)
            {
                ErrorMessage = $"Y en dehors des limites (Hauteur du mur: {murSelected.Hauteur} cm)";
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

        /// <summary>
        /// Méthode déclencher quand le mur est changer dans la popup de add/update, il permet de get les détails du mur général sélectionné
        /// (pour vérifier si l'équipement est en x et y dans le mur et pas dehors)
        /// </summary>
        /// <param name="id">ID du mur</param>
        public async Task ChangeMur(int id)
        {
            murSelected = await _murDetailService.GetTAsync("Murs", id);
            EquipementInEdition.Mur.IdMur = id;
        }
    }
}
