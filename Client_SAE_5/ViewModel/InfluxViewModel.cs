using Client_SAE_5.DTO;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class InfluxViewModel
    {
        private readonly InfluxDataService<SalleDTO> _salleService;



        public List<string> nomSalles { get; private set; } = new List<string>();


    }
}
