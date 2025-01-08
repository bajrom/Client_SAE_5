using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages;

namespace Client_SAE_5.ViewModel
{
    public class CapteurViewModel
    {
        private readonly WSService<CapteurDTO> _capteurService;
        private readonly WSService<CapteurDetailDTO> _capteurDetailService;
        private readonly WSService<CapteurSansNavigationDTO> _capteurSansNavigationService;
        private readonly WSService<UniteDTO> _uniteService;
        private readonly WSService<UniteCapteurSansNavigationDTO> _unitecapteurService;

        public CapteurViewModel()
        {
            _capteurService = new WSService<CapteurDTO>();
            _capteurDetailService = new WSService<CapteurDetailDTO>();
            _capteurSansNavigationService = new WSService<CapteurSansNavigationDTO>();
            _uniteService = new WSService<UniteDTO>();
            _unitecapteurService = new WSService<UniteCapteurSansNavigationDTO>();
        }

        public List<CapteurDTO> Capteurs { get; private set; } = new List<CapteurDTO>();

        public CapteurDetailDTO SelectedCapteurDetail { get; private set; }

        public CapteurDetailDTO CapteurInEdition { get; private set; }

        public List<MurDTO> Murs { get; private set; } = new List<MurDTO>();

        public List<UniteDTO> Unites { get; private set; } = new List<UniteDTO>();

        public List<UniteDTO> AvailableUnites { get; private set; } = new List<UniteDTO>();

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

        public async Task LoadCapteurDetailsAsync(int idCapteur)
        {
            try
            {
                SelectedCapteurDetail = await _capteurDetailService.GetTAsync("Capteurs", idCapteur);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails du capteur : {ex.Message}";
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

        public async Task LoadCapteurDetailsWithoutDefAsync(int idCapteur)
        {
            CapteurDetailDTO temp = await _capteurDetailService.GetTAsync("Capteurs", idCapteur);
            if (Unites.Count == 0)
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

            AvailableUnites = Unites.Where(unite => temp.Unites.All(u => u.IdUnite != unite.IdUnite)).ToList();
            CapteurInEdition = temp;
        }

        public void ChangeSelectedUnites(UniteDTO uniteClicked, bool checkActivated)
        {
            if (checkActivated)
            {
                CapteurInEdition.Unites.Add(uniteClicked);
            }
            else
            {
                UniteDTO uniteToRemove = CapteurInEdition.Unites.FirstOrDefault(u => u.IdUnite == uniteClicked.IdUnite);
                CapteurInEdition.Unites.Remove(uniteToRemove);
            }
        }

        public async Task SetupNewCapteur()
        {
            if (Unites.Count == 0)
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

            CapteurInEdition = new CapteurDetailDTO();
        }

        public async Task AddCapteurAsync()
        {
            CapteurSansNavigationDTO newCapteur = new CapteurSansNavigationDTO
            {
                IdCapteur = CapteurInEdition.IdCapteur,
                NomCapteur = CapteurInEdition.NomCapteur,
                EstActif = CapteurInEdition.EstActif,
                XCapteur = CapteurInEdition.XCapteur,
                YCapteur = CapteurInEdition.YCapteur,
                ZCapteur = CapteurInEdition.ZCapteur,
                IdMur = CapteurInEdition.Mur.IdMur,
            };

            if (IsValidCapteur(newCapteur))
            {
                try
                {
                    await _capteurSansNavigationService.PostTAsync("Capteurs", newCapteur);
                    //essayer de pas avoir à reload
                    await LoadCapteursAsync();
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
            CapteurSansNavigationDTO newCapteur = new CapteurSansNavigationDTO
            {
                IdCapteur = SelectedCapteurDetail.IdCapteur,
                NomCapteur = SelectedCapteurDetail.NomCapteur,
                EstActif = SelectedCapteurDetail.EstActif,
                XCapteur = SelectedCapteurDetail.XCapteur,
                YCapteur = SelectedCapteurDetail.YCapteur,
                ZCapteur = SelectedCapteurDetail.ZCapteur,
                IdMur = SelectedCapteurDetail.Mur.IdMur,
            };

            if (IsValidCapteur(newCapteur))
            {
                try
                {
                    await _capteurSansNavigationService.PutTAsync($"Capteurs/{newCapteur.IdCapteur}", newCapteur);
                    await LoadCapteursAsync();
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


        public async Task AddUniteCapteurAsync(int idCapteur, int idUnite)
        {
            UniteCapteurSansNavigationDTO uniteCapteur = new UniteCapteurSansNavigationDTO()
            {
                IdCapteur = idCapteur,
                IdUnite = idUnite
            };

            if (IsValidUniteCapteur(uniteCapteur))
            {
                try
                {
                    await _unitecapteurService.PostTAsync("UniteCapteur", uniteCapteur);
                    AvailableUnites = AvailableUnites.Where(unite => unite.IdUnite != idUnite).ToList();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout de l'unitée capteur : {ex.Message}";
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
                Capteurs.Remove(Capteurs.Single(c => c.IdCapteur == idCapteur));
                await LoadCapteursAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du capteur : {ex.Message}";
            }
        }

        public async Task DeleteUniteCapteurAsync(int idUnite, int idCapteur)
        {
            try
            {
                await _unitecapteurService.DeleteDoubleTAsync("UniteCapteur", idCapteur, idUnite);
                await LoadCapteurDetailsAsync(idCapteur);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'unitée capteur : {ex.Message}";
            }
        }

        private bool IsValidCapteur(CapteurSansNavigationDTO capteur)
        {
            return !string.IsNullOrWhiteSpace(capteur.NomCapteur) &&
                   capteur.IdMur > 0;
        }

        private bool IsValidUniteCapteur(UniteCapteurSansNavigationDTO uniteCapteur)
        {
            return uniteCapteur.IdUnite > 0 && uniteCapteur.IdCapteur > 0;
        }

        public void ResetError()
        {
            ErrorMessage = "";
        }
    }
}
