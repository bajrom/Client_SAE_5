namespace PlaywrightE2ETests
{
    [TestClass]
    public class GestionBatimentsTests : PageTest
    {
        [TestMethod]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            var url = $"http://localhost:5258/crud/batiments";
            await Page.GotoAsync(url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des batiments"));
        }
    }
}
