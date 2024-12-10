using Client_SAE_5.Models.Services;
using Client_SAE_5.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client_SAE_5.ViewModel;

namespace Client_SAE_5
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7162/api/") });

            /*            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });*/
            // Enregistre le HttpClient
            builder.Services.AddScoped<SalleViewModel>();
            builder.Services.AddScoped<CapteurViewModel>();

            await builder.Build().RunAsync();
        }
    }
}
