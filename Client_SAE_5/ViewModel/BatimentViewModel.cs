using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5.ViewModel
{
    public class BatimentViewModel
    {
        private readonly WSService<BatimentDTO> _batimentService;
        private readonly WSService<BatimentDetailDTO> _batimentDetailService;
        private readonly WSService<BatimentSansNavigationDTO> _batimentSansNavigationService;

        public BatimentViewModel()
        {
            _batimentService = new WSService<BatimentDTO>();
            _batimentDetailService = new WSService<BatimentDetailDTO>();
            _batimentSansNavigationService = new WSService<BatimentSansNavigationDTO>();
        }

        public List<BatimentDTO> Batiments { get; private set; } = new List<BatimentDTO>();

        public BatimentDetailDTO SelectedBatimentDetails { get; private set; }

        public BatimentSansNavigationDTO EditableBatiment { get; set; } = new BatimentSansNavigationDTO();

        public string ErrorMessage { get; private set; }

        public async Task LoadBatimentsAsync()
        {
            try
            {
                Batiments = await _batimentService.GetAllTAsync("Batiments");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des batiments : {ex.Message}";
            }
        }

        public async Task LoadBatimentDetailsAsync(int idBatiment)
        {
            try
            {
                SelectedBatimentDetails = await _batimentDetailService.GetTAsync("Batiments", idBatiment);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails du batiment : {ex.Message}";
            }
        }
        public async Task<BatimentDetailDTO> LoadBatimentDetailsWithoutDefAsync(int idBatiment)
        {
            return await _batimentDetailService.GetTAsync("Batiments", idBatiment);
        }

        public async Task AddBatimentAsync()
        {
            if (IsValidBatiment(EditableBatiment))
            {
                try
                {
                    await _batimentSansNavigationService.PostTAsync("Batiments", EditableBatiment);
                    await LoadBatimentsAsync();
                    EditableBatiment = new BatimentSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout du batiment : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task UpdatebatimentAsync()
        {
            if (IsValidBatiment(EditableBatiment))
            {
                try
                {
                    await _batimentSansNavigationService.PutTAsync($"Batiments/{EditableBatiment.IdBatiment}", EditableBatiment);
                    await LoadBatimentsAsync();
                    EditableBatiment = new BatimentSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour du batiment : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task DeletebatimentAsync(int idBatiment)
        {
            try
            {
                await _batimentService.DeleteTAsync("Batiments", idBatiment);
                await LoadBatimentsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du batiment : {ex.Message}";
            }
        }


        public void EditBatiment(BatimentDetailDTO batiment)
        {
            EditableBatiment = new BatimentSansNavigationDTO
            {
                IdBatiment = batiment.IdBatiment,
                NomBatiment = batiment.NomBatiment,
            };
        }

        private bool IsValidBatiment(BatimentSansNavigationDTO batiment)
        {
            return !string.IsNullOrWhiteSpace(batiment.NomBatiment);
        }
    }
}
