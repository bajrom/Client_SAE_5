using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages;
using Client_SAE_5.Pages.CRUD.Batiment;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5.ViewModel
{
    public class TypeEquipementViewModel
    {
        private readonly WSService<TypeEquipementDTO> _typeequipementService;
        private readonly WSService<TypeEquipementDetailDTO> _typeequipementDetailService;

        public TypeEquipementDetailDTO TypeEquipementInEdition { get; private set; }
        public DataStorage DBData;

        public TypeEquipementViewModel(DataStorage dBData)
        {
            _typeequipementService = new WSService<TypeEquipementDTO>();
            _typeequipementDetailService = new WSService<TypeEquipementDetailDTO>();
            DBData = dBData;
        }

        public List<TypeEquipementDTO> TypesEquipement { get; private set; } = new List<TypeEquipementDTO>();

        public TypeEquipementDetailDTO SelectedTypeEquipementDetails { get; private set; }

        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Permet de charger tous les types d'équipements de façon asynchrone.
        /// </summary>
        public async Task LoadTypesEquipementAsync()
        {
            try
            {
                DBData.TypesEquipement = await _typeequipementService.GetAllTAsync("TypeEquipements");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des type d'équipement : {ex.Message}";
            }
        }

        /// <summary>
        /// Permet de charger les détails d'un type d'équipement de façon asynchrone
        /// </summary>
        /// <param name="idTypeEquipement">ID du type d'équipement à charger</param>
        public async Task LoadTypeEquipementDetailsAsync(int idTypeEquipement)
        {
            try
            {
                SelectedTypeEquipementDetails = await _typeequipementDetailService.GetTAsync("TypeEquipements", idTypeEquipement);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails du type : {ex.Message}";
            }
        }

        /// <summary>
        /// Permet d'initialiser le type d'équipement et ses détails pour son édition
        /// </summary>
        /// <param name="idTypeEquipement"></param>
        public async Task SetupTypeEquipementEdition(int idTypeEquipement)
        {
            TypeEquipementDetailDTO temp = await _typeequipementDetailService.GetTAsync("TypeEquipements", idTypeEquipement);

            TypeEquipementInEdition = temp;
        }

        /// <summary>
        /// Permet de reset l'édition du type d'équipement utilisé quand appui sur annuler
        /// </summary>
        public async Task SetupNewTypeEquipement()
        {
            TypeEquipementInEdition = new TypeEquipementDetailDTO();
        }

        /// <summary>
        /// Permet de rajouter un type d'équipement de façon asynchrone
        /// </summary>
        public async Task AddTypeEquipementAsync()
        {
            TypeEquipementDTO newCapteur = new TypeEquipementDTO
            {
                IdTypeEquipement = TypeEquipementInEdition.IdTypeEquipement,
                NomTypeEquipement = TypeEquipementInEdition.NomTypeEquipement,
            };
            if (IsValidTypeEquipement(newCapteur))
            {
                try
                {
                    newCapteur = await _typeequipementService.PostTAsync("TypeEquipements", newCapteur);
                    await LoadTypesEquipementAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout du type d'équipement : {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Permet de mettre à jour un type d'équipement de façon asynchrone
        /// </summary>
        public async Task UpdateTypeEquipementAsync()
        {
            TypeEquipementDTO newCapteur = new TypeEquipementDTO
            {
                IdTypeEquipement = TypeEquipementInEdition.IdTypeEquipement,
                NomTypeEquipement = TypeEquipementInEdition.NomTypeEquipement,
            };
            if (IsValidTypeEquipement(newCapteur))
            {
                try
                {
                    await _typeequipementService.PutTAsync($"TypeEquipements/{newCapteur.IdTypeEquipement}", newCapteur);
                    await LoadTypesEquipementAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour du type d'équipement : {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Permet de supprimer un type d'équipement de façon asynchrone
        /// </summary>
        /// <param name="idTypeEquipement">ID du type d'équipement à supprimer</param>
        public async Task DeleteTypeEquipementAsync(int idTypeEquipement)
        {
            try
            {
                await _typeequipementService.DeleteTAsync("TypeEquipements", idTypeEquipement);
                DBData.TypesEquipement.Remove(DBData.TypesEquipement.Single(c => c.IdTypeEquipement == idTypeEquipement));
                await LoadTypesEquipementAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du type d'équipement : {ex.Message}";
            }
        }

        /// <summary>
        /// Vérifie si les propriétés d'un type d'équipement sont valides
        /// </summary>
        /// <param name="typeEquipement">Type d'équipement dont les propriétés sont à vérifier</param>
        /// <returns>true s'il est correct, false sinon</returns>
        private bool IsValidTypeEquipement(TypeEquipementDTO typeEquipement)
        {
            if (string.IsNullOrWhiteSpace(typeEquipement.NomTypeEquipement))
            {
                ErrorMessage = "Veuillez donner un nom";
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
