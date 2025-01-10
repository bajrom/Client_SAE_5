using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD.Detail
{
    [TestClass]
    public class GeneralDetailTest : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/");

        private static readonly List<string> PagesStr = new List<string>
        {
            "bâtiments",
            "capteurs",
            "équipements",
            "murs",
            "salles",
            "typesEquipement",
            "typesSalle",
            "unites"
        };

        private static readonly List<string> PagesGestion = new List<string>
        {
            "Gestion des bâtiments",
            "Gestion des capteurs",
            "Gestion des équipements",
            "Gestion des murs",
            "Gestion des salles",
            "Gestion des types d'équipement",
            "Gestion des types de salle",
            "Gestion des unités"
        };

        private static readonly List<string> PagesTitle = new List<string>
        {
            "Détails du bâtiment",
            "Détails du capteur",
            "Détails de l'équipement",
            "Détails du mur",
            "Détails de la salle",
            "Détails du type d'équipement",
            "Détails du type de salle",
            "Détails de l'unité"
        };

        [TestMethod]
        public async Task GoDetail_ContentVisible()
        {
            int index = 0;
            foreach (var page in PagesStr)
            {
                Console.WriteLine("Page "+ page);
                await Page.GotoAsync($"{Url}{page.Replace("â","a").Replace("é","e")}/1");
                Console.WriteLine($"{Url}{page.Replace("â", "a").Replace("é", "e")}/1");
                var pageTitle = Page;
                var pageHeader = Page.Locator("h1");
                var boutonRetour = Page.Locator("button:has-text('Retour')");

                // Titre H1 visible ?
                await Expect(pageHeader).ToBeVisibleAsync();

                // Tester le titre de la fenêtre
                var pageTitleName = PagesTitle[index];
                Assert.AreEqual((await pageTitle.TitleAsync()).ToUpperInvariant(), pageTitleName.ToUpperInvariant(), $"Le titre de la fenêtre de {pageTitleName} est incohérent.Espéré: {pageTitleName}, Réel: {await pageTitle.TitleAsync()}.");

                // Header == Titre page et visible
                Assert.AreEqual(await pageTitle.TitleAsync(), await pageHeader.InnerTextAsync(), $"Le titre h1 de la page de détail {await pageHeader.InnerTextAsync()} est incohérent. Espéré: {await pageTitle.TitleAsync()}, Réel: {await pageHeader.InnerTextAsync()}");
                await Expect(pageHeader).ToBeVisibleAsync();

                // Bouton Retour
                await Expect(boutonRetour).ToBeVisibleAsync();
                await boutonRetour.ClickAsync();

                var pageTitre = $"{(await Page.TitleAsync()).ToUpperInvariant()}";
                var ExpectedTitre = PagesGestion[index].ToUpperInvariant();
                Assert.IsTrue(pageTitre == ExpectedTitre, $"Le bouton de Retour n'a pas amené à la bonne page. Espéré: {ExpectedTitre}. Réel: {pageTitre}");

                index++;
            }
        }
    }
}
