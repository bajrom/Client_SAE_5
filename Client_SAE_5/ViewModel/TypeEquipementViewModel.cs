using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5.ViewModel
{
    public class TypeEquipementViewModel
    {
        private readonly WSService<TypeEquipementDTO> _typeequipementService;
        private readonly WSService<TypeEquipementDetailDTO> _typeequipementDetailService;

        public TypeEquipementViewModel()
        {
            _typeequipementService = new WSService<TypeEquipementDTO>();
            _typeequipementDetailService = new WSService<TypeEquipementDetailDTO>();
        }

        public List<TypeEquipementDTO> TypesEquipement { get; private set; } = new List<TypeEquipementDTO>();

        public TypeEquipementDetailDTO SelectedTypeEquipementDetails { get; private set; }

        public TypeEquipementDTO EditableTypeEquipement { get; set; } = new TypeEquipementDTO();

        public string ErrorMessage { get; private set; }

        public async Task LoadTypesEquipementAsync()
        {
            try
            {
                TypesEquipement = await _typeequipementService.GetAllTAsync("TypeEquipements");
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
        public async Task<TypeEquipementDetailDTO> LoadTypeEquipementsDetailsWithoutDefAsync(int idTypeEquipement)
        {
            return await _typeequipementDetailService.GetTAsync("TypeEquipements", idTypeEquipement);
        }

        public async Task AddTypeEquipementAsync()
        {
            if (IsValidTypeEquipement(EditableTypeEquipement))
            {
                try
                {
                    await _typeequipementService.PostTAsync("TypeEquipements", EditableTypeEquipement);
                    await LoadTypesEquipementAsync();
                    EditableTypeEquipement = new TypeEquipementDTO(); // Réinitialiser le formulaire
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
            if (IsValidTypeEquipement(EditableTypeEquipement))
            {
                try
                {
                    await _typeequipementService.PutTAsync($"TypeEquipements/{EditableTypeEquipement.IdTypeEquipement}", EditableTypeEquipement);
                    await LoadTypesEquipementAsync();
                    EditableTypeEquipement = new TypeEquipementDTO(); // Réinitialiser le formulaire
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
                await LoadTypesEquipementAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du type d'équipement : {ex.Message}";
            }
        }


        public void EditTypeEquipement(TypeEquipementDetailDTO typeEquipement)
        {
            EditableTypeEquipement = new TypeEquipementDTO
            {
                IdTypeEquipement = typeEquipement.IdTypeEquipement,
                NomTypeEquipement = typeEquipement.NomTypeEquipement,
            };
        }

        private bool IsValidTypeEquipement(TypeEquipementDTO typeEquipement)
        {
            return !string.IsNullOrWhiteSpace(typeEquipement.NomTypeEquipement);
        }
    }
}
