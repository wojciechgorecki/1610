namespace SmallStacker.Utills
{
    using SmallStacker.Model;
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    /// <summary>
    /// Klasa zawierająca konwenter tłumaczący akcje na wiadomosc textowa
    /// </summary>
    public class ActionListConventer : IValueConverter
    {
        /// <summary>
        /// Metoda sprawdzająca typ akcji i tlumaczaca go.
        /// </summary>
        /// <param name="value">Typ akcji</param>
        /// <param name="targetType">Rodzaj zmiennej typu akcji</param>
        /// <param name="parameter">Parametr -  nie uzywany</param>
        /// <param name="culture">Jezyk - nie uzywany</param>
        /// <returns>Zwraca tlumaczenie typu akcji</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch (value.ToString().Substring(0,4))
            {
                case "Info":
                    return Properties.Resources.ContainerInformation;
                case "Pobr":
                    string test = Find(value.ToString(), "SR");
                    if (test == null)
                        test = Find(value.ToString(), "EB");
                    else if(test == null)
                        test = "";
                    test = " " + test;
                    return String.Concat( Properties.Resources.GetContainer,test);
                case "Wysł":
                    return Properties.Resources.SendContainer;
                case "Usuw":
                    return Properties.Resources.GetContainerFromBufferStation;
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

        private string Find(string string1,string toFind1)
        {
            int start = string1.IndexOf(toFind1);
            if (start < 0) return null;
            return string1.Substring(start, string1.Length - (start + 4));

        }
  
    }
}
