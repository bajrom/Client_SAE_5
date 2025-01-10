namespace Client_SAE_5.Models
{
    public class InfluxData
    {
        public string? CapteurId { get; set; }
        public double? Value { get; set; }
        public DateTime Timestamp { get; set; }
        // Ajoutez d'autres propriétés selon la structure de vos données InfluxDB
    }
}