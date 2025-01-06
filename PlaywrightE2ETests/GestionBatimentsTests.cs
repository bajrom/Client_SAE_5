namespace PlaywrightE2ETests
{
    [TestClass]
    public class GestionBatimentsTests : PageTest
    {
        [TestMethod]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            await Page.GotoAsync("https://localhost:7283/crud/batiments");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des batiments"));

            // create a locator
            //var getStarted = Page.Locator("text=Get Started");

            // Expect an attribute "to be strictly equal" to the value.
            //await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

            // Click the get started link.
            //await getStarted.ClickAsync();

            // Expects the URL to contain intro.
            //await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));
        }
    }
}
