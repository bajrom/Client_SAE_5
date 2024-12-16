using System.Text.Json.Serialization;

namespace Client_SAE_5.DTO
{
    public class Batiment
    {
        public int IdBatiment { get; set; }

        [JsonPropertyName("nombatiment")]
        public string NomBatiment { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
