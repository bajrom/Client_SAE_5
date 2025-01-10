using Client_SAE_5.Models;
using Client_SAE_5.Models.Services;

namespace Client_SAE_5.ViewModel
{
    public class InfluxDataViewModel
    {
        private readonly IService<InfluxData> _service;

        public InfluxDataViewModel(IService<InfluxData> service)
        {
            _service = service;
        }

        public async Task<List<InfluxData>> GetCapteurData()
        {
            try
            {
                return await _service.GetAllTAsync("InfluxData/data/capteurs");
            }
            catch (Exception ex)
            {
                // Gérez les erreurs selon vos besoins
                Console.WriteLine($"Erreur lors de la récupération des données: {ex.Message}");
                return new List<InfluxData>();
            }
        }

        public async Task<List<InfluxData>> GetCapteurDataById(string capteurId)
        {
            try
            {
                // Adaptez le chemin selon votre API
                var response = await _service.GetAllTAsync($"InfluxData/data/capteurs/{capteurId}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des données du capteur {capteurId}: {ex.Message}");
                return new List<InfluxData>();
            }
        }
    }
}