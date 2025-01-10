using Client_SAE_5.DTO;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Utils.Singleton;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client_SAE_5.ViewModel
{
    public class SalleViewModel
    {
        private readonly WSService<SalleDTO> _salleService;
        private readonly WSService<SalleDetailDTO> _salleDetailService;
        private readonly WSService<SalleSansNavigationDTO> _salleSansNavigationService;
        private readonly WSService<BatimentDTO> _batimentService;
        private readonly WSService<TypeSalleDTO> _typesalleService;
        public DataStorage DBData;

        public SalleViewModel(DataStorage data)
        {
            _salleService = new WSService<SalleDTO>();
            _salleDetailService = new WSService<SalleDetailDTO>();
            _salleSansNavigationService = new WSService<SalleSansNavigationDTO>();
            _batimentService = new WSService<BatimentDTO>();
            _typesalleService = new WSService<TypeSalleDTO>();
            this.DBData = data;
        }

        // Détails d'une salle spécifique
        public SalleDetailDTO SelectedSalleDetails { get; private set; }

        public SalleDetailDTO SalleInEdition { get; private set; }

        public int SalleInEditionOldBatimentId;
        public int SalleInEditionOldTypeSalleId;

        public List<BatimentDTO> Batiments { get; private set; } = new List<BatimentDTO>();

        public List<TypeSalleDTO> TypeSalle { get; private set; } = new List<TypeSalleDTO>();

        // Indicateur d'erreur
        public string ErrorMessage { get; private set; }

        // Charger toutes les salles
        public async Task LoadSallesAsync()
        {
            try
            {
                DBData.Salles = await _salleService.GetAllTAsync("Salles");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des salles : {ex.Message}";
            }
        }

        // Charger les détails d'une salle
        public async Task LoadSalleDetailsAsync(int idSalle)
        {
            try
            {
                SelectedSalleDetails = await _salleDetailService.GetTAsync("Salles", idSalle);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails de la salle : {ex.Message}";
            }
        }

        public async Task LoadBatimentsAsync()
        {
            try
            {
                DBData.Batiments = await _batimentService.GetAllTAsync("Batiments");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des batiments : {ex.Message}";
            }
        }

        public async Task LoadTypeSalleAsync()
        {
            try
            {
                DBData.TypesSalle = await _typesalleService.GetAllTAsync("TypeSalles");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des types de salles : {ex.Message}";
            }
        }

        public async Task SetupSalleEdition(int idSalle)
        {
            SalleDetailDTO temp = await _salleDetailService.GetTAsync("Salles", idSalle);

            if (DBData.Batiments == null || DBData.Batiments.Count == 0)
            {
                await LoadBatimentsAsync();
            }

            if (DBData.TypesSalle == null || DBData.TypesSalle.Count == 0)
            {
                await LoadTypeSalleAsync();
            }

            SalleInEdition = temp;
            SalleInEditionOldBatimentId = SalleInEdition.Batiment.IdBatiment;
            SalleInEditionOldTypeSalleId = SalleInEdition.TypeSalle.IdTypeSalle;
        }

        public async Task SetupNewSalle()
        {
            if (DBData.Batiments == null || DBData.Batiments.Count == 0)
            {
                await LoadBatimentsAsync();
            }

            if (DBData.TypesSalle == null || DBData.TypesSalle.Count == 0)
            {
                await LoadTypeSalleAsync();
            }

            SalleInEdition = new SalleDetailDTO();
        }

        // Ajouter une nouvelle salle
        public async Task AddSalleAsync()
        {
            SalleSansNavigationDTO newSalle = new SalleSansNavigationDTO
            {
                IdSalle = SalleInEdition.IdSalle,
                NomSalle = SalleInEdition.NomSalle,
                IdBatiment = SalleInEdition.Batiment.IdBatiment,
                IdTypeSalle = SalleInEdition.TypeSalle.IdTypeSalle,
            };
            if (IsValidSalle(newSalle))
            {
                try
                {
                    newSalle = await _salleSansNavigationService.PostTAsync("Salles", newSalle);
                    await LoadSallesAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout de la salle : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        // Mettre à jour une salle existante
        public async Task UpdateSalleAsync()
        {
            SalleSansNavigationDTO newSalle = new SalleSansNavigationDTO
            {
                IdSalle = SalleInEdition.IdSalle,
                NomSalle = SalleInEdition.NomSalle,
                IdBatiment = SalleInEdition.Batiment.IdBatiment,
                IdTypeSalle = SalleInEdition.TypeSalle.IdTypeSalle,
            };
            if (IsValidSalle(newSalle))
            {
                try
                {
                    newSalle = await _salleSansNavigationService.PutTAsync($"Salles/{newSalle.IdSalle}", newSalle);
                    await LoadSallesAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour de la salle : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        // Supprimer une salle
        public async Task DeleteSalleAsync(int idSalle)
        {
            try
            {
                await _salleSansNavigationService.DeleteTAsync("Salles", idSalle);
                DBData.Salles.Remove(DBData.Salles.Single(c => c.IdSalle == idSalle));
                await LoadSallesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de la salle : {ex.Message}";
            }
        }

        // Vérifier si les données de la salle sont valides
        private bool IsValidSalle(SalleSansNavigationDTO salle)
        {
            return !string.IsNullOrWhiteSpace(salle.NomSalle) &&
                   salle.IdBatiment > 0 &&
                   salle.IdTypeSalle > 0;
        }
        public void ResetError()
        {
            ErrorMessage = "";
        }
    }
}
