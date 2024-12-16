using System.ComponentModel.DataAnnotations;
using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class TypeSalleDTO
    {
        private int idTypeSalle;
        private string nomTypeSalle;

        [Required]
        public int IdTypeSalle { get => idTypeSalle; set => idTypeSalle = value; }

        [Required]
        [MaxLength(20, ErrorMessage = "Le nom ne doit pas dépasser 20 caractères.")]
        public string NomTypeSalle { get => nomTypeSalle; set => nomTypeSalle = value; }

        public override bool Equals(object? obj)
        {
            return obj is TypeSalleDTO dTO &&
                   this.IdTypeSalle == dTO.IdTypeSalle &&
                   this.NomTypeSalle == dTO.NomTypeSalle;
        }
    }
}
