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

namespace Invetory_Final_Def
{
    public partial class Fill_up : Form
    {
        OleDbConnection conn;// OleDbConnection: Manages database connection.
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt;
        public Fill_up()
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
            adapter = new OleDbDataAdapter("SELECT * FROM useracc", conn);
            // Open the connection
            conn.Open();
            // Fill the DataTable with the result of the query
            adapter.Fill(dt);
            // Bind the DataTable to the DataGridView to display user information
            dgvAccount.DataSource = dt;
            // Close the database connection
            conn.Close();

        }

        private void Fill_up_Load(object sender, EventArgs e)
        {
            GetUsers();
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate textboxes and controls with data from the currently selected row
            // User ID

            tbFN.Text = dgvAccount.CurrentRow.Cells[0].Value.ToString(); // First Name
            tbLN.Text = dgvAccount.CurrentRow.Cells[1].Value.ToString(); // Last Name
            tbMN.Text = dgvAccount.CurrentRow.Cells[2].Value.ToString(); // Middle Name
            tbMI.Text = dgvAccount.CurrentRow.Cells[3].Value.ToString();   // Middle Initial
            tbUN.Text = dgvAccount.CurrentRow.Cells[4].Value.ToString();   // Username
            tbPass.Text = dgvAccount.CurrentRow.Cells[5].Value.ToString();   // Password
            tbRole.Text = dgvAccount.CurrentRow.Cells[6].Value.ToString();   // Role

            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                DataGridViewRow row = dgvAccount.Rows[e.RowIndex];

                // Populate textboxes with data from the selected row

                tbFN.Text = row.Cells["Firstname"].Value.ToString();
                tbLN.Text = row.Cells["Lastname"].Value.ToString();
                tbMN.Text = row.Cells["Middle_Name"].Value.ToString();
                tbMI.Text = row.Cells["Middle_Initial"].Value.ToString();
                tbUN.Text = row.Cells["Username"].Value.ToString();
                tbPass.Text = row.Cells["Password"].Value.ToString();
                tbRole.Text = row.Cells["Role"].Value.ToString();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Validate that all fields are filled
            if (string.IsNullOrWhiteSpace(tbFN.Text) ||
                string.IsNullOrWhiteSpace(tbLN.Text) ||
                string.IsNullOrWhiteSpace(tbMN.Text) ||
                string.IsNullOrWhiteSpace(tbMI.Text) ||
                string.IsNullOrWhiteSpace(tbUN.Text) ||
                string.IsNullOrWhiteSpace(tbPass.Text) ||
                string.IsNullOrWhiteSpace(tbRole.Text))
            {
                MessageBox.Show("Please fill in all fields."); // Display a message if any field is empty
                return; // Exit the method if any field is missing
            }

            // Define the SQL query for insertion
            string query = "INSERT INTO useracc (Firstname, Lastname, Middle_Name, Middle_Initial, Username, [Password], [Role]) " +
                           "VALUES (@fn, @ln, @mn, @mi, @un, @pass, @role)";

            // Create the command to execute the query
            using (conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb"))
            {
                try
                {
                    conn.Open(); // Open the connection
                    using (cmd = new OleDbCommand(query, conn))
                    {
                        // Add values from textboxes to the command parameters

                        cmd.Parameters.AddWithValue("@fn", tbFN.Text);    // First Name
                        cmd.Parameters.AddWithValue("@ln", tbLN.Text);    // Last Name
                        cmd.Parameters.AddWithValue("@mn", tbMN.Text);    // Middle Name
                        cmd.Parameters.AddWithValue("@mi", tbMI.Text);    // Middle Initial
                        cmd.Parameters.AddWithValue("@un", tbUN.Text);    // Username
                        cmd.Parameters.AddWithValue("@pass", tbPass.Text); // Password
                        cmd.Parameters.AddWithValue("@role", tbRole.Text); // Role

                        // Execute the query to insert data
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record saved successfully!"); // Notify the user
                    }

                    // Refresh the DataGridView to show the new data
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



        private void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFN.Text) ||
        string.IsNullOrWhiteSpace(tbLN.Text) ||
        string.IsNullOrWhiteSpace(tbMN.Text) ||
        string.IsNullOrWhiteSpace(tbMI.Text) ||
        string.IsNullOrWhiteSpace(tbUN.Text) ||
        string.IsNullOrWhiteSpace(tbPass.Text) ||
        string.IsNullOrWhiteSpace(tbRole.Text))
            {
                MessageBox.Show("Please fill in all fields."); // Display a message if any field is empty
                return; // Exit the method if any field is missing
            }

            // Ensure a row is selected in the DataGridView
            if (dgvAccount.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            // Get the ID of the selected row (assuming the first column is the primary key)
            int selectedID = Convert.ToInt32(dgvAccount.CurrentRow.Cells["UserID"].Value);

            // Define the SQL query for updating the record
            string query = "UPDATE useracc SET Firstname = @fn, Lastname = @ln, Middle_Name = @mn, Middle_Initial = @mi, " +
                           "Username = @un, [Password] = @pass, [Role] = @role WHERE UserID = @id";  // Replace 'ID' with your actual primary key column name

            // Create the command to execute the query
            using (conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb"))
            {
                try
                {
                    conn.Open(); // Open the connection
                    using (cmd = new OleDbCommand(query, conn))
                    {
                        // Add values from textboxes to the command parameters
                        cmd.Parameters.AddWithValue("@fn", tbFN.Text);     // First Name
                        cmd.Parameters.AddWithValue("@ln", tbLN.Text);     // Last Name
                        cmd.Parameters.AddWithValue("@mn", tbMN.Text);     // Middle Name
                        cmd.Parameters.AddWithValue("@mi", tbMI.Text);     // Middle Initial
                        cmd.Parameters.AddWithValue("@un", tbUN.Text);     // Username
                        cmd.Parameters.AddWithValue("@pass", tbPass.Text); // Password
                        cmd.Parameters.AddWithValue("@role", tbRole.Text); // Role
                        cmd.Parameters.AddWithValue("@id", selectedID);    // Primary key value (for WHERE clause)

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
                    // Display an error message if something goes wrong
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close(); // Ensure the connection is closed
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected in the DataGridView
            if (dgvAccount.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            // Get the ID of the selected row (assuming the first column is the primary key)
            int selectedID = Convert.ToInt32(dgvAccount.CurrentRow.Cells["UserID"].Value); // Replace "ID" with your primary key column name

            // Confirm deletion with the user
            var confirmResult = MessageBox.Show("Are you sure you want to delete this record?",
                                                "Confirm Deletion",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                // Define the SQL query to delete the record
                string query = "DELETE FROM useracc WHERE UserID = @id";  // Replace 'ID' with your actual primary key column name

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

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

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
                dgvAccount.DataSource = dt; // Reset to original data
            }
            else
            {
                // Sanitize the input to prevent errors
                string searchValue = tbSearch.Text.Replace("'", "''");

                // Apply the filter across multiple columns (case-insensitive)
                dv.RowFilter = $@"
        Convert(UserID, 'System.String') LIKE '%{searchValue}%'
        OR [Username] LIKE '%{searchValue}%'
        OR [Firstname] LIKE '%{searchValue}%'
        OR [Lastname] LIKE '%{searchValue}%'
        OR [Middle_Name] LIKE '%{searchValue}%'
        OR [Middle_Initial] LIKE '%{searchValue}%'
        OR [Role] LIKE '%{searchValue}%'";

                // Update DataGridView with the filtered data
                dgvAccount.DataSource = dv;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
