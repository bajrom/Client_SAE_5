using Client_SAE_5.DTO;
using Client_SAE_5.Pages.CRUD.Equipement;
using Client_SAE_5.ViewModel;

namespace Client_SAE_5.Utils
{
    public class Utils
    {
        /// <summary>
        /// Permet d'adapter les états de la BD à un état à afficher (OUI, NON, NSP)
        /// </summary>
        /// <param name="Etat">OUI, NON ou NSP</param>
        /// <exception cref="ArgumentException">Etat incorrect</exception>
        /// <returns>Description de l'état</returns>
        public static string RenderEtatContent(string Etat)
        {
            if (string.IsNullOrEmpty(Etat) || Etat != "OUI" && Etat != "NON" && Etat != "NSP")
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

        /// <summary>
        /// Formate le nombre décimal en chaîne de caractère
        /// </summary>
        /// <param name="number">Nombre décimal</param>
        /// <returns>Chaîne de caractère représentant le nombre avec virgule dépendant de la culture (, pour france, . pour US)</returns>
        public static string FormatNumber(decimal number) => number.ToString(System.Globalization.CultureInfo.InvariantCulture);
}
}
