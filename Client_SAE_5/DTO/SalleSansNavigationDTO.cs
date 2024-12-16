using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class SalleSansNavigationDTO
    {
        public int IdSalle { get; set; }

        public int IdBatiment { get; set; }

        public int IdTypeSalle { get; set; }

        public string NomSalle { get; set; }
    }
}
