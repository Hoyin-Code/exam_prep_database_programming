using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Oefening2
{
    public class Person
    {
        #region Public methods

        public static int GetCount()
        {
            int retValue = -1;
            SqlCommand sqlCommandSelect;

            // Via inline SQL
            string commandText = "SELECT COUNT(*) FROM [Person].[Person]";
            // Via stored procedure
            //string commandText = "dbo.spGetCount";

            sqlCommandSelect = new SqlCommand(commandText);
            // Via inline SQL
            sqlCommandSelect.CommandType = CommandType.Text;
            // Via stored procedure
            //selectCommand.CommandType = CommandType.StoredProcedure;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionPlain"].ConnectionString))
            {
                sqlCommandSelect.Connection = sqlConnection;
                sqlConnection.Open();
                retValue = Convert.ToInt32(sqlCommandSelect.ExecuteScalar());
            }

            return retValue;
        }

        #endregion
    }
}