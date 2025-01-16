using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionMursTests : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL,"/crud/Murs");

        [TestInitialize]
        public async Task Initialize()
        {
            await Page.GotoAsync(Url);
        }

        [TestMethod]
        public async Task GestionMurTitreCorrect()
        {
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Murs"));
        }

        [TestMethod]
        public async Task TableOfMurs_CorrectNumberColumn()
        {
            // Vérifier que la table des murs est visible
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des murs)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(8);
        }

        [TestMethod]
        public async Task GestionMurs_GridContentOK()
        {
            var Pagination = Page.Locator("ul.pagination").First;
            var indicationPage = Page.Locator("div.bb-grid-pagination-text").First;
            var nbPage = Page.Locator("li.page-item.active").Locator("a");

            await Expect(nbPage).ToContainTextAsync("1");
            await Expect(Pagination).ToBeVisibleAsync();
            await Expect(nbPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToContainTextAsync("murs");
        }
    }
}
