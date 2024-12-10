using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Direction
    {
        public int IdDirection { get; set; }

        [JsonPropertyName("lettredirection")]
        public string LettreDirection { get; set; }
    }
}
