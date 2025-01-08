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
        private const string Url = $"https://data-care.azurewebsites.net/crud/capteurs";

        [TestMethod]
        public async Task GestionCapteursTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Capteurs"));
        }

        [TestMethod]
        public async Task TableOfCapteurs_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des capteurs est visible
            var table = Page.Locator("table.table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des capteurs)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(3);
        }


    }
}
