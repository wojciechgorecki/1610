namespace SmallStacker.ViewModel
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using System.Windows.Media;
    using GalaSoft.MvvmLight;
    using SmallStacker.Utills;
    using NLog;

    /// <summary>
    /// Klasa odpowiedzialna za wyswietlanie logow
    /// </summary>
    public class LogViewModel : ViewModelBase
    {
        /// <summary>
        /// statyczny obiekt typu Logger, dzięki niemu mozemy odwolywac się do reguł któe znajdują sie w NLog.config
        /// </summary>
        public static Logger logger;


        private int selectedIndex = 0;

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }

            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        /// <summary>
        /// statyczna obserwowalna kolekcja typu string
        /// </summary>
        ObservableCollection<string> _lista = new ObservableCollection<string>();

        /// <summary>
        /// Zmienna zawierająca kolor logów.
        /// </summary>
        public SolidColorBrush _foregroundColor;
            LogMessage lo;
        
        public LogViewModel()
        {
            logger = LogManager.GetCurrentClassLogger();
            this.MessengerInstance.Register<LogMessage>(this, "Log", msg => AddLog(msg));

        }

        /// <summary>
        /// Typ wyliczeniowy zawierający typ logow - dla odpowiednich kolorów
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// Log defaultowy - kolor czerwony
            /// </summary>
            DEFAULT,

            /// <summary>
            /// Log informacyjny - kolor czarny
            /// </summary>
            INFO,

            /// <summary>
            /// Log błędu - kolor czerwony
            /// </summary>
            ERROR,

            /// <summary>
            /// Log błędu SAp - kolor czerwony
            /// </summary>
            ERROR_SAP,

            /// <summary>
            /// Log potwierdzenia - kolor zielony
            /// </summary>
            DONE,

            /// <summary>
            /// Log bazy danych - brak koloru, zapisywany odrazu do pliku
            /// </summary>
            DATABASELOG
        }

        /// <summary>
        ///  Gets or sets obserwowalną kolekcje z logami w przypadku dodania nowego elementu, odswieża binding do listy
        /// </summary>
        /// <value>
        ///  Kolekcja typu string
        /// </value>
        public ObservableCollection<string> LogList
        {
            get
            {
                return _lista;
            }

            set
            {
                _lista = value;
                this.RaisePropertyChanged("LogList");
            }
        }

        public SolidColorBrush myForeground
        {
            get
            {
                return _foregroundColor;
            }

            set
            {
                _foregroundColor = value;
                RaisePropertyChanged("myForeground");
            }
        }

        /// <summary>
        /// Metoda zależnie od <see cref="LogType"/>  wyświetla i zapisuje logi w róznych kolorach
        /// </summary>
        /// <param name="msg">Text komunikatu</param>
        /// <param name="logType">Typ logów, od nich zależy kolor komunikatu</param>
        public void AddLog(LogMessage msg)
        {
            switch (msg.Log)
            {
                case LogType.INFO:
                    logAdd(msg.Msg, Colors.Black);
                    WriteToFile(msg.Msg);
                    break;

                case LogType.ERROR:
                    logAdd(msg.Msg, Colors.Red);
                    WriteToFile(msg.Msg);
                    break;

                case LogType.ERROR_SAP:
                    logAdd(msg.Msg, Colors.Red);
                    WriteToFile(msg.Msg);
                    break;

                case LogType.DONE:
                    logAdd(msg.Msg, Colors.Green);
                    WriteToFile(msg.Msg);
                    break;
                case LogType.DATABASELOG:
                    WriteToFile(msg.Msg);
                    break;

                default:
                    logAdd("default log:" + msg.Msg, Colors.Red);
                    WriteToFile(msg.Msg);
                    break;
            }
        }

        /// <summary>
        /// Metoda dodaje komunikat do <see cref="_lista"/>
        /// </summary>
        /// <param name="msg">Text komunikatu</param>
        public void logAdd(string msg, Color color)
        {
            _foregroundColor = new SolidColorBrush(color);
            _lista.Add(msg);
            SelectedIndex = LogList.Count;

        }

        /// <summary>
        /// Metoda zapisuje komunikat do <see cref="_logger"/>
        /// </summary>
        /// <param name="msg"> Text komunikatu </param>
        public void WriteToFile(string msg)
        {
            logger.Trace(msg);
        }


        
    }
}
