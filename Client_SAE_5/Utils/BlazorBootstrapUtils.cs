using BlazorBootstrap;

namespace Client_SAE_5.Utils
{
    public sealed class BlazorBootstrapUtils
    {
        // Instance unique pour le singleton
        private static readonly Lazy<BlazorBootstrapUtils> _instance =
            new Lazy<BlazorBootstrapUtils>(() => new BlazorBootstrapUtils());

        // Propriété pour accéder à l'instance
        public static BlazorBootstrapUtils Instance => _instance.Value;

        // Constructeur privé pour empêcher l'instanciation externe
        private BlazorBootstrapUtils() { }

        /// <summary>
        /// Retourne les traductions nécessaires pour une GRID Bootstrap Blazor
        /// </summary>
        /// <returns>Liste de toutes les traductions</returns>
        public async Task<IEnumerable<FilterOperatorInfo>> GridFiltersTranslationProvider()
        {
            var filtersTranslation = new List<FilterOperatorInfo>
            {
                // number/date/boolean
                new FilterOperatorInfo("=", "Est égal", FilterOperator.Equals),
                new FilterOperatorInfo("!=", "N'est pas égal", FilterOperator.NotEquals),
                // number/date
                new FilterOperatorInfo("<", "Est inférieur", FilterOperator.LessThan),
                new FilterOperatorInfo("<=", "Est inférieur ou égal", FilterOperator.LessThanOrEquals),
                new FilterOperatorInfo(">", "Est supérieur", FilterOperator.GreaterThan),
                new FilterOperatorInfo(">=", "Est supérieur ou égal", FilterOperator.GreaterThanOrEquals),
                // string
                new FilterOperatorInfo("*a*", "Contient", FilterOperator.Contains),
                new FilterOperatorInfo("!*a*", "Ne contient pas", FilterOperator.DoesNotContain),
                new FilterOperatorInfo("a**", "Commence avec", FilterOperator.StartsWith),
                new FilterOperatorInfo("**a", "Se termine avec", FilterOperator.EndsWith),
                new FilterOperatorInfo("=", "Est égal", FilterOperator.Equals),
                // common
                new FilterOperatorInfo("x", "Réinitialiser", FilterOperator.Clear)
            };

            return await Task.FromResult(filtersTranslation);
        }
    }
}
