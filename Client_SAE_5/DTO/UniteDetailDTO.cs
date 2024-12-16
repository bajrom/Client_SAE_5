using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class UniteDetailDTO
    {
        private int idUnite;
        private String sigleUnite;
        private String nomUnite;
        private List<Capteur> capteurs;

        public int IdUnite { get => idUnite; set => idUnite = value; }
        public string SigleUnite { get => sigleUnite; set => sigleUnite = value; }
        public string NomUnite { get => nomUnite; set => nomUnite = value; }
        public List<Capteur> Capteurs { get => capteurs; set => capteurs = value; }
    }
}
