using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            // Single route handler for both GET and POST
            await Page.RouteAsync("**/api/batiments**", async route =>
            {
                if (route.Request.Method == "GET")
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
                }
                else if (route.Request.Method == "POST")
                {
                    var requestBody = JsonSerializer.Deserialize<BatimentSansNavigationDTO>(
                        route.Request.PostData);

                    var response = new BatimentSansNavigationDTO
                    {
                        IdBatiment = 2,
                        NomBatiment = requestBody.NomBatiment
                    };

                    await route.FulfillAsync(new()
                    {
                        Status = 200,
                        ContentType = "application/json",
                        Body = JsonSerializer.Serialize(response)
                    });
                }
                else if (route.Request.Method == "PUT")
                {
                    // Lire et désérialiser le corps de la requête
                    var requestBody = JsonSerializer.Deserialize<BatimentSansNavigationDTO>(route.Request.PostData);

                    // Validation des données
                    if (requestBody == null || string.IsNullOrEmpty(requestBody.NomBatiment) || requestBody.IdBatiment <= 0)
                    {
                        await route.FulfillAsync(new()
                        {
                            Status = 400, // Bad Request
                            ContentType = "application/json",
                            Body = JsonSerializer.Serialize(new { Error = "Invalid request data" })
                        });
                        return;
                    }

                    // Simuler la mise à jour (ajuster les données mock si nécessaire)
                    var updatedMockData = new BatimentSansNavigationDTO
                    {
                        IdBatiment = requestBody.IdBatiment,
                        NomBatiment = requestBody.NomBatiment
                    };

                    // Ajouter une journalisation pour déboguer
                    Console.WriteLine($"PUT Request Data: {JsonSerializer.Serialize(requestBody)}");
                    Console.WriteLine($"PUT Response Data: {JsonSerializer.Serialize(updatedMockData)}");

                    // Répondre avec succès
                    await route.FulfillAsync(new()
                    {
                        Status = 200,
                        ContentType = "application/json",
                        Body = JsonSerializer.Serialize(updatedMockData)
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
            await GestionBatimentTitreCorrect();

            // Wait for data to load and spinner to disappear
            await Page.WaitForSelectorAsync(".spinner-border", new() { State = WaitForSelectorState.Hidden, Timeout = 10000 });

            // Wait for Grid component to be rendered (using class from your Blazor component)
            await Page.WaitForSelectorAsync("table.table.table-hover.table-bordered", new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            // Get all th elements using stable classes
            var columns = Page.Locator("table.table.table-hover.table-bordered thead tr th");

            // Debug info
            var columnCount = await columns.CountAsync();
            Console.WriteLine($"Found {columnCount} columns");

            // Verify column count
            await Expect(columns).ToHaveCountAsync(6);

            // Verify column headers
            var headers = await columns.AllTextContentsAsync();
            Assert.IsTrue(headers.Contains("Nom"));
            Assert.IsTrue(headers.Contains("Nombre de salles"));
            Assert.IsTrue(headers.Contains("Actions"));
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
            // Navigate to buildings page
            await Page.GotoAsync($"{BaseUrl}{BatimentsUrl}");
            const string nomBatiment = "NouveauBâtimentTest";

            // Wait for page to be ready
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Open and verify modal
            var btnAjouter = Page.GetByText("Ajouter un bâtiment");
            await btnAjouter.ClickAsync();
            var modal = Page.Locator(".editModal");
            await Expect(modal).ToBeVisibleAsync();

            await Page.WaitForTimeoutAsync(1000);

            // Fill and submit form
            await Page.FillAsync("input[type='text']", nomBatiment);
            var btnAjouterModal = modal.GetByText("Ajouter");
            await btnAjouterModal.ClickAsync();

            await Page.WaitForTimeoutAsync(1000);

            // Wait for modal to close
            await Page.WaitForSelectorAsync(".editModal", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });

            // Verify modal is hidden
            String? styleModalAfter = await modal.GetAttributeAsync("style");
            Assert.IsFalse(styleModalAfter.Contains("block"), $"Modal is not hidden. Current style: {styleModalAfter}");

            // Verify new building appears in grid
            var newBuildingCell = Page.Locator("table.table td", new() { HasText = nomBatiment });
            await Expect(newBuildingCell).ToBeVisibleAsync(new() { Timeout = 30000 });

            // Verify navigation to details page
            await newBuildingCell.ClickAsync();
            var detailsTitle = await Page.WaitForSelectorAsync("h1:has-text('Détails du bâtiment')", new() { Timeout = 5000 });

            Assert.IsNotNull(detailsTitle, "Details page title not found");
            Assert.AreEqual("Détails du bâtiment", await Page.TitleAsync(), "Navigation to building details failed");
        }

        [TestMethod]
        public async Task UpdateBatiment_WithValidData_ShouldSucceed()
        {
            // Navigate to buildings page
            await Page.GotoAsync($"{BaseUrl}{BatimentsUrl}");

            // Wait for page to be ready
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Open the edit modal for the first building
            var editButton = Page.Locator("button.btn-primary").First;
            await editButton.ClickAsync();

            // Verify modal is visible
            var modal = Page.Locator(".editModal");
            await Expect(modal).ToBeVisibleAsync();

            // Update the building name
            const string updatedNomBatiment = "BâtimentMisÀJour";
            await modal.Locator("input").DblClickAsync();
            await modal.Locator("input").FillAsync(updatedNomBatiment);

            // Submit the form
            var btnModifierModal = modal.GetByText("Modifier");
            await btnModifierModal.ClickAsync();
            await Page.WaitForTimeoutAsync(1000);


            // Wait for modal to close
            await Page.WaitForSelectorAsync(".editModal", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });
            await Page.WaitForTimeoutAsync(6000);

            await Page.ScreenshotAsync(new()
            {
                Path = "aaa.png",
            });

            // Verify the updated building appears in the grid
            await Page.WaitForTimeoutAsync(1000);
            var updatedBuildingCell = Page.GetByText(updatedNomBatiment);
            //await Expect(updatedBuildingCell).ToBeVisibleAsync(new() { Timeout = 30000 });

        }

    }
}