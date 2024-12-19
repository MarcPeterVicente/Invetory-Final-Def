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
    public partial class Administrative : Form
    {
        private OleDbConnection conn;
        private int UserId;
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt; // DataTable: Stores data in-memory, can be bound to controls like DataGridView.

        ManageAccount account;
        ManageActivity activity;

        public Administrative(int userId)
        {
            InitializeComponent();
            this.UserId = userId;
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb;");


        }

        private void Administrative_Load(object sender, EventArgs e)
        {

        }
        
        private void tbAccount_Click_1(object sender, EventArgs e)
        {

            if (account == null)
            {
                account = new ManageAccount();
                account.FormClosed += Account_FormClosed;
                account.MdiParent = this;
                account.Dock = DockStyle.Fill;
                account.Show();

            }
            else
            {
                account.Activate();
            }
            
           
        }
        private void Account_FormClosed(object sender, FormClosedEventArgs e)
        {
            account = null;
        }

        private void tbActivity_Click_1(object sender, EventArgs e)
        {
            if (activity == null)
            {
                activity = new ManageActivity();
                activity.FormClosed += Activity_FormClosed;
                activity.MdiParent = this;
                activity.Dock = DockStyle.Fill;
                activity.Show();

            }
            else
            {
                activity.Activate();
            }
        }
        private void Activity_FormClosed(object sender, FormClosedEventArgs e)
        {
            activity = null;
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            conn.Open();

            // Format DateTime.Now to remove milliseconds
            string formattedTimeOut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Update the LogTable with TimeOut
            string updateLogQuery = "UPDATE LogTable SET TimeOut = @timeOut WHERE UserID = @userId AND TimeOut IS NULL";
            OleDbCommand updateCmd = new OleDbCommand(updateLogQuery, conn);
            updateCmd.Parameters.AddWithValue("@timeOut", formattedTimeOut);
            updateCmd.Parameters.AddWithValue("@userId", UserId);

            updateCmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Logout Successful.");


            this.Hide();
            Login login1 = new Login();
            login1.Show();
        }
        bool sidebarExpand = false;
        private void SidebarTrans_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                Sidebar.Width -= 5;
                if (Sidebar.Width <= 45)
                {
                    sidebarExpand = false;
                    SidebarTrans.Stop();




                }
            }
            else
            {
                Sidebar.Width += 5;
                if (Sidebar.Width >= 182)
                {
                    sidebarExpand = true;
                    SidebarTrans.Stop();




                }
            }
        }

        

        private void picturmenu_Click(object sender, EventArgs e)
        {
            SidebarTrans.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1(UserId);
            form1.Show();
        }
    }
}
