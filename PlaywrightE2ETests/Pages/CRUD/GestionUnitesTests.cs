using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionUniteTests : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/unites");

        [TestInitialize]
        public async Task Initialize()
        {
            await Page.GotoAsync(Url);
        }

        [TestMethod]
        public async Task GestionUniteTitreCorrect()
        {
            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des unités"));
        }

        [TestMethod]
        public async Task TableOfUnite_CorrectNumberColumn()
        {
            // Vérifier que la table des unités est visible
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des unités)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(6);
        }

        [TestMethod]
        public async Task GestionUnite_GridContentOK()
        {
            var Pagination = Page.Locator("ul.pagination").First;
            var indicationPage = Page.Locator("div.bb-grid-pagination-text").First;
            var nbPage = Page.Locator("li.page-item.active").Locator("a");

            await Expect(nbPage).ToContainTextAsync("1");
            await Expect(Pagination).ToBeVisibleAsync();
            await Expect(nbPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToContainTextAsync("unités");
        }
    }
}
