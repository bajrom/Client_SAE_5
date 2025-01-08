using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionEquipementsTests : PageTest
    {
        private const string Url = $"https://data-care.azurewebsites.net/crud/equipements";

        [TestMethod]
        public async Task GestionEquipementTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des équipements"));
        }

        [TestMethod]
        public async Task TableOfEquipements_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des équipements est visible
            var table = Page.Locator("table.table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des équipements)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(5);
        }
    }
}
