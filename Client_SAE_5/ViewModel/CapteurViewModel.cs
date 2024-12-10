using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class CapteurViewModel
    {
        private readonly WSService<CapteurDTO> _capteurService;
        private readonly WSService<CapteurDetailDTO> _capteurDetailService;
        private readonly WSService<CapteurSansNavigationDTO> _capteurSansNavigationService;

        public CapteurViewModel()
        {
            _capteurService = new WSService<CapteurDTO>();
            _capteurDetailService = new WSService<CapteurDetailDTO>();
            _capteurSansNavigationService = new WSService<CapteurSansNavigationDTO>();
        }

        public List<CapteurDTO> Capteurs { get; private set; } = new List<CapteurDTO>();

        public CapteurDetailDTO SelectedCatpeurDetails { get; private set; }

        public CapteurSansNavigationDTO EditableCapteur { get; set; } = new CapteurSansNavigationDTO();

        public List<MurDTO> Murs { get; private set; } = new List<MurDTO>();

        public string ErrorMessage { get; private set; }

        public async Task LoadCapteursAsync()
        {
            try
            {
                Capteurs = await _capteurService.GetAllTAsync("Capteurs");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des salles : {ex.Message}";
            }
        }

        public async Task LoadMursAsync()
        {
            try
            {
                var murService = new WSService<MurDTO>();
                Murs = await murService.GetAllTAsync("Murs");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des murs : {ex.Message}";
            }
        }

        public async Task LoadCapteurDetailsAsync(int idCapteur)
        {
            try
            {
                SelectedCatpeurDetails = await _capteurDetailService.GetTAsync("Capteurs", idCapteur);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails du capteur : {ex.Message}";
            }
        }

        public async Task<CapteurDetailDTO> LoadCapteurDetailsWithoutDefAsync(int idCapteur)
        {
            return await _capteurDetailService.GetTAsync("Capteurs", idCapteur);
        }

        public async Task AddCapteurAsync()
        {
            if (IsValidCapteur(EditableCapteur))
            {
                try
                {
                    await _capteurSansNavigationService.PostTAsync("Capteurs", EditableCapteur);
                    await LoadCapteursAsync();
                    EditableCapteur = new CapteurSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout du capteur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task UpdateCapteurAsync()
        {
            if (IsValidCapteur(EditableCapteur))
            {
                try
                {
                    await _capteurSansNavigationService.PutTAsync($"Capteurs/{EditableCapteur.IdCapteur}", EditableCapteur);
                    await LoadCapteursAsync();
                    EditableCapteur = new CapteurSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour du capteur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task DeleteCapteurAsync(int idCapteur)
        {
            try
            {
                await _capteurService.DeleteTAsync("Capteurs", idCapteur);
                await LoadCapteursAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du capteur : {ex.Message}";
            }
        }


        public void EditCapteur(CapteurDetailDTO capteur)
        {
            EditableCapteur = new CapteurSansNavigationDTO
            {
                IdCapteur = capteur.IdCapteur,
                NomCapteur = capteur.NomCapteur,
                EstActif = capteur.EstActif,
                XCapteur = capteur.XCapteur,
                YCapteur = capteur.YCapteur,
                ZCapteur = capteur.ZCapteur,
                IdMur = capteur.Mur.IdMur,
            };
        }

        private bool IsValidCapteur(CapteurSansNavigationDTO capteur)
        {
            return !string.IsNullOrWhiteSpace(capteur.NomCapteur) &&
                   capteur.IdMur > 0;
        }
    }
}
