using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class UniteCapteur
    {
        [JsonPropertyName("idUnite")]
        public int IdUnite { get; set; }

        [JsonPropertyName("idCapteur")]
        public int IdCapteur { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
