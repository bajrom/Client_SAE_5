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
        public string PredType { get; set; } = "";
        public string PredResultString = "";
        public List<string> PredResultMultipleStrings = new List<string>();
        public bool PredFenetreOuverte { get; private set; }
        public string EtatNbPersonne { get; private set; }
        public string EtatInconfort {  get; private set; }
        public List<float> PredTemperature { get; private set; }

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

        //valeurs dans periode
        public DateTime GraphStartTime { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime GraphEndTime { get; set; } = DateTime.Now;
        public List<InfluxDataReturn> GraphData { get; private set; } = new List<InfluxDataReturn>();
        public string GraphDataType { get; set; }

        /// <summary>
        /// Méthode pour charger les nom des cpateurs de l'API de l'IA
        /// </summary>
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




        // load valeurs dans un intervale de temps
        /// <summary>
        /// Méthode pour load les bonnes données de l'API
        /// </summary>
        public async Task LoadGraphValuesAsync()
        {
            switch (GraphDataType)
            {
                case "Presence de vibrations":
                    await LoadInTimeInterval(SelectedCapteurName, _vibrationsService, GraphStartTime, GraphEndTime);
                    break;

                case "Temperature interieure":
                    await LoadInTimeInterval(SelectedCapteurName, _tempIntService, GraphStartTime, GraphEndTime);
                    break;

                case "Temperature exterieure":
                    await LoadInTimeInterval(SelectedCapteurName, _tempExtService, GraphStartTime, GraphEndTime);
                    break;

                case "Taux d'humidite":
                    await LoadInTimeInterval(SelectedCapteurName, _humiditeService, GraphStartTime, GraphEndTime);
                    break;

                case "Taux de co2":
                    await LoadInTimeInterval(SelectedCapteurName, _co2Service, GraphStartTime, GraphEndTime);
                    break;

                case "Presence de mouvement":
                    await LoadInTimeInterval(SelectedCapteurName, _mouvementService, GraphStartTime, GraphEndTime);
                    break;

                case "Presence de fumee":
                    await LoadInTimeInterval(SelectedCapteurName, _fumeeService, GraphStartTime, GraphEndTime);
                    break;

                case "Taux de luminosite":
                    await LoadInTimeInterval(SelectedCapteurName, _luminositeService, GraphStartTime, GraphEndTime);
                    break;

                case "Presence de bruit":
                    await LoadInTimeInterval(SelectedCapteurName, _bruitService, GraphStartTime, GraphEndTime);
                    break;
            }
        }

        /// <summary>
        /// Get les valeurs de l'API dans un intervalle du type sélectionner du capteur sélectionner pour les mettres dans un graph visuel
        /// </summary>
        /// <param name="nomCapteur"></param>
        /// <param name="service"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public async Task LoadInTimeInterval(string nomCapteur, InfluxDataService service, DateTime start, DateTime end)
        {
            try
            {
                GraphData = await service.GetTInTimeIntervalAsync(nomCapteur, start, end);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement des valeurs pour le graphique du capteur {nomCapteur} : {ex.Message}";
            }
        }




        /// <summary>
        /// Charge la prédiction sélectionnée
        /// </summary>
        public async Task PredictData()
        {
            switch (PredType)
            {
                case "fenetre":
                    await LoadPredFenetreOuverteAsync();
                    PredResultString = PredFenetreOuverte ? "La fenêtre est ouverte" : "La fenêtre est fermée";
                    break;

                case "nbPersonnes":
                    await LoadPredNbPersonnesAsync();
                    PredResultString = "Il y a " + EtatNbPersonne +" personnes";
                    break;

                case "inconfort":
                    await LoadPredInconfortAsync();
                    PredResultString = "Inconfort : " + EtatInconfort;
                    break;

                case "temp":
                    await LoadPredTemperatureAsync();
                    PredResultString = "Temperature des 10 prochaines heures :";
                    foreach (float temp in PredTemperature)
                    {
                        PredResultMultipleStrings.Add($"{temp}°C");
                    }
                    break;
            }
        }

        /// <summary>
        /// Reset la data des prédictions
        /// </summary>
        public async Task RefreshPredData()
        {
            PredResultString = "";
            PredResultMultipleStrings = new List<string>();
        }

        /// <summary>
        /// Méthode pour get les prédictions de type fenêtre ouverte
        /// </summary>
        public async Task LoadPredFenetreOuverteAsync()
        {
            try
            {
                PredFenetreOuverte = await _predictionService.GetFenetreOuvertePredAsync(SelectedCapteurName);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la prediction de fenetre ouverte du capteur {SelectedCapteurName} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get les prédictions de type nombres de personnes
        /// </summary>
        public async Task LoadPredNbPersonnesAsync()
        {
            try
            {
                EtatNbPersonne = await _predictionService.GetNbPersonnePredAsync(SelectedCapteurName);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la prediction du nombre de personne du capteur {SelectedCapteurName} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get les prédictions de type inconfort
        /// </summary>
        public async Task LoadPredInconfortAsync()
        {
            try
            {
                EtatInconfort = await _predictionService.GetInconfortPredAsync(SelectedCapteurName);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la prediction d'inconfort du capteur {SelectedCapteurName} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get les prédictions de type température
        /// </summary>
        public async Task LoadPredTemperatureAsync()
        {
            try
            {
                PredTemperature = await _predictionService.GetTemperaturesPredAsync(SelectedCapteurName);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la prediction de temperature du capteur {SelectedCapteurName} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode déclencher dès la sélection d'une salle pour afficher toutes ses données actuelles
        /// </summary>
        /// <param name="nomCapteur"></param>
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

        /// <summary>
        /// Méthode pour get la donnée de vibration actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualVibrationAsync(string nomCapteur)
        {
            try
            {
                ActualVibration = await _vibrationsService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle de vibration mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée de température intérieure actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualTempIntAsync(string nomCapteur)
        {
            try
            {
                ActualTemperatureInt = await _tempIntService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la température intérieure actuelle mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée de température extérieure actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualTempExtAsync(string nomCapteur)
        {
            try
            {
                ActualTemperatureExt = await _tempExtService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la température extérieure actuelle mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée d'humiditée actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualHumiditeAsync(string nomCapteur)
        {
            try
            {
                ActualHumidite = await _humiditeService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle du taux d'humidite mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée de taux de CO2 actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualTauxCO2Async(string nomCapteur)
        {
            try
            {
                ActualTauxCo2 = await _co2Service.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle de taux de co2 mesuré par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée de mouvements actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualPresenceMouvementAsync(string nomCapteur)
        {
            try
            {
                ActualPresenceMouvement = await _mouvementService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la présence actuelle de mouvement mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée de présence de fumée actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualPresenceFumeeAsync(string nomCapteur)
        {
            try
            {
                ActualPresenceFumee = await _fumeeService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la présence actuelle de fumée mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée de luminositée actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualLuminositeAsync(string nomCapteur)
        {
            try
            {
                ActualLuminosite = await _luminositeService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la valeur actuelle de luminosité mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }

        /// <summary>
        /// Méthode pour get la donnée de bruits actuel de la salle sélectionnée
        /// </summary>
        public async Task LoadActualPresenceBruitAsync(string nomCapteur)
        {
            try
            {
                ActualPresenceBruit = await _bruitService.GetTNowAsync(nomCapteur); 
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erreur lors du chargement de la présence actuelle de bruit mesurée par le capteur {nomCapteur} : {ex.Message}";
            }
        }
    }
}
