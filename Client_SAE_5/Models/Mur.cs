using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Mur
    {
        public int IdMur { get; set; }

        [JsonPropertyName("longueur")]
        public decimal Longueur { get; set; }

        [JsonPropertyName("hauteur")]
        public decimal Hauteur { get; set; }

        [JsonPropertyName("orientation")]
        public decimal Orientation { get; set; }

        [JsonPropertyName("iddirection")]
        public int IdDirection { get; set; }

        [JsonPropertyName("idsalle")]
        public int IdSalle { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
