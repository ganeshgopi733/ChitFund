using System;
using System.Data;
using System.Data.SqlClient;

namespace ChitFund
{
    public static class Sql
    {
        public static SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\fFolder\Visual Studio 2019\ChitFund\ChitFund\database\alphachits.mdf;Integrated Security=True");

        public static void executeSqlStmnt(string sqlquery)
        {
            if (Sql.connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlquery, connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception custom)
            {
                throw custom;
            }
            connection.Close();
        }

    }
}
