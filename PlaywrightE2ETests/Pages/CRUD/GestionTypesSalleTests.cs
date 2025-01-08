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
        private const string Url = $"https://data-care.azurewebsites.net/crud/typessalle";

        [TestMethod]
        public async Task GestionTypesSalleTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des types de salle"));
        }

        [TestMethod]
        public async Task TableOfTypesSalle_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des types de salles est visible
            var table = Page.Locator("table.table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des types de salles)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(2);
        }
    }
}
