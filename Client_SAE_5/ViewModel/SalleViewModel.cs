using Client_SAE_5.DTO;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class SalleViewModel
    {
        private readonly WSService<SalleDTO> _salleService;
        private readonly WSService<SalleDetailDTO> _salleDetailService;
        private readonly WSService<SalleSansNavigationDTO> _salleSansNavigationService;

        public SalleViewModel()
        {
            _salleService = new WSService<SalleDTO>();
            _salleDetailService = new WSService<SalleDetailDTO>();
            _salleSansNavigationService = new WSService<SalleSansNavigationDTO>();
        }

        // Liste des salles pour l'affichage principal
        public List<SalleDTO> Salles { get; private set; } = new List<SalleDTO>();

        // Détails d'une salle spécifique
        public SalleDetailDTO SelectedSalleDetails { get; private set; }

        // Salle pour l'ajout ou la modification
        public SalleSansNavigationDTO EditableSalle { get; set; } = new SalleSansNavigationDTO();

        // Indicateur d'erreur
        public string ErrorMessage { get; private set; }

        // Charger toutes les salles
        public async Task LoadSallesAsync()
        {
            try
            {
                Salles = await _salleService.GetAllTAsync("Salles");
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

        // Ajouter une nouvelle salle
        public async Task AddSalleAsync()
        {
            if (IsValidSalle(EditableSalle))
            {
                try
                {
                    await _salleSansNavigationService.PostTAsync("Salles", EditableSalle);
                    await LoadSallesAsync();
                    EditableSalle = new SalleSansNavigationDTO(); // Réinitialiser le formulaire
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
            if (IsValidSalle(EditableSalle))
            {
                try
                {
                    await _salleSansNavigationService.PutTAsync($"Salles/{EditableSalle.IdSalle}", EditableSalle);
                    await LoadSallesAsync();
                    EditableSalle = new SalleSansNavigationDTO(); // Réinitialiser le formulaire
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
                await LoadSallesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de la salle : {ex.Message}";
            }
        }

        // Définir une salle comme modifiable
        public void EditSalle(SalleDTO salle)
        {
            EditableSalle = new SalleSansNavigationDTO
            {
                IdSalle = salle.IdSalle,
                NomSalle = salle.NomSalle
                // Vous pouvez inclure d'autres champs si nécessaire
            };
        }

        // Vérifier si les données de la salle sont valides
        private bool IsValidSalle(SalleSansNavigationDTO salle)
        {
            return !string.IsNullOrWhiteSpace(salle.NomSalle) &&
                   salle.IdBatiment > 0 &&
                   salle.IdTypeSalle > 0;
        }
    }
}
