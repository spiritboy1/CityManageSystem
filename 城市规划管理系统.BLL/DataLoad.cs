using SharpGL.SceneGraph.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static 城市规划管理系统.BLL.DataEntity;
using static 城市规划管理系统.BLL.ModelManage;

namespace 城市规划管理系统.BLL
{
    public class DataLoad
    {
        
        public static List<TIN> all_tin = new List<TIN>();
        public static List<Build<object>> all_build = new List<Build<object>>();
        public static List<Road<object>> all_road = new List<Road<object>>();
        public static List<Water> all_water = new List<Water>();
        public static List<Tree> all_tree = new List<Tree>();
        public static List<Lamp> all_lamp = new List<Lamp>();

        public DataLoad()
        {
            
        }
        //综合所有模型数据
        public void Load()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "所有文件(*.*)|*.*|文本文件(*.txt)|*.txt";
            dlg.InitialDirectory = "C:\\Users\\Administrator\\Desktop";
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                string str;
                string[] arr;
                while(sr.Peek() > -1)
                {
                    str = sr.ReadLine();
                    arr = str.Split(',');
                    if (arr[0] == "TIN")
                    {
                        Load_TIN(arr, sr);
                    }
                    else if (arr[0] == "Build")
                    {
                        Load_Build(arr, sr);
                    }
                    else if (arr[0] == "Road")
                    {
                        Load_Road(arr, sr);
                    }
                    else if (arr[0] == "Water")
                    {
                        Load_Water(arr, sr);
                    }
                    else if (arr[0] == "Tree")
                    {
                        Load_Tree(arr, sr);
                    }
                    else if (arr[0] == "Lamp")
                    {
                        Load_Lamp(arr, sr);
                    }
                }
            }
            if(all_tin.Count > 0)
            {
                MessageBox.Show("地形数据加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(all_build.Count > 0)
            {
                MessageBox.Show("建筑数据加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(all_road.Count > 0)
            {
                MessageBox.Show("道路数据加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(all_water.Count > 0)
            {
                MessageBox.Show("水域数据加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(all_lamp.Count > 0)
            {
                MessageBox.Show("路灯数据加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(all_tree.Count > 0)
            {
                MessageBox.Show("树木数据加载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //加载地形数据
        private void Load_TIN(string[] TIN_information, StreamReader sr)
        {
            List<point_3D> point_TIN = new List<point_3D>();
            string str;
            string[] arr;
            do
            {
                str = sr.ReadLine();
                arr = str.Split(',');
                if (str != "TIN_end")
                {
                    point_3D p = new point_3D(arr[0], Convert.ToSingle(arr[1]), Convert.ToSingle(arr[2]), Convert.ToSingle(arr[3]));
                    point_TIN.Add(p);
                }
            } while (str != "TIN_end");
            if (point_TIN.Count > 0)
            {               
                TIN tin = new TIN(TIN_information[1], point_TIN);
                all_tin.Add(tin);               
            }
        }
        //加载建筑数据
        private void Load_Build(string[] build_information, StreamReader sr)
        {
            if (build_information[1] == "Build_1")
            {
                Load_Build_1(build_information, sr);
            }
        }
        private void Load_Build_1(string[] build_information, StreamReader sr)
        {
            string str;
            string[] arr;
            string build_1_name = build_information[2];
            double height = Convert.ToDouble(build_information[3]);
            point_3D[] build_1_point = new point_3D[4];
            point_3D down_a, down_b, down_c, down_d;
            for (int i = 0; i < 4; i++)
            {
                str = sr.ReadLine();
                arr = str.Split(',');
                if (i == 0)
                {
                    down_a = new point_3D("down_a", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                    build_1_point[0] = down_a;
                }
                else if (i == 1)
                {
                    down_b = new point_3D("down_b", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                    build_1_point[1] = down_b;
                }
                else if (i == 2)
                {
                    down_c = new point_3D("down_c", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                    build_1_point[2] = down_c;
                }
                else if (i == 3)
                {
                    down_d = new point_3D("down_d", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                    build_1_point[3] = down_d;
                }
            }
            //简单建筑主体
            DataEntity.Cube cube = new DataEntity.Cube(build_1_name, build_1_point[0], build_1_point[1], build_1_point[2], build_1_point[3], height);
            //简单建筑类型
            Build<object>.Build_1 build_1 = new Build<object>.Build_1(cube);
            //总建筑类型
            Build<object> build = new Build<object>(build_1);
            build.build_item = build_1;
            all_build.Add(build);
        }

        //加载道路数据
        private void Load_Road(string[] road_information, StreamReader sr)
        {
            if (road_information[1] == "Road_1")
            {
                Load_Road_1(road_information, sr);
            }
            else if(road_information[1] == "Road_1")
            {
                Load_Road_2(road_information, sr);
            }
        }
        private void Load_Road_1(string[] road_information, StreamReader sr)
        {
            string str;
            string[] arr;
            string road_1_name = road_information[2];
            point_3D a_start, a_end, b_start, b_end;
            a_start = a_end = b_start = b_end = null;
            for (int i = 0; i < 4; i++)
            {
                str = sr.ReadLine();
                arr = str.Split(',');
                if (i == 0)
                {
                    a_start = new point_3D("a_start", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                }
                else if (i == 1)
                {
                    b_start = new point_3D("b_start", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                }
                else if (i == 2)
                {
                    a_end = new point_3D("a_end", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                }
                else if (i == 3)
                {
                    b_end = new point_3D("b_end", Convert.ToDouble(arr[0]), Convert.ToDouble(arr[1]), Convert.ToDouble(arr[2]));
                }
            }
            line_3D a = new line_3D(a_start, a_end);
            line_3D b = new line_3D(b_start, b_end);
            //简单道路类型
            Road<object>.Road_1 road_1 = new Road<object>.Road_1(a, b);
            //总道路类型
            Road<object> road = new Road<object>(road_1_name, road_1);
            road.road_item = road_1;
            all_road.Add(road);
        }        
        private void Load_Road_2(string[] road_information, StreamReader sr)
        {

        }
        //加载水域模型
        private void Load_Water(string[] water_information, StreamReader sr)
        {
            List<point_3D> point_Water = new List<point_3D>();
            string str;
            string[] arr;
            do
            {
                str = sr.ReadLine();
                arr = str.Split(',');
                if (str != "Water_end")
                {
                    point_3D p = new point_3D(arr[0], Convert.ToSingle(arr[1]), Convert.ToSingle(arr[2]), Convert.ToSingle(arr[3]));
                    point_Water.Add(p);
                }
            } while (str != "Water_end");
            if (point_Water.Count > 0)
            {
                Water water = new Water(water_information[2], water_information[1], point_Water);
                all_water.Add(water);
            }
        }
        //加载树木模型
        private void Load_Tree(string[] tree_information, StreamReader sr)
        {
            string str;
            string[] arr;
            str = sr.ReadLine();
            arr = str.Split(',');
            point_3D centre_point = new point_3D(arr[0], Convert.ToSingle(arr[1]), Convert.ToSingle(arr[2]), Convert.ToSingle(arr[3]));
            Tree tree = new Tree(tree_information[2], tree_information[1], centre_point, Convert.ToDouble(tree_information[3]));
            all_tree.Add(tree);
        }
        //加载路灯模型
        private void Load_Lamp(string[] lamp_information, StreamReader sr)
        {
            string str;
            string[] arr;
            str = sr.ReadLine();
            arr = str.Split(',');
            point_3D centre_point = new point_3D(arr[0], Convert.ToSingle(arr[1]), Convert.ToSingle(arr[2]), Convert.ToSingle(arr[3]));
            Lamp lamp = new Lamp(lamp_information[2], lamp_information[1], centre_point, Convert.ToDouble(lamp_information[3]));
            all_lamp.Add(lamp);
        }
    }
}
