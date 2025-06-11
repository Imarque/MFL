using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Muthoot.Helper
{
    [Serializable]
    public class SqlHepler
    {
        private readonly string _connectionstring;
        SqlConnection con = null;
        public SqlHepler(IConfiguration configuration)
        {
            _connectionstring = configuration.GetConnectionString("Constring");
        }
        public SqlConnection GetConnection()
        {
            try
            {
                Dispo();

                if (con==null)
                {
                    con = new SqlConnection(_connectionstring);
                }
                return con;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteNonQuery(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                using (SqlCommand cmd = new SqlCommand(procedureName, con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    AddParameters(cmd, parameters);
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteNonQuery(string query)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ReturnDataSet(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                using (SqlCommand cmd = new SqlCommand(procedureName, con))
                {
                    cmd.CommandType=CommandType.StoredProcedure;
                    AddParameters(cmd, parameters);
                    using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        con.Open();
                        adpt.Fill(ds);
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ReturnTableData(string procedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                using (SqlCommand cmd = new SqlCommand(procedureName, con))
                {
                    cmd.CommandType= CommandType.StoredProcedure;
                    AddParameters(cmd, parameters);
                    using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        con.Open();
                        adpt.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BulkInsertData(DataTable dt, string tableName)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void AddParameters(SqlCommand cmd, Dictionary<string, object> parameters)
        {
            try
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Dispo()
        {
            try
            {
                if (con != null)
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    con = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
