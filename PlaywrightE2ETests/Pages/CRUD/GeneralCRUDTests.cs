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
        private const string Url = $"https://data-care.azurewebsites.net/";

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
            await Page.GotoAsync(Url + "crud/Batiments");

            foreach (var item in PagesStr)
            {
                await Page.GotoAsync(Url + "crud/" + item);

                // Vérifier si les navigations dans le menu marchent
                foreach (var Iteminitem in PagesStr)
                {
                    if (!string.IsNullOrWhiteSpace(Iteminitem))
                    {
                        // Properly format the item (handle case issues)
                        string formattedItem = Iteminitem.Substring(0, 1).ToLower() + Iteminitem.Substring(1);

                        // Debugging output
                        Console.WriteLine($"Attempting to click: a[href='crud/{formattedItem.ToLower()}']");

                        var link = Page.Locator($"a[href='crud/{formattedItem.ToLower()}']");

                        try
                        {
                            // Wait for the element to be visible
                            await link.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 60000 });

                            // Click the link if visible
                            await link.ClickAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }

                // Revenir à la page à tester
                await Page.GotoAsync(Url+ "crud/" + item);

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
