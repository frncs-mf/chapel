using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChapelInventoryManagement
{
    internal class Connections
    {
        public static string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\MIGUEL\\ChapelInventoryManagement\\ChapelInventoryManagement\\aims.mdb";
        public static OleDbConnection GetConnection()
        {
            OleDbConnection con = new OleDbConnection(connection);
            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            return con;
        }
    }
}
