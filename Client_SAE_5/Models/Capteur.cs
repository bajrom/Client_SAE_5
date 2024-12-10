using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Capteur
    {

        public int IdCapteur { get; set; }

        [JsonPropertyName("nomcapteur")]
        public string NomCapteur { get; set; }

        [JsonPropertyName("estactif")]
        public string EstActif { get; set; }

        [JsonPropertyName("xcapteur")]
        public decimal XCapteur { get; set; }

        [JsonPropertyName("ycapteur")]
        public decimal YCapteur { get; set; }

        [JsonPropertyName("zcatpeur")]
        public decimal ZCapteur { get; set; }

        [JsonPropertyName("idmur")]
        public int? IdMur { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
