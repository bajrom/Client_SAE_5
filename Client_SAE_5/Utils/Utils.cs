using BlazorBootstrap;
using Client_SAE_5.DTO;
using Client_SAE_5.Pages.CRUD.Equipement;
using Client_SAE_5.ViewModel;
using Microsoft.AspNetCore.Components;
using System.Reflection;

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

        /// <summary>
        /// Permet d'envoyer l'utilisateur vers une page de détails
        /// </summary>
        /// <param name="NavigationManager">Le NavigationManager qui permet de gérer la navigation de l'utilisateur</param>
        /// <param name="pageDetail">Le type d'objet dont on veut la navigation (doit finir par un s)</param>
        /// <param name="idPageDetail">ID de l'objet qu'on veut récupérer</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void GoDetail(NavigationManager NavigationManager, String pageDetail, int idPageDetail)
        {
            if (idPageDetail <= 0 || !EstObjetValide(pageDetail))
            {
                throw new ArgumentException($"L'ID: {idPageDetail} doit être supérieur ou égal à 1! La page: {pageDetail}  doit exister");
            } else if (NavigationManager == null || String.IsNullOrEmpty(pageDetail))
            {
                throw new ArgumentNullException("Le NavigationManager ne doit pas être null! La page de détail ne peut pas être null ou vide");
            }

            NavigationManager.NavigateTo(@$"/crud/{pageDetail}/{idPageDetail}");
        }

        /// <summary>
        /// Vérifie la validité d'une page
        /// </summary>
        /// <param name="page">Page à vérifier l'authenticité</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Vrai si page authentique, faux sinon</returns>
        public static bool EstObjetValide(String? page)
        {
            if (String.IsNullOrEmpty(page) || string.IsNullOrWhiteSpace(page))
            {
                throw new ArgumentNullException("Le nom de la page est incorrect");
            }

            List<String?> objetsValidesPluriel = new List<string?>{
                "salles", "capteurs", "murs", "typessalle", "batiments", "equipements", "typesequipement", "unites"
            };

            foreach (String? item in objetsValidesPluriel)
            {
                if (item == page)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
