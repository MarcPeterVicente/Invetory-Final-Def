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
    public partial class Tools : Form
    {
        OleDbConnection conn;// OleDbConnection: Manages database connection.
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt;
        public Tools()
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
            adapter = new OleDbDataAdapter("SELECT * FROM Tools", conn);
            // Open the connection
            conn.Open();
            // Fill the DataTable with the result of the query
            adapter.Fill(dt);
            // Bind the DataTable to the DataGridView to display user information
            dgvTools.DataSource = dt;
            // Close the database connection
            conn.Close();

        }

        private void dgvTools_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Tools_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            GetUsers();
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
                dgvTools.DataSource = dt; // Reset to original data
            }
            else
            {
                // Sanitize the input to prevent errors
                string searchValue = tbSearch.Text.Replace("'", "''");

                // Apply the filter across multiple columns (case-insensitive)
                dv.RowFilter = $@"
        Convert(CropsID, 'System.String') LIKE '%{searchValue}%'
        OR [Category] LIKE '%{searchValue}%'
        OR [Item] LIKE '%{searchValue}%'";


                // Update DataGridView with the filtered data
                dgvTools.DataSource = dv;
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            FillupTools fillupTools = new FillupTools();
            fillupTools.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void QT_Click(object sender, EventArgs e)
        {
            reducetools reducetools = new reducetools();
            reducetools.Show();
        }
    }
}
