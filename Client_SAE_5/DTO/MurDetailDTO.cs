﻿using System.ComponentModel.DataAnnotations;

namespace Client_SAE_5.DTO
{
    public class MurDetailDTO
    {
        private SalleSansNavigationDTO? salleNavigation;
        private DirectionSansNavigationDTO? directionNavigation;
        private ICollection<CapteurSansNavigationDTO> capteurs = [];
        private ICollection<EquipementSansNavigationDTO> equipements = [];

        [Required]
        public int IdMur { get; set; }

        public short IdDirection { get; set; }

        public int IdSalle { get; set; }

        public decimal Longueur { get; set; }

        public decimal Hauteur { get; set; }

        public decimal Orientation { get; set; }

        public SalleSansNavigationDTO? SalleNavigation { get => salleNavigation; set => salleNavigation = value; }
        public DirectionSansNavigationDTO? DirectionNavigation { get => directionNavigation; set => directionNavigation = value; }
        public virtual ICollection<CapteurSansNavigationDTO> Capteurs { get => capteurs; set => capteurs = value; }
        public virtual ICollection<EquipementSansNavigationDTO> Equipements { get => equipements; set => equipements = value; }

        public MurDetailDTO()
        {
            IdMur = 0;
            IdDirection = 0;
            IdSalle = 0;
            Longueur = 0;
            Hauteur = 0;
            Orientation = 0;
        }
    }
}
