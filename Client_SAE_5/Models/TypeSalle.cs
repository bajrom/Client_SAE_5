using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class TypeSalle
    {
        public int IdTypeSalle { get; set; }

        [JsonPropertyName("NomTypeSalle")]
        public String? NomTypeSalle { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
