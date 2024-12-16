using System.Text.Json.Serialization;

namespace Client_SAE_5.Models
{
    public class Direction
    {
        public int IdDirection { get; set; }

        [JsonPropertyName("lettresdirection")]
        public string LettresDirection { get; set; }
    }
}
