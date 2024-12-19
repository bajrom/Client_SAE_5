using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class CapteurSansNavigationDTO
    {
        [Required]
        public int IdCapteur { get; set; }

        public int? IdMur { get; set; }

        [Required]
        public string NomCapteur { get; set; }

        public string EstActif { get; set; } = "NSP";

        public decimal XCapteur { get; set; }

        public decimal YCapteur { get; set; }

        public decimal ZCapteur { get; set; }
    }
}
