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
            // Mock de la requ�te GET initiale pour charger les b�timents
            await Page.RouteAsync("**/api/Batiments", async route =>
            {
                var mockData = new List<BatimentDTO>
                {
                    new BatimentDTO
                    {
                        IdBatiment = 1,
                        NomBatiment = "NouveauB�timentTest",
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

            // Mock de la requ�te POST pour l'ajout
            await Page.RouteAsync("**/api/Batiments", async route =>
            {
                if (route.Request.Method == "POST")
                {
                    var requestBody = JsonSerializer.Deserialize<BatimentSansNavigationDTO>(
                        route.Request.PostData);

                    var response = new BatimentSansNavigationDTO
                    {
                        IdBatiment = 2, // Simuler un ID auto-incr�ment�
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
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des b�timents"));
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
            // R�cup�rer bouton Ajout
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
            string nomBatiment = "NouveauB�timentTest";

            // Ouvrir la modale
            await Page.ClickAsync("#btnAddBatimentPage");

            // Attendre que la modale soit visible
            await Page.WaitForSelectorAsync(".editModal", new() { State = WaitForSelectorState.Visible });

            // Remplir le formulaire
            await Page.FillAsync("input[type='text']", nomBatiment);

            // V�rifier l'�tat initial (pas de toast)
            var initialToast = await Page.QuerySelectorAsync(".toast");
            Assert.IsNull(initialToast, "Un toast ne devrait pas �tre pr�sent avant l'ajout");

            // Cliquer sur le bouton Ajouter
            var btnAjout = Page.Locator("button#ajouterBatimentBtnDialog");
            await btnAjout.ClickAsync();

            // Attendre que le message de succ�s apparaisse
            await Page.WaitForSelectorAsync("Renseignement des informations du b�timent", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 5000
            });

            // V�rifier que le nouveau b�timent appara�t dans la grille
            var texte = Page.Locator(nomBatiment);
            await Expect(texte).ToBeVisibleAsync();

            // V�rifier que le modal est effectivement masqu�
            await Page.WaitForSelectorAsync("Renseignement des informations du b�timent", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 5000
            });

            // V�rifier que les navigations de l'objet cr��e marche bien
            Assert.IsNotNull(texte, "L'objet rajout� est null!");
            await texte.ClickAsync();
            Assert.IsTrue(await Page.TitleAsync() == "D�tails du b�timent", "La navigation vers les d�tails du b�timent rajout� sont fausses.");
        }
    }
}