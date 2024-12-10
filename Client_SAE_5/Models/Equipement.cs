using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Equipement
    {
        public int IdEquipement { get; set; }

        [JsonPropertyName("nomequipement")]
        public string NomEquipement { get; set; }

        [JsonPropertyName("longueur")]
        public decimal Longeur { get; set; }

        [JsonPropertyName("largeur")]
        public decimal Largeur { get; set; }

        [JsonPropertyName("hauteur")]
        public decimal Hauteur { get; set; }

        [JsonPropertyName("xequipement")]
        public decimal XEquipement { get; set; }

        [JsonPropertyName("yequipement")]
        public decimal YEquipement { get; set; }

        [JsonPropertyName("zequipement")]
        public decimal ZEquipement { get; set; }

        [JsonPropertyName("estactif")]
        public string EstActif { get; set; }

        [JsonPropertyName("idmur")]
        public int IdMur { get; set; }

        [JsonPropertyName("idtypeequipement")]
        public int IdTypeEquipement { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
