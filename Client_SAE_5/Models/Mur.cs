using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Mur
    {
        public int IdMur { get; set; }

        [JsonPropertyName("longueur")]
        public float? Longueur { get; set; }

        [JsonPropertyName("hauteur")]
        public float? Hauteur { get; set; }

        [JsonPropertyName("orientation")]
        public float? Orientation { get; set; }

        [JsonPropertyName("idDirection")]
        public int? IdDirection { get; set; }

        [JsonPropertyName("idSalle")]
        public int? IdSalle { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
