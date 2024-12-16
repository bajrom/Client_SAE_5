using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class CapteurDetailDTO
    {
        private int idCapteur;
        private string nomCapteur;
        private string nomSalle;
        private string estActif;
        private decimal xCapteur;
        private decimal yCapteur;
        private decimal zCapteur;
        private List<Unite> unites;
        private Mur mur;
        private Salle salle;

        public int IdCapteur { get => idCapteur; set => idCapteur = value; }
        public string NomCapteur { get => nomCapteur; set => nomCapteur = value; }
        public string EstActif { get => estActif; set => estActif = value; }
        public decimal XCapteur { get => xCapteur; set => xCapteur = value; }
        public decimal YCapteur { get => yCapteur; set => yCapteur = value; }
        public decimal ZCapteur { get => zCapteur; set => zCapteur = value; }
        public List<Unite> Unites { get => unites; set => unites = value; }
        public Mur Mur { get => mur; set => mur = value; }
        public Salle Salle { get => salle; set => salle = value; }
    }
}
