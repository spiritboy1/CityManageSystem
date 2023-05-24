using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using 城市规划管理系统.DAL;
using System.Diagnostics;

namespace 城市规划管理系统
{
    public partial class SystemPassword : Form
    {


        public delegate void Hide_RegisterForm();
        public event Hide_RegisterForm hide_registerform;

        public SystemPassword()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; 

            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.textBox1.PasswordChar = '*';
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == ConfigurationManager.AppSettings["administrator_password"].ToString())
            {
                MessageBox.Show("欢迎回来", "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                Login.manage_user.Show();               
                this.Close();
                this.hide_registerform();
            }
            else
            {
                MessageBox.Show("口令错误，请重新输入", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
