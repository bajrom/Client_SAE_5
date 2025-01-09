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

        [TestMethod]
        public async Task GestionTypesEquipementTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des types d'équipement"));
        }

        [TestMethod]
        public async Task TableOfTypesEquipement_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des types de Equipements est visible
            var table = Page.Locator("table.table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des types d'équipements)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(2);
        }
    }
}
