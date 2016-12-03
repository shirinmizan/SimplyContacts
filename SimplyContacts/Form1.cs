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


namespace SimplyContacts
{
    public partial class Form1 : Form
    {
        //Initialize SQLConnection and SQLCommand variables for resuablility in the program
        private SqlConnection conn = null;
        private SqlCommand cmd = null;
        //String to store current selected rows telephone number for use in different on-click methods
        private string origContact = "";

        public Form1()
        {
            InitializeComponent();
            //Fill combobox with province names
            cboProvince.Items.Add("Alberta");
            cboProvince.Items.Add("British Columbia");
            cboProvince.Items.Add("Manitoba");
            cboProvince.Items.Add("New Brunswick");
            cboProvince.Items.Add("Newfoundland and Labrador");
            cboProvince.Items.Add("Northwest Territories");
            cboProvince.Items.Add("Nova Scotia");
            cboProvince.Items.Add("Nunavut");
            cboProvince.Items.Add("Ontario");
            cboProvince.Items.Add("Prince Edward Island");
            cboProvince.Items.Add("Quebec");
            cboProvince.Items.Add("Saskatchewan");
            //Load information from database into the view
            getData();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set headers for columns in the datagrid view
            dgPhoneBook.Columns[0].HeaderText = "First Name";
            dgPhoneBook.Columns[1].HeaderText = "Last Name";
            dgPhoneBook.Columns[2].HeaderText = "Telephone #";
            dgPhoneBook.Columns[3].HeaderText = "Address";
            dgPhoneBook.Columns[4].HeaderText = "City";
            dgPhoneBook.Columns[5].HeaderText = "Postal Code";
            dgPhoneBook.Columns[6].HeaderText = "Province";

            getData();
            //Set keypress and click listeners to view objects
            dgPhoneBook.Click += dgPhoneBook_Click;
            txtFName.KeyPress += txtFName_KeyPress;
            txtTelephone.KeyPress += txtTelephone_KeyPress;
            txtLName.KeyPress += txtLName_KeyPress;
            txtAddress.KeyPress += txtAddress_KeyPress;
            txtPostalCode.KeyPress += txtPostalCode_KeyPress;
            txtCity.KeyPress += txtCity_KeyPress;
        }

        //Click listener to fill textboxes with values from the selected item in the table
        private void dgPhoneBook_Click(object sender, EventArgs e)
        {

            txtFName.Text = dgPhoneBook.CurrentRow.Cells[0].Value.ToString();
            txtLName.Text = dgPhoneBook.CurrentRow.Cells[1].Value.ToString();
            origContact = dgPhoneBook.CurrentRow.Cells[2].Value.ToString();
            txtTelephone.Text = origContact;
            txtAddress.Text = dgPhoneBook.CurrentRow.Cells[3].Value.ToString();
            txtCity.Text = dgPhoneBook.CurrentRow.Cells[4].Value.ToString();
            txtPostalCode.Text = dgPhoneBook.CurrentRow.Cells[5].Value.ToString();
            txtSetProvince.Text = dgPhoneBook.CurrentRow.Cells[6].Value.ToString();
        }

