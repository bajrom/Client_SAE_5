using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class EquipementSansNavigationDTO
    {
        public int IdEquipement { get; set; }

        public int IdMur { get; set; }

        public int IdTypeEquipement { get; set; }

        public string NomEquipement { get; set; }

        public decimal Longueur { get; set; }

        public decimal Largeur { get; set; }

        public decimal Hauteur { get; set; }

        public decimal XEquipement { get; set; }

        public decimal YEquipement { get; set; }

        public decimal ZEquipement { get; set; }

        public string EstActif { get; set; }
    }
}
