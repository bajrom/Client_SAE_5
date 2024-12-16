using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class DirectionDetailDTO
    {
        private ICollection<MurSansNavigationDTO> murs;
        private short idDirection;
        private string lettresDirection;

        [Required]
        public short IdDirection { get => idDirection; set => idDirection = value; }
        
        [Required]
        public string LettresDirection { get => lettresDirection; set => lettresDirection = value; }

        public ICollection<MurSansNavigationDTO> Murs { get => murs; set => murs = value; }
    }
}
