using Microsoft.Playwright;
using System;

namespace PlaywrightE2ETests.Pages
{
    [TestClass]
    public class AccueilTests : PageTest
    {
        private const string Url = $"https://data-care.azurewebsites.net";

        [TestMethod]
        public async Task HomePageHaveTitleDatacare()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Datacare"));
        }

        [TestMethod]
        public async Task HomePageLinkCRUDPages()
        {
            await Page.GotoAsync(Url);

            await Page.GetByRole(AriaRole.Link, new() { Name = "Gestion CRUD" }).ClickAsync();

            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Capteurs"));
        }

        [TestMethod]
        public async Task HomePageLinkGrafanaPage()
        {
            await Page.GotoAsync(Url);

            await Page.GetByRole(AriaRole.Link, new() { Name = "Grafana" }).ClickAsync();

            await Expect(Page).ToHaveTitleAsync(new Regex("Grafana"));
        }

        [TestMethod]
        public async Task HomePageLinkVisualisation3DPage()
        {
            await Page.GotoAsync(Url);

            await Page.GetByRole(AriaRole.Link, new() { Name = "Visualisation 3D" }).ClickAsync();

            await Expect(Page).ToHaveTitleAsync(new Regex("Visualisation 3D"));
        }

        [TestMethod]
        public async Task HomePageClickButtonDemarrer()
        {
            await Page.GotoAsync(Url);

            await Page.GetByRole(AriaRole.Button, new() { Name = "Démarrer" }).ClickAsync();

            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Capteurs"));
        }
    }
}
