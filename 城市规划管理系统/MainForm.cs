using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using 城市规划管理系统.BLL;
using static 城市规划管理系统.BLL.DataLoad;
using static 城市规划管理系统.BLL.LightManage;
using static 城市规划管理系统.BLL.ViewManage;
using static 城市规划管理系统.BLL.ModelManage;
using static 城市规划管理系统.BLL.DataEntity;
using static 城市规划管理系统.BLL.DataDisplay;
using System.Collections;
using SharpGL.SceneGraph.Assets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Globalization;
using 城市规划管理系统.DAL;
using System.Data;
using System.ComponentModel;

namespace 城市规划管理系统
{
    public partial class MainForm : Form
    {

        public MainForm()
        {         
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            //事件注册
            this.MouseWheel += new MouseEventHandler(this.openGLControl1_MouseWheel);
            CommandDisplay.addmodel_type_changed += Set_CommandDisplay;
        }

        public static point_3D eye_centre;
        public CameraParament camera_parameter;
        public Point NowPos;
        public Point screen_point;

        public static bool mode_change;

        public static bool view_mode;
        public static bool view2D_moving;
        public static bool ViewMove_Flag;

        public static bool DrawTIN_Flag;
        public static bool DrawBuild_Flag;
        public static bool DrawRoad_Flag;
        public static bool DrawWater_Flag;
        public static bool DrawTree_Flag;
        public static bool DrawLamp_Flag;

        public static bool Selected_Flag;
        public static bool Editing_Flag;
        public static bool ModelMove_Flag;
        public static bool AddModel_Flag;

        //public static bool DrawRay_Flag;

        //实例化一个OpenGL绘图控件
        public static OpenGL gl { get; set; }

        //数据加载
        public DataLoad dataLoad = new DataLoad();
        //数据显示
        public PropertySetting propertySetting = new PropertySetting();
        public CommandDisplay display = new CommandDisplay();
        //绘图处理
        public DataDraw dataDraw = new DataDraw();
        //实例化一个视图控制器
        public ViewController viewController;
        //实例化一个灯光控制器
        public LightControl lightControl;
        //实例化一个纹理控制器
        public TextureControl textureControl;
        //实例化一个模型控制器
        public ModelControl modelControl = new ModelControl();
        public Pick picking = new Pick();

        //数据库操作
        DataInquireSQL dis = new DataInquireSQL();
        DataAddSQL das = new DataAddSQL();
        DataDeleteSQL dds = new DataDeleteSQL();
                     
        private void Form2_Load(object sender, EventArgs e)
        {
            eye_centre = new point_3D();         
            mode_change = false;

            this.propertyGrid1.SelectedObject = propertySetting;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Maximum = 9;
            this.comboBox1.Text = "0.1";
            this.comboBox2.SelectedIndex = 0;
            this.comboBox2.SelectedIndex = 0;

        }
        //-----------------------------------------------OpenGLControl1----------------------------------------
        private void openGLControl1_Load(object sender, EventArgs e)
        {
            view_mode = false;
            view2D_moving = false;
            ViewMove_Flag = false;

            DrawTIN_Flag = false;
            DrawBuild_Flag = false;
            DrawRoad_Flag = false;
            DrawWater_Flag = false;
            DrawTree_Flag = false;
            DrawLamp_Flag = false;

            Selected_Flag = false;
            Editing_Flag = false;
            ModelMove_Flag = false;
            AddModel_Flag = false;

        }
        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            //由于该事件的加载优先级最高，准备工作的实例化如下:
            gl = new OpenGL();
            gl = this.openGLControl1.OpenGL;

            editing_point = new point_3D("null", 0, 0, 0);

            //灯光控制
            lightControl = new LightControl();
            //纹理控制
            textureControl = new TextureControl();
            //视角控制
            viewController = new ViewController();

            //启用阴影平滑
            gl.ShadeModel(OpenGL.GL_SMOOTH);
            //设置深度缓存
            gl.ClearDepth(1f);
            //启用深度测试
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            //测试类型
            gl.DepthFunc(OpenGL.GL_LEQUAL);
            //设置背景色
            gl.ClearColor(0.75f, 0.75f, 0.75f, 0);
            //lightController.SetLight(gl);
            
            //初始化天空盒纹理
            textureControl.Initialize_Texture_Skybox(gl);

