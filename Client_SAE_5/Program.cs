using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client_SAE_5.ViewModel;
using Microsoft.AspNetCore.Components;
using Client_SAE_5.Utils.Singleton;

namespace Client_SAE_5
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(" https://api-ovhiutannecy-sae5-gwbxhugwhjb2aqdu.canadacentral-01.azurewebsites.net/api/") });

            /*            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });*/
            // Enregistre le HttpClient

            builder.Services.AddScoped<SalleViewModel>();
            builder.Services.AddScoped<CapteurViewModel>();
            builder.Services.AddScoped<MurViewModel>();
            builder.Services.AddScoped<TypeSalleViewModel>();
            builder.Services.AddScoped<BatimentViewModel>();
            builder.Services.AddScoped<EquipementViewModel>();
            builder.Services.AddScoped<TypeEquipementViewModel>();
            builder.Services.AddScoped<UniteViewModel>();
            
            builder.Services.AddScoped<InfluxViewModel>();

            // Ajout singleton du DataStorage (permet de garder les listes des éléments entre les pages et les avoir à jour quand ajout/modif/suppression)
            builder.Services.AddSingleton<DataStorage>();

            builder.Services.AddBlazorBootstrap();


            builder.Services.AddBlazorBootstrap();

            await builder.Build().RunAsync();
        }
    }
}
