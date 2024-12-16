using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class UniteCapteurSansNavigationDTO
    {
        private int idCapteur;
        private int idUnite;

        public int IdCapteur { get => idCapteur; set => idCapteur = value; }
        public int IdUnite { get => idUnite; set => idUnite = value; }
    }
}
