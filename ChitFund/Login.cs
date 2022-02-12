using System;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminLogin();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void adminLogin()
        {
            string username = "alpha";
            string password = "alpha123";
            if (textBox1.Text == username && textBox2.Text == password)
            {
                this.Hide();
                Main main = new Main();
                main.Show();
            }
            else if (textBox1.Text == string.Empty || textBox2.Text == string.Empty)
            {
                MessageBox.Show("Field is Empty");
            }
            else
            {
                MessageBox.Show("Username or password is Incorrect");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
