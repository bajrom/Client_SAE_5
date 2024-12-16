using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class TypeEquipementViewModel
    {
        private readonly WSService<TypeEquipementDTO> _typeequipementService;

        public TypeEquipementViewModel()
        {
            _typeequipementService = new WSService<TypeEquipementDTO>();
        }

        public List<TypeEquipementDTO> TypesEquipement { get; private set; } = new List<TypeEquipementDTO>();

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


        public void EditTypeEquipement(TypeEquipementDTO typeEquipement)
        {
            EditableTypeEquipement = typeEquipement;
        }

        private bool IsValidTypeEquipement(TypeEquipementDTO typeEquipement)
        {
            return !string.IsNullOrWhiteSpace(typeEquipement.NomTypeEquipement);
        }
    }
}
