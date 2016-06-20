using System;
using System.Collections.Generic;
using System.Configuration;
using C = System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication8
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var connection = new C.SqlConnection(GetConnectionStringByName("harriConnection"));
            connection.Open();
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            var command = new C.SqlCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = @"SELECT TOP 10 URL, SUM (Points) as pSum FROM Pages GROUP BY URL ORDER BY pSum DESC ";
            command.Connection = connection;
            C.SqlDataReader reader = command.ExecuteReader();
            object[] values = new object[2];
            List<String> currentValues = new List<string>();
            C.SqlDataReader reader2;
            while (reader.Read())
            {
                tr = new TableRow();
                int fieldCount = reader.GetValues(values);
                tc = new TableCell();
                tc.HorizontalAlign = HorizontalAlign.Center;
                tc.Controls.Add(new LiteralControl(values[0].ToString()));
                tr.Controls.Add(tc);
                string[] keys = { " Title", "Description", "Owner", "Category" };
                foreach (string key in keys)
                {
                    tc = new TableCell();
                    tc.HorizontalAlign = HorizontalAlign.Center;
                    var command2 = new C.SqlCommand();
                    command2.CommandType = System.Data.CommandType.Text;
                    command2.Connection = connection;
                    command2.CommandText = @"SELECT DISTINCT " + key + " FROM Pages WHERE URL = " + values[0];
                    reader2 = command2.ExecuteReader();
                    DropDownList ddl = new DropDownList();
                    while (reader2.Read())
                    {
                        ddl.Items.Add(reader2.GetString(0));
                    }
                    tc.Controls.Add(ddl);
                    tr.Controls.Add(tc);
                    
                }

                    tc = new TableCell();
                    tc.HorizontalAlign = HorizontalAlign.Center;

                    tc.Controls.Add(new LiteralControl(values[1].ToString()));
                    tr.Controls.Add(tc);
                resultsTable.Controls.Add(tr);
            }
           
        }
        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
    }
}