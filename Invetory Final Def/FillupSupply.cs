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

namespace Invetory_Final_Def
{
    public partial class FillupSupply : Form
    {
        OleDbConnection conn;// OleDbConnection: Manages database connection.
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt;
        public FillupSupply()
        {
            InitializeComponent();
        }
        void GetUsers()
        {
            // Establish the connection string to connect to the Access database
            conn = new OleDbConnection("Provider= Microsoft.ACE.OleDb.12.0;Data Source=LoginMulti.accdb");
            // Initialize the DataTable to hold user data
            dt = new DataTable();
            // Set up an adapter to run the query and fetch the user data
            adapter = new OleDbDataAdapter("SELECT * FROM Supply", conn);
            // Open the connection
            conn.Open();
            // Fill the DataTable with the result of the query
            adapter.Fill(dt);
            // Bind the DataTable to the DataGridView to display user information
            dgvSupply.DataSource = dt;
            // Close the database connection
            conn.Close();

        }

        private void FillupSupply_Load(object sender, EventArgs e)
        {
            GetUsers();
        }

        private void dgvSupply_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dgvSupply.Rows[e.RowIndex];

                // Populate textboxes with data from the selected row
                tbCategory.Text = row.Cells["Category"].Value.ToString(); // Category
                tbItem.Text = row.Cells["Item"].Value.ToString(); // Item
                tbQuantity.Text = row.Cells["Quantity"].Value.ToString(); // Quantity
                tbLoc.Text = row.Cells["Location"].Value.ToString();   // Location

                // Convert and display Obtain_Date and Expiration_Date
                if (row.Cells["Obtain_Date"].Value != DBNull.Value)
                {
                    DateTime obtainDate = Convert.ToDateTime(row.Cells["Obtain_Date"].Value);
                    tbOD.Text = obtainDate.ToString("MM/dd/yyyy");  // Format as MM/DD/YYYY
                }

                if (row.Cells["Expiration_Date"].Value != DBNull.Value)
                {
                    string expValue = row.Cells["Expiration_Date"].Value.ToString();

                    // Check if the value is a date
                    if (DateTime.TryParse(expValue, out DateTime expDate))
                    {
                        tbEX.Text = expDate.ToString("MM/dd/yyyy"); // Format as MM/DD/YYYY
                    }
                    else
                    {
                        tbEX.Text = expValue; // Load raw text if not a date
                    }
                }
                else
                {
                    tbEX.Text = ""; // Handle null values
                }

