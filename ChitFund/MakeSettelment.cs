using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class MakeSettelment : Form
    {
        public MakeSettelment()
        {
            InitializeComponent();
        }

        private void MakeSettelment_Load(object sender, EventArgs e)
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
            clearfield();
            getChitInfo();
            fillsettelment();
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
                    label3.Text = reader.GetValue(0).ToString();
                    label8.Text = reader.GetString(1);
                    label9.Text = reader.GetString(2);
                    label5.Text = reader.GetValue(3).ToString();

                }
            }
            catch (Exception ch)
            {
                MessageBox.Show(ch.Message);
            }

            Sql.connection.Close();

        }

        private void fillsettelment()
        {
            try
            {
                Sql.connection.Open();
                string current = "select distinct max(auctionMonth) from " + comboBox1.SelectedItem.ToString() + "" +
                    " where month = 1 ;";
                SqlCommand command = new SqlCommand(current, Sql.connection);
                SqlDataReader reader = command.ExecuteReader();
                int month = 0, payable = 0;

                if (reader.Read())
                {
                    month = (int)reader.GetValue(0);
                    reader.Close();

                    string query = "select ticketNo, name, bidAmount from " + comboBox1.SelectedItem.ToString() + "" +
                    " where auctionMonth = " + month + " and month = " + month + " and prizeStatus != 'Prized' ;";
                   
                    SqlCommand cmd = new SqlCommand(query, Sql.connection);
                    SqlDataReader red = cmd.ExecuteReader();
                    if (red.Read())
                    {
                        textBox1.Text = red.GetValue(0).ToString();
                        textBox2.Text = red.GetString(1);
                        if (label3.Text == "100000")
                        {
                            payable = ((5000 - (int)red.GetValue(2)) * 20) + 2000;
                            textBox4.Text = payable.ToString();
                            textBox6.Text = (100000 - payable).ToString();
                        }
                        else
                        {
                            payable = ((2500 - (int)red.GetValue(2)) * 10) + 1000;
                            textBox4.Text = payable.ToString();
                            textBox6.Text = (50000 - payable).ToString();
                        }
                        textBox5.Text = label5.Text;
                        button1.Enabled = true;
                    }
                }
            }
            catch (Exception settelment)
            {
                MessageBox.Show(settelment.Message);
            }
            Sql.connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (textBox17.Text == string.Empty)
                {
                    MessageBox.Show("Please Provide Rupee Notes");
                }
                else if (textBox17.Text != textBox6.Text)
                {
                    MessageBox.Show("Total amount is not equal with amount payable");
                }
                else if (textBox17.Text == textBox6.Text)
                {
                    settelment();
                    clearfield();
                }
            }


        }

        private void settelment()
        {

            string modify = "update " + comboBox1.SelectedItem.ToString() + " set prizeStatus = 'Prized' where " +
                " ticketNo = " + textBox1.Text + ";";
            Sql.executeSqlStmnt(modify);
            int ticket = Convert.ToInt32(textBox1.Text);
            string name = textBox2.Text;
            int package = Convert.ToInt32(label3.Text);
            int dividend = Convert.ToInt32(textBox4.Text);
            int company = Convert.ToInt32(textBox5.Text);
            int pay = Convert.ToInt32(textBox6.Text);
            var date = dateTimePicker1.Value.ToShortDateString();
            int twoth = Convert.ToInt32(textBox7.Text);
            int fivehu = Convert.ToInt32(textBox8.Text);
            int twohu = Convert.ToInt32(textBox9.Text);
            int onehu = Convert.ToInt32(textBox10.Text);
            int fifty = Convert.ToInt32(textBox11.Text);
            string insert = "insert into settelments values ('" + comboBox1.SelectedItem.ToString() + "', " +
                " " + @ticket + ", '" + @name + "', " + @package + ", " + @dividend + ", " +
                " " + @company + ", " + @pay + ", '" + @date + "', " +
                " " + @twoth + ", " + @fivehu + ", " + @twohu + ", " +
                " " + @onehu + ", " + @fifty + ");";
            Sql.executeSqlStmnt(insert);
            MessageBox.Show("Settelement succesfull in " + date.ToString());

        }

        private void clearfield()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
            textBox17.Clear();
            button1.Enabled = false;

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != string.Empty)
            {
                textBox12.Text = (2000 * Convert.ToInt32(textBox7.Text)).ToString();
            }
            else { textBox12.Clear(); }
            if (textBox12.Text != string.Empty && textBox13.Text != string.Empty && textBox14.Text != string.Empty
                && textBox15.Text != string.Empty && textBox16.Text != string.Empty)
            {
                textBox17.Text = (Convert.ToInt32(textBox12.Text) + Convert.ToInt32(textBox13.Text) +
                    Convert.ToInt32(textBox14.Text) + Convert.ToInt32(textBox15.Text) +
                    Convert.ToInt32(textBox16.Text)).ToString();
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != string.Empty)
            {
                textBox13.Text = (500 * Convert.ToInt32(textBox8.Text)).ToString();
            }
            else { textBox13.Clear(); }
            if (textBox12.Text != string.Empty && textBox13.Text != string.Empty && textBox14.Text != string.Empty
               && textBox15.Text != string.Empty && textBox16.Text != string.Empty)
            {
                textBox17.Text = (Convert.ToInt32(textBox12.Text) + Convert.ToInt32(textBox13.Text) +
                    Convert.ToInt32(textBox14.Text) + Convert.ToInt32(textBox15.Text) +
                    Convert.ToInt32(textBox16.Text)).ToString();
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text != string.Empty)
            {
                textBox14.Text = (200 * Convert.ToInt32(textBox9.Text)).ToString();
            }
            else
            {
                textBox14.Clear();
            }
            if (textBox12.Text != string.Empty && textBox13.Text != string.Empty && textBox14.Text != string.Empty
               && textBox15.Text != string.Empty && textBox16.Text != string.Empty)
            {
                textBox17.Text = (Convert.ToInt32(textBox12.Text) + Convert.ToInt32(textBox13.Text) +
                    Convert.ToInt32(textBox14.Text) + Convert.ToInt32(textBox15.Text) +
                    Convert.ToInt32(textBox16.Text)).ToString();
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text != string.Empty)
            {
                textBox15.Text = (100 * Convert.ToInt32(textBox10.Text)).ToString();
            }
            else { textBox15.Clear(); }
            if (textBox12.Text != string.Empty && textBox13.Text != string.Empty && textBox14.Text != string.Empty
               && textBox15.Text != string.Empty && textBox16.Text != string.Empty)
            {
                textBox17.Text = (Convert.ToInt32(textBox12.Text) + Convert.ToInt32(textBox13.Text) +
                     Convert.ToInt32(textBox14.Text) + Convert.ToInt32(textBox15.Text) +
                     Convert.ToInt32(textBox16.Text)).ToString();
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != string.Empty)
            {
                textBox16.Text = (50 * Convert.ToInt32(textBox11.Text)).ToString();
            }
            else { textBox16.Clear(); }
            if (textBox12.Text != string.Empty && textBox13.Text != string.Empty && textBox14.Text != string.Empty
               && textBox15.Text != string.Empty && textBox16.Text != string.Empty)
            {
                textBox17.Text = (Convert.ToInt32(textBox12.Text) + Convert.ToInt32(textBox13.Text) +
                     Convert.ToInt32(textBox14.Text) + Convert.ToInt32(textBox15.Text) +
                     Convert.ToInt32(textBox16.Text)).ToString();
            }

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == '.';
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == '.';
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == '.';
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == '.';
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar == '.';
        }
    }
}
