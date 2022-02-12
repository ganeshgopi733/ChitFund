using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Createchit : Form
    {
        int count = 1;
        string[] prname = new string[21];

        public Createchit()
        {
            InitializeComponent();
        }

        private void Createchit_Load(object sender, EventArgs e)
        {
            dateTimePicker1.MinDate = DateTime.Today.AddDays(-30);
            dateTimePicker1.MaxDate = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createchit();
            clearfield();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addperson();
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addperson();
            }

        }

        private void addperson()
        {
            try
            {
                string nm = textBox5.Text.ToLower();

                if (textBox5.Text == string.Empty)
                {
                    MessageBox.Show("Please provide name ");
                }
                else if (count <= 20 && textBox5.Text != string.Empty)
                {
                    dataGridView1.Rows.Add(count, nm);
                    prname[count] = nm;
                    count++;
                    if (count == 21)
                    {
                        textBox2.Text = (count - 1).ToString();
                    }
                    else
                    {
                        textBox2.Text = count.ToString();
                    }
                    textBox5.Clear();
                }

            }
            catch (Exception person)
            {
                MessageBox.Show(person.Message);
            }
        }

        private void createchit()
        {
            string chitname = "";
            int package = 0, nooftickets = 0, company = 0;
            if (textBox1.Text != string.Empty)
            {
                chitname = textBox1.Text.Trim().ToLower();
            }
            if (comboBox1.SelectedIndex != -1)
            {
                package = Convert.ToInt32(comboBox1.SelectedItem.ToString());
                nooftickets = Convert.ToInt32(textBox3.Text);
                company = Convert.ToInt32(textBox4.Text);
            }

            var startmonth = dateTimePicker1.Value.ToShortDateString();

            var endmonth = dateTimePicker1.Value.AddMonths(20).ToString("dd-MM-yyyy");

            if (chitname == string.Empty)
            {
                MessageBox.Show("Enter chit name");
            }
            else if (nooftickets == 0)
            {
                MessageBox.Show("Enter valid details");
            }
            else if (count < 21)
            {
                MessageBox.Show("Add person name");
            }
            else if (textBox1.Text != string.Empty && textBox3.Text != string.Empty
                && textBox4.Text != string.Empty && textBox2.Text == "20")
            {
                try
                {
                    Sql.connection.Close();
                    Sql.connection.Open();
                    string query = "insert into viewGroup values ('" + @chitname + "', " + @package + "," +
                        " '" + @startmonth + "','" + @endmonth + "'," + @nooftickets + "," + @company + ");";
                    SqlCommand cmd = new SqlCommand(query, Sql.connection);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        createtable();
                        MessageBox.Show("Chit Created Succesfully");
                    }
                    else
                    {
                        MessageBox.Show("Creation failed");
                    }
                }
                catch (Exception cr)
                {
                    MessageBox.Show(cr.Message);
                }
            }

            Sql.connection.Close();
        }

        private void createtable()
        {
            string chitname = "", query = "";
            int nooftickets = 0;
            if (textBox1.Text != string.Empty && textBox3.Text != string.Empty && textBox2.Text == "20")
            {
                chitname = textBox1.Text.Trim().ToLower();
                nooftickets = Convert.ToInt32(textBox3.Text);
                query = "CREATE TABLE [dbo].[" + chitname + "] (  [ticketNo] INT NULL, " +
                                " [prizeStatus] VARCHAR(20) NULL DEFAULT '', " +
                                " [auctionMonth] INT NULL DEFAULT 0," +
                                " [name] VARCHAR(20) NULL, " +
                                " [month] INT NULL DEFAULT 1, " +
                                " [bidAmount] INT NULL DEFAULT '', " +
                                " [date] VARCHAR(20) NULL DEFAULT '', " +
                                " [amountPaid] INT NULL DEFAULT '', " +
                                " [balance] INT NULL DEFAULT '', " +
                                " [total] INT NULL DEFAULT '');";
                try
                {
                    Sql.executeSqlStmnt(query);
                    count = 1;
                    while (count <= 20)
                    {
                        string insertQuery = "insert into " + chitname + " (ticketNo, name) values (" + count + ", '" + prname[count] + "');";
                        Sql.executeSqlStmnt(insertQuery);
                        count++;
                    }

                }
                catch (Exception table)
                {
                    MessageBox.Show(table.Message);
                }

            }


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int packagevalue = Convert.ToInt32(comboBox1.SelectedItem.ToString());
            if (packagevalue == 100000)
            {
                textBox3.Text = "20";
                textBox4.Text = "2000";
            }
            else if (packagevalue == 50000)
            {
                textBox3.Text = "10";
                textBox4.Text = "1000";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox5.Text, "[^0-9]"))
            {
                textBox5.Clear();
            }

        }

        private void clearfield()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            dataGridView1.Rows.Clear();
          
        }

    }
}
