using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

namespace CIR.Common.Data
{
    public static class SQLHelper
    {
        //public static IEnumerable<dynamic> DynamicListFromSql(this DbContext db, string Sql, Dictionary<string, object> Params)
        //{
        //    using (var cmd = db.Database.GetDbConnection().CreateCommand())
        //    {
        //        cmd.CommandText = Sql;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }

        //        foreach (KeyValuePair<string, object> p in Params)
        //        {
        //            DbParameter dbParameter = cmd.CreateParameter();
        //            dbParameter.ParameterName = p.Key;
        //            dbParameter.Value = p.Value;
        //            cmd.Parameters.Add(dbParameter);
        //        }

        //        using (var dataReader = cmd.ExecuteReader())
        //        {
        //            while (dataReader.Read())
        //            {
        //                var row = new ExpandoObject() as IDictionary<string, object>;
        //                for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
        //                {
        //                    row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
        //                }
        //                yield return row;
        //            }
        //        }
        //    }
        //}

        //public static DataSet DynamicListFromSqlGrid(this DbContext db, string Sql, Dictionary<string, object> Params)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {

        //        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        //        var connectionString = configuration.GetConnectionString("DefaultConnection");
        //        using (SqlConnection sqlConn = new SqlConnection(connectionString))
        //        {
        //            // string sql = "GetDataByFields";
        //            //List<SqlParameter param = new SqlParameter();
        //            using (SqlCommand sqlCmd = new SqlCommand(Sql, sqlConn))
        //            {
        //                sqlCmd.CommandType = CommandType.StoredProcedure;
        //                foreach (KeyValuePair<string, object> p in Params)
        //                {
        //                    //DbParameter dbParameter = sqlCmd.CreateParameter();
        //                    //dbParameter.ParameterName = p.Key;
        //                    //dbParameter.Value = p.Value;
        //                    //sqlCmd.Parameters.Add(dbParameter);

        //                    sqlCmd.Parameters.AddWithValue(p.Key, p.Value);
        //                }
        //                sqlCmd.CommandTimeout = 0;
        //                sqlConn.Open();
        //                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
        //                {
        //                    sqlAdapter.Fill(ds);
        //                }
        //                sqlConn.Close();
        //                SqlConnection.ClearPool(sqlConn);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }

        //    return ds;
        //}

        //public static List<T> RawSqlQuery<T>(CIRDbContext context, string query, Func<DbDataReader, T> map)
        //{
        //    using (var command = context.Database.GetDbConnection().CreateCommand())
        //    {
        //        command.CommandText = query;
        //        command.CommandType = CommandType.Text;

        //        context.Database.OpenConnection();

        //        using (var result = command.ExecuteReader())
        //        {
        //            var entities = new List<T>();

        //            while (result.Read())
        //            {
        //                entities.Add(map(result));
        //            }

        //            return entities;
        //        }
        //    }
        //}

        //public static object ExecuteScalarCommand(CIRDbContext context, string query, List<SqlParameter> parameters)
        //{
        //    object retValue;
        //    using (var command = context.Database.GetDbConnection().CreateCommand())
        //    {
        //        command.CommandText = query;
        //        command.CommandType = CommandType.Text;
        //        if (parameters != null)
        //        {
        //            command.Parameters.AddRange(parameters.ToArray());
        //        }
        //        context.Database.OpenConnection();

        //        retValue = command.ExecuteScalar();
        //    }
        //    return retValue;
        //}

        //public static DataTable ExecuteSqlQuery(string Sql)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        //        var connectionString = configuration.GetConnectionString("DefaultConnection");
        //        using (SqlConnection sqlConn = new SqlConnection(connectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand(Sql, sqlConn);
        //            cmd.CommandType = CommandType.Text;
        //            sqlConn.Open();
        //            SqlDataAdapter da = new SqlDataAdapter();
        //            da.SelectCommand = cmd;
        //            da.Fill(dt);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return dt;
        //}


        public static DataTable ExecuteSqlQueryWithParams(string sql, Dictionary<string, object> Params)
        {
            DataTable dt = new();
            try
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
                var connectionString = configuration.GetConnectionString("CIR");
                using SqlConnection sqlConn = new(connectionString);
                SqlCommand cmd = new(sql, sqlConn);
                sqlConn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sql;

                foreach (var procParameter in Params)
                {
                    cmd.Parameters.AddWithValue(procParameter.Key, procParameter.Value);
                }

                using SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public static List<T> ConvertToGenericModelList<T>(DataTable dt)
        {
            dt ??= new DataTable();
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var objectProperties = typeof(T).GetProperties(flags);
            var targetList = dt.AsEnumerable().Select(dataRow =>
            {
                var instanceOfT = Activator.CreateInstance<T>();

                foreach (var properties in objectProperties.Where(properties => columnNames.Contains(properties.Name) && dataRow[properties.Name] != DBNull.Value))
                {
                    properties.SetValue(instanceOfT, dataRow[properties.Name], null);
                }
                return instanceOfT;
            }).ToList();

            return targetList;
        }
    }
}
