using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class TypeEquipementDetailDTO
    {
        private int idTypeEquipement;
        private string nomTypeEquipement;
        private ICollection<EquipementSansNavigationDTO> equipements;

        [Required]
        public int IdTypeEquipement
        {
            get
            {
                return this.idTypeEquipement;
            }

            set
            {
                this.idTypeEquipement = value;
            }
        }

        [Required]
        public string NomTypeEquipement
        {
            get
            {
                return this.nomTypeEquipement;
            }

            set
            {
                this.nomTypeEquipement = value;
            }
        }

        public ICollection<EquipementSansNavigationDTO> Equipements
        {
            get
            {
                return this.equipements;
            }

            set
            {
                this.equipements = value;
            }
        }
    }
}
