using SharpGL.SceneGraph.Lighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 城市规划管理系统.BLL;
using static 城市规划管理系统.BLL.DataDisplay;
using static 城市规划管理系统.BLL.DataEntity;
using static 城市规划管理系统.BLL.DataLoad;
using static 城市规划管理系统.BLL.ModelManage;

namespace 城市规划管理系统
{
    public partial class ModelBase : Form
    {
        public ModelBase()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            add_model = new AddModel<object>();
            control_points = new List<point_3D>();
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            CommandDisplay.AddModel_Type[0] = "Build";
            CommandDisplay.AddModel_Type[1] = "Build_1";
            MainForm.AddModel_Flag = true;
            this.Hide();
        }
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            MainForm.AddModel_Flag = true;
            CommandDisplay.AddModel_Type = new string[] { "Road", "Road_1" };
            this.Hide();
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            MainForm.AddModel_Flag = true;
            CommandDisplay.AddModel_Type = new string[] { "Road", "Road_2" };
            this.Hide();
        }

    }
}
