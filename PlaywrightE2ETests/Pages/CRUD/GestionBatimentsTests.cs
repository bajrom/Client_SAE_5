using System;
using System.Formats.Asn1;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionBatimentsTests : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/batiments");

        [TestMethod]
        public async Task GestionBatimentTitreCorrect()
        {
            await Page.GotoAsync(Url);
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des bâtiments"));
        }

        [TestMethod]
        public async Task TableOfBatiments_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(6);
        }

        [TestMethod]
        public async Task BatimentAddModal_ShouldWork_Cancel()
        {
            // Récupérer bouton Ajout
            await Page.GotoAsync(Url);
            await Page.ClickAsync("button#btnAddBatimentPage");

            // Ouvrir la modale ajout
            var modal = Page.Locator(".editModal");
            await Expect(modal).ToBeVisibleAsync();

            // Fermer la modale
            await Page.Locator(".editModal button.btn-secondary").ClickAsync();
        }
    }
}