using Client_SAE_5.DTO;
using Client_SAE_5.Models.Services;
using System.Xml.Linq;

namespace Client_SAE_5.ViewModel
{
    public class InfluxViewModel
    {
        private readonly InfluxPredictionService _predictionService;
        private readonly InfluxDataService<decimal> _vibrationsService; //en soit utilisé comme bool mais valeurs de 0 à 1


        public InfluxViewModel()
        {
            _predictionService = new InfluxPredictionService();
            _vibrationsService = new InfluxDataService<decimal>("vibrations");
        }

        public List<string> NomCapteurs { get; private set; } = new List<string>();

        public string SelectedCapteurName { get; set; } = "";

        public string ErrorMessage { get; private set; }

        //predictions
        public bool PredFenetreOuverte { get; private set; }

        //valeurs actuelles
        public decimal ActualVibration { get; private set; }

        public async Task LoadNomCapteurs()
        {
            try
            {
                NomCapteurs = await _predictionService.GetCapteursAsync();
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des noms de capteurs : {ex.Message}";
            }
        }


        // load predictions
        public async Task LoadPredictionsOfCapteurAsync(string nomCapteur)
        {
            await LoadPredFenetreOuverteAsync(nomCapteur);
        }

        public async Task LoadPredFenetreOuverteAsync(string nomCapteur)
        {
            try
            {
                PredFenetreOuverte = await _predictionService.GetFenetreOuvertePredAsync(nomCapteur);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la prediction de fenetre ouverte du capteur {nomCapteur} : {ex.Message}";
            }
        }

        // load valeurs actuelles
        public async Task LoadActualValuesOfCapteurAsync(string nomCapteur)
        {
            await LoadActualVibrationAsync(nomCapteur);
        }

        public async Task LoadActualVibrationAsync(string nomCapteur)
        {
            try
            {
                ActualVibration = await _vibrationsService.GetTNowAsync(nomCapteur);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle de vibration du capteur {nomCapteur} : {ex.Message}";
            }
        }
    }
}
