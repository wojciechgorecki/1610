namespace SmallStacker.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using SmallStacker.Model;
    using SmallStacker.Utills;
    using SmallStacker.Utills.DatabaseUtill;

    /// <summary>
    /// Klasa zawierająca logike <see cref="MalaUkladnica.View.GetHistoryView"/>
    /// </summary>
    public class GetHistoryViewModel : ViewModelBase
    {
        /// <summary>
        /// zmienna zawierająca numer kontenera z pola numer 1
        /// </summary>
        private string _idContainer1;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetHistoryViewModel"/> class.
        /// konstruktor klasy, inicjalizuje date oraz przypisuje metody do komend.
        /// </summary>
        public GetHistoryViewModel()
        {
            DateTime = DateTime.Now;
            DateTo = DateTime.Now;
            
            GetHistoryCommand = new RelayCommand<object>(x => GetHistoryButton(x));
            _historyList = new ObservableCollection<LOGI_MALAUKLADNICA_ACTION>();
        }

        /// <summary>
        /// Komenda zbindowana do buttona, wywołuje metode <see cref="GetHistoryButton(object)"/>
        /// </summary>
        public RelayCommand<object> GetHistoryCommand { get; set; }

        /// <summary>
        /// Obserwowalna kolekcja zawierająca liste akcjii z bazy danych.
        /// </summary>
        private ObservableCollection<LOGI_MALAUKLADNICA_ACTION> _historyList;

        /// <summary>
        /// Gets or sets Properties zwracajacy i pobierajacy <see cref="_historyList"/>
        /// zmianna powoduje odswiezenie kolekcji.
        /// </summary>
        public ObservableCollection<LOGI_MALAUKLADNICA_ACTION> HistoryList
        {
            get
            {
                return _historyList;
            }

            set
            {
                _historyList = value;
                RaisePropertyChanged("HistoryList");
            }
        }

        /// <summary>
        /// Gets or sets Properties zwracajacy i pobierający <see cref="_idContainer1"/>
        /// </summary>
        public string ContainerId1
        {
            get
            {
                return _idContainer1;
            }

            set
            {
                _idContainer1 = value;
                RaisePropertyChanged("ContainerId1");
            }
        }

        /// <summary>
        /// zmienna zawierająca numer kontenera z pola numer 2
        /// </summary>
        private string _idContainer2;

        /// <summary>
        /// Gets or sets Properties zwracajacy i pobierający <see cref="_idContainer2"/>
        /// </summary>
        public string ContainerId2
        {
            get
            {
                return _idContainer2;
            }

            set
            {
                _idContainer2 = value;
                RaisePropertyChanged("ContainerId2");
            }
        }

        /// <summary>
        /// zmienna typu <see cref="System.DateTime"/> zawierajaca date z godziną od kiedy bedzie wyszukiwane.
        /// </summary>
        private DateTime _dataTime;

        /// <summary>
        /// Gets or sets Properties zwracajacy i pobierający <see cref="_dataTime"/>
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                return _dataTime;
            }

            set
            {
                _dataTime = value;
                RaisePropertyChanged("DateTime");
            }
        }

        /// <summary>
        /// zmienna typu <see cref="System.DateTime"/> zawierajaca date z godziną do kiedy bedzie wyszukiwane
        /// </summary>
        private DateTime _dateTo;

        /// <summary>
        /// Gets or sets Properties zwracajacy i pobierający <see cref="_dateTo"/>
        /// </summary>
        public DateTime DateTo
        {
            get
            {
                return _dateTo;
            }

            set
            {
                _dateTo = value;
                RaisePropertyChanged("DateTo");
            }
        }

        /// <summary>
        /// Metoda przypisana do komendy <see cref="GetHistoryCommand"/>. Pobiera historie kontenerow pomiedzy podanymi datami
        /// </summary>
        /// <param name="x">Nie uzywane</param>
        private void GetHistoryButton(object x)
        {

            List<string> contNumbers = new List<string> { ContainerId1, ContainerId2 };
            HistoryList = new ObservableCollection<LOGI_MALAUKLADNICA_ACTION>(DatabaseController.GetActions(ContainerId1, contNumbers, DateTime, DateTo));

            Messenger.Default.Send(new LogMessage("[" + DateTime.Now + "] -> Pobrano historie", LogViewModel.LogType.ERROR), "Log");
        }
    }
}