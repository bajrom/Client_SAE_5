using Client_SAE_5.DTO;
using Client_SAE_5.Models.Services;
using System.Xml.Linq;
using Client_SAE_5.Models.InfluxDB;


namespace Client_SAE_5.ViewModel
{
    public class InfluxViewModel
    {
        private readonly InfluxPredictionService _predictionService;
        private readonly InfluxDataService _vibrationsService; //en soit utilisé comme bool mais valeurs de 0 à 1
        private readonly InfluxDataService _tempIntService;
        private readonly InfluxDataService _tempExtService;
        private readonly InfluxDataService _humiditeService;
        private readonly InfluxDataService _co2Service;
        private readonly InfluxDataService _mouvementService;
        private readonly InfluxDataService _fumeeService;
        private readonly InfluxDataService _luminositeService;
        private readonly InfluxDataService _bruitService;

        public DateTime StartDate = DateTime.Now.AddYears(-1);
        public DateTime EndDate = DateTime.Now;


        public InfluxViewModel()
        {
            _predictionService = new InfluxPredictionService();
            _vibrationsService = new InfluxDataService("vibrations");
            _tempIntService = new InfluxDataService("temp_int");
            _tempExtService = new InfluxDataService("temp_ext");
            _humiditeService = new InfluxDataService("humidite");
            _co2Service = new InfluxDataService("co");
            _mouvementService = new InfluxDataService("mouvement");
            _fumeeService = new InfluxDataService("fumee");
            _luminositeService = new InfluxDataService("luminosite");
            _bruitService = new InfluxDataService("bruit");
        }

        public List<string> NomCapteurs { get; private set; } = new List<string>();

        public string SelectedCapteurName { get; set; } = "";

        public string ErrorMessage { get; private set; }

        //predictions
        public bool PredFenetreOuverte { get; private set; }

        //valeurs actuelles
        public double ActualVibration { get; private set; }
        public double ActualTemperatureInt { get; private set; }
        public double ActualTemperatureExt { get; private set; }
        public double ActualHumidite { get; private set; }
        public double ActualTauxCo2 { get; private set; }
        public double ActualPresenceMouvement { get; private set; }
        public double ActualPresenceFumee { get; private set; }
        public double ActualLuminosite { get; private set; }
        public double ActualPresenceBruit { get; private set; }


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
            await LoadActualTempIntAsync(nomCapteur);
            await LoadActualTempExtAsync(nomCapteur);
            await LoadActualHumiditeAsync(nomCapteur);
            await LoadActualTauxCO2Async(nomCapteur);
            await LoadActualPresenceFumeeAsync(nomCapteur);
            await LoadActualPresenceBruitAsync(nomCapteur);
        }

        public async Task LoadActualVibrationAsync(string nomCapteur)
        {
            try
            {
                ActualVibration = await _vibrationsService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle de vibration mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualTempIntAsync(string nomCapteur)
        {
            try
            {
                ActualTemperatureInt = await _tempIntService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la température intérieure actuelle mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualTempExtAsync(string nomCapteur)
        {
            try
            {
                ActualTemperatureExt = await _tempExtService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la température extérieure actuelle mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualHumiditeAsync(string nomCapteur)
        {
            try
            {
                ActualHumidite = await _humiditeService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle du taux d'humidite mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualTauxCO2Async(string nomCapteur)
        {
            try
            {
                ActualTauxCo2 = await _co2Service.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle de taux de co2 mesuré par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualPresenceMouvementAsync(string nomCapteur)
        {
            try
            {
                ActualPresenceMouvement = await _mouvementService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la présence actuelle de mouvement mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualPresenceFumeeAsync(string nomCapteur)
        {
            try
            {
                ActualPresenceFumee = await _fumeeService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la présence actuelle de fumée mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualLuminositeAsync(string nomCapteur)
        {
            try
            {
                ActualLuminosite = await _luminositeService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle de luminosité mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        public async Task LoadActualPresenceBruitAsync(string nomCapteur)
        {
            try
            {
                ActualPresenceBruit = await _bruitService.GetTNowAsync(nomCapteur); //sortir la valeur
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la présence actuelle de bruit mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }
    }
}
