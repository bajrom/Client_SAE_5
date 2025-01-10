using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages.CRUD.Capteur;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5.ViewModel
{
    public class BatimentViewModel
    {
        private readonly WSService<BatimentDTO> _batimentService;
        private readonly WSService<BatimentDetailDTO> _batimentDetailService;
        private readonly WSService<BatimentSansNavigationDTO> _batimentSansNavigationService;
        public BatimentDetailDTO BatimentInEdition { get; private set; }
        public DataStorage DBData;

        public BatimentViewModel(DataStorage dBData)
        {
            _batimentService = new WSService<BatimentDTO>();
            _batimentDetailService = new WSService<BatimentDetailDTO>();
            _batimentSansNavigationService = new WSService<BatimentSansNavigationDTO>();
            DBData = dBData;
        }

        public List<BatimentDTO> Batiments { get; private set; } = new List<BatimentDTO>();

        public BatimentDetailDTO SelectedBatimentDetails { get; private set; }

        public string ErrorMessage { get; private set; }

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

        public async Task SetupBatimentEdition(int idBatiment)
        {
            BatimentDetailDTO temp = await _batimentDetailService.GetTAsync("Batiments", idBatiment);

            BatimentInEdition = temp;
        }

        public async Task SetupNewBatiment()
        {
            BatimentInEdition = new BatimentDetailDTO();
        }

        public async Task AddBatimentAsync()
        {
            BatimentSansNavigationDTO newCapteur = new BatimentSansNavigationDTO
            {
                IdBatiment = BatimentInEdition.IdBatiment,
                NomBatiment = BatimentInEdition.NomBatiment,
            };

            if (IsValidBatiment(newCapteur))
            {
                try
                {
                    newCapteur = await _batimentSansNavigationService.PostTAsync("Batiments", newCapteur);
                    await LoadBatimentsAsync();
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
            BatimentSansNavigationDTO newCapteur = new BatimentSansNavigationDTO
            {
                IdBatiment = BatimentInEdition.IdBatiment,
                NomBatiment = BatimentInEdition.NomBatiment,
            };

            if (IsValidBatiment(newCapteur))
            {
                try
                {
                    await _batimentSansNavigationService.PutTAsync($"Batiments/{newCapteur.IdBatiment}", newCapteur);
                    await LoadBatimentsAsync();
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
                DBData.Batiments.Remove(DBData.Batiments.Single(c => c.IdBatiment == idBatiment));
                await LoadBatimentsAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du batiment : {ex.Message}";
            }
        }

        private bool IsValidBatiment(BatimentSansNavigationDTO batiment)
        {
            return !string.IsNullOrWhiteSpace(batiment.NomBatiment);
        }
        public void ResetError()
        {
            ErrorMessage = "";
        }
    }
}
