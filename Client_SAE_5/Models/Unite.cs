using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Unite
    {
        public int IdUnite { get; set; }

        [JsonPropertyName("nomunite")]
        public string NomUnite { get; set; }

        [JsonPropertyName("sigleunite")]
        public string SigleUnite { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
