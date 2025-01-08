using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages.CRUD.Mur;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Client_SAE_5.ViewModel
{
    public class EquipementViewModel
    {
        private readonly WSService<EquipementDTO> _equipementService;
        private readonly WSService<EquipementDetailDTO> _equipementDetailService;
        private readonly WSService<EquipementSansNavigationDTO> _equipementSansNavigationService;

        public EquipementViewModel()
        {
            _equipementService = new WSService<EquipementDTO>();
            _equipementDetailService = new WSService<EquipementDetailDTO>();
            _equipementSansNavigationService = new WSService<EquipementSansNavigationDTO>();
        }

        public List<EquipementDTO> Equipements { get; private set; } = new List<EquipementDTO>();

        public EquipementDetailDTO SelectedEquipementDetails { get; private set; }

        public EquipementSansNavigationDTO EditableEquipement { get; set; } = new EquipementSansNavigationDTO();

        public List<MurDTO> Murs { get; private set; } = new List<MurDTO>();

        public List<string> NomSalles { get; private set; } = new List<string>();

        public List<TypeEquipementDTO> TypesEquipement { get; private set; } = new List<TypeEquipementDTO>();

        public string ErrorMessage { get; private set; }

        public async Task LoadEquipementsAsync()
        {
            try
            {
                Equipements = await _equipementService.GetAllTAsync("Equipement");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des équipements : {ex.Message}";
            }
        }

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

        public async Task LoadMursAsync()
        {
            try
            {
                var murService = new WSService<MurDTO>();
                Murs = await murService.GetAllTAsync("Murs");

                // Récupère les noms de salles non null et uniques
                NomSalles = Murs
                    .Where(mur => !string.IsNullOrEmpty(mur.NomSalle)) // Filtre les noms non nulls
                    .Select(mur => mur.NomSalle)
                    .Distinct() // Supprime les doublons
                    .ToList();

                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des murs : {ex.Message}";
            }
        }

        public async Task LoadTypesEquipementAsync()
        {
            try
            {
                var typeequipementService = new WSService<TypeEquipementDTO>();
                TypesEquipement = await typeequipementService.GetAllTAsync("TypeEquipements");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des types d'équipement : {ex.Message}";
            }
        }

        public async Task<EquipementDetailDTO> LoadEquipementsDetailsWithoutDefAsync(int idEquipement)
        {
            return await _equipementDetailService.GetTAsync("Equipement", idEquipement);
        }

        public async Task AddEquipementAsync()
        {
            if (IsValidEquipement(EditableEquipement))
            {
                try
                {
                    await _equipementSansNavigationService.PostTAsync("Equipement", EditableEquipement);
                    await LoadEquipementsAsync();
                    EditableEquipement = new EquipementSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout de l'équipement : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task UpdateEquipementAsync()
        {
            if (IsValidEquipement(EditableEquipement))
            {
                try
                {
                    await _equipementSansNavigationService.PutTAsync($"Equipement/{EditableEquipement.IdEquipement}", EditableEquipement);
                    await LoadEquipementsAsync();
                    EditableEquipement = new EquipementSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour de l'équipement : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task DeleteEquipementAsync(int idEquipement)
        {
            try
            {
                await _equipementService.DeleteTAsync("Equipement", idEquipement);
                await LoadEquipementsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'équipement : {ex.Message}";
            }
        }


        public void EditEquipement(EquipementDetailDTO equipement)
        {
            EditableEquipement = new EquipementSansNavigationDTO
            {
                IdEquipement = equipement.IdEquipement,
                NomEquipement = equipement.NomEquipement,
                XEquipement = equipement.PositionX,
                YEquipement = equipement.PositionY,
                ZEquipement = equipement.PositionZ,
                EstActif = equipement.EstActif,
                Longueur = equipement.Longueur,
                Largeur = equipement.Largeur,
                Hauteur = equipement.Hauteur,
                //IdMur = equipement.Murs.IdMur,
                IdTypeEquipement = equipement.TypeEquipement.IdTypeEquipement,
            };
        }

        private bool IsValidEquipement(EquipementSansNavigationDTO equipement)
        {
            return !string.IsNullOrWhiteSpace(equipement.NomEquipement) &&
                   equipement.IdMur > 0 && equipement.IdTypeEquipement > 0;
        }
    }
}
