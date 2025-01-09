using System;

namespace PlaywrightE2ETests.Pages.CRUD
{
    [TestClass]
    public class GestionBatimentsTests : PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL, "/crud/batiments");

        [TestMethod]
        public async Task GestionBatimentTitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Gestion des batiments"));
        }

        [TestMethod]
        public async Task TableOfBatiments_CorrectNumberColumn()
        {
            await Page.GotoAsync(Url);

            // Vérifier que la table des bâtiments est visible
            var table = Page.Locator("table.table");
            await Expect(table).ToBeVisibleAsync();

            // Vérifier que la table contient des lignes (des bâtiments)
            var columns = table.Locator("thead tr th");
            await Expect(columns).ToHaveCountAsync(3);
        }

        //[TestMethod]
        //public async Task EditBatimentOpensEditForm()
        //{
        //    await Page.GotoAsync(Url);

        //    // Cliquer sur le bouton d'édition du premier bâtiment
        //    var editButton = await Page.Locator("table tbody tr .btn-primary");
        //    await editButton.ClickAsync();

        //    // Vérifier que le champ de saisie du nom du bâtiment est visible
        //    var nameInput = await Page.Locator("input[bind='BatimentViewModel.EditableBatiment.NomBatiment']");
        //    await Expect(nameInput).ToBeVisibleAsync();
        //}

        //[TestMethod]
        //public async Task DeleteBatiment()
        //{
        //    await Page.GotoAsync(Url);

        //    // Cliquer sur le bouton de suppression du premier bâtiment
        //    var deleteButton = await Page.Locator("table tbody tr .btn-danger");
        //    await deleteButton.ClickAsync();

        //    // Vérifier qu'un message de confirmation ou un changement d'état se produit (à ajuster selon votre application)
        //    var alert = await Page.Locator("div.alert.alert-danger");
        //    await Expect(alert).ToBeVisibleAsync();
        //}

        //[TestMethod]
        //public async Task AddNewBatiment()
        //{
        //    await Page.GotoAsync(Url);

        //    // Cliquer sur le bouton "Ajouter" si vous en avez un pour ajouter un bâtiment (si c'est possible dans l'UI)
        //    var nameInput = await Page.Locator("input[bind='BatimentViewModel.EditableBatiment.NomBatiment']");
        //    await nameInput.FillAsync("Nouveau Bâtiment");

        //    // Cliquer sur le bouton "Enregistrer"
        //    var saveButton = await Page.Locator("button.btn-success");
        //    await saveButton.ClickAsync();

        //    // Vérifier que le bâtiment a été ajouté (vérifiez un nouvel élément dans la table)
        //    var rows = await Page.Locator("table tbody tr");
        //    await Expect(rows).ToHaveCountAsync(2);  // Vérifiez que le nombre de lignes est augmenté
        //}

        //[TestMethod]
        //public async Task DisplayErrorMessageWhenSaveFails()
        //{
        //    await Page.GotoAsync(Url);

        //    // Simuler un échec en laissant un champ vide ou en provoquant un message d'erreur
        //    var nameInput = await Page.Locator("input[bind='BatimentViewModel.EditableBatiment.NomBatiment']");
        //    await nameInput.FillAsync(""); // Laisser vide pour provoquer une erreur

        //    var saveButton = await Page.Locator("button.btn-success");
        //    await saveButton.ClickAsync();

        //    // Vérifier que le message d'erreur est affiché
        //    var errorMessage = await Page.Locator("div.alert.alert-danger");
        //    await Expect(errorMessage).ToBeVisibleAsync();
        //}
    }
}
