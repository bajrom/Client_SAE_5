using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class UniteCapteur
    {
        [JsonPropertyName("idunite")]
        public int IdUnite { get; set; }

        [JsonPropertyName("idcapteur")]
        public int IdCapteur { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
