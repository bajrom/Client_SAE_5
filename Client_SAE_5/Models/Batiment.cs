using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Batiment
    {
        public int IdBatiment { get; set; }

        [JsonPropertyName("nomBatiment")]
        public String? NomBatiment { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
