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

        [TestMethod]
        public async Task GestionMurTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Murs"));
        }

        [TestMethod]
        public async Task TableOfMurs_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des murs est visible
            var table = Page.Locator("table.table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des murs)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(3);
        }
    }
}
