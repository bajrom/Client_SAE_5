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
        private List<BatimentDTO> mockBatiments;

        [TestInitialize]
        public async Task TestInitialize()
        {
            await GestionBatimentTitreCorrect();
            // Wait for data to load and spinner to disappear
            await Page.WaitForSelectorAsync(".spinner-border", new() { State = WaitForSelectorState.Hidden, Timeout = 10000 });
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
            Assert.IsTrue(headers.Contains("Nom"), "Le nom de la colonne n'est pas Nom");
            Assert.IsTrue(headers.Contains("Nombre de salles"), "Le nom de la colonne n'est pas 'Nombre de salles'");
            Assert.IsTrue(headers.Contains("Actions"), "Le nom de la colonne n'est pas 'action'");
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

        private async Task DeleteBatimentAsync(string nomBatiment)
        {
            // Attendre que la page soit prête
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Trouver le bâtiment à supprimer dans la grille
            var batimentCell = Page.Locator("table.table td", new() { HasText = nomBatiment });
            await Expect(batimentCell).ToBeVisibleAsync();

            // Cliquer sur le bouton de suppression
            var deleteButton = batimentCell.Locator("..").Locator("button.btn-danger");
            await deleteButton.ClickAsync();

            // Attendre que la boîte de dialogue de confirmation apparaisse
            var confirmDialog = Page.GetByText("Confirmer la suppression du bâtiment");
            await Expect(confirmDialog).ToBeVisibleAsync();

            // Confirmer la suppression
            var confirmButton = Page.Locator("#confirmDialog button.btn-danger");
            await confirmButton.ClickAsync();

            // Attendre que la boîte de dialogue de confirmation disparaisse
            await Page.WaitForSelectorAsync(".confirm-dialog", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });

            // Vérifier que le bâtiment supprimé n'apparaît plus dans la grille
            await Expect(batimentCell).Not.ToBeVisibleAsync(new() { Timeout = 30000 });
        }

        private async Task UpdateBatimentAsync(string ancienNom, string nouveauNom)
        {
            // Attendre que la page soit prête
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Trouver le bâtiment à modifier dans la grille
            var batimentCell = Page.Locator("table.table td", new() { HasText = ancienNom });
            await Expect(batimentCell).ToBeVisibleAsync();

            // Cliquer sur le bouton de modification
            var editButton = batimentCell.Locator("..").Locator("button.btn-primary");
            await editButton.ClickAsync();

            // Attendre que la modale de modification soit visible
            var modal = Page.Locator(".editModal");
            await Expect(modal).ToBeVisibleAsync();

            // Modifier le nom du bâtiment
            await modal.Locator("input[type='text']").FillAsync(nouveauNom);

            // Soumettre le formulaire
            var btnModifierModal = modal.GetByText("Modifier");
            await btnModifierModal.ClickAsync();

            // Attendre que la modale se ferme
            await Page.WaitForSelectorAsync(".editModal", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });

            // Vérifier que le nom modifié apparaît dans la grille
            var updatedBuildingCell = Page.Locator("table.table td", new() { HasText = nouveauNom });
            await Expect(updatedBuildingCell).ToBeVisibleAsync(new() { Timeout = 30000 });
        }

        private async Task<BatimentDTO> AddBatimentAsync(string nomBatiment)
        {
            // Naviguer vers la page des bâtiments
            await Page.GotoAsync($"{BaseUrl}{BatimentsUrl}");

            // Attendre que la page soit prête
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Ouvrir la modale d'ajout(vfrzé
            var btnAjouter = Page.GetByText("Ajouter un bâtiment");
            await btnAjouter.ClickAsync();
            var modal = Page.Locator(".editModal");
            await Expect(modal).ToBeVisibleAsync();

            // Remplir le formulaire
            await modal.Locator("input[type='text']").FillAsync(nomBatiment);

            // Soumettre le formulaire
            var btnAjouterModal = modal.GetByText("Ajouter");
            await btnAjouterModal.ClickAsync();

            // Attendre que la modale se ferme
            await Page.WaitForSelectorAsync(".editModal", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });

            // Vérifier que le nouveau bâtiment apparaît dans la grille
            var newBuildingCell = Page.Locator("table.table td", new() { HasText = nomBatiment });
            await Expect(newBuildingCell).ToBeVisibleAsync(new() { Timeout = 30000 });

            // Retourner un objet BatimentDTO simulé (à adapter selon votre implémentation)
            return new BatimentDTO { NomBatiment = nomBatiment };
        }

        [TestMethod]
        public async Task Add_Update_Delete_Batiment_ShouldWork()
        {
            // Étape 1 : Ajouter un bâtiment
            const string nomBatiment = "NouveauBâtimentTest";
            var batimentAjoute = await AddBatimentAsync(nomBatiment);

            // Étape 2 : Modifier le bâtiment ajouté
            const string nouveauNom = "BâtimentMisÀJour";
            await UpdateBatimentAsync(nomBatiment, nouveauNom);

            // Étape 3 : Supprimer le bâtiment modifié
            await DeleteBatimentAsync(nouveauNom);
        }

        [TestMethod]
        public async Task GestionBatiment_GridContentOK()
        {
            var Pagination = Page.Locator("ul.pagination").First;
            var indicationPage = Page.Locator("div.bb-grid-pagination-text").First;
            var nbPage = Page.Locator("li.page-item.active").Locator("a");

            await Expect(nbPage).ToContainTextAsync("1");
            await Expect(Pagination).ToBeVisibleAsync();
            await Expect(nbPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToBeVisibleAsync();
            await Expect(indicationPage).ToContainTextAsync("bâtiments");
        }
    }
}