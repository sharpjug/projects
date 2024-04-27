using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Specialized;

namespace SQL_Database
{
    public partial class SQLTable : Form
    {
        public SQLTable()
        {
            InitializeComponent();
            btnAdd.Enabled = false;
            btnRemove.Enabled = false;
            txtX.Enabled = false;
            txtY.Enabled = false;
            Txt3.Enabled = false;
            txtSearch.Enabled = false;
        }

        public class Variables
        {
            public static int ID;
            public static string Surname;
            public static string First;
            public static string Birthday;
            public static int ilock = 0;
            public static int fieldlock = 0;
        }

        // Database variables
        public static string database_path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Database.mdf");
        public static string connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + '"' + database_path + '"' + ";Integrated Security=True";
        DataSet ds = new DataSet();
        SqlConnection cn = new SqlConnection(connection);

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Trying to connect
                cn.Open();
                cn.Close();
                MessageBox.Show("You are connected with the database.", "Connected!", MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
                TableConnect();
                // If connected enable button functions
                btnConnect.Enabled = false;
                txtX.Enabled = true;
                txtY.Enabled = true;
                btnAdd.Enabled = true;
                btnRemove.Enabled = true;
                Txt3.Enabled = true; 
                txtSearch.Enabled = true;
            }
            catch
            {
                MessageBox.Show("You are not connected with the database.", "Not Connected!", MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FieldCheck();
            if(Variables.fieldlock == 1)
            {
                //try
                //{
                    // Get the most recently deleted id or a brand new one and add
                    int new_id = Get_ID();
                    SqlCommand cmd = new SqlCommand("INSERT INTO SQL (Id, First, Surname, Birthday) VALUES (@Id, @First, @Surname, @Birthday)", cn);
                    cmd.Parameters.AddWithValue("@Id", new_id);
                    cmd.Parameters.AddWithValue("@First", txtX.Text);
                    cmd.Parameters.AddWithValue("@Surname", txtY.Text);
                    cmd.Parameters.AddWithValue("@Birthday", Txt3.Text);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("You have added a row.", "Added!", MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
                    // Reshow the data and wipe inputs
                    TableConnect();
                    txtX.Text = string.Empty;
                    txtY.Text = string.Empty;
                    Txt3.Text = string.Empty;
                //}
                //catch
                //{
                //    MessageBox.Show("You haven't inputted a correct date!", "Error!", MessageBoxButtons.OK,
                //        MessageBoxIcon.Error);
                //    Txt3.Text = string.Empty;
                //}
            }
            else
            {
                MessageBox.Show("You haven't inputted all of the fields", "Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                txtX.Text = string.Empty;
                txtY.Text = string.Empty;
                Txt3.Text = string.Empty;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //try
            //{
                foreach (DataGridViewRow row in this.dataGridView1.SelectedRows) // Gather info on what to delete
                {
                    Variables.ID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    Variables.First = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                    Variables.Surname = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    Variables.Birthday = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                }
                //try
                //{
                    // Connect to database and delete the row
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM " + "SQL" + " WHERE " + "First" + " = '" + Variables.First + "' AND " + "Surname" + " = '" + Variables.Surname + "' AND " + "Birthday" + " = '" + Variables.Birthday + "'", cn);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Row has been removed!");
                    // Store the recent ids that was deleted and reshow table
                    Deleted_ID(Variables.ID);
                    TableConnect();
                //}
                //catch
                //{
                //    MessageBox.Show("Row hasn't been removed! Please select a row!");
                //}
            //}
            //catch
            //{
            //    MessageBox.Show("An error occurred, please try again.");
            //}
        }

        private void TableConnect() // Table connector; linking database to dataset that links to datagridview
        {
            ds.Reset();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            cn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from SQL", cn);
            adapter.Fill(ds, "SQL");
            dataGridView1.VirtualMode = false;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            cn.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Using dataview to view data
            DataView DV = new DataView(ds.Tables[0]);
            DV.RowFilter = string.Format("First LIKE '%{0}%' OR Surname LIKE '%{1}%' OR Birthday LIKE '%{2}%'", txtSearch.Text, txtSearch.Text, txtSearch.Text);
            dataGridView1.DataSource = DV;
        }

        private void txtX_TextChanged(object sender, EventArgs e)
        {
            // Simple checker to make sure only valid characters are put in
            if(txtX.Text.Length > 0)
            {
                foreach (char x in txtX.Text)
                {
                    bool y = char.IsLetter(x);
                    if (y == false)
                    {
                        int length = txtX.Text.Length;
                        txtX.Text = txtX.Text.Substring(0, length - 1);
                        txtX.SelectionStart = txtX.TextLength;
                    }
                }
            }
        }

        private void txtY_TextChanged(object sender, EventArgs e)
        {
            // Simple checker to make sure only valid characters are put in
            if (txtY.Text.Length > 0)
            {
                foreach (char x in txtY.Text)
                {
                    bool y = char.IsLetter(x);
                    if (y == false)
                    {
                        int length = txtY.Text.Length;
                        txtY.Text = txtY.Text.Substring(0, length - 1);
                        txtY.SelectionStart = txtY.TextLength;
                    }
                }
            }
        }

        private void FieldCheck()
        {
            // Checking for input that all fields are filled
            if(txtX.Text != string.Empty & txtY.Text != string.Empty & Txt3.Text != string.Empty)
            {
                Variables.fieldlock = 1;
            }
            else
            {
                Variables.fieldlock = 0;
            }
        }

        void Save_StringCollection(List<string> list)
        {
            // Making collection and saving it to properties
            StringCollection collection = new StringCollection();
            collection.AddRange(list.ToArray());
            Properties.Settings.Default.DeletedIDs = collection;
            Properties.Settings.Default.Save();
        }

        void Deleted_ID(int id)
        {
            // Adding a deleted id to the deleted ids
            List<string> list = new List<string>();
            if(Properties.Settings.Default.DeletedIDs != null)
            {
                list = Properties.Settings.Default.DeletedIDs.Cast<string>().ToList();
            }

            list.Add(id.ToString());
            Save_StringCollection(list);
        }

        int Get_ID()
        {
            // If none then new id is total + 1
            if (Properties.Settings.Default.DeletedIDs == null) { return dataGridView1.Rows.Count + 1; }

            // Get recently deleted IDs if any
            var list = Properties.Settings.Default.DeletedIDs.Cast<string> ().ToList();

            if(list.Count == 0) { return 1; } // if count is nothing then it's first row

            // otherwise find most recently deleted id and return it
            var temp_id = int.Parse(list[0]);
            list.RemoveAt(0);
            Save_StringCollection(list);
            return temp_id;

        }
    }
}
