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
        public async Task SetupTypeEquipementEdition(int idTypeEquipement)
        {
            TypeEquipementDetailDTO temp = await _typeequipementDetailService.GetTAsync("TypeEquipements", idTypeEquipement);

            TypeEquipementInEdition = temp;
        }

        public async Task SetupNewTypeEquipement()
        {
            TypeEquipementInEdition = new TypeEquipementDetailDTO();
        }

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
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

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
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

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

        private bool IsValidTypeEquipement(TypeEquipementDTO typeEquipement)
        {
            return !string.IsNullOrWhiteSpace(typeEquipement.NomTypeEquipement);
        }
        public void ResetError()
        {
            ErrorMessage = "";
        }
    }
}
