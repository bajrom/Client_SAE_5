using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD
{

    [TestClass]
    public class GeneralCRUDTests : PageTest
    {
        private static readonly List<string> PagesStr = new List<string>
        {
            "Batiments",
            "Capteurs",
            "Equipements",
            "Murs",
            "Salles",
            "TypesEquipement",
            "TypesSalle",
            "Unites"
        };

        [TestMethod]
        public async Task ClickOnPagesLinkMenuCRUDCorrectPages_CorrectContent_CorrectNavigation()
        {
            // Initialisation à bâtiment
            await Page.GotoAsync(TestsConfig.BaseURL + "/crud/Batiments");

            foreach (var item in PagesStr)
            {
                // Revenir à la page à tester
                await Page.GotoAsync(TestsConfig.BaseURL + "/crud/" + item);

                // Vérifier s'il y a un bien un tableau
                var table = Page.Locator("table.table");
                await Expect(table).ToBeVisibleAsync();

                // Vérifier que la table contient au moins une ligne (des bâtiments)
                var rows = table.Locator("tbody tr");
                int nb = await rows.CountAsync();

                // Vérifier si le nombre de lignes est supérieur ou égal à 1
                bool nbSup1 = nb >= 1;

                Assert.IsTrue(nbSup1, "La table ne contient pas de lignes.");
            }
        }

    }
}
