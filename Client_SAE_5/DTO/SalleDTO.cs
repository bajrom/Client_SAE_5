using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class SalleDTO
    {
        private int idSalle;
        private string nomSalle;
        private string nomBatiment;
        private string nomType;

        public string NomSalle { get => nomSalle; set => nomSalle = value; }
        public string NomBatiment { get => nomBatiment; set => nomBatiment = value; }
        public string NomType { get => nomType; set => nomType = value; }
        public int IdSalle { get => idSalle; set => idSalle = value; }
    }
}
