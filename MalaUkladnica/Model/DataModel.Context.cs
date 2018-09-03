namespace MalaUkladnica.Model
{
    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using Resources;

    /// <summary>
    /// public partial class TME_SAPEntities : DbContext
    /// </summary>
    public partial class TME_SAPEntities : DbContext
    {
        /// <summary>
        /// Zawiera nazwe connectionStringa 
        /// </summary>
        private static string _connectionString = "name=TME_SAPEntities_PROD";

        /// <summary>
        /// Initializes a new instance of the <see cref="TME_SAPEntities"/> class.
        /// <text>
        /// Konstruktor wywołuje konstruktor z klasy bazowej z parametrem <see cref="_connectionString"/>
        /// By <see cref="_connectionString"/> był kompletny, trzeba wywołac funkcje <see cref="Init"/> przed pierwszym tworzeniem instancji klasy
        /// </text>
        /// </summary>
        public TME_SAPEntities()
            : base(_connectionString)
        {
        }

        /// <value>
        /// LOGI_MALAUKLADNICA_ACTION zwraca/ustawia  DbSet/<LOGI_MALAUKLADNICA_ACTION/>
        /// </value>
        /// <summary>
        /// Gets or sets DbSet<LOGI_MALAUKLADNICA_ACTION
        /// </summary>
        public virtual DbSet<LOGI_MALAUKLADNICA_ACTION> LOGI_MALAUKLADNICA_ACTION { get; set; }

        /// <summary>
        /// Metoda statyczna, odpowiada za odszyfrowanie hasła z pliku konfiguracyjnego "App.config"
        /// klucz szyfrujący jak i metoda odszyfrująca znajduje się w klasie <see cref="Cryptography"/>
        /// </summary>
        public static void Init()
        {
            string pass = ConfigurationManager.AppSettings["p1"];// parametr p - DEV, p1 - PROD
            var originalConnectionString = ConfigurationManager.ConnectionStrings["TME_SAPEntities_PROD"].ConnectionString;
            var entityBuilder = new EntityConnectionStringBuilder(originalConnectionString);
            var factory = DbProviderFactories.GetFactory(entityBuilder.Provider);
            var providerBuilder = factory.CreateConnectionStringBuilder();

            providerBuilder.ConnectionString = entityBuilder.ProviderConnectionString;
            providerBuilder.Add("Password", Cryptography.Decrypt(pass));
            _connectionString = entityBuilder.ProviderConnectionString = providerBuilder.ToString();
        }

        /// <summary>
        /// Sprawdza połączenie z bazą
        /// </summary>
        /// <returns>true - jeśli jest możliwe połączenie, w przeciwnym wypadku false</returns>
        public static bool TestConnectionEF()
        {
            var db = new TME_SAPEntities();
            return db.Database.Exists();
        }
    }
}
