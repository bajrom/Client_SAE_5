using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages;
using Client_SAE_5.Pages.CRUD.Batiment;
using Client_SAE_5.Pages.CRUD.TypeSalle;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5.ViewModel
{
    public class TypeSalleViewModel
    {
        private readonly WSService<TypeSalleDTO> _typesalleService;
        private readonly WSService<TypeSalleDetailDTO> _typesalleDetailService;

        public TypeSalleDetailDTO TypesalleInEdition { get; private set; }
        public DataStorage DBData;

        public TypeSalleViewModel(DataStorage dBData)
        {
            _typesalleService = new WSService<TypeSalleDTO>();
            _typesalleDetailService = new WSService<TypeSalleDetailDTO>();
            DBData = dBData;
        }

        public List<TypeSalleDTO> TypeSalles { get; private set; } = new List<TypeSalleDTO>();

        public TypeSalleDetailDTO SelectedTypeSalleDetails { get; private set; }

        public string ErrorMessage { get; private set; }

        public async Task LoadTypesSallesAsync()
        {
            try
            {
                DBData.TypesSalle = await _typesalleService.GetAllTAsync("TypeSalles");
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

        public async Task SetupTypeSalleEdition(int idTypeSalle)
        {
            TypeSalleDetailDTO temp = await _typesalleDetailService.GetTAsync("TypeSalles", idTypeSalle);

            TypesalleInEdition = temp;
        }
        public async Task SetupNewTypeSalle()
        {
            TypesalleInEdition = new TypeSalleDetailDTO();
        }

        public async Task AddTypeSallesAsync()
        {
            TypeSalleDTO newTypesalle = new TypeSalleDTO
            {
                IdTypeSalle = TypesalleInEdition.IdTypeSalle,
                NomTypeSalle = TypesalleInEdition.NomTypeSalle,
            };

            if (IsValidTypeSalle(newTypesalle))
            {
                try
                {
                    newTypesalle = await _typesalleService.PostTAsync("TypeSalles", newTypesalle);
                    await LoadTypesSallesAsync();
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
            TypeSalleDTO newTypesalle = new TypeSalleDTO
            {
                IdTypeSalle = TypesalleInEdition.IdTypeSalle,
                NomTypeSalle = TypesalleInEdition.NomTypeSalle,
            };

            if (IsValidTypeSalle(newTypesalle))
            {
                try
                {
                    await _typesalleService.PutTAsync($"TypeSalles/{newTypesalle.IdTypeSalle}", newTypesalle);
                    await LoadTypesSallesAsync();
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
                DBData.TypesSalle.Remove(DBData.TypesSalle.Single(c => c.IdTypeSalle == idTypeSalle));
                await LoadTypesSallesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du type de salle : {ex.Message}";
            }
        }

        public void ResetError()
        {
            ErrorMessage = "";
        }

        private bool IsValidTypeSalle(TypeSalleDTO typeSalle)
        {
            return !string.IsNullOrWhiteSpace(typeSalle.NomTypeSalle);
        }
    }
}
