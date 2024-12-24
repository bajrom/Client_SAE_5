using Client_SAE_5.DTO;
using Client_SAE_5.Models;
using Client_SAE_5.Pages.CRUD.Mur;

namespace Client_SAE_5.Utils
{
    /// <summary>
    /// Classe utilitaire contenant les méthodes nécéssaires afin de générer le plan d'une salle
    /// </summary>
    public class SVG_Generator
    {
        /// <summary>
        /// Permet de générer le plan d'une salle à partir d'une liste de MursSansNavigations
        /// </summary>
        /// <param name="murs">Liste de MurSansNavigation présents dans la Salle</param>
        /// <exception cref="ArgumentException">Si la liste des murs est vide</exception>
        /// <returns>Image SVG représentant le plan de la salle</returns>
        public static string GenererPlanSalleSVG(List<MurSansNavigationDTO>? murs)
        {
            if (murs == null || murs.Count == 0)
            {
                throw new ArgumentException("La liste des murs doit contenir au moins 1 mur!");
            }

            decimal echelle = 0.5M;
            decimal padding = 100;

            // Calcul des points échelonnés
            List<(decimal X, decimal Y)> points = calculPointsEchelonnes(murs, echelle);
 
            // Recalcul des limites après échelle
            decimal minX = points.Min(p => p.X);
            decimal maxX = points.Max(p => p.X);
            decimal minY = points.Min(p => p.Y) + 50;
            decimal maxY = points.Max(p => p.Y);

            // Calcul des dimensions SVG
            decimal width = maxX - minX + padding * 2;
            decimal height = maxY - minY + padding * 2;

            // Calcul des offsets
            decimal offsetX = -minX + padding;
            decimal offsetY = -minY + padding;

            string svg = $@"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 {Utils.FormatNumber(width)} {Utils.FormatNumber(height)}'>
                    <style>
                        .wall {{ stroke: #333; stroke-width: 2; fill: none; cursor: pointer; z-index:50; transition: 1s ease all; }}
                        .wall:hover,.angle-text:hover {{ stroke: #A50000; transition:1s ease all; }}
                        .angle-text {{ font-size: 12px; fill: #666; cursor: pointer; z-index: 100; transition: 1s ease all; }}
                        .image {{ position: absolute; right: 0px; top: 0px; }}
                    </style>";

            decimal currentX = offsetX;
            decimal currentY = offsetY;

            // Dessin des murs
            foreach (var mur in murs)
            {
                decimal orientationRadians = mur.Orientation * (decimal)Math.PI / 180;
                decimal endX = currentX + mur.Longueur * (decimal)Math.Cos((double)orientationRadians) * echelle;
                decimal endY = currentY + mur.Longueur * (decimal)Math.Sin((double)orientationRadians) * echelle;

                svg += CreateWall(mur, currentX, currentY, endX, endY, echelle);

                currentX = endX;
                currentY = endY;
            }

            svg += $@"<image x={Utils.FormatNumber(width - 50)} y={Utils.FormatNumber(0)} width=""50px"" height=""50px"" href={"/images/boussole.png"} class='image'></image>";

            svg += "</svg>";
            return svg;
        }

        /// <summary>
        /// Retourne une liste de points correspondant aux points de chaque mur
        /// </summary>
        /// <param name="murs">Liste de MursSansNavigation dans la salle</param>
        /// <param name="echelle">Echelle voulue (défaut=1/2)</param>
        /// <returns>Une liste de points</returns>
        private static List<(decimal X, decimal Y)> calculPointsEchelonnes(List<MurSansNavigationDTO> murs, decimal echelle = 0.5m)
        {
            decimal currentX = 0;
            decimal currentY = 0;

            List < (decimal X, decimal Y) > points = [];

            // Calcul des points échelonnés
            foreach (var mur in murs)
            {
                decimal orientationRadians = mur.Orientation * (decimal)Math.PI / 180;
                currentX += mur.Longueur * (decimal)Math.Cos((double)orientationRadians) * echelle;
                currentY += mur.Longueur * (decimal)Math.Sin((double)orientationRadians) * echelle;
                points.Add((currentX, currentY));
            }

            return points;
        }

        /// <summary>
        /// Permet de créer un mur dans le générateur de salles en SVG
        /// </summary>
        /// <param name="mur">MurSansNavigation dont on veut dessiner</param>
        /// <param name="currentX">X actuel</param>
        /// <param name="currentY">Y actuel</param>
        /// <param name="endX">X final</param>
        /// <param name="endY">Y final</param>
        /// <param name="echelle">Echelle du dessin</param>
        /// <returns>Balise SVG correspondant au mur dessiné avec un texte représentant la longueur du mur en mètres.</returns>
        private static String? CreateWall(MurSansNavigationDTO mur, decimal currentX, decimal currentY, decimal endX, decimal endY, decimal echelle = 0.5m)
        {
            String? svg = "";

            svg += CreateWallLine(currentX, currentY, endX, endY, mur.IdMur);

            // Milieux des murs pour placer le texte (mesure)
            decimal midX = (currentX + endX) / 2;
            decimal midY = (currentY + endY) / 2;

            svg += CreateText(midX, midY, mur.Longueur, mur.IdMur);

            return svg;
        }

        /// <summary>
        /// Permet de dessiner une ligne représentant le mur
        /// </summary>
        /// <param name="currentX">X actuel</param>
        /// <param name="currentY">Y actuel</param>
        /// <param name="endX">X final</param>
        /// <param name="endY">Y final</param>
        /// <param name="idMur">Id du mur (pour pouvoir cliquer dessus)</param>
        /// <returns>Balise SVG représentant la ligne du mur</returns>
        private static String? CreateWallLine(decimal currentX, decimal currentY, decimal endX, decimal endY, int idMur)
        {
            return $@"<line class='wall'
                    onclick='location.href=""/crud/murs/{idMur}""'
                    x1='{Utils.FormatNumber(currentX)}'
                    y1='{Utils.FormatNumber(currentY)}'
                    x2='{Utils.FormatNumber(endX)}'
                    y2='{Utils.FormatNumber(endY)}' />";
        }

        /// <summary>
        /// Permet de dessiner la longueur du mur en mètres
        /// </summary>
        /// <param name="midX">Position X du texte</param>
        /// <param name="midY">Position Y du texte</param>
        /// <param name="longueurMur">Longueur du mur</param>
        /// <param name="idMur">ID du mur (pour pouvoir cliquer dessus)</param>
        /// <returns>Balise SVG représentant la longueur du mur</returns>
        private static String? CreateText(decimal midX, decimal midY, decimal longueurMur, int idMur)
        {
            return $@"<rect 
                    x='{Utils.FormatNumber(midX - 22)}' 
                    y='{Utils.FormatNumber(midY - 23)}' 
                    width='42' 
                    height='23' 
                    fill='lightgray' 
                    stroke='none' />

                <text class='angle-text'
                    onclick='location.href=""/crud/murs/{idMur}""'
                
                    x='{Utils.FormatNumber(midX)}'
                    y='{Utils.FormatNumber(midY)}'
                    text-anchor='middle'
                    dy='-5'>
                    {Utils.FormatNumber(longueurMur / 100)}m
                    <title>Cliquez pour visualiser ce mur</title>
                </text>
                ";
        }
    }
}
