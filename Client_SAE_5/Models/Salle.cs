using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Salle
    {
        public int IdSalle { get; set; }

        [JsonPropertyName("nomsalle")]
        public string NomSalle { get; set; }

        [JsonPropertyName("idbatiment")]
        public int IdBatiment { get; set; }

        [JsonPropertyName("idtypesalle")]
        public int IdTypeSalle { get; set; }

        [JsonIgnore]
        [JsonPropertyName("iseditable")]
        public bool IsEditable { get; set; }
    }
}
