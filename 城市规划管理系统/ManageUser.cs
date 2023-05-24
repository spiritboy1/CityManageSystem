using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Net.Mail;
using 城市规划管理系统.DAL;
using static 城市规划管理系统.DAL.DataManageSQL;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Configuration;

namespace 城市规划管理系统
{
    public partial class ManageUser : Form
    {
        public string[] user_information;
        public delegate void Send_VerificationCode(string code);
        public event Send_VerificationCode send_verificationcode;
        public SmtpClient smtp = new SmtpClient();

        public ManageUser()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            this.tabControl1.SizeMode = TabSizeMode.Fixed;
            this.tabControl1.ItemSize = new Size(0, 1);
            this.tabControl1.Appearance = TabAppearance.FlatButtons;

            this.comboBox2.SelectedIndex = 0;
            this.textBox1.PasswordChar = '*';
            DataInquireSQL dis = new DataInquireSQL();
            DataSet ds = dis.Inquire_Loading("账户信息", "账号", null);
            this.dataGridView1.DataSource = ds.Tables[0].DefaultView;
            this.dataGridView2.DataSource = ds.Tables[0].DefaultView;

        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Node.Text)
            {
                case "首页":
                    this.tabControl1.SelectedIndex = 0;
                    break;
                case "修改管理员口令":
                    this.tabControl1.SelectedIndex = 1;
                    break;
                case "用户信息":
                    this.tabControl1.SelectedIndex = 2;
                    break;
                case "新增用户":
                    this.tabControl1.SelectedIndex = 3;
                    break;
                case "修改用户":
                    this.tabControl1.SelectedIndex = 4;
                    break;
                case "用户消息":
                    this.tabControl1.SelectedIndex = 5;
                    break;
                default:

                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                    this.label7.Text = "按账号查询：";
                    break;
                case 1:
                    this.label7.Text = "按姓名查询：";
                    break;
                case 2:
                    this.label7.Text = "按性别查询：";
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataInquireSQL dis = new DataInquireSQL();
            DataSet ds = dis.Inquire("账户信息", this.comboBox1.SelectedItem.ToString(), this.textBox2.Text);
            this.dataGridView1.DataSource = ds.Tables[0].DefaultView;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataAddSQL das = new DataAddSQL();
            int age;
            if (this.textBox7.Text.Length == 0)
            {
                DateTime dt1 = Convert.ToDateTime(this.dateTimePicker1.Text);
                DateTime dt2 = DateTime.Now;
                TimeSpan span = dt2.Subtract(dt1);
                age = (span.Days + 1) / 365;
            }
            else
            {
                age = Convert.ToInt16(this.textBox7.Text);
            }
            if (das.Add_User(Convert.ToInt64(this.textBox3.Text),
                this.textBox4.Text,
                this.textBox6.Text,
                this.textBox5.Text,
                this.comboBox2.Text, 
                age,
                Convert.ToDateTime(this.dateTimePicker1.Text)))
            {
                MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("添加失败，账号已存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool select_doORnot = false;
        private void button4_Click(object sender, EventArgs e)
        {
            DataInquireSQL dis = new DataInquireSQL();
            DataSet ds = dis.Inquire("账户信息", "账号", this.textBox8.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (this.textBox8.Text.Length != 0)
                {
                    this.textBox9.Text = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    this.textBox10.Text = ds.Tables[0].Rows[0].ItemArray[1].ToString();
                    this.textBox11.Text = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    this.textBox12.Text = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    this.comboBox3.Text = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    this.textBox13.Text = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                    this.dateTimePicker2.Text = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                    select_doORnot = true;
                }
                else if (this.textBox8.Text.Length == 0)
                {
                    MessageBox.Show("查询失败，未输入账户", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("查询失败，账户不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["administrator_password"].Value = this.textBox1.Text;
            configuration.Save();
            if (ConfigurationManager.AppSettings["administrator_password"].Equals(this.textBox1.Text))
            {
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.textBox1.Clear();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int select_row = e.RowIndex;
            DataInquireSQL dis = new DataInquireSQL();
            DataSet ds = dis.Inquire_Loading("账户信息", "账号", null);
            if (ds != null)
            {
                this.textBox9.Text = ds.Tables[0].Rows[select_row].ItemArray[0].ToString();
                this.textBox10.Text = ds.Tables[0].Rows[select_row].ItemArray[1].ToString();
                this.textBox11.Text = ds.Tables[0].Rows[select_row].ItemArray[2].ToString();
                this.textBox12.Text = ds.Tables[0].Rows[select_row].ItemArray[3].ToString();
                this.comboBox3.Text = ds.Tables[0].Rows[select_row].ItemArray[4].ToString();
                this.textBox13.Text = ds.Tables[0].Rows[select_row].ItemArray[5].ToString();
                this.dateTimePicker2.Text = ds.Tables[0].Rows[select_row].ItemArray[6].ToString();
                select_doORnot = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (select_doORnot == true)
            {
                DataUpdataSQL dus = new DataUpdataSQL();
                int age;
                if (this.textBox13.Text.Length == 0)
                {
                    DateTime dt1 = Convert.ToDateTime(this.dateTimePicker1.Text);
                    DateTime dt2 = DateTime.Now;
                    TimeSpan span = dt2.Subtract(dt1);
                    age = (span.Days + 1) / 365;
                }
                else
                {
                    age = Convert.ToInt16(this.textBox13.Text);
                }
                if (dus.Updata_User(Convert.ToInt64(this.textBox9.Text),
                    this.textBox10.Text,
                    this.textBox11.Text,
                    this.textBox12.Text,
                    this.comboBox3.Text,
                    age,
                    Convert.ToDateTime(this.dateTimePicker2.Text),
                    this.textBox8.Text))
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("修改失败，请检查数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("请先查询或者选择账户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataDeleteSQL dds = new DataDeleteSQL();
            string select_account = null;
            if (this.textBox8.Text.Length != 0)
            {
                select_account = this.textBox8.Text;
            }
            else
            {
                if (this.textBox9.Text.Length != 0)
                {
                    select_account = this.textBox9.Text;
                }
                else
                {
                    MessageBox.Show("请选择要删除的账户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (dds.Delete_User("账户信息", "账号", select_account))
            {
                MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataInquireSQL dis = new DataInquireSQL();
                DataSet ds = dis.Inquire_Loading("账户信息", "账号", null);
                this.dataGridView2.DataSource = ds.Tables[0].DefaultView;
            }
            else
            {
                MessageBox.Show("删除失败，账号不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //本地发送验证码
            string code = "2019";
            send_verificationcode = new Send_VerificationCode(Login.user_apply.Get_VerificationCode);
            if (send_verificationcode != null)
            {
                send_verificationcode.Invoke(code);
            }
            //指定电子邮件发送方式
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //指定SMTP服务器
            smtp.Host = "smtp.qq.com";
            //是否使用SSL加密链接
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //设置发件人邮箱和授权码
            smtp.Credentials = new NetworkCredential("1456478291@qq.com", "danwcpgnkfgxjdfc");

            //设置发件人与收件人
            MailMessage mail = new MailMessage("1456478291@qq.com", user_information[2]);
            //主题
            mail.Subject = "注册申请";
            //内容
            mail.Body = "申请通过，欢迎使用城市规划系统！请记住自己的用户名与密码。请查收验证码：" + code + "。验证码一日内有效";
            //编码格式
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            //设置优先级
            mail.Priority = MailPriority.Low;
            try
            {
                // 发送邮件
                smtp.Send(mail);
            }
            catch
            {
                MessageBox.Show("邮件发送失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //指定电子邮件发送方式
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //指定SMTP服务器
            smtp.Host = "smtp.qq.com";
            //是否使用SSL加密链接
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //设置发件人邮箱和密码
            smtp.Credentials = new NetworkCredential("1456478291@qq.com", "czp001030czp");

            //设置发件人与收件人
            MailMessage mail = new MailMessage("1456478291@qq.com", user_information[2]);
            //主题
            mail.Subject = "注册申请";
            //内容
            mail.Body = "抱歉，申请不通过。请检查信息";
            //编码格式
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            //设置优先级
            mail.Priority = MailPriority.Low;
            try
            {
                // 发送邮件
                smtp.Send(mail);
            }
            catch
            {
                MessageBox.Show("邮件发送失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Display_UserMessage(string[] user_information)
        {
            this.richTextBox1.SelectedText = ("------------------用户信息------------------" + '\n');
            this.richTextBox1.SelectedText = ("用户名：" + user_information[0] + '\n');
            this.richTextBox1.SelectedText = ("密码：" + user_information[1] + '\n');
            this.richTextBox1.SelectedText = ("邮箱：" + user_information[2] + '\n');
            this.richTextBox1.SelectedText = ("姓名：" + user_information[3] + '\n');
            this.richTextBox1.SelectedText = ("性别：" + user_information[4] + '\n');
            this.richTextBox1.SelectedText = ("年龄：" + user_information[5] + '\n');
            this.richTextBox1.SelectedText = ("生日：" + user_information[6] + '\n');
            this.user_information = user_information; 
        }
        public void Find_UserPassword(string user_account)
        {
            //根据用户名，查询原账户的邮箱            
            DataInquireSQL dis = new DataInquireSQL();
            DataSet ds = dis.Inquire("账户信息", "账号", user_account);
            string user_mail = ds.Tables[0].Rows[0].ItemArray[2].ToString().Trim();
            //本地发送验证码
            string code = "2019";
            send_verificationcode = new Send_VerificationCode(Login.user_apply.Get_VerificationCode);
            if (send_verificationcode != null)
            {
                send_verificationcode.Invoke(code);
            }
            //指定电子邮件发送方式
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //指定SMTP服务器
            smtp.Host = "smtp.qq.com";
            //是否使用SSL加密链接
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            //设置发件人邮箱和授权码
            smtp.Credentials = new NetworkCredential("1456478291@qq.com", "danwcpgnkfgxjdfc");

            //设置发件人与收件人
            MailMessage mail = new MailMessage("1456478291@qq.com", user_mail);
            //主题
            mail.Subject = "找回密码申请";
            //内容
            mail.Body = "申请通过。请查收验证码：" + code + "。验证码一日内有效";
            //编码格式
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            //设置优先级
            mail.Priority = MailPriority.Low;     
            try
            {
                // 发送邮件
                smtp.Send(mail);
            }
            catch
            {
                MessageBox.Show("邮件发送失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
