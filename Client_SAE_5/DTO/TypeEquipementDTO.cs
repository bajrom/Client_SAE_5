using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class TypeEquipementDTO
    {
        private int idTypeEquipement;
        private string nomTypeEquipement;

        [Required]
        public int IdTypeEquipement { get => idTypeEquipement; set => idTypeEquipement = value; }

        [Required]
        public string NomTypeEquipement { get => nomTypeEquipement; set => nomTypeEquipement = value; }
    }
}
