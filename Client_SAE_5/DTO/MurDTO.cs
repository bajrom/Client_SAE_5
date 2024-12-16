using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class MurDTO
    {
        private int idMur;
        private string direction;
        private string nomSalle;
        private decimal orientation;

        [Required]
        public int IdMur { get => idMur; set => idMur = value; }

        public string Direction { get => direction; set => direction = value; }

        public string NomSalle { get => nomSalle; set => nomSalle = value; }

        public decimal Orientation { get => orientation; set => orientation = value; }

    }
}
