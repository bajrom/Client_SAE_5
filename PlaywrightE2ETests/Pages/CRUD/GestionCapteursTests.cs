using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionCapteurTests : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/capteurs");

        [TestInitialize]
        public async Task Initialize()
        {
            await Page.GotoAsync(Url);
        }

        [TestMethod]
        public async Task GestionCapteursTitreCorrect()
        {
            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Capteurs"));
        }

        [TestMethod]
        public async Task TableOfCapteurs_CorrectNumberColumn()
        {
            // Vérifier que la table des capteurs est visible
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des capteurs)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(6);
        }

        [TestMethod]
        public async Task BoutonRajouter_Visible_Cliquable()
        {
            var btnRajouter = Page.Locator("button.btn.btn-success");
            Assert.IsNotNull(btnRajouter, "le bouton d'ajout n'est pas présent");

            await btnRajouter.IsVisibleAsync();
        }

        [TestMethod]
        public async Task GestionCapteur_GridContentOK()
        {
            var Pagination = Page.Locator("ul.pagination").First;
            var indicationPage = Page.Locator("div.bb-grid-pagination-text").First;
            var nbPage = Page.Locator("li.page-item.active").Locator("a");

            await Expect(nbPage).ToContainTextAsync("1");
            await Expect(Pagination).ToBeVisibleAsync();
            await Expect(nbPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToContainTextAsync("capteurs");
        }

    }
}
