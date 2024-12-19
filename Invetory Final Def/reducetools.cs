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
    public partial class reducetools : Form
    {
        private string accessConnString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=LoginMulti.accdb;";
        public reducetools()
        {
            InitializeComponent();
        }

        private void Update_Click(object sender, EventArgs e)
        {
            int cropsid, userid, quantity;

            // Validate inputs
            if (!int.TryParse(tbCID.Text, out cropsid))
            {
                MessageBox.Show("Please enter a valid Tools ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(tbUserID.Text, out userid))
            {
                MessageBox.Show("Please enter a valid User ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(tbQuantity.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid Quantity greater than zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime changeddate = tbCD.Value;

            // Check existence of CropsID and UserID
            bool cropsExists = CropsExists(cropsid, quantity);
            bool userExists = UserExists(userid);

            if (!cropsExists)
            {
                MessageBox.Show("Tools ID does not exist or insufficient stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!userExists)
            {
                MessageBox.Show("User ID does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Add borrowing record and update crops quantity within a transaction
            AddChangedRecordAndUpdateCrops(cropsid, userid, quantity, changeddate);
        }

        private bool CropsExists(int cropsid, int quantity)
        {
            using (OleDbConnection connection = new OleDbConnection(accessConnString))
            {
                connection.Open();
                string query = "SELECT Quantity FROM Tools WHERE ToolsID = ?";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@toolsid", cropsid);

                object result = command.ExecuteScalar();
                int availableQuantity = 0;

                if (result != null && int.TryParse(result.ToString(), out availableQuantity))
                {
                    return availableQuantity >= quantity;
                }

                return false;
            }
        }

        private bool UserExists(int userid)
        {
            using (OleDbConnection connection = new OleDbConnection(accessConnString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM useracc WHERE UserID = ?";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userid);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private void AddChangedRecordAndUpdateCrops(int cropsid, int userid, int quantity, DateTime changeddate)
        {
            using (OleDbConnection connection = new OleDbConnection(accessConnString))
            {
                connection.Open();
                OleDbTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Generate a new itemid
                    int newItemId = GetNextItemId(connection, transaction);

                    // Insert borrowing record with the new itemid
                    string insertQuery = "INSERT INTO NotificationTools (ItemID, ToolsID, UserID, Quantity, Changed_Date) VALUES (?, ?, ?, ?, ?)";
                    OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection, transaction);
                    insertCommand.Parameters.AddWithValue("@itemid", newItemId);
                    insertCommand.Parameters.AddWithValue("@toolsid", cropsid);
                    insertCommand.Parameters.AddWithValue("@userid", userid);
                    insertCommand.Parameters.AddWithValue("@Quantity", quantity);
                    insertCommand.Parameters.AddWithValue("@changeddate", changeddate);
                    insertCommand.ExecuteNonQuery();

                    // Update crops quantity
                    string updateQuery = "UPDATE Tools SET Quantity = Quantity - ? WHERE ToolsID = ?";
                    OleDbCommand updateCommand = new OleDbCommand(updateQuery, connection, transaction);
                    updateCommand.Parameters.AddWithValue("@Quantity", quantity);
                    updateCommand.Parameters.AddWithValue("@toolsid", cropsid);
                    updateCommand.ExecuteNonQuery();

                    // Commit the transaction if both operations succeed
                    transaction.Commit();
                    MessageBox.Show("Record added and crops quantity updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Rollback the transaction in case of error
                    transaction.Rollback();
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Function to get the next itemid
        private int GetNextItemId(OleDbConnection connection, OleDbTransaction transaction)
        {
            string query = "SELECT MAX(ItemID) FROM NotificationCrops";
            OleDbCommand command = new OleDbCommand(query, connection, transaction);
            object result = command.ExecuteScalar();

            // If no records exist yet, start from 1
            if (result == DBNull.Value || result == null)
            {
                return 1;
            }

            // Increment the highest existing itemid by 1
            return Convert.ToInt32(result) + 1;
        }

        private void reducetools_Load(object sender, EventArgs e)
        {

        }
    }
}
