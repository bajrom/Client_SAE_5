using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionTypesEquipementTests : PageTest
    {
        private String Url = String.Concat(TestsConfig.BaseURL,"/crud/TypesEquipement");

        [TestInitialize]
        public async Task Initialize()
        {
            await Page.GotoAsync(Url);
        }

        [TestMethod]
        public async Task GestionTypesEquipementTitreCorrect()
        {
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des types d'équipement"));
        }

        [TestMethod]
        public async Task TableOfTypesEquipement_CorrectNumberColumn()
        {
            // Vérifier que la table des types de Equipements est visible
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des types d'équipements)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(4);
        }

        [TestMethod]
        public async Task GestionTypesEquipements_GridContentOK()
        {
            var Pagination = Page.Locator("ul.pagination").First;
            var indicationPage = Page.Locator("div.bb-grid-pagination-text").First;
            var nbPage = Page.Locator("li.page-item.active").Locator("a");

            await Expect(nbPage).ToContainTextAsync("1");
            await Expect(Pagination).ToBeVisibleAsync();
            await Expect(nbPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToContainTextAsync("types d'équipement");
        }
    }
}
