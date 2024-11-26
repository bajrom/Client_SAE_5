using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Unite
    {
        public int IdUnite { get; set; }

        [JsonPropertyName("nomUnite")]
        public String? NomUnite { get; set; }

        [JsonPropertyName("sigleUnite")]
        public String? SigleUnite { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
