using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class UniteCapteurDetailDTO
    {
        private UniteDTO unite;
        private CapteurDTO capteur;

        [Required]
        public UniteDTO Unite { get => unite; set => unite = value; }

        [Required]
        public CapteurDTO Capteur { get => capteur; set => capteur = value; }
    }
}
