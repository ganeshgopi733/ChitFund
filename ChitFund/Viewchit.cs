using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace ChitFund
{
    public partial class Viewchit : Form
    {

        public Viewchit()
        {
            InitializeComponent();

        }

        private void Viewchit_Load(object sender, EventArgs e)
        {
            getTableNames();
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
            getChitInfo();
            getCurrentMonth();
            getbidamount();
            fillgrid();

        }
        private void getChitInfo()
        {
            try
            {
                Sql.connection.Open();
                string chitName = comboBox1.SelectedItem.ToString();
                string query = "select package, startMonth, endMonth, companyCommission from viewGroup where chitName = '" + chitName + "';";
                SqlCommand cmd = new SqlCommand(query, Sql.connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    label4.Text = reader.GetValue(0).ToString();
                    label6.Text = reader.GetString(1);
                    label8.Text = reader.GetString(2);
                    label14.Text = reader.GetValue(3).ToString();

                }
            }
            catch (Exception ch)
            {
                MessageBox.Show(ch.Message);
            }

            Sql.connection.Close();

        }

        private void fillgrid()
        {
            try
            {
                Sql.connection.Open();
                string query = "select * from " + comboBox1.SelectedItem.ToString() + " where " +
                    "month = " + textBox1.Text + " order by auctionMonth;";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                DataTable ds = new DataTable();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds;
            }
            catch (Exception fill)
            {
                MessageBox.Show(fill.Message);
            }

            Sql.connection.Close();
        }
        private void getCurrentMonth()
        {
            try
            {
                Sql.connection.Open();
                string currentmonth = "select distinct max(month) from " + comboBox1.SelectedItem.ToString() + "" +
                    " where bidAmount IS NOT NULL;";
                SqlCommand cmd = new SqlCommand(currentmonth, Sql.connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    textBox1.Text = reader.GetValue(0).ToString();
                    if (textBox1.Text == string.Empty)
                    {
                        textBox1.Text = "1";
                    }
                }
            }
            catch (Exception month)
            {
                MessageBox.Show(month.Message);
            }

            Sql.connection.Close();

        }
        private void getbidamount()
        {
            try
            {
                Sql.connection.Open();
                string bidamount = "select max(bidAmount) from " + comboBox1.SelectedItem.ToString() + " where month = " + textBox1.Text + " ;";
                SqlCommand cmd = new SqlCommand(bidamount, Sql.connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    textBox2.Text = reader.GetValue(0).ToString();
                }
            }
            catch (Exception bid)
            {
                MessageBox.Show(bid.Message);
            }
            Sql.connection.Close();
        }
        private Form childform;

        private void openchildform(Form child)
        {
            panel1.Height = 1;
            panel2.Height = 1;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Dock = DockStyle.Fill;
            childform = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            panel3.Controls.Add(child);
            panel3.Tag = child;
            child.BringToFront();
            child.Show();

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openchildform(new Createchit());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            int previous = Convert.ToInt32(textBox1.Text) - 1;
            if (comboBox1.SelectedIndex >= 0)
            {
                if (previous > 0)
                {
                    textBox1.Text = previous.ToString();
                    fillgrid();
                    getbidamount();
                }
            }


        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            int next = Convert.ToInt32(textBox1.Text) + 1;

            if (comboBox1.SelectedIndex >= 0)
            {
                if (next <= 20 && textBox2.Text != string.Empty)
                {
                    textBox1.Text = next.ToString();
                    fillgrid();
                    getbidamount();
                }
            }


        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex > -1)
                {
                    Sql.connection.Open();
                    string name = textBox3.Text.ToLower().ToString() + @"%";
                    string query = "select * from " + comboBox1.SelectedItem.ToString() + " where " +
                    " month = " + textBox1.Text + " and name LIKE '" + name + "';";
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

        private void button1_Click(object sender, EventArgs e)
        {
            openchildform(new Auction());
        }
    }
}
