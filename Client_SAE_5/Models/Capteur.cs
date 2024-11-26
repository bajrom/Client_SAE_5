using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Capteur
    {

        public int IdCapteur { get; set; }

        [JsonPropertyName("nomTypeCapteur")]
        public String? NomTypeCapteur { get; set; }

        [JsonPropertyName("estActif")]
        public String? EstActif { get; set; }

        [JsonPropertyName("xCapteur")]
        public float? XCapteur { get; set; }

        [JsonPropertyName("yCapteur")]
        public float? YCapteur { get; set; }

        [JsonPropertyName("zCatpeur")]
        public float? ZCapteur { get; set; }

        [JsonPropertyName("idSalle")]
        public int? IdSalle { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
