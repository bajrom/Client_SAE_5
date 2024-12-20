namespace Client_SAE_5
{
    public class Utils
    {
        /// <summary>
        /// Permet d'adapter les états de la BD à un état à afficher (OUI, NON, NSP)
        /// </summary>
        /// <param name="Etat">OUI, NON ou NSP</param>
        /// <returns>Description de l'état</returns>
        public static String RenderEtatContent(String Etat)
        {
            if (string.IsNullOrEmpty(Etat) || (Etat != "OUI" && Etat != "NON" && Etat != "NSP"))
            {
                throw new ArgumentException("L'état doit être non null, égal à Oui, Non ou Nsp");
            }

            switch (Etat.ToUpper())
            {
                case "OUI":
                    return "Activé/ouvert";
                case "NON":
                    return "Désactivé/fermé";
                default:
                    return "Inconnu";
            }
        }
    }
}
