using Client_SAE_5.Models;

namespace Client_SAE_5.DTO
{
    public class SalleDetailDTO
    {
        private int idSalle;
        private string nomSalle;
        private TypeSalle typeSalle;
        private Batiment batiment;
        private List<Capteur> capteurs;
        private List<Equipement> equipements;
        private List<Mur> murs;

        public string NomSalle { get => nomSalle; set => nomSalle = value; }
        public int IdSalle { get => idSalle; set => idSalle = value; }
        public List<Mur> Murs { get => murs; set => murs = value; }
        public List<Equipement> Equipements { get => equipements; set => equipements = value; }
        public List<Capteur> Capteurs { get => capteurs; set => capteurs = value; }
        public TypeSalle TypeSalle { get => typeSalle; set => typeSalle = value; }
        public Batiment Batiment { get => batiment; set => batiment = value; }
    }
}
