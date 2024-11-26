using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Equipement
    {
        public int IdEquipement { get; set; }

        [JsonPropertyName("nomEquipement")]
        public String? NomEquipement { get; set; }

        [JsonPropertyName("longueur")]
        public int? Longeur { get; set; }

        [JsonPropertyName("largeur")]
        public int? Largeur { get; set; }

        [JsonPropertyName("hauteur")]
        public int? Hauteur { get; set; }

        [JsonPropertyName("xEquipement")]
        public int? XEquipement { get; set; }

        [JsonPropertyName("yEquipement")]
        public int? YEquipement { get; set; }

        [JsonPropertyName("zEquipement")]
        public int? ZEquipement { get; set; }

        [JsonPropertyName("estActif")]
        public string? EstActif { get; set; }

        [JsonPropertyName("idSalle")]
        public int? IdSalle { get; set; }

        [JsonPropertyName("idTypeEquipement")]
        public int? IdTypeEquipement { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
