using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Credit : Form
    {
        public Credit()
        {
            InitializeComponent();
        }

        private void Credit_Load(object sender, EventArgs e)
        {
            getTableNames();
            dateTimePicker1.MinDate = DateTime.Today.AddDays(-30);
            dateTimePicker1.MaxDate = DateTime.Today;
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
            fillgrid();
        }

        private void fillgrid()
        {
            try
            {
                Sql.connection.Open();
                string query = "select ticketNo As No, name AS Name,date AS Date, amountPaid " +
                    " AS Amount from " + comboBox1.SelectedItem.ToString() + " where" +
                    " month = " + textBox1.Text + " and bidAmount IS NOT NULL;";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                DataTable ds = new DataTable();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds;
                dataGridView1.Columns[0].Width = 35;
            }
            catch (Exception grid)
            {
                MessageBox.Show(grid.Message);
            }
            Sql.connection.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnNo = Convert.ToInt32(dataGridView1.CurrentCell.ColumnIndex);
            if (columnNo > 0)
            {
                textBox6.Text = textBox1.Text;
                try
                {
                    Sql.connection.Open();
                    var query = "select bidAmount from " + comboBox1.SelectedItem.ToString() + " where" +
                        " month = " + textBox1.Text + " and bidAmount IS NOT NULL;";
                    SqlCommand cmd = new SqlCommand(query, Sql.connection);
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.Read() && !data.IsDBNull(0))
                    {
                        textBox5.Text = data.GetValue(0).ToString();
                        data.Close();
                        Sql.connection.Close();
                        textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString();
                        textBox3.Clear();
                        textBox3.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();

                    }
                }
                catch (Exception month)
                {
                    MessageBox.Show(month.Message);
                }
                Sql.connection.Close();

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int previous = Convert.ToInt32(textBox1.Text) - 1;
            if (comboBox1.SelectedIndex >= 0)
            {
                try
                {
                    Sql.connection.Open();
                    var query = "select bidAmount from " + comboBox1.SelectedItem.ToString() + " where" +
                        " month = " + textBox1.Text + " and bidAmount IS NOT NULL;";
                    SqlCommand cmd = new SqlCommand(query, Sql.connection);
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.Read() && !data.IsDBNull(0) && previous > 0)
                    {
                        textBox6.Text = textBox1.Text = previous.ToString();
                        textBox5.Text = data.GetValue(0).ToString();
                        //textBox6.Text = textBox1.Text;
                        data.Close();
                        Sql.connection.Close();
                        fillgrid();
                    }
                }
                catch (Exception prev)
                {
                    MessageBox.Show(prev.Message);
                }
                Sql.connection.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            int next = Convert.ToInt32(textBox1.Text) + 1;

            if (comboBox1.SelectedIndex >= 0)
            {
                try
                {
                    Sql.connection.Open();
                    var query = "select bidAmount from " + comboBox1.SelectedItem.ToString() + " where" +
                        " month = " + next + " and bidAmount IS NOT NULL;";
                    SqlCommand cmd = new SqlCommand(query, Sql.connection);
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.Read() && !data.IsDBNull(0) && next <= 20)
                    {
                        textBox6.Text = textBox1.Text = next.ToString();
                        textBox5.Text = data.GetValue(0).ToString();
                        //textBox6.Text = textBox1.Text;
                        data.Close();
                        Sql.connection.Close();
                        fillgrid();
                    }
                }
                catch (Exception month)
                {
                    MessageBox.Show(month.Message);
                }

            }
            Sql.connection.Close();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == '.';
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1 && textBox4.Text != string.Empty)
            {
                var date = dateTimePicker1.Value.ToShortDateString();
                int bidamount = Convert.ToInt32(textBox5.Text);
                int money = Convert.ToInt32(textBox4.Text);
                int balance = bidamount - money;
                if (money > bidamount)
                {
                    MessageBox.Show("You have entered too high amount");
                }
                else if (balance < 0 || money < 0)
                {
                    MessageBox.Show("You have entered too low amount");
                }
                else if (money <= bidamount)
                {
                    int total = money;
                    var query = "update " + comboBox1.SelectedItem.ToString() + " set date = '" + date + "', " +
                    " amountPaid = " + money + ", balance = " + balance + ", total = " + total + " where " +
                    " ticketNO = " + textBox3.Text + " and name = '" + textBox2.Text + "' and month = " + textBox1.Text + ";";
                    Sql.executeSqlStmnt(query);
                    MessageBox.Show("Amount updated");
                    clearfield();
                    fillgrid();
                }

            }

        }

        private void clearfield()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox2.Clear();
            button1.Enabled = false;
        }

        private void pictureBoxCollector_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                try
                {
                    Sql.connection.Open();
                    var query = "select ticketNo, name, date, amountPaid from " + comboBox1.SelectedItem.ToString() + "" +
                        " where amountPaid !=0 ORDER BY date;";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Sql.connection);
                    DataTable ds = new DataTable();
                    dataAdapter.Fill(ds);
                    dataGridView1.DataSource = ds;
                    dataGridView1.Columns[0].Width = 35;
                    panel2.Visible = false;
                    comboBox1.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = false;
                    textBox1.Visible = false;
                    panel1.Dock = DockStyle.Fill;
                    dataGridView1.Dock = DockStyle.Fill;
                }
                catch (Exception warn)
                {
                    MessageBox.Show(warn.Message);
                }
            }
            else
            {
                MessageBox.Show("Select chit name");
            }
            Sql.connection.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
