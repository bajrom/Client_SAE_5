using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5.ViewModel
{
    public class TypeSalleViewModel
    {
        private readonly WSService<TypeSalleDTO> _typesalleService;
        private readonly WSService<TypeSalleDetailDTO> _typesalleDetailService;

        public TypeSalleViewModel()
        {
            _typesalleService = new WSService<TypeSalleDTO>();
            _typesalleDetailService = new WSService<TypeSalleDetailDTO>();
        }

        public List<TypeSalleDTO> TypeSalles { get; private set; } = new List<TypeSalleDTO>();

        public TypeSalleDetailDTO SelectedTypeSalleDetails { get; private set; }

        public TypeSalleDTO EditableTypeSalle { get; set; } = new TypeSalleDTO();

        public string ErrorMessage { get; private set; }

        public async Task LoadTypesSallesAsync()
        {
            try
            {
                TypeSalles = await _typesalleService.GetAllTAsync("TypeSalles");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des type de salles : {ex.Message}";
            }
        }

        public async Task LoadTypeSalleDetailsAsync(int idTypeSalle)
        {
            try
            {
                SelectedTypeSalleDetails = await _typesalleDetailService.GetTAsync("TypeSalles", idTypeSalle);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails du type : {ex.Message}";
            }
        }

        public async Task<TypeSalleDetailDTO> LoadTypeSalleDetailsWithoutDefAsync(int idTypeSalle)
        {
            return await _typesalleDetailService.GetTAsync("TypeSalles", idTypeSalle);
        }

        public async Task AddTypeSallesAsync()
        {
            if (IsValidTypeSalle(EditableTypeSalle))
            {
                try
                {
                    await _typesalleService.PostTAsync("TypeSalles", EditableTypeSalle);
                    await LoadTypesSallesAsync();
                    EditableTypeSalle = new TypeSalleDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout du type de salle : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task UpdateTypeSallesAsync()
        {
            if (IsValidTypeSalle(EditableTypeSalle))
            {
                try
                {
                    await _typesalleService.PutTAsync($"TypeSalles/{EditableTypeSalle.IdTypeSalle}", EditableTypeSalle);
                    await LoadTypesSallesAsync();
                    EditableTypeSalle = new TypeSalleDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour du type de salle : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task DeleteTypeSalleAsync(int idTypeSalle)
        {
            try
            {
                await _typesalleService.DeleteTAsync("TypeSalles", idTypeSalle);
                await LoadTypesSallesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du type de salle : {ex.Message}";
            }
        }


        public void EditTypeSalle(TypeSalleDetailDTO typeSalle)
        {
            EditableTypeSalle = new TypeSalleDTO
            {
                IdTypeSalle = typeSalle.IdTypeSalle,
                NomTypeSalle = typeSalle.NomTypeSalle,
            };
        }

        private bool IsValidTypeSalle(TypeSalleDTO typeSalle)
        {
            return !string.IsNullOrWhiteSpace(typeSalle.NomTypeSalle);
        }
    }
}
