namespace Client_SAE_5.DTO
{
    public class BatimentDTO
    {
        private int idBatiment;
        private string nomBatiment;
        private int nbSalle;

        public int IdBatiment { get => idBatiment; set => idBatiment = value; }
        public string NomBatiment { get => nomBatiment; set => nomBatiment = value; }
        public int NbSalle { get => nbSalle; set => nbSalle = value; }
    }
}
