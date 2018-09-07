﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SmallStacker.Model
{

    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using Resources;


    public partial class TME_SAPEntities : DbContext
{
        private static string _connectionString = "name=TME_SAPEntities_PROD";

        public TME_SAPEntities()
        : base(_connectionString)
    {

    }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TME_SAPEntities>(null);
            base.OnModelCreating(modelBuilder);
        }

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


        public static bool TestConnectionEF()
        {
            using (var db = new TME_SAPEntities())
            {
                return db.Database.Exists();
            }
        }



        public virtual DbSet<LOGI_MALAUKLADNICA_ACTION> LOGI_MALAUKLADNICA_ACTION { get; set; }

}

}
