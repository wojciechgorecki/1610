namespace MalaUkladnica.Resources.Langue
{
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Klasa odpowiedzialna za obsługe ObjectDataProvider, oraz obsługe  lokalizacji i zasób z językami
    /// </summary>
    public class CultureResources
    {
        private static ObjectDataProvider _provider;

        /// <summary>
        /// Zmienia język na podany jako parametr i odswieża widok.
        /// </summary>
        /// <param name="culture">Język który chcemy ustawić jako aktualny</param>
        public static void ChangeCulture(CultureInfo culture)
        {
            Properties.Resources.Culture = culture;
            GetResourceProvider().Refresh();
        }

        /// <summary>
        /// Gets wszystkie dostępne zasoby
        /// </summary>
        /// <returns>Zasób jako ObjectDataProvider</returns>
        public static ObjectDataProvider GetResourceProvider()
        {
            if (_provider == null)
            {
                _provider = (ObjectDataProvider)App.Current.FindResource("Resources");
            }

            return _provider;
        }

        /// <summary>
        /// Tworzy nowy zasób językowi i go zwraca
        /// </summary>
        /// <returns>Nową instancje Properties.Resources()</returns>
        public Properties.Resources GetResourceInstance()
        {
            return new Properties.Resources();
        }
    }
}