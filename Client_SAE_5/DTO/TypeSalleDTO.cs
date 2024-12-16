using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class TypeSalleDTO
    {
        private int idTypeSalle;
        private string nomTypeSalle;

        [Required]
        public int IdTypeSalle { get => idTypeSalle; set => idTypeSalle = value; }

        [Required]
        public string NomTypeSalle { get => nomTypeSalle; set => nomTypeSalle = value; }

        public override bool Equals(object? obj)
        {
            return obj is TypeSalleDTO dTO &&
                   this.IdTypeSalle == dTO.IdTypeSalle &&
                   this.NomTypeSalle == dTO.NomTypeSalle;
        }
    }
}
