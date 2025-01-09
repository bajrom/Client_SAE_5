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

        [TestMethod]
        public async Task GestionUniteTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des unités"));
        }

        [TestMethod]
        public async Task TableOfUnite_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des unités est visible
            var table = Page.Locator("table.table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des unités)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(3);
        }
    }
}
