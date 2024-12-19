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
    public partial class ManageAccount : Form
    {
        OleDbConnection conn;// OleDbConnection: Manages database connection.
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt;
        public ManageAccount()
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

        private void ManageAccount_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            GetUsers();
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Fill_up fillup = new Fill_up();
            fillup.Show();
        }
    }
}
