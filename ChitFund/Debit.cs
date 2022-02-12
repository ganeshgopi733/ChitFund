using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Debit : Form
    {
        public Debit()
        {
            InitializeComponent();
        }

        private void Debit_Load(object sender, EventArgs e)
        {
            try
            {
                Sql.connection.Open();
                var query = "select chitName, ticketNo, name, date, payable from settelments ORDER BY date;";
                SqlDataAdapter adapter = new SqlDataAdapter(query, Sql.connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
            }
            Sql.connection.Close();
        }
    }
}
