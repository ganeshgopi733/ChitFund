using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Settelments : Form
    {
        public Settelments()
        {
            InitializeComponent();
        }

        private void Settelments_Load(object sender, EventArgs e)
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
                string query = "select ticketNo AS No, prizeStatus, auctionMonth, name from " +
                    " " + comboBox1.SelectedItem.ToString() + " where month = 1 and " +
                    "prizeStatus IS NOT NULL and prizeStatus <> '';";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                DataTable ds = new DataTable();
                adapter.Fill(ds);
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = ds;
                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[2].Width = 130;
                DataGridViewButtonColumn viewbutton = new DataGridViewButtonColumn();
                viewbutton.Name = "show";
                viewbutton.Text = "show";
                viewbutton.DataPropertyName = "show";
                viewbutton.HeaderText = "view customer";
                dataGridView1.Columns.Add(viewbutton);
            }
            catch (Exception grid)
            {
                MessageBox.Show(grid.Message);
            }

            Sql.connection.Close();
        }


        private Form childform;

        private void openchildform(Form child)
        {
            panel1.Height = 1;
            panel1.Visible = false;
            panel2.Dock = DockStyle.Fill;
            childform = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            panel2.Controls.Add(child);
            panel2.Tag = child;
            child.BringToFront();
            child.Show();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openchildform(new MakeSettelment());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnNo = Convert.ToInt32(dataGridView1.CurrentCell.ColumnIndex);
            if (columnNo == 4)
            {
                int no = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                string name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                var table = comboBox1.SelectedItem.ToString();
                var query = "select * from settelments where chitName = '" + table + "' and " +
                    " ticketNo = " + no + " and name = '" + name + "';";
                try
                {
                    Sql.connection.Open();
                    SqlCommand command = new SqlCommand(query, Sql.connection);
                    SqlDataReader read = command.ExecuteReader();
                    if (read.Read())
                    {
                        MessageBox.Show("\n CHIT NAME : " + read.GetString(0) +
                            "\n \n TICKET NO : " + read.GetValue(1).ToString() +
                            "\n NAME : " + read.GetString(2) +
                            "\n PACKAGE AMOUNT : " + read.GetValue(3).ToString() +
                            "\n DIVIDEND : " + read.GetValue(4).ToString() +
                            "\n COMPANY COMMISION : " + read.GetValue(5).ToString() +
                            "\n SETTELED DATE : " + read.GetString(7) +
                            "\n TOTAL AMOUNT GIVEN : " + read.GetValue(6).ToString() +
                            "\n \n \n RUPEE INFORMATION \n \n " +
                            "\n TWO THOUSANDS (2000) : " + read.GetValue(8).ToString() +
                            "\n FIVE HUNDREDS (500) : " + read.GetValue(9).ToString() +
                            "\n TWO HUNDREDS (200) : " + read.GetValue(10).ToString() +
                            "\n ONE HUNDREDS (100) : " + read.GetValue(11).ToString() +
                            "\n FIFTYS (50) : " + read.GetValue(12).ToString()
                            );
                    }
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message);
                }

            }
            Sql.connection.Close();
        }
    }
}
