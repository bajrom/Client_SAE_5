using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class EquipementDTO
    {
        private int idEquipement;
        private string nomEquipement;
        private string dimensions; // Dimensions: LxlxH
        private string nomSalleEquipement;
        private string nomTypeEquipement;

        public int IdEquipement { get => idEquipement; set => idEquipement = value; }
        public string NomEquipement { get => nomEquipement; set => nomEquipement = value; }
        public string NomSalleEquipement { get => nomSalleEquipement; set => nomSalleEquipement = value; }
        public string NomTypeEquipement { get => nomTypeEquipement; set => nomTypeEquipement = value; }
        public string Dimensions { get => dimensions; set => dimensions = value; }
    }
}
