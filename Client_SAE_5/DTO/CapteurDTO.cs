using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class CapteurDTO
    {
        private int idCapteur;
        private string nomCapteur;
        private string nomSalle;

        [Required]
        public int IdCapteur { get => idCapteur; set => idCapteur = value; }

        [Required]
        public string NomCapteur { get => nomCapteur; set => nomCapteur = value; }

        public string NomSalle { get => nomSalle; set => nomSalle = value; }

    }
}
