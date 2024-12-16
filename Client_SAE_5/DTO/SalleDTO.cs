using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class SalleDTO
    {
        private int idSalle;
        private string nomSalle;
        private string nomBatiment;
        private string nomType;

        [Required]
        public int IdSalle { get => idSalle; set => idSalle = value; }

        [Required]
        public string NomSalle { get => nomSalle; set => nomSalle = value; }

        public string NomBatiment { get => nomBatiment; set => nomBatiment = value; }

        public string NomType { get => nomType; set => nomType = value; }
    }
}
