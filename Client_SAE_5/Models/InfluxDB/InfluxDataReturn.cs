namespace Client_SAE_5.Models.InfluxDB
{
    public class InfluxDataReturn
    {
        public DateTime time;
        public double value;

        public InfluxDataReturn(DateTime time, double value)
        {
            this.time = time;
            this.value = value;
        }
    }
}
