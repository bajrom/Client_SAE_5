using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionSallesTests : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/salles");

        [TestMethod]
        public async Task GestionSalleTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Salles"));
        }

        [TestMethod]
        public async Task TableOfSalles_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des salles est visible
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des salles)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(8);
        }
    }
}
