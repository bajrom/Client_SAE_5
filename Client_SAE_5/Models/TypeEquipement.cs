using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class TypeEquipement
    {
        public int IdTypeEquipement { get; set; }

        [JsonPropertyName("nomtypeequipement")]
        public string NomTypeEquipement { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
