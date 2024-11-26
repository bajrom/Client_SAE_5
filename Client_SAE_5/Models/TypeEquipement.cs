using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class TypeEquipement
    {
        public int IdTypeEquipement { get; set; }

        [JsonPropertyName("nomTypeEquipement")]
        public String? NomTypeEquipement { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
