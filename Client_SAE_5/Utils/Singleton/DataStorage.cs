using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using System.Collections.Generic;

namespace Client_SAE_5.Utils.Singleton
{
    public class DataStorage
    {
        public List<BatimentDTO> Batiments { get; set; } = new List<BatimentDTO>();
        public List<SalleDTO> Salles { get; set; } = new List<SalleDTO>();
        public List<CapteurDTO> Capteurs { get; set; } = new List<CapteurDTO>();
        public List<EquipementDTO> Equipements { get; set; } = new List<EquipementDTO>();
        public List<DirectionSansNavigationDTO> Directions { get; set; } = new List<DirectionSansNavigationDTO>();
        public List<MurDTO> Murs { get; set; } = new List<MurDTO>();
        public List<TypeEquipementDTO> TypesEquipement { get; set; } = new List<TypeEquipementDTO>();
        public List<TypeSalleDTO> TypesSalle { get; set; } = new List<TypeSalleDTO>();
        public List<UniteDTO> Unites { get; set; } = new List<UniteDTO>();
    }
}
