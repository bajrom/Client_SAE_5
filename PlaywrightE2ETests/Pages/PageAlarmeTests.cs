using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages
{
    [TestClass]
    public class PageAlarmeTests : PageTest
    {

        [TestMethod]
        public async Task ContentExists_IsVisible()
        {
            await Page.GotoAsync($"{TestsConfig.BaseURL}/alarme");

            var btn = Page.Locator("button.alarm-button");
            await Expect(btn).ToBeVisibleAsync();
            await Expect(btn).ToHaveTextAsync("OFF");
        }

        [TestMethod]
        public async Task ClicksAlarmButton_ConfirmDialogShow()
        {
            // modal modal-confirmation fade
            await Page.GotoAsync($"{TestsConfig.BaseURL}/alarme");

            await Page.Locator("button.alarm-button").ClickAsync();

            var modal = Page.Locator(".modal.modal-confirmation");
            String classesModal = modal.GetAttributeAsync("style").Result;
            Assert.IsTrue(classesModal.Contains("display:block"), $"La modale n'a pas le style block (n'est pas visible). Actuellement: {classesModal}");
            await Expect(modal).ToBeVisibleAsync();
        }

        [TestMethod]
        public async Task ConfirmationPopUp_IsVisible()
        {
            await ClicksAlarmButton_ConfirmDialogShow();

            // Localiser chaque élément
            var titreModal = Page.Locator("h5.modal-title");
            var texteModal = Page.Locator("div.modal-body div.pb-2");
            var btnClose = Page.Locator("button.btn-close");
            var btnDeclencer = Page.Locator("div.modal-footer button:nth-child(2)");
            var btnAnnuler = Page.Locator("div.modal-footer button:first-of-type");

            // Vérifier visibilité des différents éléments de la modale
            await titreModal.IsVisibleAsync();
            await texteModal.IsVisibleAsync();
            await btnClose.IsVisibleAsync();
            await btnDeclencer.IsVisibleAsync();
            await btnAnnuler.IsVisibleAsync();

            // Vérifier texte à l'intérieur de chaque élément
            await Expect(texteModal).ToHaveTextAsync("Êtes vous sûr de déclencher l'alarme ?");
            await Expect(titreModal).ToHaveTextAsync("Avertissement");
            await Expect(btnDeclencer).ToHaveTextAsync("Déclencher");
            await Expect(btnAnnuler).ToHaveTextAsync("Annuler");

            // Vérifier les classes des boutons
            String classesBtnAnnuler = btnAnnuler.GetAttributeAsync("class").Result;
            Assert.IsTrue(classesBtnAnnuler.Contains("btn-primary"), "Le bouton annuler n'a pas la classe btn-primary: "+classesBtnAnnuler);

            String classesBtnDeclencher = btnDeclencer.GetAttributeAsync("class").Result;
            Assert.IsTrue(classesBtnDeclencher.Contains("btn-danger"), "Le bouton déclencher n'a pas la classe btn-danger: "+classesBtnDeclencher);

            String classesBtnClose = btnClose.GetAttributeAsync("class").Result;
            Assert.IsTrue(classesBtnClose.Contains("btn-close"), "Le bouton de fermeture n'a pas la classe btn-close: " + classesBtnDeclencher);
        }

        [TestMethod]
        public async Task ConfirmationPopUp_Functionality_BtnClose()
        {
            await ClicksAlarmButton_ConfirmDialogShow();

            var btnClose = Page.Locator("button.btn-close");

            await btnClose.ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Alarme");

            // Modale disparue
            var modal = Page.Locator(".modal.modal-confirmation");
            String classesModal = modal.GetAttributeAsync("style").Result;
            Assert.IsFalse(classesModal.Contains("display:block"), $"La modale n'a pas le style none (est visible). Actuellement: {classesModal}");
        }

        [TestMethod]
        public async Task ConfirmationPopUp_Functionality_BtnCancel()
        {
            await ClicksAlarmButton_ConfirmDialogShow();

            var btnAnnuler = Page.Locator("div.modal-footer button:first-of-type");

            await btnAnnuler.ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Alarme");

            // Modale disparue
            var modal = Page.Locator(".modal.modal-confirmation");
            String classesModal = modal.GetAttributeAsync("style").Result;
            Assert.IsFalse(classesModal.Contains("display:block"), $"La modale n'a pas le style none (est visible). Actuellement: {classesModal}");
        }

        [TestMethod]
        public async Task ConfirmationPopUp_Functionality_BtnDeclencher()
        {
            await ClicksAlarmButton_ConfirmDialogShow();

            // Déclenchement de l'alarme
            var btnDeclencer = Page.Locator("div.modal-footer button:nth-child(2)");
            await btnDeclencer.ClickAsync();
            await Expect(Page).ToHaveTitleAsync("Alarme");

            // Modale disparue
            var modal = Page.Locator(".modal.modal-confirmation");
            String classesModal = modal.GetAttributeAsync("style").Result;
            Assert.IsFalse(classesModal.Contains("display:block"), $"La modale n'a pas le style none (est visible). Actuellement: {classesModal}");

            // Toast affiché
            await Expect(Page.Locator("div.toast")).ToBeVisibleAsync();

            // Bouton bien activé
            var btnON = Page.Locator("button.alarm-button.on");
            await Expect(btnON).ToHaveTextAsync("ON");
            await Expect(btnON).ToHaveCSSAsync("color", "rgb(255, 255, 255)");
            await Expect(btnON).ToBeVisibleAsync();

            // Texte bien changé
            var texte = Page.Locator("div.alarm-message");
            await Expect(texte).ToHaveTextAsync("🚨 L'alarme est active ! 🚨");
            await Expect(texte).ToHaveCSSAsync("color", "rgb(255, 0, 0)");
            await Expect(texte).ToBeVisibleAsync();

            // Eteindre l'alarme
            await btnON.ClickAsync();
            await Expect(Page.Locator("button.alarm-button")).ToHaveTextAsync("OFF");
            await Expect(Page.Locator("button.alarm-button")).ToHaveCSSAsync("color", "rgb(255, 0, 0)");
            await Expect(Page.Locator("button.alarm-button")).ToBeVisibleAsync();
        }
    }
}
