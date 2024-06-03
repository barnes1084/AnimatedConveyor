using System;
using System.Data;
using System.Data.SqlClient;
//using System.Data.OracleClient;

namespace CommonTools
{
    public static class DbAccess
    {


        // SQL
        public static DataTable QuerySqlDB(string query, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Send.Email("DataDump Error", $"{ex.Message} - {ex.StackTrace}");
                Log.ToFile($"{ex.Message} - {ex.StackTrace}");
                return null;
            }
        }




        public static void DeleteSqlData(string deleteQuery, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand delete = new SqlCommand(deleteQuery, conn);
                    delete.Connection.Open();
                    delete.ExecuteNonQuery();
                    delete.Connection.Close();
                    Log.ToFile("Deleted data.");
                }
            }
            catch (Exception ex)
            {
                Send.Email("DataDump Error", $"{ex.Message} - {ex.StackTrace}");
                Log.ToFile($"{ex.Message} - {ex.StackTrace}");
            }
        }



        public static long SQLsingleInteger(string query, string connectionString)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DbAccess.QuerySqlDB(query, connectionString);
                var i = dt.Rows[0][0];
                if (i is DBNull)
                {
                    return 0;
                }
                else
                {
                    long highestId = Convert.ToInt64(i);
                    return highestId;
                }

            }
            catch (Exception ex)
            {
                Send.Email("DataDump Error", $"{ex.Message} - {ex.StackTrace}");
                Log.ToFile($"{ex.Message} - {ex.StackTrace}");
                return -1;
            }
        }



        // Oracle
        //public static DataTable QueryOraDB(string query, string connectionString)
        //{
        //    try
        //    {
        //        using (OracleConnection con = new OracleConnection(connectionString))
        //        {
        //            using (OracleCommand cmd = con.CreateCommand())
        //            {
        //                con.Open();
        //                Log.ToFile("open connection");
        //                cmd.CommandText = query;
        //                OracleDataAdapter adapt = new OracleDataAdapter(cmd);
        //                DataTable data = new DataTable();
        //                adapt.Fill(data);
        //                Log.ToFile("successful Oracle data fill");
        //                return data;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Send.Email("DataDump Error", $"{ex.Message} - {ex.StackTrace}");
        //        Log.ToFile($"{ex.Message} - {ex.StackTrace}");
        //        return null;
        //    }
        //}



        //public static long ORAsingleInteger(string query, string connectionString)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = DbAccess.QueryOraDB(query, connectionString);
        //        var i = dt.Rows[0][0];
        //        if (i is DBNull)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            long highestId = Convert.ToInt64(i);
        //            return highestId;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Send.Email("DataDump Error", $"{ex.Message} - {ex.StackTrace}");
        //        Log.ToFile($"{ex.Message} - {ex.StackTrace}");
        //        return -1;
        //    }
        //}


        

    }
}
