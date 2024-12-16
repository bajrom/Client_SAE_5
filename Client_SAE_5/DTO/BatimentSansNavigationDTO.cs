using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Client_SAE_5.DTO
{
    public class BatimentSansNavigationDTO
    {
        private int idBatiment;
        private string nomBatiment;

        public int IdBatiment { get => idBatiment; set => idBatiment = value; }

        [Required]
        public string NomBatiment { get => nomBatiment; set => nomBatiment = value; }
    }
}
