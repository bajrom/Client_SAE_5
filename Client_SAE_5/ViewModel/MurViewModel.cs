using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class MurViewModel
    {
        private readonly WSService<MurDTO> _murService;
        private readonly WSService<MurSansNavigationDTO> _murSansNavigationService;

        public MurViewModel()
        {
            _murService = new WSService<MurDTO>();
            _murSansNavigationService = new WSService<MurSansNavigationDTO>();
        }

        public List<MurDTO> Murs { get; private set; } = new List<MurDTO>();

        public MurSansNavigationDTO SelectedMurDetails { get; private set; }

        public MurSansNavigationDTO EditableMur { get; set; } = new MurSansNavigationDTO();

        public List<Direction> Directions { get; private set; } = new List<Direction>();

        public List<SalleDTO> Salles { get; private set; } = new List<SalleDTO>();

        public string ErrorMessage { get; private set; }

        public async Task LoadMursAsync()
        {
            try
            {
                Murs = await _murService.GetAllTAsync("Murs");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des murs : {ex.Message}";
            }
        }

        public async Task LoadMurDetailsAsync(int idMur)
        {
            try
            {
                SelectedMurDetails = await _murSansNavigationService.GetTAsync("Murs", idMur);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des détails du mur : {ex.Message}";
            }
        }

        public async Task LoadDirectionsAsync()
        {
            try
            {
                var directionService = new WSService<Direction>();
                Directions = await directionService.GetAllTAsync("Direction");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des directions : {ex.Message}";
            }
        }

        public async Task LoadSallesAsync()
        {
            try
            {
                var salleService = new WSService<SalleDTO>();
                Salles = await salleService.GetAllTAsync("Salles");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des salles : {ex.Message}";
            }
        }

        public async Task<MurSansNavigationDTO> LoadMurDetailsWithoutDefAsync(int idMur)
        {
            return await _murSansNavigationService.GetTAsync("Murs", idMur);
        }

        public async Task AddMurAsync()
        {
            if (IsValidMur(EditableMur))
            {
                try
                {
                    await _murSansNavigationService.PostTAsync("Murs", EditableMur);
                    await LoadMursAsync();
                    EditableMur = new MurSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout du mur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task UpdateMurAsync()
        {
            if (IsValidMur(EditableMur))
            {
                try
                {
                    await _murSansNavigationService.PutTAsync($"Murs/{EditableMur.IdMur}", EditableMur);
                    await LoadMursAsync();
                    EditableMur = new MurSansNavigationDTO(); // Réinitialiser le formulaire
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour du mur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez remplir tous les champs obligatoires.";
            }
        }

        public async Task DeleteMurAsync(int idMur)
        {
            try
            {
                await _murService.DeleteTAsync("Murs", idMur);
                await LoadMursAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du mur : {ex.Message}";
            }
        }


        public void EditMur(MurSansNavigationDTO mur)
        {
            EditableMur = mur;
        }

        private bool IsValidMur(MurSansNavigationDTO mur)
        {
            return mur.IdDirection > 0 &&
                   mur.IdSalle > 0 &&
                   mur.Hauteur > 0 &&
                   mur.Longueur > 0 &&
                   mur.Orientation >= 0 && mur.Orientation < 360;
        }
    }
}
