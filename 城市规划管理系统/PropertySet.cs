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
using static 城市规划管理系统.BLL.DataEntity;
using static 城市规划管理系统.BLL.DataLoad;
using static 城市规划管理系统.BLL.ModelManage;

namespace 城市规划管理系统
{
    public partial class PropertySet : Form
    {
        public PropertySet()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            //加载基本信息
            this.textBox1.Text = selected_data.data_index.ToString();
            this.comboBox1.Text = selected_data.data_type.ToString();
            this.textBox2.Text = selected_data.ToString();
            //加载详细信息

        }
        private void button1_Click(object sender, EventArgs e)
        {            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "修改纹理";
            //文件过滤
            ofd.Filter = "图像文件(*.*)|*.*|JPG文件(*.jpg)|*.jpg|PNG文件(*.png)|*.png|JPEG文件(*.jpeg)|*.jpeg";
            //初始对话框
            ofd.InitialDirectory = "C:\\Users\\Administrator\\Desktop";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //获取选择的纹理路径
                string texture_path = ofd.FileName;
                if (texture_path != null)
                {
                    if (selected_data.data_type[0] == "TIN")
                    {
                        this.textBox3.Text = texture_path;
                    }
                    else if (selected_data.data_type[0] == "Build")
                    {
                        this.textBox10.Text = texture_path;
                    }
                    else if (selected_data.data_type[0] == "Road")
                    {
                        this.textBox11.Text = texture_path;
                    }
                }
 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected_data.data_type[0] == "TIN")
            {
                //确定修改数据元在数据集合中的位置
                for(int i = 0; i < all_tin.Count; i++)
                {
                    for(int j = 0; j < all_tin[i].triangle_NET.Count; j++)
                    {
                        if(selected_data.data_name == all_tin[i].triangle_NET[j].surface_name)
                        {
                            //修改基本属性

                            //修改纹理
                            if (this.textBox3.Text != null)
                            {
                                all_tin[i].triangle_NET[j].texture_index = Login.main_form.textureControl.Updata_Texture(MainForm.gl, selected_data, this.textBox3.Text);
                            }
                        }
                    }
                }

            }
            if(selected_data.data_type[0] == "Build")
            {
                //修改基本属性

                //修改中心坐标点信息

                //修改纹理
                if (this.textBox10.Text != null)
                {
                    Build<object>.Build_1 build_1 = all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1;
                    build_1.main_body.round_texture_index = Login.main_form.textureControl.Updata_Texture(MainForm.gl, selected_data, this.textBox10.Text);
                    all_build[selected_data.data_index[0]].build_item = build_1;
                }
            }
            if (selected_data.data_type[0] == "Road")
            {
                //修改基本属性

                //修改中心坐标点信息

                //修改纹理
                if (this.textBox11.Text != null)
                {
                    Road<object>.Road_1 road_1 = all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1;
                    road_1.texture_index = Login.main_form.textureControl.Updata_Texture(MainForm.gl, selected_data, this.textBox11.Text);
                    all_build[selected_data.data_index[0]].build_item = road_1;
                }
            }
            this.Close();
        }
    }
}
