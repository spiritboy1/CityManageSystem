using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace 城市规划管理系统
{
    public partial class Login : Form
    {
        //提前创建所有窗口对象
        public static MainForm main_form = new MainForm();
        public static SystemPassword system_password = new SystemPassword();
        public static ManageUser manage_user = new ManageUser();
        public static PropertySet property_set = new PropertySet();
        public static ModelBase model_base = new ModelBase();
        public static UserApply user_apply = new UserApply();
        public static FindPassword find_password = new FindPassword();

        Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public List<string> account { get; set; }
        public List<string> keyword { get; set; }

        public Login()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.account = new List<string>();
            this.keyword = new List<string>();

            this.textBox2.PasswordChar = '*';
            if (ConfigurationManager.AppSettings["remember"].Equals("true"))
            {
                this.textBox1.Text = ConfigurationManager.AppSettings["account"].ToString();
                this.textBox2.Text = ConfigurationManager.AppSettings["password"].ToString();
                this.checkBox1.Checked = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mes = MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mes == DialogResult.Yes)
            {
                Application.Exit();
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mydbs = "Data Source=(local);Initial Catalog=城市规划管理系统;Integrated Security=True";
            string mycom1 = "select * from 账户信息 where 账号='" + this.textBox1.Text + "'";
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            SqlCommand cmd = new SqlCommand(mycom1, con);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    account.Add((sdr[0] + "").Trim());
                    keyword.Add((sdr[1] + "").Trim());
                }
                if (this.textBox1.Text == account[0] && this.textBox2.Text == keyword[0])
                {                   
                    if (this.checkBox1.Checked)
                    {
                        configuration.AppSettings.Settings["account"].Value = this.textBox1.Text;
                        configuration.AppSettings.Settings["password"].Value = this.textBox2.Text;
                        configuration.AppSettings.Settings["remember"].Value = "true";
                        configuration.Save();
                    }
                    else
                    {
                        configuration.AppSettings.Settings["account"].Value = "";
                        configuration.AppSettings.Settings["password"].Value = "";
                        configuration.AppSettings.Settings["remember"].Value = "flase";
                        configuration.Save();
                    }
                    MessageBox.Show("欢迎回来", "登录成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Login.main_form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("账号密码错误，请重新输入", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("尊敬的用户，未查询到您的信息。请重新检查用户名和密码或者联系管理员", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            con.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            Login.system_password.hide_registerform += this.Hide;
            Login.system_password.Show();          
        }
        private void label5_Click(object sender, EventArgs e)
        {
            this.Hide();           
            Login.user_apply.Show();            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(sender, e);
            }
        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(sender, e);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            find_password.Show();

        }
    }
}
