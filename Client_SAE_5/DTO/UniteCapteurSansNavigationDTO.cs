using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class UniteCapteurSansNavigationDTO
    {
        private int idCapteur;
        private int idUnite;

        [Required]
        public int IdCapteur { get => idCapteur; set => idCapteur = value; }

        [Required]
        public int IdUnite { get => idUnite; set => idUnite = value; }
    }
}
