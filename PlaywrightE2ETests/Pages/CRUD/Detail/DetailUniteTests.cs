using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD.Detail
{
    [TestClass]
    public class DetailUniteTests: PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/unites/");

        [TestInitialize]
        public async Task Initialize()
        {
            await Page.GotoAsync(Url + "1");
        }

        [TestMethod]
        public async Task UniteDetail_contentOK()
        {
            var champNom = Page.GetByText("Nom:").First;
            var xCapteurAssocieTexte = Page.GetByText("capteur").Nth(2);

            await Expect(champNom).ToBeVisibleAsync();
            await Expect(xCapteurAssocieTexte).ToBeVisibleAsync();
            int nbLiCapteur = await Page.Locator("li").CountAsync();
            Assert.IsTrue(xCapteurAssocieTexte.TextContentAsync().Result.Contains(nbLiCapteur.ToString()), $"Le nombre de capteurs ({nbLiCapteur}) ne correspond pas au nombre de capteurs dans la liste.");

        }

        [TestMethod]
        public async Task UniteDetail_linksOK()
        {
            var listeCapteur = Page.Locator("ul");
            var liCapteur = Page.Locator("li");

            // Vérifier la navigation pour chaque unité
            for (int i=0; i<liCapteur.CountAsync().Result-1; i++)
            {
                var elementCliquable = listeCapteur.Nth(i).Locator("p");
                String? nomUnite = await elementCliquable.TextContentAsync();
                await elementCliquable.ClickAsync();
                await Expect(Page).ToHaveTitleAsync("Détails du Capteur");
                var UnitePageCapteur = Page.GetByText(nomUnite);
                await Expect(UnitePageCapteur).ToBeVisibleAsync();
                await Page.GotoAsync(Url + "1");
            }
            
        }
    }
}
