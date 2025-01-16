using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;
using Client_SAE_5.Pages;
using Client_SAE_5.Pages.CRUD.Mur;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5.ViewModel
{
    public class CapteurViewModel
    {
        private readonly WSService<CapteurDTO> _capteurService;
        private readonly WSService<CapteurDetailDTO> _capteurDetailService;
        private readonly WSService<CapteurSansNavigationDTO> _capteurSansNavigationService;
        private readonly WSService<UniteDTO> _uniteService;
        private readonly WSService<UniteCapteurSansNavigationDTO> _unitecapteurService;
        private readonly WSService<MurDTO> _murService;
        private readonly WSService<MurDetailDTO> _murDetailService;
        public DataStorage DBData;

        public CapteurViewModel(DataStorage data)
        {
            _capteurService = new WSService<CapteurDTO>();
            _capteurDetailService = new WSService<CapteurDetailDTO>();
            _capteurSansNavigationService = new WSService<CapteurSansNavigationDTO>();
            _uniteService = new WSService<UniteDTO>();
            _unitecapteurService = new WSService<UniteCapteurSansNavigationDTO>();
            _murService = new WSService<MurDTO>();
            _murDetailService = new WSService<MurDetailDTO>();
            this.DBData = data;
        }

        public CapteurDetailDTO SelectedCapteurDetail { get; private set; }

        public CapteurDetailDTO CapteurInEdition { get; private set; }

        public string CapteurInEditionNomSalleSelected { get; set; } = "";

        public List<UniteDTO> CapteurInEditionOldUnites { get; private set; }

        public int CapteurInEditionOldMurId;

        public List<string> NomSalles { get; private set; } = new List<string>();

        public List<UniteDTO> AvailableUnites { get; private set; } = new List<UniteDTO>();
        private MurDetailDTO murSelected { get; set; } = new MurDetailDTO();

        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Permet de charger tous les capteurs de façon asynchrone.
        /// </summary>
        public async Task LoadCapteursAsync()
        {
            try
            {
                DBData.Capteurs = await _capteurService.GetAllTAsync("Capteurs");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des salles : {ex.Message}";
            }
        }

        /// <summary>
        /// permet de charger toutes les unitées pour faire les clé étrangères
        /// </summary>
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

        /// <summary>
        /// Permet de charger les détails d'un capteur de façon asynchrone
        /// </summary>
        /// <param name="idCapteur">ID du capteur à charger</param>
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

        /// <summary>
        /// Permet d'obtenir la liste unique de tous les nom des salles à partir des mur
        /// </summary>
        public void LoadNomSallesAsync()
        {
            NomSalles = DBData.Murs
            .Where(mur => !string.IsNullOrEmpty(mur.NomSalle)) // Filtre les noms non nulls
            .Select(mur => mur.NomSalle)
            .Distinct() // Supprime les doublons
            .ToList();
        }

        /// <summary>
        /// permet de charger tous les murs pour faire les clé étrangères
        /// </summary>
        public async Task LoadMursAsync()
        {
            try
            {
                DBData.Murs = await _murService.GetAllTAsync("Murs");
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des murs : {ex.Message}";
            }
        }

        /// <summary>
        /// Permet d'initialiser le capteur et ses détails pour son édition
        /// </summary>
        /// <param name="idCapteur"></param>
        public async Task SetupCapteurEdition(int idCapteur)
        {
            CapteurDetailDTO temp = await _capteurDetailService.GetTAsync("Capteurs", idCapteur);

            murSelected = new MurDetailDTO
            {
                IdMur = temp.Mur.IdMur,
                IdSalle = temp.Mur.IdSalle,
                IdDirection = temp.Mur.IdDirection,
                Longueur = temp.Mur.Longueur,
                Hauteur = temp.Mur.Hauteur,
                Orientation = temp.Mur.Orientation,
            };

            if (DBData.Unites == null || DBData.Unites.Count == 0)
            {
                await LoadUnitesAsync();
            }
            
            if (DBData.Murs == null || DBData.Murs.Count == 0)
            {
                await LoadMursAsync();
            }

            LoadNomSallesAsync();

            AvailableUnites = DBData.Unites.Where(unite => temp.Unites.All(u => u.IdUnite != unite.IdUnite)).ToList();
            CapteurInEdition = temp;
            CapteurInEditionOldUnites = new List<UniteDTO>(CapteurInEdition.Unites); // on garde la liste des unites au début de la modif pour pouvoir la comparer à celle lors de la confirmation de modif pour faire les changements correspondant dans les UniteCapteur
            CapteurInEditionNomSalleSelected = CapteurInEdition.Salle.NomSalle;
            CapteurInEditionOldMurId = CapteurInEdition.Mur.IdMur;
        }

        /// <summary>
        /// Permet de setup les unitées lié au capteur édité correspondant à la table de liaison UniteCapteur
        /// </summary>
        /// <param name="uniteClicked"></param>
        /// <param name="checkActivated"></param>
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

        /// <summary>
        /// Permet de reset l'édition de capteur utilisé quand appui sur annuler
        /// </summary>
        public async Task SetupNewCapteur()
        {
            if (DBData.Unites == null || DBData.Unites.Count == 0)
            {
                await LoadUnitesAsync();
            }

            if (DBData.Murs == null || DBData.Murs.Count == 0)
            {
                await LoadMursAsync();
            }

            if (NomSalles == null || NomSalles.Count == 0)
            {
                LoadNomSallesAsync();
            }

            if (DBData.Murs == null || DBData.Murs.Count == 0)
            {
                await LoadMursAsync();
            }

            LoadNomSallesAsync();
            CapteurInEdition = new CapteurDetailDTO();
        }

        /// <summary>
        /// Permet de rajouter un capteur de façon asynchrone
        /// </summary>
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
                    newCapteur = await _capteurSansNavigationService.PostTAsync("Capteurs", newCapteur);
                    foreach(UniteDTO unite in CapteurInEdition.Unites)
                    {
                        await AddUniteCapteurAsync(newCapteur.IdCapteur, unite.IdUnite);
                    }
                    //essayer de pas avoir à reload
                    await LoadCapteursAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout du capteur : {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Permet de mettre à jour un capteur de façon asynchrone
        /// </summary>
        public async Task UpdateCapteurAsync()
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
                    await _capteurSansNavigationService.PutTAsync($"Capteurs/{newCapteur.IdCapteur}", newCapteur);

                    //Ajout des liens avec les unites qui n'étaient pas déjà liées auparavant
                    foreach(UniteDTO unite in CapteurInEdition.Unites)
                    {
                        if (!CapteurInEditionOldUnites.Contains(unite)) // si c'est une nouvelle unite
                        {
                            await AddUniteCapteurAsync(newCapteur.IdCapteur, unite.IdUnite);
                        }
                    }

                    //supression des liens avec les unites qui étaient liées avant mais ne le sont plus
                    foreach(UniteDTO ancienneUnite in CapteurInEditionOldUnites)
                    {
                        if (!CapteurInEdition.Unites.Contains(ancienneUnite)) // si elle n'est plus liée
                        {
                            await DeleteUniteCapteurAsync(newCapteur.IdCapteur, ancienneUnite.IdUnite);
                        }
                    }

                    await LoadCapteursAsync();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de la mise à jour du capteur : {ex.Message}";
                }
            }
        }

        /// <summary>
        /// Permet d'inséré une unité dans la table de liaison uniteCapteur
        /// </summary>
        /// <param name="idCapteur"></param>
        /// <param name="idUnite"></param>
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
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Erreur lors de l'ajout de l'unitée capteur : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage = "Veuillez sélectionner une unitée.";
            }
        }

        /// <summary>
        /// Permet de supprimer un capteur de façon asynchrone
        /// </summary>
        /// <param name="idCapteur">ID du capteur à supprimer</param>
        public async Task DeleteCapteurAsync(int idCapteur)
        {
            try
            {
                await _capteurService.DeleteTAsync("Capteurs", idCapteur);
                //DBData.Capteurs.Remove(DBData.Capteurs.Single(c => c.IdCapteur == idCapteur));
                await LoadCapteursAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression du capteur : {ex.Message}";
            }
        }

        /// <summary>
        /// Permet de supprimer une uniteCapteur de la table de liaison uniteCapteur correspondant au capteur
        /// </summary>
        /// <param name="idCapteur"></param>
        /// <param name="idUnite"></param>
        public async Task DeleteUniteCapteurAsync(int idCapteur, int idUnite)
        {
            try
            {
                await _unitecapteurService.DeleteDoubleTAsync("UniteCapteur", idCapteur, idUnite);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors de la suppression de l'unitée capteur : {ex.Message}";
            }

            if(idCapteur == SelectedCapteurDetail.IdCapteur) // pour mettre à jour si jamais la suppression se fait depuis la page de détail
            {
                SelectedCapteurDetail.Unites.RemoveAll(unite => unite.IdUnite == idUnite);
            }
        }

        /// <summary>
        /// Vérifie si les propriétés d'un capteur sont valides
        /// </summary>
        /// <param name="capteur">Capteur dont les propriétés sont à vérifier</param>
        /// <returns>true s'il est correct, false sinon</returns>
        private bool IsValidCapteur(CapteurSansNavigationDTO capteur)
        {
            if (string.IsNullOrWhiteSpace(capteur.NomCapteur))
            {
                ErrorMessage = "Pas de nom";
                return false;
            }
            else if (capteur.IdMur <= 0)
            {
                ErrorMessage = "Pas de mur sélectionné";
                return false;
            }
            else if (capteur.XCapteur < 0 || capteur.XCapteur > murSelected.Longueur)
            {
                ErrorMessage = $"X en dehors des limites (Longueur du mur: {murSelected.Longueur} cm)";
                return false;
            }
            else if (capteur.YCapteur < 0 || capteur.YCapteur > murSelected.Hauteur)
            {
                ErrorMessage = $"Y en dehors des limites (Hauteur du mur: {murSelected.Hauteur} cm)";
                return false;
            }
            else return true;
        }

        /// <summary>
        /// Vérifie si les propriétés d'une uniteCapteur sont valides
        /// </summary>
        /// <param name="uniteCapteur">UniteCapteur dont les propriétés sont à vérifier</param>
        /// <returns>true s'il est correct, false sinon</returns>
        private bool IsValidUniteCapteur(UniteCapteurSansNavigationDTO uniteCapteur)
        {
            return uniteCapteur.IdUnite > 0 && uniteCapteur.IdCapteur > 0;
        }

        /// <summary>
        /// Permet de réinitialiser le message d'erreur
        /// </summary>
        public void ResetError()
        {
            ErrorMessage = "";
        }

        /// <summary>
        /// Méthode déclencher quand le mur est changer dans la popup de add/update, il permet de get les détails du mur général sélectionné
        /// (pour vérifier si le capteur est en x et y dans le mur et pas dehors)
        /// </summary>
        /// <param name="id"></param>
        public async Task ChangeMur(int id)
        {
            murSelected = await _murDetailService.GetTAsync("Murs", id);
            CapteurInEdition.Mur.IdMur = id;
        }
    }
}
