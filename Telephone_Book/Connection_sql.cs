using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Telephone_Book
{
    internal class Connection_sql
    {
        private string Conn_Sql = "Server=أحمد\\AHMAD;Database=TELEPHONE_BOOK;User Id=sa;Password=123;TrustServerCertificate=True;";

        public SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection(Conn_Sql);
            con.Open();
            return con;
        }
    }
}

