using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class TypeEquipementDTO
    {
        private int idTypeEquipement;
        private string nomTypeEquipement;

        public int IdTypeEquipement { get => idTypeEquipement; set => idTypeEquipement = value; }
        public string NomTypeEquipement { get => nomTypeEquipement; set => nomTypeEquipement = value; }
    }
}
