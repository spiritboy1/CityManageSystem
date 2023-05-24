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
using static 城市规划管理系统.UserApply;

namespace 城市规划管理系统
{
    public partial class FindPassword : Form
    {
        public delegate void Send_UserAccount(string user_account);
        public event Send_UserAccount send_useraccount;

        public FindPassword()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            this.tabControl1.SizeMode = TabSizeMode.Fixed;
            this.tabControl1.ItemSize = new Size(0, 1);
            this.tabControl1.Appearance = TabAppearance.FlatButtons;

            this.tabControl1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.textBox1.Text != null)
            {
                send_useraccount += new Send_UserAccount(Login.manage_user.Find_UserPassword);
                if (send_useraccount != null)
                {
                    send_useraccount.Invoke(this.textBox1.Text);
                }
                MessageBox.Show("请注意查收邮箱", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(this.textBox2.Text == verification_code)
            {
                this.tabControl1.SelectedIndex = 1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(this.textBox3.Text != null)
            {
                string[] user_information = new string[7]; 
                //先根据用户名查询用户所有信息
                DataInquireSQL dis = new DataInquireSQL();
                DataSet ds = dis.Inquire_Loading("账户信息", "账号", null);
                if (ds != null)
                {
                    user_information[0] = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    user_information[1] = this.textBox3.Text;
                    user_information[2] = ds.Tables[0].Rows[0].ItemArray[2].ToString();
                    user_information[3] = ds.Tables[0].Rows[0].ItemArray[3].ToString();
                    user_information[4] = ds.Tables[0].Rows[0].ItemArray[4].ToString();
                    user_information[5] = ds.Tables[0].Rows[0].ItemArray[5].ToString();
                    user_information[6] = ds.Tables[0].Rows[0].ItemArray[6].ToString();
                }
                //然后修改部分信息，再覆盖
                DataUpdataSQL dus = new DataUpdataSQL();
                if (dus.Updata_User(Convert.ToInt64(user_information[0]),
                    user_information[1],
                    user_information[2],
                    user_information[3],
                    user_information[4],
                    Convert.ToInt32(user_information[5]),
                    Convert.ToDateTime(user_information[6]),
                    user_information[0]))
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("修改失败，请检查数据", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
