using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            getTableNames();
            label2.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
        }

        private void getTableNames()
        {

            try
            {
                Sql.connection.Open();
                string query = "select chitName from viewGroup;";
                SqlCommand cmd = new SqlCommand(query, Sql.connection);
                SqlDataReader reader = cmd.ExecuteReader();
                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString(0));
                }
            }
            catch (Exception sqlex)
            {
                MessageBox.Show(sqlex.Message);
            }
            Sql.connection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillamtdue();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fillbaldue();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    Sql.connection.Open();
                    string name = textBox1.Text.ToLower().ToString() + @"%";
                    string query = "select ticketNo, name, month, " +
                        " bidAmount, amountPaid from " + comboBox1.SelectedItem.ToString() + " " +
                        " where amountPaid = 0 and bidAmount > 0 and name LIKE '" + name + "';";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                    DataTable ds = new DataTable();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds;

                }
            }
            catch (Exception search)
            {
                MessageBox.Show(search.Message);
            }

            Sql.connection.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    Sql.connection.Open();
                    string name = textBox2.Text.ToLower().ToString() + @"%";
                    string query = "select ticketNo, name, month, " +
                        " bidAmount, balance from " + comboBox1.SelectedItem.ToString() + " " +
                        " where balance > 0 and bidAmount > 0 and name LIKE '" + name + "';";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                    DataTable ds = new DataTable();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds;

                }
            }
            catch (Exception search)
            {
                MessageBox.Show(search.Message);
            }

            Sql.connection.Close();
        }

        private void fillamtdue()
        {
            try
            {
                Sql.connection.Open();
                if (comboBox1.SelectedIndex > -1)
                {
                    string query = "select ticketNo, name, month, " +
                        " bidAmount, amountPaid from " + comboBox1.SelectedItem.ToString() + " " +
                        " where amountPaid = 0 and bidAmount > 0 ;";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                    DataTable ds = new DataTable();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds;
                    label2.Visible = true;
                    textBox1.Visible = true;
                    textBox2.Visible = false;
                }
                else
                {
                    MessageBox.Show("Please select chit name");
                }

            }
            catch (Exception fill)
            {
                MessageBox.Show(fill.Message);
            }

            Sql.connection.Close();
        }

        private void fillbaldue()
        {
            try
            {
                Sql.connection.Open();
                if (comboBox1.SelectedIndex > -1)
                {
                    string query = "select ticketNo, name, month, " +
                        " bidAmount, balance from " + comboBox1.SelectedItem.ToString() + " " +
                        " where balance > 0 and bidAmount > 0 ;";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                    DataTable ds = new DataTable();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds;
                    label2.Visible = true;
                    textBox1.Visible = false;
                    textBox2.Visible = true;
                }
                else
                {
                    MessageBox.Show("Please select chit name");
                }

            }
            catch (Exception fill)
            {
                MessageBox.Show(fill.Message);
            }

            Sql.connection.Close();
        }
    }
}
