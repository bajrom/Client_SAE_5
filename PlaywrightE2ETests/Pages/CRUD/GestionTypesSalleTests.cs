using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionTypesSalleTests : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL,"/crud/typessalle");

        [TestInitialize]
        public async Task Initialize()
        {
            await Page.GotoAsync(Url);
        }

        [TestMethod]
        public async Task GestionTypesSalleTitreCorrect()
        {
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des types de salle"));
        }

        [TestMethod]
        public async Task TableOfTypesSalle_CorrectNumberColumn()
        {
            // Vérifier que la table des types de salles est visible
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des types de salles)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(4);
        }

        [TestMethod]
        public async Task GestionTypeSalle_GridContentOK()
        {
            var Pagination = Page.Locator("ul.pagination").First;
            var indicationPage = Page.Locator("div.bb-grid-pagination-text").First;
            var nbPage = Page.Locator("li.page-item.active").Locator("a");

            await Expect(nbPage).ToContainTextAsync("1");
            await Expect(Pagination).ToBeVisibleAsync();
            await Expect(nbPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToContainTextAsync("types de salle");
        }
    }
}
