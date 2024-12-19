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
    public partial class Crops : Form
    {
        OleDbConnection conn;// OleDbConnection: Manages database connection.
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt;
        public Crops()
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
            adapter = new OleDbDataAdapter("SELECT * FROM Crops", conn);
            // Open the connection
            conn.Open();
            // Fill the DataTable with the result of the query
            adapter.Fill(dt);
            // Bind the DataTable to the DataGridView to display user information
            dgvCrops.DataSource = dt;
            // Close the database connection
            conn.Close();

        }

        private void Crops_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            GetUsers();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FillupCrops fillcrops = new FillupCrops();
            fillcrops.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            reduceCrops reducecrops = new reduceCrops();
            reducecrops.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
