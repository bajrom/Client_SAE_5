using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class CapteurSansNavigationDTO
    {
        public int IdCapteur { get; set; }

        public int? IdMur { get; set; }

        public string NomCapteur { get; set; }

        public string EstActif { get; set; }

        public decimal XCapteur { get; set; }

        public decimal YCapteur { get; set; }

        public decimal ZCapteur { get; set; }
    }
}
