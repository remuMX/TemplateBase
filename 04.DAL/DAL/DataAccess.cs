
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    public class DataAccess
    {
        #region Singleton
        private static volatile DataAccess instance = null;
        private static readonly object padlock = new object();
        readonly string conString = "Data Source=DESKTOP-D2G24SC;Initial Catalog=CollectIT;User ID=test;Password=test;Persist Security Info=True;";


        private DataAccess() { }
        public static DataAccess Instance()
        {
            if (instance == null)
                lock (padlock)
                    if (instance == null)
                        instance = new DataAccess();
            return instance;
        }
        #endregion

        #region queryExecute
        public DataTable Query(String query)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(query, con) { CommandType = CommandType.StoredProcedure })
            {
                con.Open();
                DataTable resultado = new DataTable();
                resultado.Load(cmd.ExecuteReader());
                return resultado;
            }
        }

        public DataTable Query(String query, SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(query, con) { CommandType = CommandType.StoredProcedure })
            {
                con.Open();
                DataTable resultado = new DataTable();
                cmd.Parameters.AddRange(parameters);
                resultado.Load(cmd.ExecuteReader());
                return resultado;
            }
        }

        public int Execute(String query, SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand cmd = new SqlCommand(query, con) { CommandType = CommandType.StoredProcedure })
            {
                con.Open();
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}