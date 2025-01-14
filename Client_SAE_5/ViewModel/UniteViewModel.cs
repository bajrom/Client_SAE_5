using Client_SAE_5.DTO;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Utils.Singleton;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Client_SAE_5.ViewModel
{
    public class UniteViewModel
    {
        private readonly WSService<UniteDTO> _uniteService;
        private readonly WSService<UniteDetailDTO> _uniteDetailService;
        public DataStorage DBData;

        public UniteViewModel(DataStorage data)
        {
            _uniteService = new WSService<UniteDTO>();
            _uniteDetailService = new WSService<UniteDetailDTO>();
            this.DBData = data;
        }

        public UniteDetailDTO SelectedUniteDetails { get; private set; }

        public UniteDetailDTO UniteInEdition { get; set; } = new UniteDetailDTO();

        public string ErrorMessage { get; private set; }

        public async Task LoadUnitesAsync()
        {
            try
            {
                DBData.Unites = await _uniteService.GetAllTAsync("Unites");
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

        public async Task SetupUniteEdition(int idUnite)
        {
            UniteDetailDTO temp = await _uniteDetailService.GetTAsync("Unites", idUnite);

            if (DBData.Unites.Count == 0)
            {
                await LoadUnitesAsync();
            }

            UniteInEdition = temp;
        }

        public async Task SetupNewUnite()
        {
            UniteInEdition = new UniteDetailDTO();
        }

        public async Task AddUniteAsync()
        {
            UniteDTO newUnite = new UniteDTO
            {
                IdUnite = UniteInEdition.IdUnite,
                NomUnite = UniteInEdition.NomUnite,
                SigleUnite = UniteInEdition.SigleUnite,
            };

            if (IsValidUnite(newUnite))
            {
                try
                {
                    await _uniteService.PostTAsync("Unites", newUnite);
                    await LoadUnitesAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout de l'unitée : {ex.Message}";
                }
            }
        }

        public async Task UpdateUniteAsync()
        {
            UniteDTO newUnite = new UniteDTO
            {
                IdUnite = UniteInEdition.IdUnite,
                NomUnite = UniteInEdition.NomUnite,
                SigleUnite = UniteInEdition.SigleUnite,
            };

            if (IsValidUnite(newUnite))
            {
                try
                {
                    await _uniteService.PutTAsync($"Unites/{newUnite.IdUnite}", newUnite);
                    await LoadUnitesAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour de l'unitée : {ex.Message}";
                }
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

        // Vérifier si les données de la salle sont valides
        private bool IsValidUnite(UniteDTO unite)
        {
            if (string.IsNullOrWhiteSpace(unite.NomUnite))
            {
                ErrorMessage = "Veuillez remplir le nom";
                return false;
            }
            else if (string.IsNullOrWhiteSpace(unite.SigleUnite))
            {
                ErrorMessage = "Veuillez remplir le sigle";
                return false;
            }
            return true;
        }

        public void ResetError()
        {
            ErrorMessage = "";
        }
    }
}
