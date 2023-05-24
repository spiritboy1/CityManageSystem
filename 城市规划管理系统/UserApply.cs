using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 城市规划管理系统.DAL;

namespace 城市规划管理系统
{
    public partial class UserApply : Form
    {
        public UserApply()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public string[] user_information = new string[7];
        public static string verification_code;
        public delegate void Send_UserInformation(string[] user_information);
        public event Send_UserInformation send_userinformation;

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.textBox6.Text == verification_code)
            {
                DataAddSQL das = new DataAddSQL();
                int age;
                if (this.textBox5.Text.Length == 0)
                {
                    DateTime dt1 = Convert.ToDateTime(this.dateTimePicker1.Text);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan span = dt2.Subtract(dt1);
                    age = (span.Days + 1) / 365;
                }
                else
                {
                    age = Convert.ToInt16(this.textBox5.Text);
                }
                if (das.Add_User(Convert.ToInt64(this.textBox1.Text),
                    this.textBox2.Text,
                    this.textBox3.Text,
                    this.textBox4.Text,
                    this.comboBox1.Text,
                    age,
                    Convert.ToDateTime(this.dateTimePicker1.Text)))
                {
                    MessageBox.Show("注册成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    Program.login_form.Show();
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login.manage_user.Show();
            if(this.textBox1.Text != null &&
                this.textBox2.Text != null &&
                this.textBox3.Text != null)
            {
                user_information[0] = this.textBox1.Text;
                user_information[1] = this.textBox2.Text;
                user_information[2] = this.textBox3.Text;

                if (this.textBox4.Text != null)
                {
                    user_information[3] = this.textBox4.Text;
                }
                else
                {
                    user_information[3] = "";
                }

                if (this.comboBox1.Text != null)
                {
                    user_information[4] = this.comboBox1.Text;
                }
                else
                {
                    user_information[3] = "";
                }

                if (this.textBox5.Text != null)
                {
                    user_information[5] = this.textBox5.Text;
                }
                else
                {
                    user_information[3] = "";
                }

                if (this.dateTimePicker1.Text != null)
                {
                    user_information[6] = this.dateTimePicker1.Text;
                }
                else
                {
                    user_information[3] = "";
                }

                send_userinformation += new Send_UserInformation(Login.manage_user.Display_UserMessage);
                if(send_userinformation != null)
                {
                    send_userinformation.Invoke(user_information);
                }
                MessageBox.Show("请注意查收邮箱", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("请注意：用户名、密码和邮箱必填", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Get_VerificationCode(string code)
        {
            verification_code = code;
        }
    }
}
