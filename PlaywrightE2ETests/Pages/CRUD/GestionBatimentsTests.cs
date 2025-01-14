using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Client_SAE_5.DTO;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionBatimentsTests : PageTest
    {
        private string BaseUrl = TestsConfig.BaseURL;
        private const string BatimentsUrl = "/crud/batiments";

        [TestInitialize]
        public async Task TestInitialize()
        {
            // Mock de la requête GET initiale pour charger les bâtiments
            await Page.RouteAsync("**/api/Batiments", async route =>
            {
                var mockData = new List<BatimentDTO>
                {
                    new BatimentDTO
                    {
                        IdBatiment = 1,
                        NomBatiment = "NouveauBâtimentTest",
                        NbSalle = 5
                    }
                };

                await route.FulfillAsync(new()
                {
                    Status = 200,
                    ContentType = "application/json",
                    Body = JsonSerializer.Serialize(mockData)
                });
            });

            // Mock de la requête POST pour l'ajout
            await Page.RouteAsync("**/api/Batiments", async route =>
            {
                if (route.Request.Method == "POST")
                {
                    var requestBody = JsonSerializer.Deserialize<BatimentSansNavigationDTO>(
                        route.Request.PostData);

                    var response = new BatimentSansNavigationDTO
                    {
                        IdBatiment = 2, // Simuler un ID auto-incrémenté
                        NomBatiment = requestBody.NomBatiment
                    };

                    await route.FulfillAsync(new()
                    {
                        Status = 200,
                        ContentType = "application/json",
                        Body = JsonSerializer.Serialize(response)
                    });
                }
            });
        }

        [TestMethod]
        public async Task GestionBatimentTitreCorrect()
        {
            await Page.GotoAsync($"{BaseUrl}{BatimentsUrl}");
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des bâtiments"));
        }

        [TestMethod]
        public async Task TableOfBatiments_CorrectNumberColumn()
        {
            await Page.GotoAsync($"{BaseUrl}{BatimentsUrl}");
            var table = Page.Locator("table.bb-table");
            await Expect(table).ToBeVisibleAsync();
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(6);
        }

        [TestMethod]
        public async Task BatimentAddModal_ShouldWork_Cancel()
        {
            await Page.GotoAsync($"{BaseUrl}{BatimentsUrl}");
            // Récupérer bouton Ajout
            await Page.ClickAsync("button#btnAddBatimentPage");

            // Ouvrir la modale ajout
            var modal = Page.Locator(".editModal");
            await Expect(modal).ToBeVisibleAsync();

            // Fermer la modale
            await Page.Locator(".editModal button.btn-secondary").ClickAsync();
        }

        [TestMethod]
        public async Task AddBatiment_WithValidData_ShouldSucceed()
        {
            await Page.GotoAsync($"{BaseUrl}{BatimentsUrl}");
            string nomBatiment = "NouveauBâtimentTest";

            // Ouvrir la modale
            await Page.ClickAsync("#btnAddBatimentPage");

            // Attendre que la modale soit visible
            await Page.WaitForSelectorAsync(".editModal", new() { State = WaitForSelectorState.Visible });

            // Remplir le formulaire
            await Page.FillAsync("input[type='text']", nomBatiment);

            // Vérifier l'état initial (pas de toast)
            var initialToast = await Page.QuerySelectorAsync(".toast");
            Assert.IsNull(initialToast, "Un toast ne devrait pas être présent avant l'ajout");

            // Cliquer sur le bouton Ajouter
            var btnAjout = Page.Locator("button#ajouterBatimentBtnDialog");
            await btnAjout.ClickAsync();

            // Attendre que le message de succès apparaisse
            await Page.WaitForSelectorAsync("Renseignement des informations du bâtiment", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 5000
            });

            // Vérifier que le nouveau bâtiment apparaît dans la grille
            var texte = Page.Locator(nomBatiment);
            await Expect(texte).ToBeVisibleAsync();

            // Vérifier que le modal est effectivement masqué
            await Page.WaitForSelectorAsync("Renseignement des informations du bâtiment", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 5000
            });

            // Vérifier que les navigations de l'objet créée marche bien
            Assert.IsNotNull(texte, "L'objet rajouté est null!");
            await texte.ClickAsync();
            Assert.IsTrue(await Page.TitleAsync() == "Détails du bâtiment", "La navigation vers les détails du bâtiment rajouté sont fausses.");
        }
    }
}