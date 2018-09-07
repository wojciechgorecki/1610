namespace SmallStacker.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using SmallStacker.SAP;
    using SmallStacker.Utills;
    using static SmallStacker.ViewModel.LogViewModel;

    /// <summary>
    /// Klasa zawierająca logike GetContainerView
    /// </summary>
    public class GetContainerViewModel : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Przechowuje komunikat otrzymany z metody <see cref="DriverSAP.Z_MFCS_SEND_HT(string, string, string, string, char, string, char, int, out string)"/>
        /// </summary>
        private string error;

        /// <summary>
        /// Przechowuje priorytet 0 lub 9
        /// </summary>
        private int priority = 0;

        /// <summary>
        /// Wartość startowa pola, użyte jako " " aby nie uruchamiac walidatora
        /// </summary>
        private string _idContainer = " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContainerViewModel"/> class.
        /// Konstruktor klasy, dodaje wartosci do <see cref="DriverTarget"/> oraz przypisuje metody do komend.
        /// </summary>
        public GetContainerViewModel()
        {
            DriverTarget = new ObservableCollection<string>();
            DriverTarget.Add("SR00");
            DriverTarget.Add("SR02");
            DriverTarget.Add("SR06");
            DriverTarget.Add("SR08");
            DriverTarget.Add("EB01");
            DriverTarget.Add("SR14");

            SelectedValue = DriverTarget[0];

            GetCommand = new RelayCommand<object>(x => GetContainerButton(x));
            PriorityCommand = new RelayCommand(SetPriority);

        }

        /// <summary>
        /// Gets or sets Komenda obsługująca przycisk, wywułująca metode SAP:  <see cref="DriverSAP.Z_MFCS_SEND_HT(string, string, string, string, char, string, char, int, out string)"/>
        /// - Pobiera kontener
        /// </summary>
        /// <value>
        /// Komenda obsługująca przycisk
        /// </value>
        public RelayCommand<object> GetCommand { get; set; }

        /// <summary>
        /// Gets or sets Komenda obsługująca CheckBox - priorytet
        /// </summary>
        /// <value>
        /// Komenda obsługująca CheckBox - priorytet
        /// </value>
        public RelayCommand PriorityCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Zmienna pomocnicza przochowująca wartość logiczną checkBoxa
        /// </summary>
        /// <value>
        /// Zmienna pomocnicza przochowująca wartość logiczną checkBoxa
        /// </value>
        public bool IsCheckedPriority { get; set; }

        /// <summary>
        /// Gets or sets Zmienna pomocnicza przochowująca wybraną wartość z pola Controls:SplitButton
        /// </summary>
        /// <value>
        ///  Zmienna pomocnicza przochowująca wybraną wartość z pola Controls:SplitButton
        /// </value>
        public string SelectedValue { get; set; }

        /// <summary>
        /// Gets or sets Propertis celów dla sterownika, zbindowany z SplitButton
        /// </summary>
        /// <value>
        ///  Propertis celów dla sterownika, zbindowany z SplitButton
        /// </value>
        public ObservableCollection<string> DriverTarget { get; set; }

        /// <summary>
        /// Gets or sets Propertis ID kontenera
        /// </summary>
        /// <value>
        ///  Propertis ID kontenera
        /// </value>
        public string ContainerId
        {
            get
            {
                return _idContainer;
            }

            set
            {
                _idContainer = value;
            }
        }

        /// <summary>
        /// Gets or sets Propertis priorytetu 0  lub 9
        /// </summary>
        /// <value>
        ///  Propertis ID kontenera
        /// </value>
        private void SetPriority()
        {
            if (IsCheckedPriority)
            {
                priority = 9;
            }
            else if (!IsCheckedPriority)
            {
                priority = 0;
            }
        }

        /// <summary>
        /// Metoda podczepiona do komendy <see cref="GetCommand"/>
        /// Wykonuje akcje SAP: <see cref="DriverSAP.Z_MFCS_SEND_HT(string, string, string, string, char, string, char, int, out string)"/>
        /// i zapisuje logi do bazy danych
        /// </summary>
        /// <param name="x">nie uzywane</param>
        private void GetContainerButton(object x)
        {
            if (string.IsNullOrWhiteSpace(ContainerId))
            {
                Messenger.Default.Send(new LogMessage("Nie podano Id kontenera, przerwano operacje.", LogType.ERROR), "Log");
                return;
            }

            int _return = DriverSAP.Inst.Z_MFCS_SEND_HT(
                   Environment.UserName,
                   ContainerId,
                   ' ',
                   SelectedValue,
                   ' ',
                   priority,
                   out error);
            Messenger.Default.Send(new LogMessage("[" + DateTime.Now + "]-> " + error + " ", LogType.ERROR), "Log");
        }

        /// <summary>
        /// Gets Error, domylnie error jest pusty
        /// </summary>
        public string Error
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Wykonuje walidacje
        /// </summary>
        /// <param name="columnName">Nazwa kolumny</param>
        /// <returns>zwaraca zawartosc erroru</returns>
        public string this[string columnName]
        {
            get
            {
                return Validate();
            }
        }

        /// <summary>
        /// validacja pol, sprawdza czy nie jest puste, jesli jest -> dodaje log z errorem
        /// </summary>
        /// <returns>Zwraca <see cref="Error"/></returns>
        private string Validate()
        {
            string error = null;

            if (string.IsNullOrEmpty(ContainerId))
            {
                error = error = Properties.Resources.ErrorMessage; //"Can not be empty!";
            }
            else
            {
                error = null;
            }

            return error;
        }
    }
}
