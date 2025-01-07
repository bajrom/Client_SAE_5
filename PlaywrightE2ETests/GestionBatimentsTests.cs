namespace PlaywrightE2ETests
{
    [TestClass]
    public class GestionBatimentsTests : PageTest
    {
        private readonly string baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "https://localhost:7283";

        [TestMethod]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            var url = $"{baseUrl}/crud/batiments";
            await Page.GotoAsync(url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des batiments"));
        }
    }
}
