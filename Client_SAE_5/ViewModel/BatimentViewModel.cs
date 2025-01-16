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

        /// <summary>
        /// Permet de charger tous les bâtiments de façon asynchrone.
        /// </summary>
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

        /// <summary>
        /// Permet de charger les détails d'un bâtiment de façon asynchrone
        /// </summary>
        /// <param name="idBatiment">ID du bâtiment à charger</param>
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

        /// <summary>
        /// Permet d'initialiser le batiment et ses détails pour son édition
        /// </summary>
        /// <param name="idBatiment"></param>
        public async Task SetupBatimentEdition(int idBatiment)
        {
            BatimentDetailDTO temp = await _batimentDetailService.GetTAsync("Batiments", idBatiment);

            BatimentInEdition = temp;
        }

        /// <summary>
        /// Permet de reset l'édition de batiment utilisé quand appui sur annuler
        /// </summary>
        public async Task SetupNewBatiment()
        {
            BatimentInEdition = new BatimentDetailDTO();
        }

        /// <summary>
        /// Permet de rajouter un bâtiment de façon asynchrone
        /// </summary>
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
        }

        /// <summary>
        /// Permet de mettre à jour un bâtiment de façon asynchrone
        /// </summary>
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
        }

        /// <summary>
        /// Permet de supprimer un bâtiment de façon asynchrone
        /// </summary>
        /// <param name="idBatiment">ID du bâtiment à supprimer</param>
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

        /// <summary>
        /// Vérifie si les propriétés d'un bâtiment sont valides
        /// </summary>
        /// <param name="batiment">Batiment dont les propriétés sont à vérifier</param>
        /// <returns>true s'il est correct, false sinon</returns>
        private bool IsValidBatiment(BatimentSansNavigationDTO batiment)
        {
            if (string.IsNullOrWhiteSpace(batiment.NomBatiment))
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
