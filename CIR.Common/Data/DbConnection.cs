using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CIR.Common.Data
{
    public class DbConnection : IDisposable
    {
        public SqlConnection Connection { get; set; }
        public SqlTransaction Transaction { get; set; }
        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        #region "Constructor"

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnection" /> class.
        /// </summary>
        public DbConnection()
        {
            var connectionString = configuration.GetConnectionString("CIR");
            Dapper.SqlMapper.Settings.CommandTimeout = 0;
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnection" /> class.
        /// </summary>
        /// <param name="isTrans">if set to <c>true</c> [is trans].</param>
        public DbConnection(bool isTrans)
        {
            Connection = new SqlConnection("<<Default Connection String>>");
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }

        public void Dispose()
        {
            if (Connection != null)
            {
                Connection.Dispose();
            }
        }
        #endregion
    }
}
