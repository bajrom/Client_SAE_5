using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages
{
    [TestClass]
    public class DiverseTests : PageTest
    {
        [TestMethod]
        public async Task NotFoundWhenRandomURL()
        {
            // Create a random URL by appending digits to the base URL
            string randomUrl = TestsConfig.BaseURL + "/1";  // Start with some static string part
            for (int i = 0; i < 10; i++)
            {
                randomUrl += new Random().Next(10).ToString();  // Append random digits
            }

            // Navigate to the generated URL
            await Page.GotoAsync(randomUrl);

            // Check if the title contains "Not found"
            await Expect(Page).ToHaveTitleAsync(new Regex("Not found"));

            // Check if the paragraph with id "pNotFound" has the expected text
            var notFoundText = await Page.Locator("p#pNotFound").InnerTextAsync();
            Assert.AreEqual("Désolé, il n'y a rien à cette adresse.", notFoundText);
        }

        [TestMethod]
        public async Task ClickDatacare_GoHome()
        {
            await Page.GotoAsync(TestsConfig.BaseURL + "/crud/batiments");

            var dataCare = Page.Locator("a.navbar-brand");
            await dataCare.IsVisibleAsync();

            dataCare.ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Datacare");

        }
    }
}