                tbNotes.Text = row.Cells["Notes"].Value.ToString(); // Notes
                tbCost.Text = row.Cells["Item_Cost"].Value.ToString();
                tbValue.Text = row.Cells["Item_Value"].Value.ToString();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Validate that all fields are filled
            if (string.IsNullOrWhiteSpace(tbCategory.Text) ||
                string.IsNullOrWhiteSpace(tbItem.Text) ||
                string.IsNullOrWhiteSpace(tbQuantity.Text) ||  // Treating quantity as text
                string.IsNullOrWhiteSpace(tbLoc.Text) ||
                string.IsNullOrWhiteSpace(tbOD.Text) ||
                string.IsNullOrWhiteSpace(tbEX.Text) ||
                string.IsNullOrWhiteSpace(tbNotes.Text) ||
                string.IsNullOrWhiteSpace(tbCost.Text) ||
                string.IsNullOrWhiteSpace(tbValue.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Validate Obtain_Date
            if (!DateTime.TryParse(tbOD.Text, out DateTime obtainDate))
            {
                MessageBox.Show("Obtain Date must be a valid date (format: MM/DD/YYYY).");
                return;
            }

            // No validation for Expiration_Date — it can be any text or a date
            string expirationDate = tbEX.Text; // Store as a string

            // SQL query for insertion
            string query = "INSERT INTO Supply (Category, Item, Quantity, Location, Obtain_Date, Expiration_Date, Notes, Item_Cost, Item_Value) " +
                           "VALUES (@category, @item, @quantity, @location, @obtainDate, @expirationDate, @notes, @itemcost, @itemvalue)";

            using (conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb"))
            {
                try
                {
                    conn.Open();
                    using (cmd = new OleDbCommand(query, conn))
                    {
                        // Add values from textboxes to the command parameters
                        cmd.Parameters.AddWithValue("@category", tbCategory.Text);
                        cmd.Parameters.AddWithValue("@item", tbItem.Text);
                        cmd.Parameters.AddWithValue("@quantity", tbQuantity.Text);
                        cmd.Parameters.AddWithValue("@location", tbLoc.Text);
                        cmd.Parameters.AddWithValue("@obtainDate", obtainDate); // Parsed date
                        cmd.Parameters.AddWithValue("@expirationDate", expirationDate); // Raw text or date string
                        cmd.Parameters.AddWithValue("@notes", tbNotes.Text);
                        cmd.Parameters.AddWithValue("@itemcost", tbCost.Text);
                        cmd.Parameters.AddWithValue("@itemvalue", tbValue.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved successfully!");
                    }

                    GetUsers(); // Refresh DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            // Validate that all fields are filled
            if (string.IsNullOrWhiteSpace(tbCategory.Text) ||
                string.IsNullOrWhiteSpace(tbItem.Text) ||
                string.IsNullOrWhiteSpace(tbQuantity.Text) ||
                string.IsNullOrWhiteSpace(tbLoc.Text) ||
                string.IsNullOrWhiteSpace(tbOD.Text) ||
                string.IsNullOrWhiteSpace(tbEX.Text) ||
                string.IsNullOrWhiteSpace(tbNotes.Text) ||
                string.IsNullOrWhiteSpace(tbCost.Text) ||
                string.IsNullOrWhiteSpace(tbValue.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Validate Obtain_Date
            if (!DateTime.TryParse(tbOD.Text, out DateTime obtainDate))
            {
                MessageBox.Show("Obtain_Date must be a valid date (format: MM/DD/YYYY).");
                return;
            }

            // No validation for Expiration_Date — it can be any text or a date
            string expirationDate = tbEX.Text; // Treat as string

            // Ensure a row is selected in the DataGridView
            if (dgvSupply.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            // Get the primary key of the selected row (assuming it's in the first column)
            int selectedID = Convert.ToInt32(dgvSupply.CurrentRow.Cells["ToolsID"].Value); // Replace "ToolsID" with your actual primary key column name

            // Define the SQL query for updating the record
            string query = "UPDATE Supply SET Category = @category, Item = @item, Quantity = @quantity, Location = @location, " +
                           "Obtain_Date = @obtainDate, Expiration_Date = @expirationDate, Notes = @notes, Item_Cost = @itemcost, Item_Value = @itemvalue " +
                           "WHERE SupplyID = @id";

            using (conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb"))
            {
                try
                {
                    conn.Open();
                    using (cmd = new OleDbCommand(query, conn))
                    {
                        // Add values from textboxes to the command parameters
                        cmd.Parameters.AddWithValue("@category", tbCategory.Text);
                        cmd.Parameters.AddWithValue("@item", tbItem.Text);
                        cmd.Parameters.AddWithValue("@quantity", tbQuantity.Text);  // Treating quantity as text
                        cmd.Parameters.AddWithValue("@location", tbLoc.Text);
                        cmd.Parameters.AddWithValue("@obtainDate", obtainDate);     // Parsed date
                        cmd.Parameters.AddWithValue("@expirationDate", expirationDate); // Raw text or date string
                        cmd.Parameters.AddWithValue("@notes", tbNotes.Text);
                        cmd.Parameters.AddWithValue("@itemcost", tbCost.Text);
                        cmd.Parameters.AddWithValue("@itemvalue", tbValue.Text);
                        cmd.Parameters.AddWithValue("@id", selectedID);  // Primary key value (for WHERE clause)

                        // Execute the query to update the data
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record updated successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No record was updated. Please check your selection.");
                        }
                    }

                    // Refresh the DataGridView to show the updated data
                    GetUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected in the DataGridView
            if (dgvSupply.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            // Get the ID of the selected row (assuming the first column is the primary key)
            int selectedID = Convert.ToInt32(dgvSupply.CurrentRow.Cells["SupplyID"].Value); // Replace "ID" with your primary key column name

            // Confirm deletion with the user
            var confirmResult = MessageBox.Show("Are you sure you want to delete this record?",
                                                "Confirm Deletion",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                // Define the SQL query to delete the record
                string query = "DELETE FROM Tools WHERE SupplyID = @id";  // Replace 'ID' with your actual primary key column name

                // Create the command to execute the query
                using (conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb"))
                {
                    try
                    {
                        conn.Open(); // Open the connection
                        using (cmd = new OleDbCommand(query, conn))
                        {
                            // Add the ID parameter to the command
                            cmd.Parameters.AddWithValue("@id", selectedID);

                            // Execute the query to delete the data
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record deleted successfully!");
                            }
                            else
                            {
                                MessageBox.Show("No record was deleted. Please check your selection.");
                            }
                        }

                        // Refresh the DataGridView to show the updated data
                        GetUsers();
                    }
                    catch (Exception ex)
                    {
                        // Display an error message if something goes wrong
                        MessageBox.Show("Error: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close(); // Ensure the connection is closed
                    }
                }
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            // Create a DataView for filtering
            DataView dv = dt.DefaultView;

            // Check if the search box is empty
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                dgvSupply.DataSource = dt; // Reset to original data
            }
            else
            {
                // Sanitize the input to prevent errors
                string searchValue = tbSearch.Text.Replace("'", "''");

                // Apply the filter across multiple columns (case-insensitive)
                dv.RowFilter = $@"
        Convert(ToolsID, 'System.String') LIKE '%{searchValue}%'
        OR [Category] LIKE '%{searchValue}%'
        OR [Item] LIKE '%{searchValue}%'";
                // Update DataGridView with the filtered data
                dgvSupply.DataSource = dv;
            }
        }
    }
}