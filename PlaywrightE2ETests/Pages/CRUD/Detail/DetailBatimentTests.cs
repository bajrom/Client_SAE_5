﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightE2ETests.Pages.CRUD.Detail
{
    public class DetailBatimentTests:PageTest
    {
        private string Url = String.Concat(TestsConfig.BaseURL,"/crud/batiments/1");

        [TestMethod]
        public async Task DetailBatiment_TitreCorrect()
        {
            await Page.GotoAsync(Url);

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Détails du bâtiment"));
        }

        [TestMethod]
        public async Task DetailBatiment_ContentVisible()
        {
            await Page.GotoAsync(Url);

            var titre = Page.Locator("h1");
            await Expect(titre).ToBeVisibleAsync();
        }


    }
}