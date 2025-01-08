using Microsoft.Playwright;
using System;

namespace PlaywrightE2ETests.Pages
{
    [TestClass]
    public class AccueilTests : PageTest
    {
        private const string Url = $"http://localhost:5258";

        [TestMethod]
        public async Task HomePageHaveTitleDatacare()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Datacare"));
        }

        [TestMethod]
        public async Task HomePageHaveImgLaptop()
        {
            await Page.GotoAsync(Url);

            var img = Page.GetByTestId("laptop");

            await img.IsVisibleAsync();
        }

        [TestMethod]
        public async Task HomePageHaveMainText()
        {
            await Page.GotoAsync(Url);

            var txt = Page.GetByTestId("text-container");

            await txt.GetByRole(AriaRole.Paragraph, new() { Name = "txtDesc" }).IsVisibleAsync();
            await txt.GetByRole(AriaRole.Heading, new() { Name = "txtH1" }).IsVisibleAsync();
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

            var btn = Page.GetByRole(AriaRole.Button, new() { Name = "Démarrer" });

            await btn.IsVisibleAsync();

            await btn.ClickAsync();
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Capteurs"));
        }
    }
}
