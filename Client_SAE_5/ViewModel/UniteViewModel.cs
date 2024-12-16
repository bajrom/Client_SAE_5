using Client_SAE_5.DTO;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class UniteViewModel
    {
        private readonly WSService<UniteDTO> _uniteService;
        private readonly WSService<UniteDetailDTO> _uniteDetailService;

        public UniteViewModel()
        {
            _uniteService = new WSService<UniteDTO>();
            _uniteDetailService = new WSService<UniteDetailDTO>();
        }

        public List<UniteDTO> Unites { get; private set; } = new List<UniteDTO>();

        public UniteDetailDTO SelectedUniteDetails { get; private set; }

        public UniteDTO EditableUnite { get; set; } = new UniteDTO();

        public string ErrorMessage { get; private set; }

        public async Task LoadUnitesAsync()
        {
            try
            {
                Unites = await _uniteService.GetAllTAsync("Unites");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des unitées : {ex.Message}";
            }
        }

        public async Task LoadUniteDetailsAsync(int idUnite)
        {
            try
            {
                SelectedUniteDetails = await _uniteDetailService.GetTAsync("Unites", idUnite);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails de l'unitée : {ex.Message}";
            }
        }

        public async Task<UniteDetailDTO> LoadUniteDetailsWithoutDefAsync(int idUnite)
        {
            return await _uniteDetailService.GetTAsync("Unites", idUnite);
        }

        public async Task AddUniteAsync()
        {
            if (IsValidUnite(EditableUnite))
            {
                try
                {
                    await _uniteService.PostTAsync("Unites", EditableUnite);
                    await LoadUnitesAsync();
                    EditableUnite = new UniteDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout de l'unitée : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task UpdateUniteAsync()
        {
            if (IsValidUnite(EditableUnite))
            {
                try
                {
                    await _uniteService.PutTAsync($"Unites/{EditableUnite.IdUnite}", EditableUnite);
                    await LoadUnitesAsync();
                    EditableUnite = new UniteDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour de l'unitée : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task DeleteUniteAsync(int idUnite)
        {
            try
            {
                await _uniteService.DeleteTAsync("Unites", idUnite);
                await LoadUnitesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'unitée : {ex.Message}";
            }
        }

        public void EditUnite(UniteDetailDTO unite)
        {
            EditableUnite = new UniteDTO
            {
                IdUnite = unite.IdUnite,
                NomUnite = unite.NomUnite,
                SigleUnite = unite.SigleUnite,
                // Vous pouvez inclure d'autres champs si nécessaire
            };
        }

        // Vérifier si les données de la salle sont valides
        private bool IsValidUnite(UniteDTO unite)
        {
            return !string.IsNullOrWhiteSpace(unite.NomUnite) && !string.IsNullOrWhiteSpace(unite.SigleUnite);
        }
    }
}