            //其他初始化
            Set_CommandDisplay();
        }        
        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.Perspective(50.0f, (double)Width / (double)Height, 0.01, 1000);
            camera_parameter = viewController.GetCamera_Parameter();
            #region//视角切换，第一人称游戏视角
            //if (view_mode == false)
            //{
            //    timing_parameter = my_camera.GetCamera_Parameter();
            //    my_camera.setLook2D(gl, timing_parameter.position.x, timing_parameter.position.y, timing_parameter.position.z,
            //        timing_parameter.view.x, timing_parameter.view.y, timing_parameter.view.z,
            //        timing_parameter.upVector.x, timing_parameter.upVector.y, timing_parameter.upVector.z);
            //}
            //else if (view_mode == true)
            //{
            //    my_camera.setLook3D(gl);
            //    int middleX = this.openGLControl1.Width / 2;
            //    int middleY = this.openGLControl1.Height / 2;
            //    if (draw_startOrend == true)
            //    {
            //        my_camera.setViewByMouse(middleX, middleY);
            //    }
            //}
            #endregion
            if (view_mode == false)
            {
                viewController.SetLook2D(gl, camera_parameter);
            }
            else if (view_mode == true)
            {
                viewController.SetLook3D(gl);
            }
            //开始绘制
            //实时刷新天空盒
            dataDraw.Draw_SkyBox(gl, new Cube("SkyBox", new point_3D("centre_point", camera_parameter.eye.X, camera_parameter.eye.Y, -camera_parameter.eye.Z - 500), 1000, 1000, 1000),
                ModelManage.TextureControl.skybox_textures[this.comboBox2.SelectedIndex]);
            //绘制基本格网和坐标轴
            dataDraw.Draw_BaseGrid(gl);
            dataDraw.Draw_Axis(gl);
            //绘制TIN地形
            if (DrawTIN_Flag == true)
            {
                int number = all_tin.Count;
                for(int i = 0;i < number; i++)
                {
                    int number1 = all_tin[i].triangle_NET.Count;
                    for (int j = 0; j < number1; j++)
                    {
                        dataDraw.Draw_TIN(gl, all_tin[i].triangle_NET[j], TIN.height_TIN);
                    }
                }
            }
            //绘制建筑模型
            if (DrawBuild_Flag == true)
            {
                //绘制最简单的建筑
                int number = all_build.Count;
                for(int i = 0; i < number; i++)
                {
                    if (all_build[i].build_type == "Build_1")
                    {
                        Build<object>.Build_1 build_1 = (Build<object>.Build_1)all_build[i].build_item;
                        dataDraw.Draw_Build_1(gl, build_1, Build<object>.height_build);
                    }
                }
            }
            //绘制道路模型
            if(DrawRoad_Flag == true)
            {
                int number = all_road.Count;
                for (int i = 0; i < number; i++)
                {
                    if (all_road[i].road_type == "Road_1")
                    {
                        Road<object>.Road_1 road_1 = (Road<object>.Road_1)all_road[i].road_item;
                        dataDraw.Draw_Road_1(gl, road_1, Road<object>.height_road);
                    }
                    if (all_road[i].road_type == "Road_2")
                    {
                        Road<object>.Road_2 road_2 = (Road<object>.Road_2)all_road[i].road_item;
                        dataDraw.Draw_Road_2(gl, road_2, Road<object>.height_road);
                    }
                }
            }
            //绘制水域模型
            if(DrawWater_Flag == true)
            {
                int number = all_water.Count;
                for(int i = 0; i < number; i++)
                {
                    dataDraw.Draw_Water(gl, all_water[i]);
                }
            }
            //绘制路灯模型
            if(DrawLamp_Flag == true)
            {
                int number = all_lamp.Count;
                for (int i = 0; i < number; i++)
                {
                    dataDraw.Draw_Lamp(gl, all_lamp[i], 1);
                }
            }
            //绘制树木模型
            if(DrawTree_Flag == true)
            {
                int number = all_tree.Count;
                for (int i = 0; i < number; i++)
                {
                    dataDraw.Draw_Tree(gl, all_tree[i]);
                }
            }
            //选择编辑模式
            if (mode_change == true)
            {
                if (Selected_Flag == true)
                {
                    //选择TIN地形并且高亮显示边框（除TIN地形外，其余模型一律高亮显示包围盒）                    
                    if (Pick.selected_data.data_type[0] == "TIN")
                    {                      
                        triangle_3D TIN_item = Pick.selected_data.data_item as triangle_3D;
                        dataDraw.Highlight_Selected(gl, TIN_item, TIN.height_TIN);
                    }
                    //选择其他模型
                    else
                    {                        
                        dataDraw.Highlight_Selected(gl, Pick.selected_data.bounding_box);
                    }
                }
                if(AddModel_Flag == true)
                {
                    if (add_model.data_type[0] == "Build")
                    {
                        if (add_model.data_type[1] == "Build_1")
                        {
                            Build<object> build = (Build<object>)add_model.data_item;
                            Build<object>.Build_1 build_1 = (Build<object>.Build_1)build.build_item;
                            dataDraw.Draw_Build_1(gl, build_1, 0);
                        }
                    }
                    else if (add_model.data_type[0] == "Road")
                    {
                        if (add_model.data_type[1] == "Road_1")
                        {
                            Road<object>.Road_1 road_1 = (Road<object>.Road_1)add_model.data_item;
                            dataDraw.Draw_Road_1(gl, road_1, 0);
                        }
                    }

                    if(ModelManage.control_points.Count > 0)
                    {
                        dataDraw.Draw_ControlPoint(gl, ModelManage.control_points);
                    }
                }
            }

