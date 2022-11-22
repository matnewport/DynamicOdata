using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GenericCrud;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;

namespace Tamp.Repositories
{
    /// <summary>
    /// A very simple and basic base class for all "micro" ADO.NET repositories to inherit
    /// from. Provides a basic means to lazily instantiate an IDbConnection when needed and
    /// to dispose of the IDbConnection when it is no longer needed. Does not provide any
    /// formal CRUD methods (see DbCrudRepositoryBase for that).
    /// </summary>
    ///
    public abstract class DbRepositoryBase : IDisposable
    {
        private IDbConnection _connection;

        /// <summary>
        /// Initializes a new instance using the specified connection string and ADO.NET provider name.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        public DbRepositoryBase(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;

            // initialize any property-to-column mappings
            this.DefineMappings();
        }

        /// <summary>
        /// Initializes a new instance using the specified connection string.
        /// </summary>
        /// <param name="config">The IConfiguration object injected by .NET Core runtime.</param>
        /// <param name="connectionStringName">
        /// The name of the connection string to use from applications configuration (appsettings.json) file.
        /// </param>
        public DbRepositoryBase(IConfiguration config, string connectionStringName)
            : this(
                    config[$"ConnectionStrings:{connectionStringName}:ConnectionString"],
                  config[$"ConnectionStrings:{connectionStringName}:ProviderName"])
        //config[$"AzureFunctionsJobHost:ConnectionStrings:{connectionStringName}:ConnectionString"],
        //config[$"AzureFunctionsJobHost:ConnectionStrings:{connectionStringName}:ProviderName"])
        {
            /*
            //#if RELEASE
            if (connectionStringName == "ScheduleDB")
            {
                string keyVaultName = Environment.GetEnvironmentVariable("VaultUri");
                keyVaultName = "MtSchedulesAPIvault";
                var kvUri = "https://" + keyVaultName + ".vault.azure.net";

                var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

                this.ProviderName = client.GetSecret("ConnectionStringProvider--scheduledb").Value.Value;
                this.ConnectionString = client.GetSecret("ConnectionStrings--scheduledb").Value.Value;
            }
            */
            //#endif
        }

        /// <summary>
        /// Creates and opens a new IDbConnection instance.
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {
            IDbConnection newConnection = DbProviderFactories.GetFactory(ProviderName).CreateConnection();
            newConnection.ConnectionString = ConnectionString;
            newConnection.Open();

            return newConnection;
        }

        /// <summary>
        /// Used to define any Object to Relational database Mappings (ORM). Also referred
        /// to as definining C# property to database column mappings.
        /// </summary>
        protected abstract void DefineMappings();

        protected string ReadCommandText(string methodName)
        {
            //  var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            folder = Path.Combine(folder, "DbRepositoryBase");
            folder = Path.Combine(folder, "CommandText");

            var file = $"{this.GetType().Namespace}.{this.GetTypeName()}.{methodName}.sql";
            var fileName = Path.Combine(folder, file);

            if (!System.IO.File.Exists(fileName))
                throw new InvalidOperationException($"The script {file} does not exist.");

            return File.ReadAllText(fileName); ;
        }

        /// <summary>
        /// Returns the connection string used when connecting to the database.
        /// </summary>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// The name of the ADO.NET DbProviderFactory to use.
        /// </summary>
        protected string ProviderName { get; private set; }

        /// <summary>
        /// Returns the current IDbConnection instance. A new one will be created and
        /// opened if one does not already exist.
        /// </summary>
        protected IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    // lazy instantiation
                    _connection = CreateConnection();
                }

                return _connection;
            }
        }

        /// <summary>
        /// Closes and disposes the underlying IDbConnection instance.
        /// </summary>
        public virtual void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}