using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Client_SAE_5.DTO;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionSallesTests : PageTest
    {
        private string BaseUrl = TestsConfig.BaseURL;
        private const string SallesUrl = "/crud/salles";

        [TestInitialize]
        public async Task TestInitialize()
        {
            await Page.GotoAsync($"{BaseUrl}{SallesUrl}");
            // Wait for data to load and spinner to disappear
            await Page.WaitForSelectorAsync(".spinner-border", new() { State = WaitForSelectorState.Hidden, Timeout = 10000 });
        }

        [TestMethod]
        public async Task GestionSalleTitreCorrect()
        {
            await Page.GotoAsync($"{BaseUrl}{SallesUrl}");
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des Salles"));
        }

        [TestMethod]
        public async Task TableOfSalles_CorrectNumberColumn()
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
            await Expect(columns).ToHaveCountAsync(4);

            // Verify column headers
            var headers = await columns.AllTextContentsAsync();
            Assert.IsTrue(headers.Contains("Nom de la Salle"));
            Assert.IsTrue(headers.Contains("Bâtiment"));
            Assert.IsTrue(headers.Contains("Type"));
            Assert.IsTrue(headers.Contains("Actions"));
        }

        [TestMethod]
        public async Task SalleAddModal_ShouldWork_Cancel()
        {
            await Page.GotoAsync($"{BaseUrl}{SallesUrl}");
            // Récupérer bouton Ajout
            await Page.ClickAsync("button.btn-success");

            // Ouvrir la modale ajout
            var modal = Page.Locator(".modal");
            await Expect(modal).ToBeVisibleAsync();

            // Fermer la modale
            await Page.Locator(".modal button.btn-secondary").ClickAsync();
        }

        private async Task DeleteSalleAsync(string nomSalle)
        {
            // Attendre que la page soit prête
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Trouver la salle à supprimer dans la grille
            var salleCell = Page.Locator("table.table td", new() { HasText = nomSalle });
            await Expect(salleCell).ToBeVisibleAsync();

            // Cliquer sur le bouton de suppression
            var deleteButton = salleCell.Locator("..").Locator("button.btn-danger");
            await deleteButton.ClickAsync();

            // Attendre que la boîte de dialogue de confirmation apparaisse
            var confirmDialog = Page.GetByText("Confirmer la suppression de la salle");
            await Expect(confirmDialog).ToBeVisibleAsync();

            // Confirmer la suppression
            var confirmButton = Page.Locator("#confirmDialog button.btn-danger");
            await confirmButton.ClickAsync();

            // Attendre que la boîte de dialogue de confirmation disparaisse
            await Page.WaitForSelectorAsync(".confirm-dialog", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });

            // Vérifier que la salle supprimée n'apparaît plus dans la grille
            await Expect(salleCell).Not.ToBeVisibleAsync(new() { Timeout = 30000 });
        }

        private async Task UpdateSalleAsync(string ancienNom, string nouveauNom)
        {
            // Attendre que la page soit prête
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Trouver la salle à modifier dans la grille
            var salleCell = Page.Locator("table.table td", new() { HasText = ancienNom });
            await Expect(salleCell).ToBeVisibleAsync();

            // Cliquer sur le bouton de modification
            var editButton = salleCell.Locator("..").Locator("button.btn-primary");
            await editButton.ClickAsync();

            // Attendre que la modale de modification soit visible
            var modal = Page.Locator(".modal");
            await Expect(modal).ToBeVisibleAsync();

            // Modifier le nom de la salle
            await modal.Locator("input[type='text']").FillAsync(nouveauNom);

            // Soumettre le formulaire
            var btnModifierModal = modal.GetByText("Modifier");
            await btnModifierModal.ClickAsync();

            // Attendre que la modale se ferme
            await Page.WaitForSelectorAsync(".modal", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });

            // Vérifier que le nom modifié apparaît dans la grille
            var updatedSalleCell = Page.Locator("table.table td", new() { HasText = nouveauNom });
            await Expect(updatedSalleCell).ToBeVisibleAsync(new() { Timeout = 30000 });
        }

        private async Task<SalleDTO> AddSalleAsync(string nomSalle)
        {
            // Naviguer vers la page des salles
            await Page.GotoAsync($"{BaseUrl}{SallesUrl}");

            // Attendre que la page soit prête
            await Page.WaitForSelectorAsync(".spinner-border", new()
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            // Ouvrir la modale d'ajout
            var btnAjouter = Page.GetByText("Ajouter une salle");
            await btnAjouter.ClickAsync();
            var modal = Page.Locator(".modal");
            await Expect(modal).ToBeVisibleAsync();

            // Remplir le formulaire
            await modal.Locator("input[type='text']").FillAsync(nomSalle);

            // Soumettre le formulaire
            var btnAjouterModal = modal.GetByText("Ajouter");
            await btnAjouterModal.ClickAsync();

            // Attendre que la modale se ferme
            await Page.WaitForSelectorAsync(".modal", new() { State = WaitForSelectorState.Hidden, Timeout = 15000 });

            // Vérifier que la nouvelle salle apparaît dans la grille
            var newSalleCell = Page.Locator("table.table td", new() { HasText = nomSalle });
            await Expect(newSalleCell).ToBeVisibleAsync(new() { Timeout = 30000 });

            // Retourner un objet SalleDTO simulé (à adapter selon votre implémentation)
            return new SalleDTO { NomSalle = nomSalle };
        }

        [TestMethod]
        public async Task Add_Update_Delete_Salle_ShouldWork()
        {
            // Étape 1 : Ajouter une salle
            const string nomSalle = "NouvelleSalleTest";
            var salleAjoutee = await AddSalleAsync(nomSalle);

            // Étape 2 : Modifier la salle ajoutée
            const string nouveauNom = "SalleMisÀJour";
            await UpdateSalleAsync(nomSalle, nouveauNom);

            // Étape 3 : Supprimer la salle modifiée
            await DeleteSalleAsync(nouveauNom);
        }
    }
}