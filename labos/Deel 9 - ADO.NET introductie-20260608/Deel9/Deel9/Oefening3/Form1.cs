using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oefening3
{
    public partial class Form1 : Form
    {
        #region Constructors

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            ShowPersons();
        }

        #endregion

        #region Private methods

        private void ShowPersons()
        {
            int counter = 0;
            StringBuilder stringBuilder;
            SqlCommand sqlCommandSelect;
            SqlDataReader sqlDataReader;            

            // Via inline SQL
            string commandText = "SELECT TOP 100 * FROM Person.Person";
            // Via stored procedure
            // string commandText = "spGetTop";

            sqlCommandSelect = new SqlCommand(commandText);
            // Via inline SQL
            sqlCommandSelect.CommandType = CommandType.Text;
            // Via stored procedure
            // selectCommand.CommandType = CommandType.StoredProcedure;

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionPlain"].ConnectionString))
            {
                sqlCommandSelect.Connection = sqlConnection;
                stringBuilder = new StringBuilder();
                sqlConnection.Open();
                sqlDataReader = sqlCommandSelect.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    counter++;
                    stringBuilder.AppendLine($"{counter.ToString()}.\t{sqlDataReader["FirstName"].ToString()} {sqlDataReader["LastName"].ToString()}");
                }

                this.textBoxResults.Text= stringBuilder.ToString();
            }
        }

        #endregion
    }
}