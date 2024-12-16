using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class SalleDetailDTO
    {
        private int idSalle;
        private string nomSalle;
        private TypeSalleDTO typeSalle;
        private BatimentSansNavigationDTO batiment;
        private List<CapteurSansNavigationDTO> capteurs;
        private List<EquipementSansNavigationDTO> equipements;
        private List<MurSansNavigationDTO> murs;

        [Required]
        public int IdSalle { get => idSalle; set => idSalle = value; }

        [Required]
        public string NomSalle { get => nomSalle; set => nomSalle = value; }
        
        public TypeSalleDTO TypeSalle { get => typeSalle; set => typeSalle = value; }
        
        public BatimentSansNavigationDTO Batiment { get => batiment; set => batiment = value; }
        
        public List<CapteurSansNavigationDTO> Capteurs { get => capteurs; set => capteurs = value; }
        
        public List<EquipementSansNavigationDTO> Equipements { get => equipements; set => equipements = value; }
        
        public List<MurSansNavigationDTO> Murs { get => murs; set => murs = value; }
    }
}
