using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Salle
    {
        public int IdSalle { get; set; }

        [JsonPropertyName("nomSalle")]
        public String? NomSalle { get; set; }

        [JsonPropertyName("superficieSalle")]
        public float? SuperficieSalle { get; set; }

        [JsonPropertyName("idBatiment")]
        public int? IdBatiment { get; set; }

        [JsonPropertyName("idTypeSalle")]
        public int? IdTypeSalle { get; set; }

        [JsonIgnore]
        [JsonPropertyName("isEditable")]
        public bool IsEditable { get; set; }
    }
}
