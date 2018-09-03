namespace MalaUkladnica.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using MalaUkladnica.Resources.Langue;

    /// <summary>
    /// Klasa odpowiedzialna za logike ButtonsView, zmiane języka aplikacjii
    /// </summary>
    public class ButtonsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonsViewModel"/> class.
        /// Konstruktor klasy, przypisuje komendą odpowiednie metody
        /// </summary>
        public ButtonsViewModel()
        {
            uaButtonCommand = new RelayCommand<object>(param => uaButton(param));
            plButtonCommand = new RelayCommand<object>(param => plButton(param));
            ukButtonCommand = new RelayCommand<object>(param => ukButton(param));
        }

        /// <summary>
        /// Gets or sets Komenda odpowiedajaca za zmiane jezyka na Ukrainski
        /// </summary>
        /// <value>
        /// Komenda odpowiedajaca za zmiane jezyka na Ukrainski
        /// </value>
        public RelayCommand<object> uaButtonCommand { get; set; }

        /// <summary>
        /// Gets or sets Komenda odpowiedajaca za zmiane jezyka na Polski
        /// </summary>
        /// <value>
        /// Komenda odpowiedajaca za zmiane jezyka na Polski
        /// </value>
        public RelayCommand<object> plButtonCommand { get; set; }

        /// <summary>
        /// Gets or sets Komenda odpowiedajaca za zmiane jezyka na Angielski
        /// </summary>
        /// <value>
        /// Komenda odpowiedajaca za zmiane jezyka na Angielski
        /// </value>
        public RelayCommand<object> ukButtonCommand { get; set; }

        /// <summary>
        /// Metoda odpowiadająca za zmiane języka na Ukrainski
        /// </summary>
        /// <param name="param">Nie uzywany</param>
        private void uaButton(object param)
        {
            Properties.Settings.Default.Language = "ua-UA";
            CultureResources.ChangeCulture(new System.Globalization.CultureInfo(Properties.Settings.Default.Language));
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Metoda odpowiadająca za zmiane języka na Polski
        /// </summary>
        /// <param name="param">Nie uzywany</param>
        private void plButton(object param)
        {
            Properties.Settings.Default.Language = "pl-PL";
            CultureResources.ChangeCulture(new System.Globalization.CultureInfo(Properties.Settings.Default.Language));
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Metoda odpowiadająca za zmiane języka na Angielski
        /// </summary>
        /// <param name="param">Nie uzywany</param>
        private void ukButton(object param)
        {
            Properties.Settings.Default.Language = "en-US";
            CultureResources.ChangeCulture(new System.Globalization.CultureInfo(Properties.Settings.Default.Language));
            Properties.Settings.Default.Save();
        }
    }
}
