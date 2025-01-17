using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD.Detail
{
    [TestClass]
    public class DetailBatimentTests:PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL,"/crud/batiments/");

        [TestMethod]
        public async Task DetailBatiment_TitreCorrect()
        {
            await Page.GotoAsync(Url+"2");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Détails du bâtiment"));
        }

        [TestMethod]
        public async Task DetailBatiment_ContentVisible()
        {
            await Page.GotoAsync(Url+"2");

            var titre = Page.Locator("h1");
            await Expect(titre).ToBeVisibleAsync();

            var contenuBatimentConnu = Page.GetByText("Nombre de salles");

            // Attendre que l'élément contenant le nombre de salles soit visible
            var nbSalleDOM = await Page.WaitForSelectorAsync("p#nbSallesBatiment", new() { State = WaitForSelectorState.Visible, Timeout = 15000 });

            int nbSallesAffiches = int.Parse(await nbSalleDOM.TextContentAsync());

            // Attendre que la liste des salles soit visible
            await Page.Locator("ul").WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
            var listeSalles = Page.Locator("ul");
            var nbSallesListe = await listeSalles.Locator("li").CountAsync();


            if (nbSallesListe > 0)
            {
                await Expect(contenuBatimentConnu).ToBeVisibleAsync();
                Assert.IsTrue(nbSallesListe == nbSallesAffiches, $"Le nombre de salles réel {nbSallesAffiches} ne correspond pas à la liste de salles affichées {nbSallesListe}");
            }
        }

        [TestMethod]
        public async Task DetailBatiment_LiensCorrects()
        {
            List<int> Ids = [ 2, 28 ];

            for (int i = 0; i <= 1; i++)
            {
                await Page.GotoAsync(Url + Ids[i]);

                var contenuBatimentInconnu = Page.GetByText("Aucune salle n'est associée à ce bâtiment. Voulez-vous en rajouter un ?");
                var contenuBatimentConnu = Page.GetByText("Nombre de salles");
                var btnCancel = Page.GetByText("Retour");

                if (contenuBatimentConnu == null)
                {
                    IReadOnlyList<IElementHandle> listeSalles = await Page.QuerySelectorAllAsync("li");

                    foreach (var item in listeSalles)
                    {
                        String? contenuLi = await item.TextContentAsync();
                        await item.ClickAsync();
                        var textePage = Page.GetByText(contenuLi);
                        await Expect(textePage).ToBeVisibleAsync();
                    }
                    await Page.GotoAsync(Url + Ids[i]);
                }
            }
        }

        [TestMethod]
        public async Task DetailBatiment_RetourFonctionne()
        {
            await Page.GotoAsync(Url + "2");
            await Page.GetByText("Retour").ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Gestion des bâtiments");
        }


    }
}
