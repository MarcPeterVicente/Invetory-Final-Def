using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Invetory_Final_Def
{
    public partial class ManageActivity : Form
    {
        OleDbConnection conn;// OleDbConnection: Manages database connection.
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt;
        public ManageActivity()
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
            adapter = new OleDbDataAdapter("SELECT * FROM LogTable", conn);
            // Open the connection
            conn.Open();
            // Fill the DataTable with the result of the query
            adapter.Fill(dt);
            // Bind the DataTable to the DataGridView to display user information
            dgvActivity.DataSource = dt;
            // Close the database connection
            conn.Close();

        }

        private void ManageActivity_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            GetUsers();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            
            // Check if DataTable has data
            if (dt == null || dt.Rows.Count == 0)
                return;

            // Create a DataView for filtering
            DataView dv = dt.DefaultView;

            // Check if the search box is empty, and reset if so
            if (string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                dgvActivity.DataSource = dt; // Reset to original data
                return;
            }

            // Filter rows based on LogID or UserID (case-insensitive)
            dv.RowFilter = $"Convert(LogID, 'System.String') LIKE '%{tbSearch.Text}%' OR " +
                           $"Convert(UserID, 'System.String') LIKE '%{tbSearch.Text}%'";

            // Update DataGridView with the filtered data
            dgvActivity.DataSource = dv;
            
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected in the DataGridView
            if (dgvActivity.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            // Get the ID of the selected row (assuming the first column is the primary key)
            int selectedID = Convert.ToInt32(dgvActivity.CurrentRow.Cells["UserID"].Value); // Replace "ID" with your primary key column name

            // Confirm deletion with the user
            var confirmResult = MessageBox.Show("Are you sure you want to delete this record?",
                                                "Confirm Deletion",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                // Define the SQL query to delete the record
                string query = "DELETE FROM LogTable WHERE UserID = @id";  // Replace 'ID' with your actual primary key column name

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

        private void dgvActivity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tbUserID.Text = dgvActivity.CurrentRow.Cells[0].Value.ToString();
        }
    }
}