        //Connect to database and get schema. Get rows from database into datagrid view
        private void getData()
        {
            //string connstr = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\Christian\\Downloads\\SimplyContacts\\SimplyContacts\\SimplyContacts\\contacts.mdf;Integrated Security=True";

            /*string connstr = "Data Source=(LocalDB)\\v11.0;AttachDbFilename=D:\\Documents\\Sheridan\\Winter 2016\\PROG37721\\SimplyContacts-2\\SimplyContacts\\SimplyContacts\\contacts.mdf;Integrated Secutrity=True";*/

            string connstr = "Data Source=(LocalDB)\\v11.0;AttachDbFilename= E:\\Semester 5 - Winter 2016\\Web Service using .NET & C#\\SimplyContacts\\SimplyContacts\\SimplyContacts\\contacts.mdf;Integrated Security=True";


            //D:\Documents\Sheridan\Winter 2016\PROG37721\SimplyContacts-2\SimplyContacts\SimplyContacts

            try
            {
                conn = new SqlConnection(connstr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM [tContacts]";
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    bindingSource1.DataSource = dr;
                    dgPhoneBook.DataSource = bindingSource1;
                    dgPhoneBook.ClearSelection();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                if (conn != null)
                {
                    conn.Close();
                }
                MessageBox.Show(ex.Message, "Error Reading Data");
            }
        }

        //Check to make sure user can only input alpha-numeric values 
        void txtFName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            int len = ((TextBox)sender).Text.Length;
            ((TextBox)sender).SelectionStart = len;
            if (c != 8)
            {
                if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32))
                {
                    e.Handled = true;
                }
                if (len < 2)
                {
                    if (c == 32)
                    {
                        e.Handled = true;
                    }
                    else if (len == 0 && (c > 96 && c < 123))
                    {
                        e.KeyChar = (char)(c - 32);
                    }
                    else if (len > 0 && (c > 64 && c < 91))
                    {
                        e.KeyChar = (char)(c + 32);
                    }
                }
            }
        }

        //Check to make sure user can only input alpha-numeric values
        void txtLName_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            int len = ((TextBox)sender).Text.Length;
            ((TextBox)sender).SelectionStart = len;

            if (c != 8)
            {
                if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32))
                {
                    e.Handled = true;
                }
                if (len < 2)
                {
                    if (c == 32)
                    {
                        e.Handled = true;
                    }
                    else if (len == 0 && (c > 96 && c < 123))
                    {
                        e.KeyChar = (char)(c - 32);
                    }
                    else if (len > 0 && (c > 64 && c < 91))
                    {
                        e.KeyChar = (char)(c + 32);
                    }
                }
            }
        }
        //Check to make sure user can only enter numbers or dashes following the 123-456-7890 format.
        void txtTelephone_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            int len = txtTelephone.Text.Length;
            txtTelephone.SelectionStart = len;
            if (c != 8)
            {
                if (len == 3 || len == 7)
                {
                    if (c != 45)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    if (c < 48 || c > 57)
                    {
                        e.Handled = true;
                    }
                }
            }
        }


        //Check to make sure user can only input alpha-numeric values in address box
        void txtAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            int len = ((TextBox)sender).Text.Length;
            ((TextBox)sender).SelectionStart = len;
            if (c != 8)
            {
                if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32) && (c < 48 || c > 57))
                {
                    e.Handled = true;
                }
            }

        }

        void txtCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            int len = ((TextBox)sender).Text.Length;
            ((TextBox)sender).SelectionStart = len;

            if (c != 8)
            {
                if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32))
                {
                    e.Handled = true;
                }
                if (len < 2)
                {
                    if (c == 32)
                    {
                        e.Handled = true;
                    }
                    else if (len == 0 && (c > 96 && c < 123))
                    {
                        e.KeyChar = (char)(c - 32);
                    }
                    else if (len > 0 && (c > 64 && c < 91))
                    {
                        e.KeyChar = (char)(c + 32);
                    }
                }
            }
        }

        //Check to make sure users can only enter letters and numbers following the canadian postal code format
        void txtPostalCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            int len = txtPostalCode.Text.Length;
            txtPostalCode.SelectionStart = len;

            if (c != 8)
            {
                if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32) && (c < 48 || c > 57))
                {
                    e.Handled = true;
                }

                //check for d,f,i,o,q,u, key press
                else if (c == 68 || c == 100) //D, d
                {
                    e.Handled = true;
                }
                else if (c == 70 || c == 102) //F, f
                {
                    e.Handled = true;
                }
                else if (c == 73 || c == 105) // I, i
                {
                    e.Handled = true;
                }
                else if (c == 79 || c == 111) //O, o
                {
                    e.Handled = true;
                }
                else if (c == 81 || c == 113)//Q, q
                {
                    e.Handled = true;
                }
                else if (c == 85 || c == 117)//U, u
                {
                    e.Handled = true;
                }

                //check if second, fifth, and seventh characters are numbers
                if (len == 1)
                {
                    if (c < 48 || c > 57)
                    {
                        e.Handled = true;
                    }

                }
                if (len == 4)
                {
                    if (c < 48 || c > 57)
                    {
                        e.Handled = true;
                    }
                }
                if (len == 6)
                {
                    if (c < 48 || c > 57)
                    {
                        e.Handled = true;
                    }
                }

                //check if first, third, and fifth characters are letters
                if (len == 0)//*remember first character cannot be a W or Z
                {
                    if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32))
                    {
                        e.Handled = true;
                    }
                    if (c == 87 || c == 119)//W, w
                    {
                        e.Handled = true;
                    }
                    else if (c == 90 || c == 122)//Z, z
                    {
                        e.Handled = true;
                    }

                    else if (c > 96 && c < 123)
                    {
                        e.KeyChar = (char)(c - 32);
                    }

                }

                if (len == 2)
                {
                    if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32))
                    {
                        e.Handled = true;
                    }
                    else if (c > 96 && c < 123)
                    {
                        e.KeyChar = (char)(c - 32);
                    }
                }

                if (len == 5)
                {
                    if ((c < 65 || c > 90) && (c < 97 || c > 122) && (c != 32))
                    {
                        e.Handled = true;
                    }
                    else if (c > 96 && c < 123)
                    {
                        e.KeyChar = (char)(c - 32);
                    }
                }

                //check for space as third character
                if (len == 3)
                {
                    if (c != 32)//if character not a space remove
                    {
                        e.Handled = true;
                    }

                }
            }
        }

        //Insert contact into phonebook
        private void cmdInsert_Click(object sender, EventArgs e)
        {
            //confirm empty strings are not being inserted
            if (dataGood())
            {
                //confirm record with same primary key is not in the table
                if (isValidPrimaryKey())
                {

                    txtSetProvince.Text = (String)cboProvince.SelectedItem;

                    //Parameterized insert statement with parameters to add below in the try block
                    string sql = "INSERT INTO [tContacts]([firstname], [lastname], [telephone], [address], [city], [postalcode], [province]) VALUES(@firstname, @lastname, @telephone, @address, @city, @postalcode, @province) ";
                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 15).Value = txtFName.Text.ToString();
                        cmd.Parameters.Add("@lastname", SqlDbType.VarChar, 15).Value = txtLName.Text.ToString();
                        cmd.Parameters.Add("@telephone", SqlDbType.VarChar, 12).Value = txtTelephone.Text.ToString();
                        cmd.Parameters.Add("@address", SqlDbType.VarChar, 50).Value = txtAddress.Text.ToString();
                        cmd.Parameters.Add("@city", SqlDbType.VarChar, 15).Value = txtCity.Text.ToString();
                        cmd.Parameters.Add("@postalcode", SqlDbType.VarChar, 7).Value = txtPostalCode.Text.ToString();
                        cmd.Parameters.Add("@province", SqlDbType.VarChar, 25).Value = txtSetProvince.Text.ToString();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        //Refresh table
                        getData();
                    }
                    catch (SqlException ex)
                    {
                        if (conn != null)
                        {
                            conn.Close();
                        }
                        MessageBox.Show(ex.Message, "Error Inserting Record");
                    }
                }
            }
        }

        //Updates selected record with new values.
        private void cmdUpdate_Click(object sender, EventArgs e)
        {

            //confirm empty strings are not being inserted
            if (dataGood())
            {
                
                     txtSetProvince.Text = (String)cboProvince.SelectedItem;
                    String sql = "UPDATE[tContacts]SET[firstname]=@firstname,[lastname]=@lastname,[telephone]=@telephone,[address]=@address,[city]=@city,[postalcode]=@postalcode,[province]=@province WHERE[telephone]='" + origContact + "'";

                    try
                    {
                        conn.Open();
                        cmd.CommandText = sql;
                        cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 15).Value = txtFName.Text.ToString();
                        cmd.Parameters.Add("@lastname", SqlDbType.VarChar, 15).Value = txtLName.Text.ToString();
                        cmd.Parameters.Add("@telephone", SqlDbType.VarChar, 12).Value = txtTelephone.Text.ToString();
                        cmd.Parameters.Add("@address", SqlDbType.VarChar, 50).Value = txtAddress.Text.ToString();
                        cmd.Parameters.Add("@city", SqlDbType.VarChar, 15).Value = txtCity.Text.ToString();
                        cmd.Parameters.Add("@postalcode", SqlDbType.VarChar, 7).Value = txtPostalCode.Text.ToString();
                        cmd.Parameters.Add("@province", SqlDbType.VarChar, 25).Value = txtSetProvince.Text.ToString();

                        cmd.ExecuteNonQuery();
                        conn.Close();
                        getData();
                    }
                    catch (SqlException ex)
                    {
                        if (conn != null)
                        {
                            conn.Close();
                        }
                        MessageBox.Show(ex.Message, "Error Updating Record");
                   }
            }
        }
        //Delete selected row from table and update the GUI with new table state
        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Record Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                //Parameterized delete statement with paramteres to add in the try block
                string sql = "DELETE FROM [tContacts] WHERE [telephone] = @telephone";
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.Add("@telephone", SqlDbType.VarChar, 12).Value = origContact;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    getData();
                    clearText();
                }
                catch (SqlException ex)
                {
                    if (conn != null)
                    {
                        conn.Close();
                        MessageBox.Show(ex.Message, "Error Deleting Record");
                    }
                }
            }
        }
        //Search for specific record based on any fields filled in
        private void cmdSearch_Click(object sender, EventArgs e)
        {
            Boolean isFirstAppend = true;
            String sqlSelectStatement = "SELECT * FROM[tContacts] WHERE ";
            if (txtFName.Text != null && txtFName.Text != "")
            {
                sqlSelectStatement += "[firstname] = @firstname ";
                isFirstAppend = false;
            }
            if (txtLName.Text != null && txtLName.Text != "")
            {
                if (isFirstAppend)
                {
                    isFirstAppend = false;
                    sqlSelectStatement += "[lastname] = @lastname ";
                }
                else
                {
                    sqlSelectStatement += "AND [lastname] = @lastname ";
                }
            }
            if (txtTelephone.Text != null && txtTelephone.Text != "")
            {
                if (isFirstAppend)
                {
                    isFirstAppend = false;
                    sqlSelectStatement += "[telephone] = @telephone ";
                }
                else
                {
                    sqlSelectStatement += "AND [telephone] = @telephone ";
                }
            }
            if (txtAddress.Text != null && txtAddress.Text != "")
            {
                if (isFirstAppend)
                {
                    isFirstAppend = false;
                    sqlSelectStatement += "[address] = @address ";
                }
                else
                {
                    sqlSelectStatement += "AND [address] = @address ";
                }
            }
            if (txtCity.Text != null && txtCity.Text != "")
            {
                if (isFirstAppend)
                {
                    isFirstAppend = false;
                    sqlSelectStatement += "[city] = @city ";
                }
                else
                {
                    sqlSelectStatement += "AND [city] = @city ";
                }
            }
            if (txtPostalCode.Text != null && txtPostalCode.Text != "")
            {
                if (isFirstAppend)
                {
                    isFirstAppend = false;
                    sqlSelectStatement += "[postalcode] = @postalcode ";
                }
                else
                {
                    sqlSelectStatement += "AND [postalcode] = @postalcode ";
                }
            }
            if (txtSetProvince.Text != null && txtSetProvince.Text != "")
            {
                if (isFirstAppend)
                {
                    isFirstAppend = false;
                    sqlSelectStatement += "[province] = @province ";
                }
                else
                {
                    sqlSelectStatement += "AND [province] = @province ";
                }
            }
            sqlSelectStatement += ";";
            if (isFirstAppend)
            {
                MessageBox.Show("At least one field must be completed to search the Phonebook", "Empty Fields");
            }
            else
            {
                try
                {
                    cmd = new SqlCommand();
                    cmd.CommandText = sqlSelectStatement;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.Parameters.Add("@firstname", SqlDbType.VarChar, 15).Value = txtFName.Text.ToString();
                    cmd.Parameters.Add("@lastname", SqlDbType.VarChar, 15).Value = txtLName.Text.ToString();
                    cmd.Parameters.Add("@telephone", SqlDbType.VarChar, 12).Value = txtTelephone.Text.ToString();
                    cmd.Parameters.Add("@address", SqlDbType.VarChar, 50).Value = txtAddress.Text.ToString();
                    cmd.Parameters.Add("@city", SqlDbType.VarChar, 15).Value = txtCity.Text.ToString();
                    cmd.Parameters.Add("@postalcode", SqlDbType.VarChar, 7).Value = txtPostalCode.Text.ToString();
                    cmd.Parameters.Add("@province", SqlDbType.VarChar, 25).Value = txtSetProvince.Text.ToString();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        bindingSource1.DataSource = reader;
                        dgPhoneBook.DataSource = bindingSource1;
                        dgPhoneBook.ClearSelection();
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                    MessageBox.Show(ex.Message, "Error Searching Database");
                }

            }
        }
        //Simple check and warning to stop user from entering null values into the database
        private bool dataGood()
        {
            if (txtFName.Text.Length < 1)
            {
                MessageBox.Show("Firstname Required!", "Missing First Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFName.Focus();
                return false;
            }

            if (txtTelephone.Text.Length < 1)
            {
                MessageBox.Show("Telephone Number Required!", "Missing Telephone Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTelephone.Focus();
                return false;
            }

            return true;
        }

        //Checks to see if a record with the same phonenumber(primary key) exists and shows a message box for the user
        private bool isValidPrimaryKey()
        {

            for (int i = 0; i < dgPhoneBook.Rows.Count; i++)
            {

                if (txtTelephone.Text.Equals(dgPhoneBook.Rows[i].Cells[2].Value.ToString()))
                {
                    MessageBox.Show("This telephone number exists...Enter a new telephone number!", "Primary Key Violation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTelephone.Focus();
                    return false;
                }
            }

            return true;
        }
        //Clear all text fields and deselect current selected row in table
        private void clearText()
        {
            txtFName.Text = "";
            txtLName.Text = "";
            txtTelephone.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtPostalCode.Text = "";
            txtSetProvince.Text = "";
            cboProvince.Text = "";
            txtFName.Focus();
            dgPhoneBook.ClearSelection();
        }
        //Used to go back to original view from search view
        private void cmdViewAll_Click(object sender, EventArgs e)
        {
            getData();
            clearText();
        }
        //Button listener for clear all button. Clears all text fields.
        private void cmdClearFields_Click(object sender, EventArgs e)
        {
            clearText();
        }
    }
}