            this.textBox1.Text = camera_parameter.eye.X.ToString("0.000");
            this.textBox2.Text = camera_parameter.eye.Y.ToString("0.000");
            this.textBox3.Text = camera_parameter.eye.Z.ToString("0.000");
            this.label4.Text = angle.ToString() + "°";
            
            gl.Finish();
            gl.Flush();
            
        }
        private void openGLControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (mode_change == false)
            {
                if (view_mode == true)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.W:
                            viewController.GoFront();
                            break;
                        case Keys.S:
                            viewController.GoBack();
                            break;
                        case Keys.A:
                            viewController.GoLeft();
                            break;
                        case Keys.D:
                            viewController.GoRight();
                            break;
                        case Keys.Q:
                            viewController.GoUp();
                            break;
                        case Keys.E:
                            viewController.GoDown();
                            break;
                        default:

                            break;
                    }
                }
            }
            else if(mode_change == true)
            {
                if (AddModel_Flag == true)
                {
                    if (CommandDisplay.AddModel_Type[0] == "Road")
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.D:
                                int number = ModelManage.control_points.Count;
                                if (number > 0)
                                {
                                    ModelManage.control_points.RemoveAt(number - 1);
                                }
                                break;
                            case Keys.Q:
                                control_points.Clear();
                                CommandDisplay.AddModel_Type = new string[] { "null", "null" };
                                add_model.Initialize_AddModle();
                                AddModel_Flag = false;
                                break;
                            default:

                                break;
                        }
                    }
                    Set_CommandDisplay();
                }
            }
        }
        private void openGLControl1_KeyUp(object sender, KeyEventArgs e)
        {

        }       
        private void openGLControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
        private void openGLControl1_MouseClick(object sender, MouseEventArgs e)
        {           
            if (mode_change == true)
            {
                screen_point.X = e.X;
                screen_point.Y = e.Y;
                if(Editing_Flag == false)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if(AddModel_Flag == true)
                        {
                            point_3D now_pos = new point_3D();
                            now_pos = picking.Get_GroundPoint(gl, screen_point);
                            if (CommandDisplay.AddModel_Type[0] == "Build")
                            {
                                all_build.Add((Build<object>)add_model.data_item);
                                AddModel_Flag = false;
                            }
                            else if (CommandDisplay.AddModel_Type[0] == "Road")
                            {
                                bool YesOrNo = false;
                                if (CommandDisplay.AddModel_Type[1] == "Road_1")
                                {
                                    if(ModelManage.control_points.Count == 3)
                                    {
                                        ModelManage.control_points.Add(now_pos);
                                        YesOrNo = true;
                                    }
                                    else
                                    {
                                        ModelManage.control_points.Add(now_pos);
                                    }
                                }
                                else if (CommandDisplay.AddModel_Type[1] == "Road_2")
                                {
                                    if (ModelManage.control_points.Count == 100)
                                    {
                                        ModelManage.control_points.Add(now_pos);
                                        YesOrNo = true;
                                    }
                                    else
                                    {
                                        ModelManage.control_points.Add(now_pos);
                                    }
                                }
                                if (YesOrNo == true)
                                {
                                    modelControl.Create_Model(control_points, CommandDisplay.AddModel_Type);
                                    control_points.Clear();
                                    CommandDisplay.AddModel_Type = new string[] { "null", "null" };
                                    add_model.Initialize_AddModle();
                                    AddModel_Flag = false;
                                }
                            }                         
                        }
                        else
                        {   
                            //拾取模型
                            SelectedData<object> selected_now = new SelectedData<object>();
                            selected_now = picking.Picking(gl, screen_point);
                            Pick.selected_data = Pick.selected_data & selected_now;
                            if (selected_now.data_type != null)
                            {
                                Selected_Flag = true;
                                Pick.selected_return = Pick.selected_return & selected_now;
                                propertySetting.Display_Information();
                                this.propertyGrid1.Refresh();
                            }
                            else
                            {
                                this.propertyGrid1.ResetText();
                                Selected_Flag = false;
                            }
                        }
                    }
                    //右键显示菜单
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (Pick.selected_data.data_type != null)
                        {
                            bool show_menu = picking.Show_Menu_Confine(gl, new Point(e.X, e.Y), Pick.selected_data);
                            if (show_menu == true)
                            {
                                this.contextMenuStrip1.Show(Cursor.Position);
                            }
                        }
                    }
                }
            }
            Set_CommandDisplay();
        }
        private void openGLControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (mode_change == false)
            {
                if (view_mode == false)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        ViewMove_Flag = true;
                        NowPos.X = e.X;
                        NowPos.Y = e.Y;
                    }
                }
                else if (view_mode == true)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        NowPos = e.Location;
                    }
                }
            }
            else if(mode_change == true)
            {
                if(Selected_Flag == true)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (Pick.selected_data.data_type != null)
                        {
                            if (ModelMove_Flag == false)
                            {
                                NowPos.X = e.X;
                                NowPos.Y = e.Y;
                                picking.Intersect_Point(false, gl, NowPos, camera_parameter, Pick.selected_data, (double)Width, (double)Height);
                                if (editing_point.point_name != "null")
                                {
                                    Editing_Flag = true;
                                }
                            }
                            else if(ModelMove_Flag == true)
                            {
                                NowPos.X = e.X;
                                NowPos.Y = e.Y;
                                Editing_Flag = true;
                            }
                        }
                    }
                }

            }
        }
        private void openGLControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mode_change == false)
            {
                if (view_mode == false)
                {
                    //二维移动
                    if (e.Button == MouseButtons.Left && ViewMove_Flag)
                    {
                        double move_X = (e.X - NowPos.X) * viewController.GetSpeed() / 100;
                        double move_Y = (e.Y - NowPos.Y) * viewController.GetSpeed() / 100;
                        camera_parameter.eye.X = camera_parameter.eye.X + move_X;
                        camera_parameter.eye.Y = camera_parameter.eye.Y + move_Y;
                        camera_parameter.center.X = camera_parameter.eye.X;
                        camera_parameter.center.Y = camera_parameter.eye.Y;
                        viewController.SetCamera_Parameter(camera_parameter);

                    }
                }
                else if (view_mode == true)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        var turnAngle = (e.Location.X - NowPos.X) * viewController.GetSpeed() / 100;
                        var staggerAngle = (e.Location.Y - NowPos.Y) * viewController.GetSpeed() / 100;
                        NowPos = e.Location;
                        viewController.Turn(-turnAngle);
                        viewController.Stagger(-staggerAngle);
                        viewController.SetLook3D(gl);
                    }
                }
            }
            else if(mode_change == true)
            {
                if(Selected_Flag == true)
                {
                    if(ModelMove_Flag == false)
                    {
                        point_3D p = new point_3D();
                        if (selected_data.data_type[0] != "TIN")
                        {
                             p = picking.Intersect_Point(true, gl, e.Location, camera_parameter, Pick.selected_data, (double)Width, (double)Height);
                        }
                        if (this.radioButton1.Checked == true)
                        {
                            if (p.point_name == "up_a" || p.point_name == "up_d")
                            {
                                this.Cursor = Cursors.SizeNWSE;
                            }
                            else if (p.point_name == "up_b" || p.point_name == "up_c")
                            {
                                this.Cursor = Cursors.SizeNESW;
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                            }
                        }
                        else if (this.radioButton2.Checked == true)
                        {
                            if (p.point_name == "up_a" || p.point_name == "up_b" || p.point_name == "up_c" || p.point_name == "up_d")
                            {
                                this.Cursor = Cursors.SizeNS;
                            }
                            else
                            {
                                this.Cursor = Cursors.Default;
                            }
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                    else if(ModelMove_Flag == true)
                    {
                        picking.Create_Ray(gl, e.Location);
                        double t = picking.Intersect_AABB(picking.ray_start, picking.ray_direction, Pick.selected_data.bounding_box);
                        if (t > 0)
                        {
                            this.Cursor = Cursors.Hand;
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }

                    if (Editing_Flag == true)
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            if(ModelMove_Flag == false)
                            {
                                if (Pick.selected_data.data_type != null && Pick.editing_point.point_name != "null")
                                {
                                    //高度不变在XY平面调整长度和宽度                     
                                    if (this.radioButton1.Checked == true)
                                    {
                                        double move_X = (e.X - NowPos.X) * viewController.GetSpeed() / 50;
                                        double move_Y = (e.Y - NowPos.Y) * viewController.GetSpeed() / 50;
                                        modelControl.Change_BoundingBox_Size(move_X, move_Y);
                                        modelControl.Change_Model_Size(move_X, move_Y);
                                    }
                                    //调整高度
                                    else if (this.radioButton2.Checked == true)
                                    {
                                        double move_Z = (e.Y - NowPos.Y) * viewController.GetSpeed() / 100;
                                        modelControl.Change_BoundingBox_Size(move_Z);
                                        modelControl.Change_Model_Size(move_Z);
                                    }
                                }
                            }
                            else if (ModelMove_Flag == true)
                            {
                                double move_X = (e.X - NowPos.X) * viewController.GetSpeed() / 50;
                                double move_Y = (e.Y - NowPos.Y) * viewController.GetSpeed() / 50;
                                modelControl.Move_BoundingBox(move_X, move_Y);
                                modelControl.Move_Model(move_X, move_Y);
                            }                        
                        }
                    }
                    if (AddModel_Flag == true)
                    {
                        modelControl.Create_Model(gl, e.Location, picking, CommandDisplay.AddModel_Type);
                    }
                }
            }
            gl.Flush();
        }
        private void openGLControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if(mode_change == false)
            {
                if (view_mode == false)
                {
                    ViewMove_Flag = false;
                }
            }
            else if(mode_change == true)
            {
                if (Editing_Flag == true)
                {
                    editing_point = new point_3D("null", 0, 0, 0);
                    //this.Cursor = Cursors.Default;
                    Editing_Flag = false;
                    ModelMove_Flag = false;
                }
            }
        }
        private void openGLControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            if(mode_change == false)
            {
                if (view_mode == false)
                {
                    //鼠标放大
                    if (e.Delta > 0)
                    {
                        if (camera_parameter.eye.Z < -1)
                        {
                            camera_parameter.eye.Z = camera_parameter.eye.Z + viewController.GetSpeed();
                        }
                    }
                    else if (e.Delta < 0)
                    {
                        if (camera_parameter.eye.Z < -1)
                        {
                            camera_parameter.eye.Z = camera_parameter.eye.Z - viewController.GetSpeed();
                        }
                    }
                    viewController.SetCamera_Parameter(camera_parameter);
                }
            }
        }
        private void openGLControl1_MouseEnter(object sender, EventArgs e)
        {
            if(mode_change == false)
            {
                view2D_moving = true;
                this.NowPos = Control.MousePosition;
                var item = this.openGLControl1.Parent;
                Control preItem = this.openGLControl1;
                while (item != null)
                {
                    preItem = item;
                    item = item.Parent;
                }
                this.NowPos.Offset(preItem.Location);
            }
        }
        private void openGLControl1_MouseLeave(object sender, EventArgs e)
        {
            if(mode_change == false)
            {
                view2D_moving = false;
            }
            else if(mode_change == true)
            {
                this.Cursor = Cursors.Default;
            }
        }
        //-------------------------------------------------------ToolStripMenuItem------------------------------------------
        private void 打开项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataLoad.Load();
        }
        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mes = MessageBox.Show("确定退出吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mes == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void 数据入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool YesOrNo_All = false;
            bool YesOrNo_List = false;
            if(all_tin.Count > 0)
            {
                for(int i = 0;i < all_tin.Count; i++)
                {
                    DataSet ds = dis.Inquire("数据总览", "data_name", all_tin[i].TIN_name);
                    if(ds.Tables[0].Rows.Count == 0)//重复性检查
                    {
                        int number_now = dis.Inquire_ModelNumber("数据总览", "data_index", null);
                        YesOrNo_List = das.Add_TIN(number_now + 1, all_tin[i]);
                        YesOrNo_All = das.Add_ModelOverview(number_now + 1, all_tin[i].TIN_name, "TIN");
                    }
                }
                if(YesOrNo_List == false || YesOrNo_All == false)
                {
                    MessageBox.Show("地形数据存入数据库失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if(all_build.Count > 0)
            {
                for (int i = 0; i < all_build.Count; i++)
                {
                    DataSet ds = dis.Inquire("数据总览", "data_name", all_build[i].build_name);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        int number_now = dis.Inquire_ModelNumber("数据总览", "data_index", null);
                        YesOrNo_List = das.Add_Build(number_now + 1, all_build[i]);
                        YesOrNo_All = das.Add_ModelOverview(number_now + 1, all_build[i].build_name, all_build[i].build_type);
                    }
                }
                if (YesOrNo_List == false || YesOrNo_All == false)
                {
                    MessageBox.Show("建筑数据存入数据库失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (all_road.Count > 0)
            {
                for (int i = 0; i < all_road.Count; i++)
                {
                    DataSet ds = dis.Inquire("数据总览", "data_name", all_road[i].road_name);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        int number_now = dis.Inquire_ModelNumber("数据总览", "data_index", null);
                        YesOrNo_List = das.Add_Road(number_now + 1, all_road[i]);
                        YesOrNo_All = das.Add_ModelOverview(number_now + 1, all_road[i].road_name, all_road[i].road_type);
                    }
                }
                if (YesOrNo_List == false || YesOrNo_All == false)
                {
                    MessageBox.Show("道路数据存入数据库失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (YesOrNo_List == true || YesOrNo_All == true)
            {
                MessageBox.Show("数据成功存入数据库", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void 数据出库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int number_now = dis.Inquire_ModelNumber("数据总览", "data_index", null);
            bool YesOrNo = false;
            if(number_now > 0)
            {
                YesOrNo = dds.Delete_AllModel("TIN", null, null);
                YesOrNo = dds.Delete_AllModel("Build", null, null);
                YesOrNo = dds.Delete_AllModel("Road", null, null);
                YesOrNo = dds.Delete_AllModel("数据总览", null, null);
                if(YesOrNo == true)
                {
                    MessageBox.Show("数据出库成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("数据出库失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }
        private void 维度切换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraParament change_parameter = viewController.GetCamera_Parameter();
            if (view_mode == false)
            {
                change_parameter.eye.Y = change_parameter.eye.Y - 30.0f;
                change_parameter.up.X = 0.0f;
                change_parameter.up.Y = 0.0f;
                change_parameter.up.Z = -1.0f;
                viewController.SetLook2D(gl, change_parameter);
                view_mode = true;
            }
            else if (view_mode == true)
            {
                change_parameter.eye.Y = change_parameter.eye.Y + 30.0f;
                change_parameter.center.X = change_parameter.eye.X;
                change_parameter.center.Y = change_parameter.eye.Y;
                change_parameter.center.Z = 0.0f;
                change_parameter.up.X = 0.0f;
                change_parameter.up.Y = 1.0f;
                change_parameter.up.Z = 0.0f;
                view_mode = false;
            }
            viewController.SetCamera_Parameter(change_parameter);
        }
        private void tIN地形模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawTIN_Flag == false)
            {
                if (all_tin.Count > 0)
                {
                    //初始化地形纹理
                    textureControl.Initialize_Texture_TIN(gl);
                    DrawTIN_Flag = true;
                    ModelManage.DrawTIN_Flag = true;
                }
                else
                {
                    MessageBox.Show("加载地形失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (DrawTIN_Flag == true)
            {
                DrawTIN_Flag = false;
                ModelManage.DrawTIN_Flag = false;
            }
        }
        private void 建筑模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawBuild_Flag == false)
            {
                if (all_build.Count > 0)
                {
                    //初始化建筑纹理
                    textureControl.Initialize_Texture_Build(gl);
                    DrawBuild_Flag = true;
                    ModelManage.DrawBuild_Flag = true;
                }
                else
                {
                    MessageBox.Show("加载建筑失败，可能还未导入建筑数据集", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (DrawBuild_Flag == true)
            {
                DrawBuild_Flag = false;
                ModelManage.DrawBuild_Flag = false;
            }
        }
        private void 道路模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(DrawRoad_Flag == false)
            {
                if(all_road.Count > 0)
                {
                    //初始化道路纹理
                    textureControl.Initialize_Texture_Road(gl);
                    DrawRoad_Flag = true;
                    ModelManage.DrawRoad_Flag = true;
                }
                else
                {
                    MessageBox.Show("加载道路失败，可能还未导入道路数据集", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (DrawRoad_Flag == true)
            {
                DrawRoad_Flag = false;
                ModelManage.DrawRoad_Flag = false;
            }
        }
        private void 水域模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawWater_Flag == false)
            {
                if (all_water.Count > 0)
                {
                    //初始化道路纹理
                    textureControl.Initialize_Texture_Water(gl);
                    DrawWater_Flag = true;
                    ModelManage.DrawWater_Flag = true;
                }
                else
                {
                    MessageBox.Show("加载水域失败，可能还未导入水域数据集", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (DrawWater_Flag == true)
            {
                DrawWater_Flag = false;
                ModelManage.DrawWater_Flag = false;
            }
        }
        private void 树木模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawTree_Flag == false)
            {
                if (all_tree.Count > 0)
                {
                    //初始化道路纹理
                    textureControl.Initialize_Texture_Tree(gl);
                    DrawTree_Flag = true;
                    ModelManage.DrawTree_Flag = true;
                }
                else
                {
                    MessageBox.Show("加载树木失败，可能还未导入水域数据集", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (DrawTree_Flag == true)
            {
                DrawTree_Flag = false;
                ModelManage.DrawTree_Flag = false;
            }
        }
        private void 路灯模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DrawLamp_Flag == false)
            {
                if (all_lamp.Count > 0)
                {
                    //初始化道路纹理
                    textureControl.Initialize_Texture_Lamp(gl);
                    DrawLamp_Flag = true;
                    ModelManage.DrawLamp_Flag = true;
                }
                else
                {
                    MessageBox.Show("加载路灯失败，可能还未导入水域数据集", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (DrawLamp_Flag == true)
            {
                DrawLamp_Flag = false;
                ModelManage.DrawLamp_Flag = false;
            }
        }
        private void 添加模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {          
            Login.model_base.Show();
        }
        private void 删除模型ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(selected_data.data_type != null)
            {
                if(selected_data.data_type[0] == "TIN")
                {

                }
                else if(selected_data.data_type[0] == "Build")
                {
                    all_build.RemoveAt(selected_data.data_index[0]);
                    selected_data.Initialize_SelectedData();
                    Selected_Flag = false;
                }
                else if (selected_data.data_type[0] == "Road")
                {
                    all_road.RemoveAt(selected_data.data_index[0]);
                    selected_data.Initialize_SelectedData();
                    Selected_Flag = false;
                }
            }
        }

        //--------------------------------------------------------ToolStripButton--------------------------------------------

        private void toolStripButton7_Click_1(object sender, EventArgs e)
        {
            维度切换ToolStripMenuItem_Click(sender, e);
            Set_CommandDisplay();
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (mode_change == false)
            {
                mode_change = true;
            }
            else if (mode_change == true)
            {
                mode_change = false;
            }
            Set_CommandDisplay();
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            List<point_3D> centre_point = new List<point_3D>();
            if (all_tin.Count > 0)
            {
                for (int i = 0; i < all_tin.Count; i++)
                {
                    centre_point.Add(TIN.Find_CentrePoint(all_tin[i].point_TIN));
                }
            }
            if (all_build.Count > 0)
            {
                for (int i = 0; i < all_build.Count; i++)
                {
                    centre_point.Add(Build<object>.Find_CentrePoint(all_build));
                }
            }

            //...
            else
            {

            }
            double x_total, y_total;
            x_total = y_total = 0;
            int count = centre_point.Count;
            for (int i = 0; i < count; i++)
            {
                x_total = x_total + centre_point[i].point_x;
                y_total = y_total + centre_point[i].point_y;
            }
            double x_centre = x_total / count;
            double y_centre = y_total / count;
            eye_centre.point_x = x_centre;
            eye_centre.point_y = y_centre;
            CameraParament change_parameter = viewController.GetCamera_Parameter();
            if (view_mode == false)
            {
                change_parameter.eye.X = eye_centre.point_x;
                change_parameter.eye.Y = eye_centre.point_y;
                change_parameter.center.X = change_parameter.eye.X;
                change_parameter.center.Y = change_parameter.eye.Y;


            }
            else if (view_mode == true)
            {
                change_parameter.eye.X = eye_centre.point_x;
                change_parameter.eye.Y = eye_centre.point_y - 30.0f;
                change_parameter.center.X = change_parameter.eye.X;
                change_parameter.center.Y = change_parameter.eye.Y + 30.0f;
                change_parameter.center.Z = 0;

            }
            viewController.SetCamera_Parameter(change_parameter);
        }
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            添加模型ToolStripMenuItem_Click(sender, e);
        }
        private void toolStripButton11_Click(object sender, EventArgs e)
        {

            if (toolStripButton11.CheckState == CheckState.Checked)
            {
                this.tabControl1.SelectedIndex = 2;
            }
            else if (toolStripButton11.CheckState == CheckState.Unchecked)
            {
                this.tabControl1.SelectedIndex = 0;
                this.radioButton1.Checked = false;
                this.radioButton2.Checked = false;
            }
        }
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            删除模型ToolStripMenuItem1_Click(sender, e);
        }
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            
        }


        //------------------------------------------------ContextMenuStrip1--------------------------------------------
        private void 属性编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Pick.selected_data.data_type != null)
            {
                PropertySet form5 = new PropertySet();
                form5.Show();
            }
            
        }
        private void 删除模型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            删除模型ToolStripMenuItem1_Click(sender, e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            switch (this.comboBox1.Text)
            {
                case "0.1":
                    viewController.SetSpeed(this.trackBar1.Value * 0.1);
                    break;
                case "1":
                    viewController.SetSpeed(this.trackBar1.Value);
                    break;
                case "10":
                    viewController.SetSpeed(this.trackBar1.Value * 10);
                    break;
                default:
                    viewController.SetSpeed(0);
                    break;
            }

        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            ModelManage.angle = Convert.ToDouble(this.trackBar2.Value);
            modelControl.Rotate_BoundingBox();
            modelControl.Rotate_Model();
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (ModelMove_Flag == false)
            {
                ModelMove_Flag = true;
                this.Cursor = Cursors.Hand;
            }
        }


        private void sceneControl1_Load(object sender, EventArgs e)
        {
            SharpGL.SceneGraph.Quadrics.Sphere sphere = new SharpGL.SceneGraph.Quadrics.Sphere();
            sceneControl1.Scene.SceneContainer.AddChild(sphere);

        }
        //--------------------------------------------------Other------------------------------------------------------
        private void AddModel_Type_Changed(object sender, PropertyChangedEventArgs e)
        {
            Set_CommandDisplay();
        }
        public void Set_CommandDisplay()
        {
            this.richTextBox1.Clear();
            if (mode_change == false)
            {
                this.richTextBox1.SelectedText = ("视图模式" + '\n');
                if (view_mode == false)
                {
                    this.richTextBox1.SelectedText = ("二维视角" + '\n');
                    this.richTextBox1.SelectedText = ("操作提示：鼠标左键按住移动，松开停止移动；鼠标滚轮放大和缩小");
                }
                else if (view_mode == true)
                {
                    this.richTextBox1.SelectedText = ("三维视角" + '\n');
                    this.richTextBox1.SelectedText = ("操作提示：鼠标左键按住旋转视角，松开停止；W：前进；S：后退；A：向左移动；D：向右移动");
                }
            }
            else if (mode_change == true)
            {
                this.richTextBox1.SelectedText = ("编辑模式" + '\n');
                if (AddModel_Flag == true)
                {
                    if (CommandDisplay.AddModel_Type[0] == "Road")
                    {
                        this.richTextBox1.SelectedText = ("添加道路模型" + '\n');
                        if (CommandDisplay.AddModel_Type[1] == "Road_1")
                        {
                            if (ModelManage.control_points.Count == 0)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择一边的起点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 1)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择一边的终点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 2)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择另一边的起点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 3)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择另一边的终点。D：删除上一个点；Q：退出创建模型");
                            }
                        }
                        else if (CommandDisplay.AddModel_Type[1] == "Road_2")
                        {
                            if (ModelManage.control_points.Count == 0)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择一边的起点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 1)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择一边的曲线控制点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 2)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择一边的终点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 3)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择另一边的起点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 4)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择另一边的曲线控制点。D：删除上一个点；Q：退出创建模型");
                            }
                            else if (ModelManage.control_points.Count == 5)
                            {
                                this.richTextBox1.SelectedText = ("操作提示：请选择另一边的终点。D：删除上一个点；Q：退出创建模型");
                            }
                        }
                    }
                }
            }
            else
            {
                this.richTextBox1.SelectedText = ("暂无");
            }
        }
    }
}
