using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChitFund
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            //this.Text = string.Empty;
            //this.ControlBox = false;
            //this.DoubleBuffered = true;
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("hh:mm:ss");
            labelDay.Text = DateTime.Now.ToString("dddd");
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private Form childform;

        private void openchildform(Form child)
        {
            childform = child;
            child.TopLevel = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock = DockStyle.Fill;
            panel5.Controls.Add(child);
            panel5.Tag = child;
            child.BringToFront();
            child.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openchildform(new Viewchit());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openchildform(new Settelments());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openchildform(new Credit());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openchildform(new Debit());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            openchildform(new Report());
        }
    }
}
