using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class TypeSalle
    {
        public int IdTypeSalle { get; set; }

        [JsonPropertyName("nomtypesalle")]
        public string NomTypeSalle { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
