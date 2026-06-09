using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oefening4
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

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            GetPersonsUsingInlineSQL();
            //GetPersonsUsingSP();
            
        }
        
        #endregion

        #region Private methods

        private void GetPersonsUsingInlineSQL()
        {
            this.textBoxResult.Text = "";
            int counter = 0;
            StringBuilder stringBuilder;
            SqlCommand selectCommand;
            SqlParameter sqlParameterFirstName;
            SqlParameter sqlParameterLastName;
            SqlDataReader sqlDataReader;

            string commandText = @"
                                    SELECT 
                                        FirstName, 
                                        LastName 
                                    FROM 
                                        Person.Person 
                                    WHERE 
                                        ((@prmFirstName IS NULL) OR (FirstName LIKE @prmFirstName)) 
                                        AND ((@prmLastName IS NULL) OR (LastName LIKE @prmLastName))";

            selectCommand = new SqlCommand(commandText);
            selectCommand.CommandType = CommandType.Text;

            sqlParameterFirstName = new SqlParameter("@prmFirstName", SqlDbType.NVarChar, 50);
            sqlParameterLastName = new SqlParameter("@prmLastName", SqlDbType.NVarChar, 50);

            if (string.IsNullOrEmpty(this.textBoxFirstName.Text))
            {
                sqlParameterFirstName.Value = DBNull.Value;
            }
            else
            {
                sqlParameterFirstName.Value = "%" + this.textBoxFirstName.Text + "%";
            }

            if (string.IsNullOrEmpty(this.textBoxLastName.Text))
            {
                sqlParameterLastName.Value = DBNull.Value;
            }
            else
            {
                sqlParameterLastName.Value = "%" + this.textBoxLastName.Text + "%";
            }

            selectCommand.Parameters.Add(sqlParameterFirstName);
            selectCommand.Parameters.Add(sqlParameterLastName);

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionPlain"].ConnectionString))
            {
                stringBuilder = new StringBuilder();
                selectCommand.Connection = sqlConnection;
                sqlConnection.Open();
                sqlDataReader = selectCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    counter++;
                    stringBuilder.AppendLine($"{counter.ToString()}.\t{sqlDataReader["FirstName"].ToString()} {sqlDataReader["LastName"].ToString()}");
                }

                this.textBoxResult.Text= stringBuilder.ToString();
            }
        }

        private void GetPersonsUsingSP()
        {
            this.textBoxResult.Text = "";
            int counter = 0;
            SqlDataReader rdr;
            string commandText = "spGetPersonsOef4";

            SqlCommand selectCommand = new SqlCommand(commandText);
            selectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter prmFirstName = new SqlParameter("@prmFirstName", SqlDbType.VarChar);
            SqlParameter prmLastName = new SqlParameter("@prmLastName", SqlDbType.VarChar);

            if (this.textBoxFirstName.Text.Length == 0)
            {
                prmFirstName.Value = DBNull.Value;
            }
            else
            {
                prmFirstName.Value = "%" + this.textBoxFirstName.Text + "%";
            }

            if (this.textBoxLastName.Text.Length == 0)
            {
                prmLastName.Value = DBNull.Value;
            }
            else
            {
                prmLastName.Value = "%" + this.textBoxLastName.Text + "%";
            }

            selectCommand.Parameters.Add(prmFirstName);
            selectCommand.Parameters.Add(prmLastName);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionPlain"].ConnectionString))
            {
                selectCommand.Connection = conn;
                conn.Open();
                rdr = selectCommand.ExecuteReader();

                while (rdr.Read())
                {
                    counter++;
                    this.textBoxResult.AppendText($"{counter.ToString()}.\t{rdr["FirstName"].ToString()} {rdr["LastName"].ToString()}\n");
                }
            }
        }

        #endregion
    }
}
