using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD.Detail
{
    [TestClass]
    public class DetailCapteurTests: PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/capteurs/");

        [TestInitialize]
        public async Task Initialize()
        {
            await Page.GotoAsync(Url + "2");
        }

        [TestMethod]
        public async Task DetailCapteur_CorrectTitles()
        {
            var titreInfoGenerales = Page.GetByText("Informations générales");
            var titreLocalisation = Page.GetByText("Localisation");
            var titreUnites = Page.GetByText("Unités").Nth(1);
            var titrePositionCapteurMur = Page.GetByText("Position du capteur sur le mur");
            var nom = Page.GetByText("Nom :");
            var etat = Page.GetByText("Etat:");

            await Expect(titreInfoGenerales).ToBeVisibleAsync();
            await Expect(titreLocalisation).ToBeVisibleAsync();
            await Expect(titreUnites).ToBeVisibleAsync();
            await Expect(titrePositionCapteurMur).ToBeVisibleAsync();
            await Expect(nom).ToBeVisibleAsync();
            await Expect(etat).ToBeVisibleAsync();
        }

        [TestMethod]
        public async Task DetailCapteur_CorrectLinks()
        {
            // Tester les liens de la page
            var uniteLink = Page.Locator("p.listeCliquable").First;
            var murLinkLeft = Page.Locator("h5.murInfo").Nth(1);
            var salleLinkLeft = Page.Locator("h5.murInfo").Nth(0);
            var schemaMur = Page.Locator("div#cadreMur");

            // Tester chaque lien
            if (uniteLink != null) { 
                await uniteLink.ClickAsync();
                await Expect(Page).ToHaveTitleAsync("Détails de l'unité");
                await Page.GotoAsync(Url + "2");
            }

            await murLinkLeft.ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Détails du mur");
            await Page.GotoAsync(Url + "2");
            await salleLinkLeft.ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Détails de la salle");
            await Page.GotoAsync(Url + "2");
            await schemaMur.ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Détails du mur");
            await Page.GotoAsync(Url + "2");
        }

        [TestMethod]
        public async Task HoverInformationIcon_ShouldDisplayMessage()
        {
            await Page.Locator(".bi.bi-info-circle-fill").HoverAsync();
            await Expect(Page.Locator("div.tooltip")).ToBeVisibleAsync();
            await Page.Locator("h3").First.HoverAsync();
            await Expect(Page.Locator("div.tooltip")).ToBeHiddenAsync();
        }
    }
}
