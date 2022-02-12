using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Auction : Form
    {
        public Auction()
        {
            InitializeComponent();
        }
        public int[] ticket = new int[20];
        public string[] przSts = new string[20];
        public int[] aucMnth = new int[20];
        public string[] name = new string[20];

        private void Auction_Load(object sender, EventArgs e)
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
            fillgrid();
            getCurrentMonth();
            textBox4.Text = label5.Text;
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

        private void fillgrid()
        {
            try
            {
                Sql.connection.Open();
                string query = "select ticketNo As No, name AS Not_Auctioned from " + comboBox1.SelectedItem.ToString() + " where" +
                    " auctionMonth = " + 0 + " and month = 1;";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                DataTable ds = new DataTable();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds;
                dataGridView1.Columns[0].Width = 35;
                textBox4.Text = label5.Text;
            }
            catch (Exception grid)
            {
                MessageBox.Show(grid.Message);
            }
            Sql.connection.Close();
        }

        private void getCurrentMonth()
        {
            try
            {
                Sql.connection.Open();
                string currentmonth = "select distinct max(month) from " + comboBox1.SelectedItem.ToString() + ";";
                SqlCommand cmd = new SqlCommand(currentmonth, Sql.connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    textBox6.Text = reader.GetValue(0).ToString();

                }
            }
            catch (Exception month)
            {
                MessageBox.Show(month.Message);
            }
            Sql.connection.Close();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnNo = Convert.ToInt32(dataGridView1.CurrentCell.ColumnIndex);
            if (columnNo > 0)
            {
                textBox1.Text = dataGridView1.CurrentCell.Value.ToString();
                textBox2.Clear();
                textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[columnNo - 1].Value.ToString();
                button1.Enabled = true;
                textBox3.ReadOnly = false;
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            int dividend = 0, company = 0, nooftickets = 0, netpay = 0, bidamount = 0;

            if (comboBox1.SelectedIndex >= 0)
            {
                company = Convert.ToInt32(label5.Text);
                nooftickets = company / 100;
                netpay = (Convert.ToInt32(label3.Text)) / nooftickets;
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, "[^0-9]"))
                {
                    textBox3.Clear();
                }
                else if (textBox3.Text != string.Empty && !System.Text.RegularExpressions.Regex.IsMatch(textBox3.Text, "[^0-9]"))
                {

                    dividend = Convert.ToInt32(textBox3.Text);
                    if (dividend < company)
                    {
                        label16.Visible = true;
                        label16.Text = "*Min spread amount = " + company + "";
                        textBox5.Clear();
                    }
                    else if (dividend > 36000)
                    {
                        label16.Visible = true;
                        label16.Text = "*Max spread amount = 36000";
                        textBox5.Clear();
                    }
                    else
                    {
                        label16.Visible = false;
                        bidamount = Convert.ToInt32((netpay - ((dividend - company) / 20)));
                        textBox5.Text = bidamount.ToString();
                    }
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sql.connection.Close();
            try
            {
                Sql.connection.Open();
                var auction = "select distinct max(auctionMonth) from " + comboBox1.SelectedItem.ToString() + "" +
                    " where month = 1;";
                SqlCommand command = new SqlCommand(auction, Sql.connection);
                SqlDataReader red = command.ExecuteReader();

                if (red.Read())
                {
                    int month = (int)red.GetValue(0);
                    red.Close();
                    Sql.connection.Close();
                    if (month == 0)
                    {
                        conductAuction();
                        getCustomers();
                        createNextMnth();
                        Sql.connection.Close();
                    }
                    else
                    {
                        var check = "select prizeStatus from " + comboBox1.SelectedItem.ToString() + " where " +
                            " auctionMonth = " + month + " and month = 1 ;";
                        Sql.connection.Open();
                        SqlCommand cmd = new SqlCommand(check, Sql.connection);
                        SqlDataReader sqlData = cmd.ExecuteReader();
                        if (sqlData.Read())
                        {
                            var data = sqlData.GetString(0);
                            Sql.connection.Close();
                            if (data == string.Empty)
                            {
                                MessageBox.Show("You are not made settelment for previous customer");
                            }
                            else
                            {
                                conductAuction();
                                getCustomers();
                                createNextMnth();
                                Sql.connection.Close();
                            }

                        }

                    }
                }
            }
            catch (Exception auctioncheck)
            {
                MessageBox.Show(auctioncheck.Message);
            }

            Sql.connection.Close();

        }

        private void conductAuction()
        {
            if (comboBox1.SelectedIndex > -1 && textBox5.Text != string.Empty && textBox4.Text != string.Empty)
            {
                int ticketNo = Convert.ToInt32(textBox2.Text);
                int auction = Convert.ToInt32(textBox6.Text);

                int bidamount = Convert.ToInt32(textBox5.Text);
                string upAuction = "update " + comboBox1.SelectedItem.ToString() + " set" +
                      " auctionMonth = " + auction + " where ticketNo = " + ticketNo + ";";
                Sql.executeSqlStmnt(upAuction);
                string upBidAmt = "update " + comboBox1.SelectedItem.ToString() + " set " +
                      " bidAmount = " + bidamount + " where month = " + auction + ";";
                Sql.executeSqlStmnt(upBidAmt);

            }
        }

        private void getCustomers()
        {
            if (comboBox1.SelectedIndex > -1)
            {
                try
                {
                    Sql.connection.Open();
                    string query = "select ticketNo, prizeStatus, auctionMonth, name from " + comboBox1.SelectedItem.ToString() + " where" +
                        " month = 1 ;";
                    SqlCommand cmd = new SqlCommand(query, Sql.connection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    int iterable = 0;
                    while (reader.Read())
                    {
                        ticket[iterable] = (int)reader.GetValue(0);
                        if (!reader.IsDBNull(1))
                        {
                            przSts[iterable] = reader.GetString(1);
                        }
                        if (!reader.IsDBNull(2))
                        {
                            aucMnth[iterable] = (int)reader.GetValue(2);
                        }
                        name[iterable] = reader.GetString(3);
                        iterable++;
                    }
                }
                catch (Exception cus)
                {
                    MessageBox.Show(cus.Message);
                }

                Sql.connection.Close();
            }
        }

        private void createNextMnth()
        {
            if (comboBox1.SelectedIndex > -1 && textBox5.Text != string.Empty && textBox4.Text != string.Empty)
            {
                int month = Convert.ToInt32(textBox6.Text) + 1;
                int item = 0;
                while (item < 20)
                {
                    string insert = "insert into " + comboBox1.SelectedItem.ToString() + "" +
                            " (ticketNo, prizeStatus, auctionMonth, name, month) values " +
                            " (" + ticket[item] + ", '" + przSts[item] + "', " + aucMnth[item] + "," +
                            " '" + name[item] + "', " + month + ");";
                    Sql.executeSqlStmnt(insert);
                    item++;
                }
                MessageBox.Show("Auction done succesfully");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Text = (Convert.ToInt32(textBox6.Text) + 1).ToString();
                button1.Enabled = false;
                textBox3.ReadOnly = true;
                fillgrid();
            }
        }

    }
}
