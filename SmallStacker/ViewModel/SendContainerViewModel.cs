namespace SmallStacker.ViewModel
{
    using System;
    using System.ComponentModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using SmallStacker.SAP;
    using SmallStacker.SAP.Returns;
    using SmallStacker.Utills;
    using static SmallStacker.ViewModel.LogViewModel;

    /// <summary>
    /// Klasa zawierająca logike SendContainerView
    /// </summary>
    public class SendContainerViewModel : ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendContainerViewModel"/> class.
        /// Konstruktor klasy, przypisuje komendą odpowiednie metody
        /// </summary>
        public SendContainerViewModel()
        {
            SendContainerCommand = new RelayCommand<object>(x => SendContainer(x));
            ContainerIsEmptyCommand = new RelayCommand(setFVI_Empty);
            DontCheckCointainerInSAPCommand = new RelayCommand(setFVI_NO_LAGP);
            ContainerRadioButtonPointer = new RelayCommand(setFVI_ABC);
            isCheckedC = true;
        }

        /// <summary>
        /// Gets or sets Komenda wysyłająca kontener na układnice.
        /// </summary>
        /// <value>
        /// Komenda wysyłająca kontener na układnice.
        /// </value>
        public RelayCommand<object> SendContainerCommand { get; set; }

        /// <summary>
        /// Gets or sets Komenda ustawiąjąca zmienna <see cref="FVI_EMPTY"/> zaleznie od <see cref="isCheckedFVI_Empty"/>
        /// </summary>
        /// <value>
        /// Komenda ustawiąjąca zmienna <see cref="FVI_EMPTY"/> zaleznie od <see cref="isCheckedFVI_Empty"/>
        /// </value>
        public RelayCommand ContainerIsEmptyCommand { get; set; }

        /// <summary>
        /// Gets or sets Komenda ustawiąjąca zmienna <see cref="FVI_NO_LAGP"/> zaleznie od <see cref="isCheckedFVI_NO_LAGP"/>
        /// </summary>
        /// <value>
        /// Komenda ustawiąjąca zmienna <see cref="FVI_NO_LAGP"/> zaleznie od <see cref="isCheckedFVI_NO_LAGP"/>
        /// </value>
        public RelayCommand DontCheckCointainerInSAPCommand { get; set; }

        /// <summary>
        /// Gets or sets Komenda ustawiąjąca zmienną <see cref="FVI_ABC"/>   zaleznie od <see cref="isCheckedA"/> i <see cref="isCheckedB"/> i <see cref="isCheckedC"/>
        /// </summary>
        /// <value>
        /// Komenda ustawiąjąca zmienną <see cref="FVI_ABC"/>   zaleznie od <see cref="isCheckedA"/> i <see cref="isCheckedB"/> i <see cref="isCheckedC"/>
        /// </value>
        public RelayCommand ContainerRadioButtonPointer { get; set; }

        /// <summary>
        /// Wartość startowa pola, użyte jako " " aby nie uruchamiac walidatora
        /// </summary>
        private string _idContainer = " ";
        private string isVisibility = "Hidden";

        public string IsVisibility
        {
            get
            {
                return isVisibility;
            }
            set
            {
                isVisibility = value;
                RaisePropertyChanged("IsVisibility");
            }
        }

        /// <summary>
        /// Gets or sets Wartosc logiczna checkBoxa o nazwie "Kontener jest pusty"
        /// </summary>
        /// <value>
        /// Wartosc logiczna checkBoxa o nazwie "Kontener jest pusty"
        /// </value>
        public bool isCheckedFVI_Empty { get; set; }

        /// <summary>
        /// Gets or sets Wartosc logiczna checkBoxa o nazwie "Nie sprawdzaj kontenera w bazie SAP"
        /// </summary>
        /// <value>
        /// Wartosc logiczna checkBoxa o nazwie "Nie sprawdzaj kontenera w bazie SAP"
        /// </value>
        public bool IsCheckedFVI_NO_LAGP
        {
            get
            {
                return isCheckedFVI_NO_LAGP;
            }

            set
            {
                isCheckedFVI_NO_LAGP = value;
                RaisePropertyChanged("isCheckedFVI_NO_LAGP");
            }
        }

        /// <summary>
        /// Gets or sets Wartosc logiczna radioButtona o nazwie "A"
        /// </summary>
        /// <value>
        /// Wartosc logiczna radioButtona o nazwie "A"
        /// </value>
        public bool isCheckedA { get; set; }

        /// <summary>
        /// Gets or sets Wartosc logiczna radioButtona o nazwie "B"
        /// </summary>
        /// <value>
        /// Wartosc logiczna radioButtona o nazwie "B"
        /// </value>
        public bool isCheckedB { get; set; }

        /// <summary>
        /// Gets or sets Wartosc logiczna radioButtona o nazwie "C"
        /// </summary>
        /// <value>
        /// Wartosc logiczna radioButtona o nazwie "C"
        /// </value>
        public bool isCheckedC { get; set; }

        /// <summary>
        /// X - Kontener jest pusty, '  ' - Konter z zawartoscia
        /// </summary>
        private char FVI_EMPTY = ' ';

        /// <summary>
        /// X - Nie sprawdza kontenera w bazie SAP, '  ' - Sprawdza kontener w bazie SAP
        /// </summary>
        private char FVI_NO_LAGP = ' ';

        /// <summary>
        /// Mozliwe wartosci wskaznika: A, B, C , ' '
        /// Przy czym jesli zostaiwmy pole puste (' ') przyjmuje wartosc C
        /// </summary>
        private char FVI_ABC = ' ';

        private bool isCheckedFVI_NO_LAGP = false;

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
        /// Metoda zmieniajaca wartosc <see cref="FVI_EMPTY"/> zaleznie od wartosci <see cref="isCheckedFVI_Empty"/>
        /// </summary>
        private void setFVI_Empty()
        {
            if (isCheckedFVI_Empty)
            {
                FVI_EMPTY = 'X';
            }
            else
            {
                FVI_EMPTY = ' ';
            }
        }

        /// <summary>
        /// Metoda zmieniajaca wartosc <see cref="FVI_NO_LAGP"/> zaleznie od wartosci <see cref="isCheckedFVI_NO_LAGP"/>
        /// </summary>
        private void setFVI_NO_LAGP()
        {
            if (isCheckedFVI_NO_LAGP)
            {
                FVI_NO_LAGP = 'X';
                IsVisibility = "Visible";
            }
            else
            {
                FVI_NO_LAGP = ' ';
                IsVisibility = "Hidden";
            }
        }

        /// <summary>
        /// Metoda zmieniajaca wartosc <see cref="FVI_ABC"/> zaleznie od  <see cref="isCheckedA"/> i  <see cref="isCheckedB"/> i  <see cref="isCheckedC"/>
        /// </summary>
        private void setFVI_ABC()
        {

            if (isCheckedA)
            {
                FVI_ABC = 'A';
            }
            else if (isCheckedB)
            {
                FVI_ABC = 'B';
            }
            else if (isCheckedC)
            {
                FVI_ABC = 'C';
            }
            else
            {
                FVI_ABC = ' ';
            }
        }

        /// <summary>
        /// Klasa odpowiedzialna za wykonanie operacji na bazie SAP - wyslanie kontenera na ukladnice
        /// </summary>
        /// <param name="x">Nie uzywane</param>
        private void SendContainer(object x)
        {
            if (string.IsNullOrWhiteSpace(ContainerId))
            {
                Messenger.Default.Send(new LogMessage("Nie podano Id kontenera, przerwano operacje.", LogType.ERROR), "Log");
                return;
            }

            var sc = new SendingContainerReturn();
            bool done = DriverSAP.Inst.SendingContainer(
                Environment.UserName,
                ContainerId,
                FVI_EMPTY,
                FVI_NO_LAGP,
                FVI_ABC,
                sc);

            if (done)
            {
                Messenger.Default.Send(new LogMessage("Wysłano kontener " + ContainerId, LogType.DONE), "Log");
            }
            else
            {
                Messenger.Default.Send(new LogMessage(string.Format("Wystąpił błąd przy wysyłaniu kontenera {0}, kod błedu: {1} - {2}", ContainerId, sc.ReturnCode, sc.Error), LogType.ERROR), "Log");
                return;
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
