using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class BatimentDetailDTO
    {
        private ICollection<SalleSansNavigationDTO> salles;
        private int idBatiment;
        private string nomBatiment;

        [Required]
        public int IdBatiment { get => idBatiment; set => idBatiment = value; }

        [Required]
        public string NomBatiment { get => nomBatiment; set => nomBatiment = value; }

        public ICollection<SalleSansNavigationDTO> Salles { get => salles; set => salles = value; }
    }
}
