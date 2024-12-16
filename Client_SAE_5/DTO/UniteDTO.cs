using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class UniteDTO
    {
        private int idUnite;
        private String sigleUnite;
        private String nomUnite;

        public int IdUnite { get => idUnite; set => idUnite = value; }
        public string SigleUnite { get => sigleUnite; set => sigleUnite = value; }
        public string NomUnite { get => nomUnite; set => nomUnite = value; }
    }
}
