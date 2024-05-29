using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace ChapelInventoryManagement
{
    public partial class frmLogin : Form    
    {
        private OleDbConnection con = Connections.GetConnection();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            con.Open();

            string username = txtUser.Text;
            string password = txtPass.Text;

            OleDbCommand retrieve = new OleDbCommand("SELECT Username, Password FROM Users WHERE Username = @Username AND Password = @Password", con);
            retrieve.Parameters.AddWithValue("Username", username);
            retrieve.Parameters.AddWithValue("Password", password);
            OleDbDataReader reader = retrieve.ExecuteReader();

            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (reader.Read())
                    {
                        this.tmr.Start();   

                    }
                    else
                    {
                        DialogResult response = MessageBox.Show("Username and Password doesn't match!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }


            con.Close();
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            this.progressBar.Increment(3);

            if (progressBar.Value >= progressBar.Maximum)
            {
                tmr.Stop();
                this.Hide();
                frmItems i = new frmItems();
                i.Show();
            }
        }

    }
}








