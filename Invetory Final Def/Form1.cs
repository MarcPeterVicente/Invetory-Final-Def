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
    public partial class Form1 : Form
    {
        private OleDbConnection conn;
        private int userId;


        //forms
        Dashboard Dashboard;
        Inventory inventory;
        Crops crops;
        Livestock livestock;
        Tools tools;
        Supply supply;
        

        public Form1(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb;");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        bool sidebarExpand = true;
        private void siderbartrans_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                Sidebar.Width -= 5;
                if (Sidebar.Width <= 40)
                {
                    sidebarExpand = false;
                    siderbartrans.Stop();




                }
            }
            else
            {
                Sidebar.Width += 5;
                if (Sidebar.Width >= 162)
                {
                    sidebarExpand = true;
                    siderbartrans.Stop();




                }
            }
        }

        private void picturmenu_Click(object sender, EventArgs e)
        {
            siderbartrans.Start();
        }

        //bool inventoryExpand = false;//InvetoryTransition
        /*private void inventorytrans_Tick(object sender, EventArgs e)
        {
            if (inventoryExpand == false)
            {
                Inventorybar.Height += 10;
                if (Inventorybar.Height >= 189)
                {
                    inventorytrans.Stop();
                    inventoryExpand = true;

                }

            }
            else
            {
                Inventorybar.Height -= 10;
                if (Inventorybar.Height <= 38)
                {
                    inventorytrans.Stop();
                    inventoryExpand = false;
                }
            }
        }*/

        private void btnInvetory_Click(object sender, EventArgs e)
        {
            //inventorytrans.Start();

            if (inventory == null)
            {
                inventory = new Inventory();
                inventory.FormClosed += Inventory_FormClosed;
                inventory.MdiParent = this;
                inventory.Dock = DockStyle.Fill;
                inventory.Show();

            }
            else
            {
                inventory.Activate();
            }
        }
        private void Inventory_FormClosed(object sender, FormClosedEventArgs e)
        {
            inventory = null;
        }


        private void btndashboard_Click(object sender, EventArgs e)
        {
            if (Dashboard == null)
            {
                Dashboard = new Dashboard();
                Dashboard.FormClosed += Dashboard_FormClosed;
                Dashboard.MdiParent = this;
                Dashboard.Dock = DockStyle.Fill;
                Dashboard.Show();

            }
            else
            {
                Dashboard.Activate();
            }

            
        }
        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dashboard = null;
        }


        private void btncrops_Click(object sender, EventArgs e)
        {
            

            if (crops == null)
            {
                crops = new Crops();
                crops.FormClosed += Crops_FormClosed;
                crops.MdiParent = this;
                crops.Dock = DockStyle.Fill;
                crops.Show();

            }
            else
            {
                crops.Activate();
            }
        }
        private void Crops_FormClosed(object sender, FormClosedEventArgs e)
        {
            crops = null;
        }

        private void btnlivestock_Click(object sender, EventArgs e)
        {
            if (livestock == null)
            {
                livestock = new Livestock();
                livestock.FormClosed += Livestock_FormClosed;
                livestock.MdiParent = this;
                livestock.Dock = DockStyle.Fill;
                livestock.Show();

            }
            else
            {
                livestock.Activate();
            }
        }
        private void Livestock_FormClosed(object sender, FormClosedEventArgs e)
        {
            livestock = null;
        }

        private void btntools_Click(object sender, EventArgs e)
        {
            if (tools == null)
            {
                tools = new Tools();
                tools.FormClosed += Tools_FormClosed;
                tools.MdiParent = this;
                tools.Dock = DockStyle.Fill;
                tools.Show();

            }
            else
            {
                tools.Activate();
            }
        }
        private void Tools_FormClosed(object sender, FormClosedEventArgs e)
        {
            tools = null;
        }

        private void btnsupply_Click(object sender, EventArgs e)
        {
            if (supply == null)
            {
                supply = new Supply();
                supply.FormClosed += Supply_FormClosed;
                supply.MdiParent = this;
                supply.Dock = DockStyle.Fill;
                supply.Show();

            }
            else
            {
                supply.Activate();
            }
        }
        private void Supply_FormClosed(object sender, FormClosedEventArgs e)
        {
            supply = null;
        }

        private void Inventorybar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            conn.Open();

            // Format DateTime.Now to remove milliseconds
            string formattedTimeOut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Update the LogTable with TimeOut
            string updateLogQuery = "UPDATE LogTable SET TimeOut = @timeOut WHERE UserID = @userId AND TimeOut IS NULL";
            OleDbCommand updateCmd = new OleDbCommand(updateLogQuery, conn);
            updateCmd.Parameters.AddWithValue("@timeOut", formattedTimeOut);
            updateCmd.Parameters.AddWithValue("@userId", userId);

            updateCmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Logout Successful.");
            

            this.Hide();
            Login login1 = new Login();
            login1.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            conn.Open();

            // Format DateTime.Now to remove milliseconds
            string formattedTimeOut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Update the LogTable with TimeOut
            string updateLogQuery = "UPDATE LogTable SET TimeOut = @timeOut WHERE UserID = @userId AND TimeOut IS NULL";
            OleDbCommand updateCmd = new OleDbCommand(updateLogQuery, conn);
            updateCmd.Parameters.AddWithValue("@timeOut", formattedTimeOut);
            updateCmd.Parameters.AddWithValue("@userId", userId);

            updateCmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Logout Successful.");


            this.Hide();
            Login login1 = new Login();
            login1.Show();
        }
    }
}
