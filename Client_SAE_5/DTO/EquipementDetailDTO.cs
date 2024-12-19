using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class EquipementDetailDTO
    {
        private int idEquipement;
        private String nomEquipement;

        private decimal longueur;
        private decimal largeur;
        private decimal hauteur;

        private String estActif;
        private SalleSansNavigationDTO salle;
        private TypeEquipementDTO typeEquipement;
        private MurSansNavigationDTO mur;

        private decimal positionX;
        private decimal positionY;
        private decimal positionZ;

        [Required]
        public int IdEquipement { get => idEquipement; set => idEquipement = value; }

        [Required]
        public string NomEquipement { get => nomEquipement; set => nomEquipement = value; }

        public string EstActif { get => estActif; set => estActif = value; }

        public decimal PositionX { get => positionX; set => positionX = value; }

        public decimal PositionY { get => positionY; set => positionY = value; }

        public decimal PositionZ { get => positionZ; set => positionZ = value; }

        public decimal Longueur { get => longueur; set => longueur = value; }
        public decimal Largeur { get => largeur; set => largeur = value; }
        public decimal Hauteur { get => hauteur; set => hauteur = value; }
        public MurSansNavigationDTO Mur { get => mur; set => mur = value; }
        public SalleSansNavigationDTO Salle { get => salle; set => salle = value; }

        public TypeEquipementDTO TypeEquipement { get => typeEquipement; set => typeEquipement = value; }
    }
}
