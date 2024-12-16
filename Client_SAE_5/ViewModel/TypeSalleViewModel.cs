using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class TypeSalleViewModel
    {
        private readonly WSService<TypeSalleDTO> _typesalleService;

        public TypeSalleViewModel()
        {
            _typesalleService = new WSService<TypeSalleDTO>();
        }

        public List<TypeSalleDTO> TypeSalles { get; private set; } = new List<TypeSalleDTO>();

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


        public void EditTypeSalle(TypeSalleDTO typeSalle)
        {
            EditableTypeSalle = typeSalle;
        }

        private bool IsValidTypeSalle(TypeSalleDTO typeSalle)
        {
            return !string.IsNullOrWhiteSpace(typeSalle.NomTypeSalle);
        }
    }
}
