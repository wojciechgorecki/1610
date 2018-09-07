namespace SmallStacker.Utills
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Klasa zawierająca konwenter tłumaczący kod akcje na odpowiedni komunikat
    /// </summary>
    public class MessageListConventer : IValueConverter
    {
        /// <summary>
        /// Metoda sprawdzająca kod akcji i zwraca odpowiedni komunikat.
        /// </summary>
        /// <param name="value">Kod akcji</param>
        /// <param name="targetType">Rodzaj zmiennej typu akcji</param>
        /// <param name="parameter">Parametr -  nie uzywany</param>
        /// <param name="culture">Jezyk - nie uzywany</param>
        /// <returns>Zwraca komunikat zalezny od kodu akcji</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string temp = value.ToString();
            int code = Int32.Parse(temp.Substring(value.ToString().Length - 3));
            switch(code)
                {
                case 100:
                    return Properties.Resources.ContainerActionComplete; // "Akcja wykonana";
                case 101:
                    return Properties.Resources.ContainerDontExist; // "Kontener nie istnieje";
                case 102:
                    return Properties.Resources.ContainerEmptyLocalizationInSAP; // "Pusta lokalizacja w SAP";
                case 103:
                    return Properties.Resources.ContainerSpendOrder; // "Nie należy przyjmować -> istnieje zlecenie wydania";
                case 333:
                    return Properties.Resources.ServerComunnicationError; // "Błąd komunikacji z serwerem układnicy";
                case 666:
                    return Properties.Resources.NoPermission; // "Brak uprawnień";
                default:
                    return Properties.Resources.UnknownMessage;
            }
        }

        /// <summary>
        /// Metoda nie uzywana
        /// </summary>
        /// <param name="value">Typ akcji</param>
        /// <param name="targetType">Rodzaj zmiennej typu akcji</param>
        /// <param name="parameter">Parametr -  nie uzywany</param>
        /// <param name="culture">Jezyk - nie uzywany</param>
        /// <returns>Nic nie zwraca</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
