using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class SalleSansNavigationDTO
    {
        [Required]
        public int IdSalle { get; set; }

        public int IdBatiment { get; set; }

        public int IdTypeSalle { get; set; }

        [Required]
        public string NomSalle { get; set; }
    }
}
