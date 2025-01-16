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

        /// <summary>
        /// permet de charger toutes les types de salles pour faire les clé étrangères
        /// </summary>
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

        /// <summary>
        /// Permet de charger les détails d'un type de salle de façon asynchrone
        /// </summary>
        /// <param name="idTypeSalle">ID du type de salle à charger</param>
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

        /// <summary>
        /// Permet d'initialiser le type de salle et ses détails pour son édition
        /// </summary>
        /// <param name="idTypeSalle"></param>
        public async Task SetupTypeSalleEdition(int idTypeSalle)
        {
            TypeSalleDetailDTO temp = await _typesalleDetailService.GetTAsync("TypeSalles", idTypeSalle);

            TypesalleInEdition = temp;
        }

        /// <summary>
        /// Permet de reset l'édition du type de salle utilisé quand appui sur annuler
        /// </summary>
        public async Task SetupNewTypeSalle()
        {
            TypesalleInEdition = new TypeSalleDetailDTO();
        }

        /// <summary>
        /// Permet de rajouter un type de salle de façon asynchrone
        /// </summary>
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
        }

        /// <summary>
        /// Permet de mettre à jour un type de salle de façon asynchrone
        /// </summary>
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
        }

        /// <summary>
        /// Permet de supprimer un type de salle de façon asynchrone
        /// </summary>
        /// <param name="idTypeSalle">ID du type de salle à supprimer</param>
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

        /// <summary>
        /// Vérifie si les propriétés d'un type de salle sont valides
        /// </summary>
        /// <param name="typeSalle">Type de salle dont les propriétés sont à vérifier</param>
        /// <returns>true s'il est correct, false sinon</returns>
        private bool IsValidTypeSalle(TypeSalleDTO typeSalle)
        {
            if (string.IsNullOrWhiteSpace(typeSalle.NomTypeSalle))
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
