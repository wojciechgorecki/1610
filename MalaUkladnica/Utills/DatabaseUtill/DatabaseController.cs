namespace MalaUkladnica.Utills.DatabaseUtill
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using MalaUkladnica.Model;

    /// <summary>
    /// Klasa operująca na bazie danych, zawiera metody stworzone do testów
    /// </summary>
    public class DatabaseController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseController"/> class.
        ///  Konstruktor klasy <see cref="DatabaseController"/>, Wywołuje metode <see cref="TME_SAPEntities.Init"/> doklejając hasło do connectionString
        /// Tworzy obiekt <see cref="TME_SAPEntities"/> oraz obserwowalną kolekcje <see cref="LOGI_MALAUKLADNICA_ACTION"/>
        /// </summary>
        public DatabaseController()
        {
            TME_SAPEntities.Init();
            this.TME_ENTities = new TME_SAPEntities();
            this.Logi = new ObservableCollection<LOGI_MALAUKLADNICA_ACTION>(this.TME_ENTities.LOGI_MALAUKLADNICA_ACTION);
        }

        /// <summary>
        /// Gets or sets  Obserwowalną kolekcje LOGI_MALAUKLADNICA_ACTION
        /// </summary>
        /// <value>
        /// Pobiera/ustawia obserwowalną kolekcje LOGI_MALAUKLADNICA_ACTION
        /// </value>
        public ObservableCollection<LOGI_MALAUKLADNICA_ACTION> Logi { get; set; }

        /// <summary>
        /// Gets or sets obiekt TME_SAPEntities
        /// </summary>
        /// <value>
        /// Pobiera/ustawia obiekt typu TME_SAPEntities
        /// </value>
        public TME_SAPEntities TME_ENTities { get; set; }

        /// <summary>
        /// Statyczna metoda zwracająca liste akcji kontenera o danym numerze pomiędzy danymi datami.
        /// </summary>
        /// <param name="conteinerNumber">Numer Kontenera</param>
        /// <param name="cntList">Lista numerów kontenerów -do poprawki</param>
        /// <param name="dateTime">Czas od którego będzie przszukiwana historia</param>
        /// <param name="dateTo">Czas do którego będzie przszukiwana historia</param>
        /// <returns>Zwraca liste akcjii kontenera/ów</returns>
        public static List<LOGI_MALAUKLADNICA_ACTION> GetActions(string conteinerNumber, List<string> cntList, DateTime dateTime, DateTime dateTo)
        {
            dateTo = dateTo.AddDays(1);
            using (TME_SAPEntities db = new TME_SAPEntities())
            {
                string res1 = cntList[0];
                string res2 = cntList[1];
                var highScores = (from student in db.LOGI_MALAUKLADNICA_ACTION
                                  where ((student.LMUA_CONTAINER_NR.Contains(conteinerNumber)
                                  || (res1.Length > 0 && student.LMUA_CONTAINER_NR.Contains(res1))
                                  || (res2.Length > 0 && student.LMUA_CONTAINER_NR.Contains(res2)))
                                 && student.LMUA_CREATED_DATE > dateTime.Date.Date && student.LMUA_CREATED_DATE < dateTo)
                                  orderby student.LMUA_CREATED_DATE descending
                                  select student).Distinct();

                return highScores.OrderByDescending(r => r.LMUA_CREATED_DATE).ToList();
            }
        }

        /// <summary>
        /// Metoda statyczna, dodaje logi do bazy danych
        /// </summary>
        /// <param name="FVI_VLPLA">Wartość SAP - BARAN </param>
        /// <param name="message">Wiadomośc do zapisania w bazie </param>
        /// <param name="idUser_NUM8">ID użytkownika wykonującego akcje </param>
        /// <param name="userName">Nazwa użytkowanika wykonującego akcje </param>
        /// <param name="actionType">Typ wykonywanej akcji </param>
        /// <param name="returnCode">Kod akcji, zostanie dodany do wiadomosci </param>
        public static void AddingLogData(string FVI_VLPLA, string message, string idUser_NUM8, string userName, string actionType, int returnCode)
        {
            using (TME_SAPEntities db = new TME_SAPEntities())
            {
                LOGI_MALAUKLADNICA_ACTION test = new LOGI_MALAUKLADNICA_ACTION()
                {
                    LMUA_CONTAINER_NR = FVI_VLPLA,
                    LMUA_MESSAGE = message + returnCode,
                    LMUA_USERID = idUser_NUM8,
                    LMUA_USERNAME = userName,
                    LMUA_ACTION_TYPE = actionType,
                    LMUA_CREATED_DATE = DateTime.Now

                };
                try
                {
                    db.LOGI_MALAUKLADNICA_ACTION.Add(test);
                    db.SaveChanges();
                }catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        /// <summary>
        /// Sprawdza czy dany użytkownik istnieje w bazie danych.
        /// </summary>
        /// <param name="userName">
        /// Nazwa użytkownika którego szukamy
        /// </param>
        /// <returns>Zwraca prawde jeśli dany użytkownik istnieje w bazie, w przeciwnym przypadku fałsz</returns>
        public bool UserNameExists(string userName)
        {
            foreach (LOGI_MALAUKLADNICA_ACTION temp1 in Logi)
            {
                if (temp1.LMUA_USERNAME.Equals(userName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Metoda zlicza ID użytkowników
        /// </summary>
        /// <returns>Liczbe ID użytkownikó (z powtórzeniami)</returns>
        public int CountIDUsers()
        {
            return this.Logi.Count;
        }

        /// <summary>
        ///  Sprawdza połączenie z bazą danych.
        /// </summary>
        /// <returns>Zwraca prawde jęsli istenije połączenie z bazą, w przeciwnym wypadku fałsz</returns>
        public bool ConnectionToDatabase()
        {
            return this.TME_ENTities.Database.Exists();
        }

        /// <summary>
        /// Sprawdza czy dane ID  użytkownika istnieje w bazie.
        /// </summary>
        /// <param name="searchingID">
        /// ID użytkownika.
        /// </param>
        /// <returns>Zwraca prawde jeśli  ID użytkownika istnieje w bazie, w przeciwnym wypadku fałsz</returns>
        public bool IDExists(int searchingID)
        {
            foreach (LOGI_MALAUKLADNICA_ACTION temp1 in Logi)
            {
                if (temp1.LMUA_ID.Equals(searchingID))
                {
                    return true;
                }
            }

            return false;
        }
    }
}