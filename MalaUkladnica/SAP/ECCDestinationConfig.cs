namespace MalaUkladnica.SAP
{
    using global::SAP.Middleware.Connector;

    /// <summary>
    /// Klasa konfiguracyjna połączenia z SAP
    /// </summary>
    public class ECCDestinationConfig : IDestinationConfiguration
    {
        string _userTest = string.Empty;
        string _passwordTest = string.Empty;

        string _user = string.Empty;
        string _password = string.Empty;

        public ECCDestinationConfig(string userTest, string passwordTest, string user, string password)
        {
            _userTest = userTest;
            _passwordTest = passwordTest;

            _user = user;
            _password = password;
        }

        /// <summary>
        /// Event nie uzywany
        /// </summary>
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        /// <summary>
        ///  Metoda określająca czy wspierane jest zmiana evenentów.
        /// </summary>
        /// <returns> zwraca fałsz</returns>
        public bool ChangeEventsSupported()
        {
            return false;
        }

        /// <summary>
        /// Metoda konfiguruje połączenie zależnie od parametru
        /// Mozliwe dwie konfiguracje PRODUKCYJNA(PROD) oraz DEVELOPERSKA(DEV).
        /// </summary>
        /// <param name="destinationName"> nazwa konfiguracji</param>
        /// <returns> zwraca konfiguracje RfcConfigParameters</returns>
        public RfcConfigParameters GetParameters(string destinationName)
        {
            RfcConfigParameters parms = new RfcConfigParameters();
            string language = "pl-PL";
            if (!Properties.Settings.Default.Language.Equals("ua-UA"))
            {
                language = Properties.Settings.Default.Language;
            }


            language = language.Substring(3);
            if (destinationName.Equals("DEV"))
            {
                parms.Add(RfcConfigParameters.AppServerHost, "/H/10.14.0.40/H/10.15.0.59");
                parms.Add(RfcConfigParameters.SystemNumber, "00");
                parms.Add(RfcConfigParameters.SystemID, "TED");
                parms.Add(RfcConfigParameters.User, _userTest);
                parms.Add(RfcConfigParameters.Password, _passwordTest);
                parms.Add(RfcConfigParameters.Client, "100");
                parms.Add(RfcConfigParameters.Language, language);
                parms.Add(RfcConfigParameters.PoolSize, Properties.Settings.Default.PoolSize);
            }
            else if (destinationName.Equals("PROD"))
            {
                parms.Add(RfcConfigParameters.AppServerHost, "/H/10.14.0.40/H/10.15.0.56");
                parms.Add(RfcConfigParameters.SystemNumber, "00");
                parms.Add(RfcConfigParameters.SystemID, "TEP");
                parms.Add(RfcConfigParameters.User, _user);
                parms.Add(RfcConfigParameters.Password, _password);
                parms.Add(RfcConfigParameters.Client, "100");
                parms.Add(RfcConfigParameters.Language, language);
                parms.Add(RfcConfigParameters.PoolSize, Properties.Settings.Default.PoolSize);
            }

            return parms;
        }
    }
}