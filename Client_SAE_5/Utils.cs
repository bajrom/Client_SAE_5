using Client_SAE_5.DTO;
using Client_SAE_5.ViewModel;

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

        public static string GenererPlanSalleSVG(List<MurSansNavigationDTO> murs)
        {

            decimal echelle = 0.5M;
            decimal padding = 100;

            List<(decimal X, decimal Y)> points = new() { (0, 0) };
            decimal currentX = 0, currentY = -100;

            foreach (var mur in murs)
            {
                decimal orientationRadians = mur.Orientation * (decimal)Math.PI / 180;
                currentX += mur.Longueur * (decimal)Math.Cos((double)orientationRadians) * echelle;
                currentY += mur.Longueur * (decimal)Math.Sin((double)orientationRadians) * echelle;
                points.Add((currentX, currentY));
            }

            decimal minX = points.Min(p => p.X);
            decimal maxX = points.Max(p => p.X);
            decimal minY = points.Min(p => p.Y);
            decimal maxY = points.Max(p => p.Y);

            decimal width = maxX - minX + (padding * 2);
            decimal height = maxY - minY + (padding * 2);

            decimal offsetX = -minX + padding;
            decimal offsetY = -minY + padding;

            // Utilisez FormatNumber pour garantir l'utilisation du point comme séparateur décimal
            string FormatNumber(decimal number) => number.ToString(System.Globalization.CultureInfo.InvariantCulture);

            string svg = $@"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 {FormatNumber(width)} {FormatNumber(height)}'>
        <style>
            .wall {{ stroke: #333; stroke-width: 2; fill: none; }}
            .angle-text {{ font-size: 12px; fill: #666; }}
        </style>";

            currentX = offsetX;
            currentY = offsetY;

            foreach (var mur in murs)
            {
                decimal orientationRadians = mur.Orientation * (decimal)Math.PI / 180;
                decimal endX = currentX + (mur.Longueur * (decimal)Math.Cos((double)orientationRadians) * echelle);
                decimal endY = currentY + (mur.Longueur * (decimal)Math.Sin((double)orientationRadians) * echelle);

                svg += $@"<line class='wall'
            x1='{FormatNumber(currentX)}'
            y1='{FormatNumber(currentY)}'
            x2='{FormatNumber(endX)}'
            y2='{FormatNumber(endY)}' />";

                decimal midX = (currentX + endX) / 2;
                decimal midY = (currentY + endY) / 2;
                svg += $@"<text class='angle-text'
            x='{FormatNumber(midX)}'
            y='{FormatNumber(midY)}'
            text-anchor='middle'
            dy='-5'>
        {FormatNumber(mur.Orientation)}°
        </text>";

                currentX = endX;
                currentY = endY;
            }

            svg += "</svg>";
            return svg;
        }
    }
}
