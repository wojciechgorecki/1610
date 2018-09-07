namespace SmallStacker.ViewModel
{
    using System;
    using System.ComponentModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using SmallStacker.SAP;
    using SmallStacker.Utills;
    using static SmallStacker.ViewModel.LogViewModel;
    using static SmallStacker.ViewModel.MainViewModel;

    /// <summary>
    /// Klasa zawierająca logike GetContainerInfoViewM
    /// </summary>
    public class GetContainerInfoViewModel : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Wartość startowa pola, użyte jako " " aby nie uruchamiac walidatora
        /// </summary>
        private string _idContainer = " ";

        private string localization = "Lokalizacja: ";
        /// <summary>
        /// to-do
        /// </summary>
        public RelayCommand<object> GetInfoCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContainerInfoViewModel"/> class.
        /// Konstruktor klasy przypisuje metody do komend.
        /// </summary>
        public GetContainerInfoViewModel()
        {
            GetInfoCommand = new RelayCommand<object>(x => GetInfoButton(x));
        }

        public string Localization
        {
            get
            {
                return localization;
            }

            set
            {
                localization = value;
                RaisePropertyChanged("Localization");
            }
        }

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
        /// Klasa odpowiedzialna za wykonanie operacji na bazie SAP - Pobranie informacji o kontenerze
        /// </summary>
        /// <param name="x">Nie uzywane</param>
        private void GetInfoButton(object x)
        {

            if (string.IsNullOrWhiteSpace(ContainerId))
            {
                Messenger.Default.Send(new LogMessage("Nie podano Id kontenera, przerwano operacje.", LogType.ERROR), "Log");
                return;
            }

        var ci = new ContainerInfoReturn();
        bool done = DriverSAP.Inst.ContainerInfo( Environment.UserName,_idContainer, _FVI_NO_LAGP, ci);
            if (done)
            {
                Messenger.Default.Send(new LogMessage("[" + DateTime.Now + "]->Pobrano dane o kontenerze:" + _idContainer, LogType.DONE), "Log");
                Messenger.Default.Send(new LogMessage("Lokalizacja: " + ci.FVO_LOC, LogType.DONE), "Log");
                Localization = "Lokalizacja: " + ci.FVO_LOC;
            }
            else
            {
                Messenger.Default.Send(new LogMessage(string.Format("Wystąpił błąd przy pobieraniu informacji o kontenerze {0}, kod błedu: {1} - {2}", _idContainer, ci.ReturnCode, ci.Error), LogType.ERROR), "Log");
                if (ci.FVO_ER != "0")
                {
                    Messenger.Default.Send(new LogMessage(string.Format("- wykod błędu otrzymany Z MFCS: {0}", ci.FVO_ER), LogType.ERROR), "Log");
                    Messenger.Default.Send(new LogMessage("Lokalizacja: -----------------------", LogType.ERROR), "Log");
                    
                }

                Localization = "Lokalizacja: ";
            }
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
