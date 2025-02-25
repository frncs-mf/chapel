﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChapelInventoryManagement
{
    public partial class Borrow : Form
    {
        private OleDbConnection con = Connections.GetConnection();
        public Borrow()
        {
            InitializeComponent();
        }

        private void loaddatagrid()
        {
            using (OleDbConnection con = Connections.GetConnection())
            {
                con.Open();

                OleDbCommand items = new OleDbCommand("SELECT * FROM Items", con);

                OleDbDataAdapter adap = new OleDbDataAdapter(items);
                DataTable dt = new DataTable();
                adap.Fill(dt);

                dgvItems.DataSource = dt;
            }
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            using (OleDbConnection con = new OleDbConnection(Connections.connection))
            {
                con.Open();

                if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtItem.Text) || string.IsNullOrEmpty(txtQty.Text) || string.IsNullOrEmpty(txtAvail.Text))
                {
                    MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (int.TryParse(txtID.Text, out int itemID))
                    {
                        OleDbCommand chkItem = new OleDbCommand("SELECT [ItemID] FROM Items WHERE [ItemID] = @ItemID", con);
                        chkItem.Parameters.AddWithValue("ItemID", itemID);

                        if (chkItem.ExecuteScalar() != null)
                        {
                            MessageBox.Show("Item is already Recorded!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            OleDbCommand add = new OleDbCommand("INSERT INTO Items ([ItemID], ItemName, Availability, Quantity) VALUES (@ItemID, @ItemName, @Availability, @Quantity)", con);
                            add.Parameters.AddWithValue("ItemID", itemID);
                            add.Parameters.AddWithValue("ItemName", txtItem.Text);
                            add.Parameters.AddWithValue("Availability", txtAvail.Text);
                            add.Parameters.AddWithValue("Quantity", txtQty.Text);
                            add.ExecuteNonQuery();

                            MessageBox.Show("Item Successfully Added!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            loaddatagrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item ID should be an integer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
            {
                using (OleDbConnection con = new OleDbConnection(Connections.connection))
                {
                    con.Open();

                    if (string.IsNullOrEmpty(txtID.Text) || string.IsNullOrEmpty(txtItem.Text) || string.IsNullOrEmpty(txtQty.Text) || string.IsNullOrEmpty(txtAvail.Text))
                    {
                        MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        try
                        {
                            // Data Validation (example for quantity)
                            int quantity;
                            if (!int.TryParse(txtQty.Text, out quantity))
                            {
                                MessageBox.Show("Invalid quantity format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            OleDbCommand chkItem = new OleDbCommand("SELECT [ItemID] FROM Items WHERE[ItemID] = @ItemID", con);
                            chkItem.Parameters.AddWithValue("ItemID", txtID.Text);

                            if (chkItem.ExecuteScalar() != null)
                            {
                                OleDbCommand update = new OleDbCommand("UPDATE Items SET ItemName = @ItemName, Quantity = @Quantity, Availability = @Availability WHERE [ItemID] = @ItemID", con);
                                update.Parameters.AddWithValue("ItemID", txtID.Text);
                                update.Parameters.AddWithValue("ItemName", txtItem.Text);
                                update.Parameters.AddWithValue("Quantity", quantity);
                                update.Parameters.AddWithValue("Availability", txtAvail.Text);
                                update.ExecuteNonQuery();

                                MessageBox.Show("Item Successfully Updated!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                loaddatagrid();
                            }
                            else
                            {
                                MessageBox.Show("Item cannot be updated! Item doesn't Exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        finally
                        {
                            con.Close(); // Ensure connection is closed
                        }
                    }
                }
            }

            private void btnClear_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtItem.Clear();
            txtQty.Clear();
            txtAvail.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            con.Open();

            OleDbCommand searchCmd = new OleDbCommand("SELECT * FROM Items WHERE ([ItemID] & ' ' & ItemName & ' ' & Availability & ' ' & Quantity) LIKE @searchString", con);
            searchCmd.Parameters.AddWithValue("searchString", "%" + txtSearch.Text + "%");
            searchCmd.ExecuteNonQuery();

            OleDbDataAdapter adap = new OleDbDataAdapter(searchCmd);
            DataTable dt = new DataTable();
            adap.Fill(dt);

            dgvItems.DataSource = dt;

            con.Close();
        }

        
    }
}