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

        [TestMethod]
        public async Task DetailCapteur_CorrectTitles()
        {
            await Page.GotoAsync(Url + "2");
            var titreInfoGenerales = Page.GetByText("Informations générales");
            var titreLocalisation = Page.GetByText("Localisation");
            var titreUnites = Page.GetByText("Unités");
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
            await Page.GotoAsync(Url + "2");
            
            // Tester les liens de la page
        }
    }
}
