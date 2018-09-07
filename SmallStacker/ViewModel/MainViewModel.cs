namespace SmallStacker.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;
    using SmallStacker.Model;
    using SmallStacker.Resources;
    using SmallStacker.Resources.Langue;
    using SmallStacker.SAP;
    using SmallStacker.Utills;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using static SmallStacker.ViewModel.LogViewModel;

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// zmienna przechowuj�ca sciezke do katalogu domowego uzytkownika.
        /// </summary>
        static string pathToUserDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "TME", "Mala Ukladnica");


        const string TestModeCfg = "DEV";
        const string ProductionModeCfg = "PRODUCTION";

        const string TestModeStr = "Tryb TESTOWY";
        const string ProductionModeStr = "Tryb PRODUKCYJNY";

        /// <summary>
        /// zmienna przechowujaca nazwe pliku tekstowego zawierajacego zahaszowane loginy i hasla do bazy SAP.
        /// </summary>
        const string ConfigFileName = "malaukladnica.txt";

        /// <summary>
        /// zmienna przechowujaca nazwe domeny
        /// </summary>
        string _domain = Environment.UserDomainName;

        public static char _FVI_NO_LAGP = ' ';

        /// <summary>
        /// zmienna przechowujaca numer ID uzytkownika z SAP
        /// </summary>
        public string _pernr;

        /// <summary>
        /// zmienna przechowuj�ca tryb dost�pu do SAP PROD lub DEV
        /// </summary>
        public string _mode = "PROD";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// Konstruktor Klasy, przypisuje nazwe uzytkownika, wywo�uje <see cref="TME_SAPEntities.Init"/>, <see cref="initSap"/> oraz pobiera numer u�ytkownika z SAP.
        /// </summary>
        public MainViewModel()
        {
            CultureResources.ChangeCulture(new System.Globalization.CultureInfo(Properties.Settings.Default.Language));
            TME_SAPEntities.Init();
            initSap();
            GeneratePerNr();
            Pernr = _pernr;


        }


        public string AppVersion
        {
            get
            {
                return "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }

        }

        /// <summary>
        /// Gets or sets _userName, Properties operujacy na _userName, nadanie _userName odswieza binding do niego.
        /// </summary>
        public string UserName
        {
            get
            {
                return Environment.UserName;
            }

        }

        public string Pernr
        {
            get
            {
                return _pernr;
            }

            set
            {
                _pernr= value;
                RaisePropertyChanged("Pernr");
            }
        }

        /// <summary>
        /// inicjalizuje zmienne potrzebne do po��czenia z baz� SAP, i rejestruje po��czenie z SAP.
        /// </summary>
        public void initSap()
        {
           // MessageBox.Show("przed try");
            try
            {
             //   MessageBox.Show("po try, przed loadcfg");
                string[] str = LoadCFG(ConfigFileName);
              //  MessageBox.Show("PO loadcfg");
                if (str == null)
                {
               //     MessageBox.Show("jesli str to null");
                    Messenger.Default.Send(new LogMessage("Nie znaleziono pliku konfiguracyjnego : " + ConfigFileName, LogType.ERROR), "Log");
                    MessageBox.Show("Nie znaleziono pliku konfiguracyjnego\r\n " + ConfigFileName + "\r\nProgram zostanie zamkni�ty");
                    Application.Exit();
                    return;
                }

              //  MessageBox.Show("Miedzy ifami");
                if (str.Length != 4)
                {
                 //   MessageBox.Show("ostatni if");
                    Messenger.Default.Send(new LogMessage("Uszkodzony plik konfiguracyjny : " + ConfigFileName, LogType.ERROR), "Log");
                    MessageBox.Show("Uszkodzony plik konfiguracyjny\r\n " + ConfigFileName + "\r\nProgram zostanie zamkni�ty");
                    Application.Exit();
                    return;
                }

              //  MessageBox.Show("9");
                string userTest = Cryptography.Decrypt(str[0]);
                string passwordTest = Cryptography.Decrypt(str[1]);
                string user = Cryptography.Decrypt(str[2]);
                string password = Cryptography.Decrypt(str[3]);
              //  MessageBox.Show("11");
                if (passwordTest == string.Empty || userTest == string.Empty || password == string.Empty || user == string.Empty)
                {
                    Messenger.Default.Send(new LogMessage(string.Concat(new string[] { "userTest: ", userTest, " passwordTest: ", passwordTest, "\r\ncuser: ", user, " password: ", password }), LogType.ERROR),"Log");
                    MessageBox.Show("Nie uda�o si� odczyta� danych z rejestru");
                    Application.Exit();
                    return;
                }

              //  MessageBox.Show("12");
                ECCDestinationConfig cfg = new ECCDestinationConfig(userTest, passwordTest, user, password);
             //   MessageBox.Show("14");
                DriverSAP.Inst.RegisterConfig(cfg);
              //  MessageBox.Show("916");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        ///  Metoda �aduj�ca plik konfiguracyjny <see cref="ConfigFileName"/> ze sciezki podanej jako parametr.
        /// </summary>
        /// <param name="filepath">Sciezka do pliku konfiguracyjnego</param>
        /// <returns>Tablice zahaszowanych login�w i hase� do bazy SAP.</returns>
        private string[] LoadCFG(string filepath)
        {
            try
            {
                string all = string.Empty;
                all = File.ReadAllText(filepath);

                return all.Split('\r');
            }
            catch (Exception ex)
            {
                 Messenger.Default.Send(new LogMessage(ex.Message, LogType.ERROR),"Log");
                return null;
            }
        }

        /// <summary>
        /// Metoda pobieraj�ca numer uzytkownika z bazy SAP, i zapisuj�ca do zmiennej <see cref="_pernr"/> i wyswietla numer uzytkownika, w przypadku niepowodzenia zostaje wyswietlony komunikat o niepowodzeniu.
        /// </summary>
        private void GeneratePerNr()
        {
            int ret = DriverSAP.Inst.GetPernr(_mode, Environment.UserName, out _pernr);
            if (ret != 100)
            {
                string msgErr = string.Concat(new object[]
                {
                    "nie pobrano HR_Nr dla ",
                    Environment.UserName,
                    " , kod b�edu: ",
                    ret
                });
                Messenger.Default.Send(new LogMessage(msgErr, LogType.ERROR),"Log");
                return;
            }

            Messenger.Default.Send(new LogMessage("Pobrano HR_Nr: " + _pernr, LogType.DONE),"Log");
        }

    }
}