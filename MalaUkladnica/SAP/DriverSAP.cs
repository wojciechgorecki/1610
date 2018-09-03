namespace MalaUkladnica.SAP
{
    using System;
    using GalaSoft.MvvmLight.Messaging;
    using global::SAP.Middleware.Connector;
    using MalaUkladnica.SAP.Returns;
    using MalaUkladnica.Utills;
    using MalaUkladnica.Utills.DatabaseUtill;
    using static MalaUkladnica.ViewModel.LogViewModel;

    /// <summary>
    /// Klasa wykonująca operacje na bazie SAP.
    /// </summary>
    public class DriverSAP
    {
        /// <summary>
        /// Zmienna pomocnicza zastepujaca 'X' (prawda z SAP)
        /// </summary>
        private const char TRUE = 'X';

        /// <summary>
        /// Zmienna pomocnicza zastepujaca ' ' (falsz z SAP)
        /// </summary>
        private const char FALSE = ' ';

        /// <summary>
        /// statyczna tylko do odczytu instancja klasy <see cref="DriverSAP"/>
        /// </summary>
        static readonly DriverSAP _inst = new DriverSAP();

        /// <summary>
        /// Gets Properties do <see cref="_inst"/>
        /// </summary>
        /// <value>
        /// Properties do <see cref="_inst"/>
        /// </value>
        public static DriverSAP Inst { get { return _inst; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverSAP"/> class.
        /// Konstruktor klasy, pusty
        /// </summary>
        private DriverSAP()
        {
        }

        /// <summary>
        /// Metoda rejestrujaca docelowa konfiguracje
        /// </summary>
        /// <param name="cfg">Interfejs konfiguracyjny</param>
        public void RegisterConfig(IDestinationConfiguration cfg)
        {
            // Register config
            try
            {
                RfcDestinationManager.RegisterDestinationConfiguration(cfg);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new LogMessage(ex.Message, LogType.ERROR),"Log");
            }
        }

        /// <summary>
        /// Metoda wyfocująca kontener z bufora stacji.
        /// </summary>
        /// <param name="mode">Tryb pracy (PROD/DEV)</param>
        /// <param name="userName">Nazwa uzytkownika</param>
        /// <param name="idUser_NUM8">ID uzytkownika z SAP</param>
        /// <param name="FVI_VLPLA">Numer kontenera</param>
        /// <param name="FVI_NO_LAGP">Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie) </param>
        /// <param name="bc">Zmienna pomocnicza, zapisywane sa do niej zwracane kody i informacje przez SAP</param>
        /// <returns>Zwraca prawde/falsz zaleznie od tego czy udalo sie wykonac akcje.</returns>
        public bool BackContainerBuffor(string mode, string userName, string idUser_NUM8, string FVI_VLPLA, char FVI_NO_LAGP, BackContainerBufforReturn bc)
        {
            bool success = false;
            try
            {
                RfcDestination dest = RfcDestinationManager.GetDestination(mode);
                RfcRepository repo = dest.Repository;

                // Informacja o kontenerze
                // FUNCTION z_mfcs_info.
                IRfcFunction Z_MFCS_DEL_FROM_ST = repo.CreateFunction("Z_MFCS_DEL_FROM_ST");

                // IMPORTING
                // VALUE(FVI_VLPLA) TYPE  LTAP_VLPLA
                // VALUE(FVI_PERNR) TYPE  PERNR_D

                // FVI_VLPLA - nr kontenera (CHAR10)
                Z_MFCS_DEL_FROM_ST.SetValue("FVI_VLPLA", FVI_VLPLA);

                // FVI_PERNR - id użytkownika (NUM8)
                Z_MFCS_DEL_FROM_ST.SetValue("FVI_PERNR", idUser_NUM8);

                // Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie)
                Z_MFCS_DEL_FROM_ST.SetValue("FVI_NO_LAGP", FVI_NO_LAGP);

                // call
                Z_MFCS_DEL_FROM_ST.Invoke(dest);

                bc.FVO_RETURN = Z_MFCS_DEL_FROM_ST.GetValue("FVO_RETURN").ToString();
                bc.ReturnCode = Convert.ToInt32(Z_MFCS_DEL_FROM_ST.GetValue("FVO_RETURN"));

                // EXPORTING
                // VALUE(FVO_RETURN) TYPE  NUM03
                //* return codes
                //* 100 - OK
                //* 101 - Błąd
                //* 333 - błąd komunikacji z serwerem układnicy
                //* 666 - brak uprawnień

                switch (bc.ReturnCode)
                {
                    case 100:
                        bc.Error = "OK";
                        success = true;
                        break;
                    case 101:
                        bc.Error = $"Kontener [{FVI_VLPLA}] nie istnieje."; // zmiana komunikatu: 11/02/16
                        break;
                    case 333:
                        bc.Error = "Błąd komunikacji z serwerem układnicy.";
                        break;
                    case 666:
                        bc.Error = "Brak uprawnień.";
                        break;

                    default:
                        bc.Error = "Nieznany komunikat błędu: " + bc.ReturnCode;
                        break;
                }

                try
                {
                    DatabaseController.AddingLogData(FVI_VLPLA, "Usuwanie kontenera z buforu stacji ", idUser_NUM8, userName, "Z_MFCS_DEL_FROM_ST", bc.ReturnCode);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new LogMessage(ex.Message, LogType.DATABASELOG),"Log");
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new LogMessage(ex.Message, LogType.ERROR_SAP),"Log");
                return false;
            }

        return success;
        }

        /// <summary>
        /// Metoda zwracająca informacje o kontenerze.
        /// </summary>
        /// <param name="mode">Tryb pracy (PROD/DEV)</param>
        /// <param name="userName">Nazwa uzytkownika</param>
        /// <param name="idUser_NUM8">ID uzytkownika z SAP</param>
        /// <param name="FVI_VLPLA">Numer kontenera</param>
        /// <param name="FVI_NO_LAGP">Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie) </param>
        /// <param name="_containerInfoReturn">Zmienna pomocnicza, zapisywane sa do niej zwracane kody i informacje przez SAP<</param>
        /// <returns>Zwraca prawde/falsz zaleznie od tego czy udalo sie wykonac akcje.</returns>
        public bool ContainerInfo(string mode, string userName, string idUser_NUM8, string FVI_VLPLA, char FVI_NO_LAGP, ContainerInfoReturn _containerInfoReturn) // idUser - HR id
        {
            bool success = false;
            try
            {
                RfcDestination dest = RfcDestinationManager.GetDestination(mode);
                RfcRepository repo = dest.Repository;

                // Informacja o kontenerze
                // FUNCTION z_mfcs_info.
                IRfcFunction z_mfcs_info = repo.CreateFunction("Z_MFCS_INFO");

                // IMPORTING
                // VALUE(FVI_VLPLA) TYPE  LTAP_VLPLA
                // VALUE(FVI_PERNR) TYPE  PERNR_D

                // FVI_VLPLA - nr kontenera (CHAR10)
                z_mfcs_info.SetValue("FVI_VLPLA", FVI_VLPLA);

                // FVI_PERNR - id użytkownika (NUM8)
                z_mfcs_info.SetValue("FVI_PERNR", idUser_NUM8);

                // Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie)
                z_mfcs_info.SetValue("FVI_NO_LAGP", FVI_NO_LAGP);

                // call
                z_mfcs_info.Invoke(dest);

                //var FVO_RETURN = z_mfcs_info.GetValue("FVO_RETURN").ToString();
                //var FVO_LOC = z_mfcs_info.GetValue("FVO_LOC").ToString();
                //var FVO_ER = z_mfcs_info.GetValue("FVO_ER").ToString();

                _containerInfoReturn.ReturnCode = Convert.ToInt32(z_mfcs_info.GetValue("FVO_RETURN"));
                _containerInfoReturn.FVO_RETURN = z_mfcs_info.GetValue("FVO_RETURN").ToString();
                _containerInfoReturn.FVO_LOC = z_mfcs_info.GetValue("FVO_LOC").ToString();
                _containerInfoReturn.FVO_ER = z_mfcs_info.GetValue("FVO_ER").ToString();

                // EXPORTING
                // VALUE(FVO_RETURN) TYPE  NUM03
                //* return codes
                //* 100 - OK
                //* 101 - Błąd
                //* 102 - Odpytaj jeszcze raz
                //* 333 - błąd komunikacji z serwerem układnicy
                //* 666 - brak uprawnień
                // VALUE(FVO_LOC) TYPE  CHAR12 - kod błędu otrzymany Z MFCS
                // VALUE(FVO_ER) TYPE  INT4  - ostatnia lokalizacja kontenera

                switch (_containerInfoReturn.ReturnCode)
                {
                    case 100:
                        _containerInfoReturn.Error = "OK";
                        success = true;
                        break;
                    case 101:
                        _containerInfoReturn.Error = $"Kontener [{FVI_VLPLA}] nie istnieje."; // zmiana komunikatu: 11/02/16
                        break;
                    case 102:
                        _containerInfoReturn.Error = "Odpytaj jeszcze raz.";
                        break;
                    case 333:
                        _containerInfoReturn.Error = "Błąd komunikacji z serwerem układnicy.";
                        break;
                    case 666:
                        _containerInfoReturn.Error = "Brak uprawnień.";
                        break;

                    default:
                        _containerInfoReturn.Error = "Nieznany komunikat błędu: " + _containerInfoReturn.ReturnCode;
                        break;
                }

                try
                {
                    DatabaseController.AddingLogData(FVI_VLPLA, "Informacje o kontenerze ", idUser_NUM8, userName, "Z_MFCS_INFO", _containerInfoReturn.ReturnCode);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new LogMessage(ex.Message, LogType.DATABASELOG),"Log");
                }
            }
            catch (Exception ex)
            {
                _containerInfoReturn.Error = ex.Message;
                Messenger.Default.Send(new LogMessage(ex.Message, LogType.ERROR_SAP),"Log");
                return false;

            }

            return success;
        }

        /// <summary>
        /// Metoda wysylajaca kontener na ukladnice.
        /// </summary>
        /// <param name="mode">Tryb pracy (PROD/DEV)</param>
        /// <param name="userName">Nazwa uzytkownika</param>
        /// <param name="idUser_NUM8">ID uzytkownika z SAP</param>
        /// <param name="idContainer_CHAR10">Numer kontenera</param>
        /// <param name="FVI_EMPTY">czy pusty kontener (CHAR1 X - tak; spacja - nie)</param>
        /// <param name="FVI_NO_LAGP">Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie) </param>
        /// <param name="FVI_ABC">Wskazniki ABC</param>
        /// <param name="sc">Zmienna pomocnicza, zapisywane sa do niej zwracane kody i informacje przez SAP<</param>
        /// <returns>Zwraca prawde/falsz zaleznie od tego czy udalo sie wykonac akcje.</returns>
        public bool SendingContainer(string mode, string userName, string idUser_NUM8, string idContainer_CHAR10, char FVI_EMPTY, char FVI_NO_LAGP, char FVI_ABC, SendingContainerReturn sc)
        {
            // char FVI_NO_LQUA_CHK, domyślny 'X'
            bool success = false;
            try
            {
                // Get a destination object from the destination manager
                // To connect to sap and extract data from it you must create an instance of SAP.Middleware.Connector.RfcDestination
                RfcDestination dest = RfcDestinationManager.GetDestination(mode);

                // Now that we have a valid RfcDestination object, we need access to the SAP Repository.
                // The repository contains information about the BAPI calls we are going to make.

                // Using the RfcRepository object, acquire a reference to the SAP BAPI by calling method RfcRepository.CreateFunction() method.
                // Pass in the name of the desired function as a string parameter to CreateFunction().
                // This method returns an object we can use to setup parameters, invoke the function, and retrieve results.
                RfcRepository repo = dest.Repository;

                // function: Z_MFCS_SEND
                IRfcFunction z_mfcs_send = repo.CreateFunction("Z_MFCS_SEND");

                // IMPORTING
                // VALUE(FVI_VLPLA) TYPE  LTAP_VLPLA
                // VALUE(FVI_PERNR) TYPE  PERNR_D
                // VALUE(FVI_EMPTY) TYPE  FLAG_X
                // VALUE(FVI_NO_LQUA_CHK) TYPE  FLAG_X
                // FVI_NO_LAGP CHAR(1) - nowy 11/02/16
                // FVI_ABC CHAR(1) [A,B,C] - Wskaźnik ABC - nowy 11/02/16

                // FVI_VLPLA - nr kontenera (CHAR10)
                z_mfcs_send.SetValue("FVI_VLPLA", idContainer_CHAR10);

                // FVI_PERNR - id użytkownika (NUM 8)
                z_mfcs_send.SetValue("FVI_PERNR", idUser_NUM8);

                // czy pusty kontener (CHAR1 X - tak; spacja - nie)
                z_mfcs_send.SetValue("FVI_EMPTY", FVI_EMPTY);

                // FVI_NO_LQUA_CHK - brak kontroli ilosci (CHAR1 X - tak; spacja - nie)
                z_mfcs_send.SetValue("FVI_NO_LQUA_CHK", 'X'); // X

                // Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie)
                z_mfcs_send.SetValue("FVI_NO_LAGP", FVI_NO_LAGP);

                // Wskaźnik ABC (CHAR1 A,B,C)
                if (FVI_ABC != FALSE) // domyślny parametr C
                    z_mfcs_send.SetValue("FVI_ABC", FVI_ABC);

                // call
                z_mfcs_send.Invoke(dest);

                // EXPORTING
                // VALUE(FVO_RETURN) TYPE  NUM03
                //* 100 - OK
                //* 101 - Błąd  (FVI_NO_LAGP, 'X') -> testowy błedy numer kontenera | (FVI_NO_LAGP, ' ') -> Kontener nie istnieje.
                //* 102 - Pusta lokalizacja w SAP
                //* 103 - Nie należy przyjmować -> istnieje zlecenie wydania
                //* 333 - błąd komunikacji z serwerem układnicy
                //* 666 - brak uprawnień
                sc.FVO_RETURN = z_mfcs_send.GetValue("FVO_RETURN").ToString();
                sc.ReturnCode = Convert.ToInt32(z_mfcs_send.GetValue("FVO_RETURN"));

                switch (sc.ReturnCode)
                {
                    case 100:
                        sc.Error = "OK";
                        success = true;
                        break;

                    case 101:
                        if (FVI_NO_LAGP == FALSE) // zmiana komunikatu: 11/02/16
                            sc.Error = $"Kontener [{idContainer_CHAR10}] nie istnieje.";
                        else
                            sc.Error = $"Błedny numer kontenera.";
                        break;
                    case 102:
                        sc.Error = "Pusta lokalizacja w SAP.";
                        break;
                    case 103:
                        sc.Error = "Istnieje zlecenie wydania.";
                        break;
                    case 333:
                        sc.Error = "Błąd komunikacji z serwerem układnicy.";
                        break;
                    case 666:
                        sc.Error = "Brak uprawnień.";
                        break;

                    default:
                        sc.Error = "Nieznany komunikat błędu: " + sc.ReturnCode;
                        break;
                }

                try
                {
                    DatabaseController.AddingLogData(idContainer_CHAR10, "Wysłanie kontenera na układnicę ", idUser_NUM8, userName, "Z_MFCS_SEND", sc.ReturnCode);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new LogMessage(ex.Message, LogType.DATABASELOG),"Log");
                }

            }
            catch (Exception ex)
            {
                if(ex is RfcAbapRuntimeException || ex is RfcAbapBaseException || ex is RfcBaseException)
                {
                    Messenger.Default.Send(new LogMessage(ex.Message, LogType.ERROR_SAP),"Log");
                    return false;
                }

            }

            return success;
        }

        /// <summary>
        /// Metoda pobierajaca kontener z malej ukladnicy.
        /// </summary>
        /// <param name="mode">Tryb pracy (PROD/DEV)</param>
        /// <param name="userName">Nazwa uzytkownika</param>
        /// <param name="FVI_PERNR">ID uzytkownika z SAP</param>
        /// <param name="FVI_VLPLA">Numer kontenera</param>
        /// <param name="FVI_EMPTY">Czy pusty kontener ( X - tak; spacja - nie)</param>
        /// <param name="FVI_DEST">Miejsce docelowe kontenera </param>
        /// <param name="FVI_NO_LAGP">Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie) </param>
        /// <param name="FVI_PRIORITY">Czy akcja jest priorytetowa ( X - tak; spacja - nie)(</param>
        /// <param name="error">Zmienna pomocnicza, zapisywane sa do niej zwracane kody i informacje przez SAP<</param>
        /// <returns>Zwraca prawde/falsz zaleznie od tego czy udalo sie wykonac akcje.</returns>
        public int Z_MFCS_SEND_HT(string mode, string userName, string FVI_PERNR, string FVI_VLPLA, char FVI_EMPTY, string FVI_DEST, char FVI_NO_LAGP, int FVI_PRIORITY, out string error)
        {
            error = string.Empty;
            int ret = -1;
            try
            {
                RfcDestination dest = RfcDestinationManager.GetDestination(mode);
                RfcRepository repo = dest.Repository;

                // Informacja o kontenerze
                // FUNCTION z_mfcs_info.
                IRfcFunction z_MFCS_SEND_HT = repo.CreateFunction("Z_MFCS_SEND_HT");

                // IMPORTING
                // VALUE(FVI_VLPLA) TYPE  LTAP_VLPLA
                // VALUE(FVI_PERNR) TYPE  PERNR_D

                // FVI_VLPLA - nr kontenera (CHAR10)
                z_MFCS_SEND_HT.SetValue("FVI_VLPLA", FVI_VLPLA);

                // FVI_PERNR - id użytkownika (NUM8)
                z_MFCS_SEND_HT.SetValue("FVI_PERNR", FVI_PERNR);

                // FVI_EMPTY
                z_MFCS_SEND_HT.SetValue("FVI_EMPTY", ' ');

                // FVI_DEST
                z_MFCS_SEND_HT.SetValue("FVI_DEST", FVI_DEST);

                // Ignoruje czy kontener istnieje w SAPie (CHAR1 X - tak; spacja - nie)
                z_MFCS_SEND_HT.SetValue("FVI_NO_LAGP", FVI_NO_LAGP);

                // Priorytet
                z_MFCS_SEND_HT.SetValue("FVI_PRIORITY", FVI_PRIORITY);

                // call
                z_MFCS_SEND_HT.Invoke(dest);

                var FVO_RETURN = z_MFCS_SEND_HT.GetValue("FVO_RETURN").ToString();
                ret = Convert.ToInt32(FVO_RETURN);
                // bc.ReturnCode = Convert.ToInt32(Z_MFCS_DEL_FROM_ST.GetValue("FVO_RETURN"));

                switch (ret)
                {
                    case 100:
                        error = "OK";
                        break;
                    case 101:
                        error = $"Kontener [{FVI_VLPLA}] nie istnieje.";
                        break;
                    case 333:
                        error = "Błąd komunikacji z serwerem układnicy.";
                        break;
                    case 666:
                        error = "Brak uprawnień.";
                        break;

                    default:
                        error = "Nieznany komunikat błędu: " + ret;
                        break;
                }

                // EXPORTING
                // VALUE(FVO_RETURN) TYPE  NUM03
                //* return codes
                //* 100 - OK
                //* 101 - Błąd
                //* 333 - błąd komunikacji z serwerem układnicy
                //* 666 - brak uprawnień
                try
                {
                    DatabaseController.AddingLogData(FVI_VLPLA, "Pobranie kontenera " + FVI_DEST + " ", FVI_PERNR, userName, "Z_MFCS_SEND_HT", ret);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new LogMessage(ex.Message, LogType.DATABASELOG),"Log");
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new LogMessage(ex.Message, LogType.ERROR_SAP),"Log");
                return -1;
            }

            return ret;
        }

        /// <summary>
        /// Metoda zwracajaca ID uzytkownika z bazy SAP.
        /// </summary>
        /// <param name="mode">Tryb pracy (PROD/DEV)</param>
        /// <param name="user">Nazwa uzytkownika</param>
        /// <param name="FVI_PERNR">Zmienna do której zostanie zapisany numer ID uzytkownika</param>
        /// <returns>Zwraca kod, 100 - poprawnie odczytany ID uzytkownikam, 101 - nie znaleziono uzytkownika</returns>
        public int GetPernr(string mode, string user, out string FVI_PERNR)
        {
            FVI_PERNR = string.Empty;
            int ret = -1;
            try
            {
                RfcDestination dest = RfcDestinationManager.GetDestination(mode);
                RfcRepository repo = dest.Repository;

                // FUNCTION z_get_pernr_4_user.
                IRfcFunction z_get_pernr_4_user = repo.CreateFunction("Z_GET_PERNR_4_USER");

                // IMPORTING
                //VALUE(FVI_ENV_USER) TYPE  ZHR_ENVUSER

                // FVI_VLPLA - nr kontenera (CHAR10)
                z_get_pernr_4_user.SetValue("FVI_ENV_USER", user);

                // call
                z_get_pernr_4_user.Invoke(dest);

                var FVO_RETURN = z_get_pernr_4_user.GetValue("FVO_RETURN").ToString();
                ret = Convert.ToInt32(FVO_RETURN);

                FVI_PERNR = z_get_pernr_4_user.GetValue("FVO_PERNR").ToString(); // if 100 -> 00000000
                var FVO_UNAME = z_get_pernr_4_user.GetValue("FVO_UNAME").ToString(); // if 100 -> ""

                return ret;

                // EXPORTING
                // VALUE(FVO_RETURN) TYPE  NUM03
                //* return codes
                //* 100 - OK
                //* 101 - nie znalazlem uzytkownika
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new LogMessage(ex.Message, LogType.ERROR_SAP),"Log");
                return -1;
            }
        }
    }
}
