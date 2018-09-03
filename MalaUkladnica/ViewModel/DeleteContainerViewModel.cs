namespace MalaUkladnica.ViewModel
{
    using System.ComponentModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using MalaUkladnica.SAP;
    using MalaUkladnica.Utills;
    using static MalaUkladnica.ViewModel.LogViewModel;

    /// <summary>
    /// Klasa zawierająca logike DeleteContainerViewM
    /// </summary>
    public class DeleteContainerViewModel : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Wartość startowa pola, użyte jako " " aby nie uruchamiac walidatora
        /// </summary>
        private string _idContainer = " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteContainerViewModel"/> class.
        /// Konstruktor klasy, przypisuje komendą odpowiednie metody
        /// </summary>
        public DeleteContainerViewModel()
        {
            DeleteCommand = new RelayCommand<object>(param => DeleteButton(param));
        }

        public RelayCommand<object> DeleteCommand { get; set; }

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
        /// Klasa odpowiedzialna za wykonanie operacji na bazie SAP - usuniecia kontenera z bu
        /// </summary>
        /// <param name="x">Nie uzywane</param>
        private void DeleteButton(object x)
        {

            if (string.IsNullOrWhiteSpace(ContainerId))
            {
                Messenger.Default.Send(new LogMessage("Nie podano Id kontenera, przerwano operacje.",LogType.ERROR), "Log");
                return;
            }

            char FVI_NO_LAGP = ' '; // zmiana: 11/02/16

             var cd = new BackContainerBufforReturn();
            bool done = DriverSAP.Inst.BackContainerBuffor(MainViewModel._mode, MainViewModel._userName, MainViewModel._pernr, ContainerId, FVI_NO_LAGP, cd);
            if (cd.ReturnCode == 100)
            {
                Messenger.Default.Send(new LogMessage(string.Format("Wycofano kontener {0} z buforu stacji.", ContainerId), LogType.DONE), "Log");
            }
            else
            {
                Messenger.Default.Send(new LogMessage(string.Format("Wystąpił błąd przy wycofaniu kontenera {0} z buforu stacji, kod błedu: {1} - {2}", ContainerId, cd.ReturnCode, cd.Error), LogType.ERROR), "Log");
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
                error = Properties.Resources.ErrorMessage;//"Can not be empty!";
            }
            else
            {
                error = null;
            }

            return error;
        }
    }
}
