using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class TypeSalleDetailDTO
    {
        private ICollection<SalleSansNavigationDTO> salles = [];
        private int idTypeSalle;
        private string nomTypeSalle;

        [Required]
        public int IdTypeSalle { get => idTypeSalle; set => idTypeSalle = value; }

        [Required]
        public string NomTypeSalle { get => nomTypeSalle; set => nomTypeSalle = value; }

        public ICollection<SalleSansNavigationDTO> Salles { get => salles; set => salles = value; }
    }
}
