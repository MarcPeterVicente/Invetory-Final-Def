using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Invetory_Final_Def
{
    public partial class Login : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;

        private int loginAttempts = 0; // Counter for login attempts
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Establish the connection string to connect to the Access database
            conn = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0;Data Source=LoginMulti.accdb");

            // SQL query to check if the username and password match and retrieve the user type
            string query = "SELECT UserId, [Role] FROM useracc WHERE Username = @username AND [Password] = @password";

            // Create and configure the command
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", tbUsername.Text);
            cmd.Parameters.AddWithValue("@password", tbPassword.Text);

            try
            {
                // Open the connection
                conn.Open();

                // Execute the query and get the result
                object result = cmd.ExecuteScalar();
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    // The user exists, and we have retrieved the Type
                    string userType = reader.GetString(1);

                    // Format DateTime.Now to remove milliseconds
                    string formattedTimeIn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // Insert the TimeIn value as Date/Time in LogTable
                    string insertLogQuery = "INSERT INTO LogTable (UserID, TimeIn) VALUES (@userId, @timeIn)";
                    OleDbCommand insertCmd = new OleDbCommand(insertLogQuery, conn);
                    insertCmd.Parameters.AddWithValue("@userId", userId);
                    insertCmd.Parameters.AddWithValue("@timeIn", formattedTimeIn); // Use formatted time without milliseconds
                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Login Successfully");


                    // Check the user type and open the corresponding form
                    this.Hide();

                    if (userType == "Admin")
                    {
                        Administrative admin1 = new Administrative(userId);  // Admin Form
                        admin1.Show();
                    }
                    else if (userType == "Staff")
                    {
                        Form1 form1 = new Form1(userId);  // Student Form
                        form1.Show();
                    }
                    else
                    {
                        MessageBox.Show("Unrecognized user type.");
                        this.Show(); // Show the login form again if type is unrecognized
                    }
                }
                else
                {
                    // Increment the login attempts and show an error message
                    loginAttempts++;
                    MessageBox.Show("Invalid username or password.");

                    if (loginAttempts >= 3)
                    {
                        // Close the application if 3 failed login attempts
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close the connection
                conn.Close();

            }
        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
