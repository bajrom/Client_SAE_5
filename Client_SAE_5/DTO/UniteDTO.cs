using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class UniteDTO
    {
        private int idUnite;
        private String sigleUnite;
        private String nomUnite;

        [Required]
        public int IdUnite { get => idUnite; set => idUnite = value; }

        public string SigleUnite { get => sigleUnite; set => sigleUnite = value; }

        [Required]
        public string NomUnite { get => nomUnite; set => nomUnite = value; }
    }
}
