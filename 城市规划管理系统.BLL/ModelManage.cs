using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using static 城市规划管理系统.BLL.DataEntity;
using static 城市规划管理系统.BLL.DataLoad;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Raytracing;
using SharpGL.SceneGraph.Lighting;
using System.Drawing.Drawing2D;


namespace 城市规划管理系统.BLL
{
    public class ModelManage
    {
        public static bool DrawTIN_Flag;
        public static bool DrawBuild_Flag;
        public static bool DrawRoad_Flag;
        public static bool DrawWater_Flag;
        public static bool DrawTree_Flag;
        public static bool DrawLamp_Flag;

        public static SelectedData<object> selected_data;
        public static SelectedData<object> selected_return;
        public static point_3D editing_point;
        public static double angle = 0;

        public static AddModel<object> add_model;
        public static List<point_3D> control_points;
        public ModelManage()
        {

        }
        //恢复编辑前的模型参数
        public void Return_Model()
        {
            //防止在编辑完成后想撤销时，失误点击其他模型
            if (selected_data.data_name == selected_return.data_name)
            {
                //这个Build_2代表其他建筑类型
                if (selected_return.data_type[1] == "Build_1" ||
                selected_return.data_type[1] == "Build_2")
                {
                    Build<object> item = new Build<object>();
                    string a = selected_return.data_item.GetType().ToString();
                    item = (Build<object>)Convert.ChangeType(selected_return.data_item, typeof(Build<object>));
                    string b = item.GetType().ToString();
                    all_build[selected_return.data_index[0]].build_item = item;
                }
            }

        }

        public class ModelControl
        {
            public ModelControl()
            {
                DrawTIN_Flag = false;
                DrawBuild_Flag = false;
                DrawRoad_Flag = false;
                DrawWater_Flag = false;
                DrawTree_Flag = false;
                DrawLamp_Flag = false;
            }
            //根据包围盒的尺寸改变，同比例放大缩小模型的尺寸大小
            //方法一：
            //寻找不变的对角点。
            //生成模型中的其他点到对角点的向量集合
            //计算包围盒选中点前后的XYZ轴的缩放因子
            //再将缩放因子乘以向量集合后加上不变的对角点
            //实验结果：
            //Z轴由于值比较小，变化明显。但是，缩放后，包围盒与模型的距离改变。
            //而XY轴由于值比较大，导致缩放因子变化太小从而XY轴变化太小
            //这部分目前没有很好的方法去统一改变不同模型的大小
            public void Change_Model_Size(double move_Z)
            {
                if (selected_data.data_type[0] == "Build")
                {
                    Change_Build_Size(move_Z);
                }

            }
            public void Change_Model_Size(double move_X, double move_Y)
            {
                //这个Build_2代表其他建筑类型
                if (selected_data.data_type[0] == "Build")
                {
                    Change_Build_Size(move_X, move_Y);
                }
                else if (selected_data.data_type[0] == "Road")
                {
                    Change_Road_Size(move_X, move_Y);
                }
            }
            private void Change_Build_Size(double move_Z)
            {
                if (selected_data.data_type[1] == "Build_1")
                {
                    if (editing_point.point_name == "up_a" ||
                        editing_point.point_name == "up_b" ||
                        editing_point.point_name == "up_c" ||
                        editing_point.point_name == "up_d")
                    {
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_a.point_z -= move_Z;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_b.point_z -= move_Z;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_c.point_z -= move_Z;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_d.point_z -= move_Z;
                    }
                }
            }
            private void Change_Build_Size(double move_X, double move_Y)
            {
                if (selected_data.data_type[1] == "Build_1")
                {
                    if (editing_point.point_name == "up_a")
                    {
                        //上面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_a.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_a.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_b.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_c.point_x -= move_X;
                        //下面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_a.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_a.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_b.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_c.point_x -= move_X;

                    }
                    else if (editing_point.point_name == "up_b")
                    {
                        //上面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_a.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_b.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_b.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_d.point_x -= move_X;
                        //下面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_a.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_b.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_b.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_d.point_x -= move_X;
                    }
                    else if (editing_point.point_name == "up_c")
                    {
                        //上面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_a.point_x -= move_X;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_c.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_c.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_d.point_y -= move_Y;
                        //下面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_a.point_x -= move_X;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_c.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_c.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_d.point_y -= move_Y;
                    }
                    else if (editing_point.point_name == "up_d")
                    {
                        //上面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_b.point_x -= move_X;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_c.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_d.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_d.point_y -= move_Y;
                        //下面
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_b.point_x -= move_X;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_c.point_y -= move_Y;

                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_d.point_x -= move_X;
                        (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_d.point_y -= move_Y;
                    }
                }
            }
            private void Change_Road_Size(double move_X, double move_Y)
            {
                if (selected_data.data_type[1] == "Road_1")
                {
                    if (editing_point.point_name == "up_a")
                    {
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_begin.point_x -= move_X;
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_begin.point_y -= move_Y;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_end.point_y -= move_Y;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_begin.point_x -= move_X;
                    }
                    else if (editing_point.point_name == "up_b")
                    {
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_begin.point_y -= move_Y;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_end.point_x -= move_X;
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_end.point_y -= move_Y;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_end.point_x -= move_X;
                    }
                    else if (editing_point.point_name == "up_c")
                    {
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_begin.point_x -= move_X;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_begin.point_x -= move_X;
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_begin.point_y -= move_Y;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_end.point_y -= move_Y;
                    }
                    else if (editing_point.point_name == "up_d")
                    {
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_end.point_x -= move_X;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_begin.point_y -= move_Y;

                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_end.point_x -= move_X;
                        (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_end.point_y -= move_Y;
                    }
                }
            }
            public void Change_BoundingBox_Size(double move_Z)
            {
                Cube change_bounding_box = selected_data.bounding_box;
                if (editing_point.point_name == "up_a" ||
                    editing_point.point_name == "up_b" ||
                    editing_point.point_name == "up_c" ||
                    editing_point.point_name == "up_d")
                {
                    change_bounding_box.up_a.point_z = change_bounding_box.up_a.point_z - move_Z;
                    change_bounding_box.up_b.point_z = change_bounding_box.up_b.point_z - move_Z;
                    change_bounding_box.up_c.point_z = change_bounding_box.up_c.point_z - move_Z;
                    change_bounding_box.up_d.point_z = change_bounding_box.up_d.point_z - move_Z;
                }
                selected_data.bounding_box = change_bounding_box;
            }
            public void Change_BoundingBox_Size(double move_X, double move_Y)
            {
                Cube change_bounding_box = selected_data.bounding_box;
                if (editing_point.point_name == "up_a")
                {
                    //上面
                    change_bounding_box.up_a.point_x = change_bounding_box.up_a.point_x - move_X;
                    change_bounding_box.up_a.point_y = change_bounding_box.up_a.point_y - move_Y;

                    change_bounding_box.up_b.point_y = change_bounding_box.up_b.point_y - move_Y;

                    change_bounding_box.up_c.point_x = change_bounding_box.up_c.point_x - move_X;
                    //下面
                    change_bounding_box.down_a.point_x = change_bounding_box.down_a.point_x - move_X;
                    change_bounding_box.down_a.point_y = change_bounding_box.down_a.point_y - move_Y;

                    change_bounding_box.down_b.point_y = change_bounding_box.down_b.point_y - move_Y;

                    change_bounding_box.down_c.point_x = change_bounding_box.down_c.point_x - move_X;

                }
                else if (editing_point.point_name == "up_b")
                {
                    //上面
                    change_bounding_box.up_a.point_y = change_bounding_box.up_a.point_y - move_Y;

                    change_bounding_box.up_b.point_x = change_bounding_box.up_b.point_x - move_X;
                    change_bounding_box.up_b.point_y = change_bounding_box.up_b.point_y - move_Y;

                    change_bounding_box.up_d.point_x = change_bounding_box.up_d.point_x - move_X;
                    //下面
                    change_bounding_box.down_a.point_y = change_bounding_box.down_a.point_y - move_Y;

                    change_bounding_box.down_b.point_x = change_bounding_box.down_b.point_x - move_X;
                    change_bounding_box.down_b.point_y = change_bounding_box.down_b.point_y - move_Y;

                    change_bounding_box.down_d.point_x = change_bounding_box.down_d.point_x - move_X;
                }
                else if (editing_point.point_name == "up_c")
                {
                    //上面
                    change_bounding_box.up_a.point_x = change_bounding_box.up_a.point_x - move_X;

                    change_bounding_box.up_c.point_x = change_bounding_box.up_c.point_x - move_X;
                    change_bounding_box.up_c.point_y = change_bounding_box.up_c.point_y - move_Y;

                    change_bounding_box.up_d.point_y = change_bounding_box.up_d.point_y - move_Y;
                    //下面
                    change_bounding_box.down_a.point_x = change_bounding_box.down_a.point_x - move_X;

                    change_bounding_box.down_c.point_x = change_bounding_box.down_c.point_x - move_X;
                    change_bounding_box.down_c.point_y = change_bounding_box.down_c.point_y - move_Y;

                    change_bounding_box.down_d.point_y = change_bounding_box.down_d.point_y - move_Y;
                }
                else if (editing_point.point_name == "up_d")
                {
                    //上面
                    change_bounding_box.up_b.point_x = change_bounding_box.up_b.point_x - move_X;

                    change_bounding_box.up_c.point_y = change_bounding_box.up_c.point_y - move_Y;

                    change_bounding_box.up_d.point_x = change_bounding_box.up_d.point_x - move_X;
                    change_bounding_box.up_d.point_y = change_bounding_box.up_d.point_y - move_Y;
                    //下面
                    change_bounding_box.down_b.point_x = change_bounding_box.down_b.point_x - move_X;

                    change_bounding_box.down_c.point_y = change_bounding_box.down_c.point_y - move_Y;

                    change_bounding_box.down_d.point_x = change_bounding_box.down_d.point_x - move_X;
                    change_bounding_box.down_d.point_y = change_bounding_box.down_d.point_y - move_Y;
                }
                selected_data.bounding_box = change_bounding_box;
            }
            //旋转模型
            public void Rotate_Model()
            {
                int number_point = selected_data.data_points.Count;
                //由余弦定理求向量夹角
                double radian = angle * Math.PI / 180.0;

                TriTuple rotate_axis = new TriTuple(0, 0, 1);
                //计算旋转前的向量
                TriTuple[] rotate_before = new TriTuple[number_point];
                for (int i = 0; i < number_point; i++)
                {
                    rotate_before[i] = selected_return.data_points[i].PointToTrituple() - selected_data.bounding_box.centre_point.PointToTrituple();
                }
                //计算旋转后的向量
                TriTuple[] rotate_after = new TriTuple[number_point];
                for (int i = 0; i < number_point; i++)
                {
                    rotate_after[i] = Math.Cos(radian) * rotate_before[i] + (1 - Math.Cos(radian)) * (rotate_before[i].Dot(rotate_axis)) * rotate_axis + (Math.Sin(radian) * rotate_axis).Cross(rotate_before[i]);
                }
                //旋转后点位归算
                for (int i = 0; i < number_point; i++)
                {
                    selected_data.data_points[i] = (selected_data.bounding_box.centre_point.PointToTrituple() + rotate_after[i]).TriTupleToPoint(selected_data.data_points[i].point_name);
                }
                //根据不同的模型复原点位
                Rotate_Build();
                Rotate_Road();
            }
            private void Rotate_Build()
            {
                if (selected_data.data_type[1] == "Build_1")
                {
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).point_Build_1 = selected_data.data_points;

                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_a = selected_data.data_points[0];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_b = selected_data.data_points[1];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_c = selected_data.data_points[2];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_d = selected_data.data_points[3];

                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_a = selected_data.data_points[4];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_b = selected_data.data_points[5];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_c = selected_data.data_points[6];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_d = selected_data.data_points[7];
                }
            }
            private void Rotate_Road()
            {
                if (selected_data.data_type[1] == "Road_1")
                {
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).point_Road_1 = selected_data.data_points;

                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_begin = selected_data.data_points[0];
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_end = selected_data.data_points[1];
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_begin = selected_data.data_points[2];
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_end = selected_data.data_points[3];
                }
            }
            //同步旋转包围盒
            public double Rotate_BoundingBox()
            {
                //由余弦定理求向量夹角
                double radian = angle * Math.PI / 180.0;

                TriTuple rotate_axis = new TriTuple(0, 0, 1);
                TriTuple[] rotate_before = new TriTuple[4];

                rotate_before[0] = selected_return.bounding_box.down_a.PointToTrituple() - selected_data.bounding_box.centre_point.PointToTrituple();
                rotate_before[1] = selected_return.bounding_box.down_b.PointToTrituple() - selected_data.bounding_box.centre_point.PointToTrituple();
                rotate_before[2] = selected_return.bounding_box.down_c.PointToTrituple() - selected_data.bounding_box.centre_point.PointToTrituple();
                rotate_before[3] = selected_return.bounding_box.down_d.PointToTrituple() - selected_data.bounding_box.centre_point.PointToTrituple();

                //根据向量夹角求旋转后的向量
                TriTuple[] rotate_after = new TriTuple[4];
                rotate_after[0] = Math.Cos(radian) * rotate_before[0] + (1 - Math.Cos(radian)) * (rotate_before[0].Dot(rotate_axis)) * rotate_axis + (Math.Sin(radian) * rotate_axis).Cross(rotate_before[0]);
                rotate_after[1] = Math.Cos(radian) * rotate_before[1] + (1 - Math.Cos(radian)) * (rotate_before[1].Dot(rotate_axis)) * rotate_axis + (Math.Sin(radian) * rotate_axis).Cross(rotate_before[1]);
                rotate_after[2] = Math.Cos(radian) * rotate_before[2] + (1 - Math.Cos(radian)) * (rotate_before[2].Dot(rotate_axis)) * rotate_axis + (Math.Sin(radian) * rotate_axis).Cross(rotate_before[2]);
                rotate_after[3] = Math.Cos(radian) * rotate_before[3] + (1 - Math.Cos(radian)) * (rotate_before[3].Dot(rotate_axis)) * rotate_axis + (Math.Sin(radian) * rotate_axis).Cross(rotate_before[3]);

                selected_data.bounding_box.down_a = (selected_data.bounding_box.centre_point.PointToTrituple() + rotate_after[0]).TriTupleToPoint(selected_data.bounding_box.down_a.point_name);
                selected_data.bounding_box.down_b = (selected_data.bounding_box.centre_point.PointToTrituple() + rotate_after[1]).TriTupleToPoint(selected_data.bounding_box.down_b.point_name);
                selected_data.bounding_box.down_c = (selected_data.bounding_box.centre_point.PointToTrituple() + rotate_after[2]).TriTupleToPoint(selected_data.bounding_box.down_c.point_name);
                selected_data.bounding_box.down_d = (selected_data.bounding_box.centre_point.PointToTrituple() + rotate_after[3]).TriTupleToPoint(selected_data.bounding_box.down_d.point_name);

                selected_data.bounding_box.up_a.point_x = selected_data.bounding_box.down_a.point_x;
                selected_data.bounding_box.up_a.point_y = selected_data.bounding_box.down_a.point_y;
                selected_data.bounding_box.up_b.point_x = selected_data.bounding_box.down_b.point_x;
                selected_data.bounding_box.up_b.point_y = selected_data.bounding_box.down_b.point_y;
                selected_data.bounding_box.up_c.point_x = selected_data.bounding_box.down_c.point_x;
                selected_data.bounding_box.up_c.point_y = selected_data.bounding_box.down_c.point_y;
                selected_data.bounding_box.up_d.point_x = selected_data.bounding_box.down_d.point_x;
                selected_data.bounding_box.up_d.point_y = selected_data.bounding_box.down_d.point_y;

                return radian;
            }
            //移动模型
            public void Move_Model(double move_X, double move_Y)
            {
                int number_point = selected_data.data_points.Count;
                for (int i = 0; i < number_point; i++)
                {
                    selected_data.data_points[i].point_x = selected_data.data_points[i].point_x - move_X;
                    selected_data.data_points[i].point_y = selected_data.data_points[i].point_y - move_Y;
                }
                Move_Build();
                Move_Road();
            }
            private void Move_Build()
            {
                if (selected_data.data_type[1] == "Build_1")
                {
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).point_Build_1 = selected_data.data_points;

                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_a = selected_data.data_points[0];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_b = selected_data.data_points[1];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_c = selected_data.data_points[2];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.up_d = selected_data.data_points[3];

                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_a = selected_data.data_points[4];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_b = selected_data.data_points[5];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_c = selected_data.data_points[6];
                    (all_build[selected_data.data_index[0]].build_item as Build<object>.Build_1).main_body.down_d = selected_data.data_points[7];
                }
            }
            private void Move_Road()
            {
                if (selected_data.data_type[1] == "Road_1")
                {
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).point_Road_1 = selected_data.data_points;

                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_begin = selected_data.data_points[0];
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).a.point_end = selected_data.data_points[1];
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_begin = selected_data.data_points[2];
                    (all_road[selected_data.data_index[0]].road_item as Road<object>.Road_1).b.point_end = selected_data.data_points[3];
                }
            }
            //同步移动包围盒
            public void Move_BoundingBox(double move_X, double move_Y)
            {

                selected_data.bounding_box.centre_point.point_x = selected_data.bounding_box.centre_point.point_x - move_X;
                selected_data.bounding_box.centre_point.point_y = selected_data.bounding_box.centre_point.point_y - move_Y;

                selected_data.bounding_box.down_a.point_x = selected_data.bounding_box.down_a.point_x - move_X;
                selected_data.bounding_box.down_a.point_y = selected_data.bounding_box.down_a.point_y - move_Y;

                selected_data.bounding_box.down_b.point_x = selected_data.bounding_box.down_b.point_x - move_X;
                selected_data.bounding_box.down_b.point_y = selected_data.bounding_box.down_b.point_y - move_Y;

                selected_data.bounding_box.down_c.point_x = selected_data.bounding_box.down_c.point_x - move_X;
                selected_data.bounding_box.down_c.point_y = selected_data.bounding_box.down_c.point_y - move_Y;

                selected_data.bounding_box.down_d.point_x = selected_data.bounding_box.down_d.point_x - move_X;
                selected_data.bounding_box.down_d.point_y = selected_data.bounding_box.down_d.point_y - move_Y;

                selected_data.bounding_box.up_a.point_x = selected_data.bounding_box.down_a.point_x;
                selected_data.bounding_box.up_a.point_y = selected_data.bounding_box.down_a.point_y;

                selected_data.bounding_box.up_b.point_x = selected_data.bounding_box.down_b.point_x;
                selected_data.bounding_box.up_b.point_y = selected_data.bounding_box.down_b.point_y;

                selected_data.bounding_box.up_c.point_x = selected_data.bounding_box.down_c.point_x;
                selected_data.bounding_box.up_c.point_y = selected_data.bounding_box.down_c.point_y;

                selected_data.bounding_box.up_d.point_x = selected_data.bounding_box.down_d.point_x;
                selected_data.bounding_box.up_d.point_y = selected_data.bounding_box.down_d.point_y;
            }
            //实时刷新预生成模型
            public void Create_Model(OpenGL gl, Point NowPos, Pick picking, string[] data_type)
            {
                if (data_type[0] == "Build")
                {
                    Create_Build(gl, NowPos, picking, data_type);
                }
            }
            public void Create_Model(List<point_3D> control_points, string[] data_type)
            {
                if (data_type[0] == "Road")
                {
                    Create_Road(control_points, data_type);
                }
            }
            private void Create_Build(OpenGL gl ,Point NowPos, Pick picking, string[] data_type)
            {
                if (data_type[1] == "Build_1")
                {
                    int[] data_index = add_model.data_index;
                    add_model.data_name = "B" + all_build.Count + 1;
                    Cube cube = new Cube(add_model.data_name, picking.Get_GroundPoint(gl, NowPos), 42, 10, 16.8);
                    Build<object>.Build_1 build_1 = new Build<object>.Build_1(cube);
                    build_1.main_body.top_texture_index = 0;
                    build_1.main_body.round_texture_index = 1;
                    Build<object> build = new Build<object>(build_1);
                    build.build_item = build_1;
                    add_model = new AddModel<object>(build, data_index);
                    add_model.data_item = build;
                }

            }
            //生成道路流程与建筑不一样
            //建筑刷新预生成模型雏形，由用户一次性确定位置
            //道路需要与用户交互，由用户确定各个控制点
            private void Create_Road(List<point_3D> control_points, string[] data_type)
            {
                int[] data_index = add_model.data_index;
                add_model.data_name = "R" + all_road.Count + 1;
                if (data_type[1] == "Road_1")
                {
                    line_3D a = new line_3D(control_points[0], control_points[1]);
                    line_3D b = new line_3D(control_points[2], control_points[3]);
                    //简单道路类型
                    Road<object>.Road_1 road_1 = new Road<object>.Road_1(a, b);
                    //总道路类型
                    Road<object> road = new Road<object>(add_model.data_name, road_1);
                    road.road_item = road_1;
                    all_road.Add(road);
                }
                else if (data_type[1] == "Road_2")
                {
                    //解析控制点
                    point_3D[] start_point = new point_3D[2] { new point_3D(), new point_3D() };
                    point_3D[] middle_point = new point_3D[2] { new point_3D(), new point_3D() };
                    point_3D[] end_point = new point_3D[2] { new point_3D(), new point_3D() };
                    start_point[0] = start_point[0] & control_points[0];
                    middle_point[0] = middle_point[0] & control_points[1];
                    end_point[0] = end_point[0] & control_points[2];
                    start_point[1] = start_point[1] & control_points[3];
                    middle_point[1] = middle_point[1] & control_points[4];
                    end_point[1] = end_point[1] & control_points[5];
                    //弯道
                    Road<object>.Road_2 road_2 = new Road<object>.Road_2(start_point, middle_point, end_point);
                    //总道路类型
                    Road<object> road = new Road<object>(add_model.data_name, road_2);
                    road.road_item = road_2;
                    all_road.Add(road);
                }
            }
        }

        public class Pick : ModelManage
        {

            public static bool selected_flag;
            public Point screen_point;
            public TriTuple ray_start;
            public TriTuple ray_end;
            public TriTuple ray_direction;
            public TriTuple ray_intersect;

            int[] viewport = new int[4];
            double[] model_view = new double[16];
            double[] projection = new double[16];
            public Pick()
            {
                selected_flag = false;
                selected_data = new SelectedData<object>();
                selected_return = new SelectedData<object>();
                editing_point = new point_3D();
            }
            public void Create_Ray(OpenGL gl, Point p)
            {
                double o1x, o1y, o1z;
                double o2x, o2y, o2z;
                o1x = o1y = o1z = 0;
                o2x = o2y = o2z = 0;
                screen_point = p;
                //更新矩阵 
                gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, model_view);
                gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, projection);
                gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

                gl.UnProject(screen_point.X, (viewport[3] - screen_point.Y - 1), 0.0,
                    model_view, projection, viewport,
                    ref o1x, ref o1y, ref o1z);
                gl.UnProject(screen_point.X, (viewport[3] - screen_point.Y - 1), 1.0,
                    model_view, projection, viewport,
                    ref o2x, ref o2y, ref o2z);
                //得到射线起始点与终止点
                ray_start.X = o1x;
                ray_start.Y = o1y;
                ray_start.Z = o1z;
                ray_end.X = o2x;
                ray_end.Y = o2y;
                ray_end.Z = o2z;
                //计算射线的方向
                ray_direction = ray_end - ray_start;
                ray_direction.Normalize();
            }
            public point_3D Intersect_Point(bool change_cursor, OpenGL gl, Point p, CameraParament camera_parament, SelectedData<object> selectedData, double wide, double height)
            {
                //设置缓冲区
                uint[] buffer = new uint[32];
                gl.SelectBuffer(32, buffer);
                //获取当前视口矩阵
                int[] viewport = new int[4];
                gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

                //进入渲染模式
                gl.RenderMode(OpenGL.GL_SELECT);
                //初始化名字栈
                gl.InitNames();
                //压入名字0，保证名字栈非空
                gl.PushName(0);

                //进入投影矩阵模式
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                //矩阵压栈，保存之前的投影矩阵
                gl.PushMatrix();
                //加载单位矩阵，清除之前的矩阵变换
                gl.LoadIdentity();

                //设置拾取框矩阵
                gl.PickMatrix(p.X, viewport[3] - p.Y, 10, 10, viewport);
                //设置投影矩阵，保证投影矩阵与之前一致。
                //gl.Ortho(-10, 10, -10, 10, -10, 10);
                gl.Perspective(50.0f, wide / height, 0.01, 1000);
                gl.LookAt(camera_parament.eye.X, camera_parament.eye.Y, camera_parament.eye.Z,
                    camera_parament.center.X, camera_parament.center.Y, camera_parament.center.Z,
                    camera_parament.up.X, camera_parament.up.Y, camera_parament.up.Z);

                //再次渲染，这里我认为只需要在选择模式下渲染需要选择的物体
                gl.PointSize(20.0f);
                gl.Color(1f, 0f, 0f);
                //上面
                //为up_a命名
                gl.LoadName(11);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.up_a.point_x, selectedData.bounding_box.up_a.point_y, -selectedData.bounding_box.up_a.point_z);
                }
                gl.End();
                //为up_b命名
                gl.LoadName(12);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.up_b.point_x, selectedData.bounding_box.up_b.point_y, -selectedData.bounding_box.up_b.point_z);
                }
                gl.End();
                //为up_c命名
                gl.LoadName(13);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.up_c.point_x, selectedData.bounding_box.up_c.point_y, -selectedData.bounding_box.up_c.point_z);
                }
                gl.End();
                //为up_d命名
                gl.LoadName(14);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.up_d.point_x, selectedData.bounding_box.up_d.point_y, -selectedData.bounding_box.up_d.point_z);
                }
                gl.End();
                //下面
                //为down_a命名
                gl.LoadName(21);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.down_a.point_x, selectedData.bounding_box.down_a.point_y, -selectedData.bounding_box.down_a.point_z);
                }
                gl.End();
                //为down_b命名
                gl.LoadName(22);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.down_b.point_x, selectedData.bounding_box.down_b.point_y, -selectedData.bounding_box.down_b.point_z);
                }
                gl.End();
                //为down_c命名
                gl.LoadName(23);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.down_c.point_x, selectedData.bounding_box.down_c.point_y, -selectedData.bounding_box.down_c.point_z);
                }
                gl.End();
                //为down_d命名
                gl.LoadName(24);
                gl.Begin(OpenGL.GL_POINTS);
                {
                    gl.Vertex(selectedData.bounding_box.down_d.point_x, selectedData.bounding_box.down_d.point_y, -selectedData.bounding_box.down_d.point_z);
                }
                gl.End();
                
                gl.Finish();
                gl.Flush();

                //进入投影矩阵模式
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                //矩阵出栈，恢复之前的投影矩阵
                gl.PopMatrix();

                //获取命中个数
                int num_picks = gl.RenderMode(OpenGL.GL_RENDER);
                //使用PushName()
                //uint[] selected = new uint[5];
                //int old_index = 0;
                //int now_index = 0;
                //int ID_increase = 0;
                //for(int i = 0; i < num_picks; i++)
                //{
                //    //这里-1是把初始化的0给减去，只是selected值-1，now_index值没变
                //    selected[i] = buffer[old_index] - 1;
                //    ID_increase = (int)buffer[old_index] - 1;
                //    now_index = old_index + 3 + ID_increase;                    
                //    old_index = now_index + 1;
                //}
                //使用LoadName()
                uint[] selected = new uint[8];
                for (int i = 0; i < num_picks; i++)
                {
                    selected[i] = buffer[(i + 1) * 4 - 1];
                }
                point_3D selected_point = new point_3D("null", 0, 0, 0);
                if(change_cursor == false)
                {
                    switch (selected[0])
                    {
                        case 11:
                            editing_point = selectedData.bounding_box.up_a;
                            break;
                        case 12:
                            editing_point = selectedData.bounding_box.up_b;
                            break;
                        case 13:
                            editing_point = selectedData.bounding_box.up_c;
                            break;
                        case 14:
                            editing_point = selectedData.bounding_box.up_d;
                            break;
                        case 21:
                            editing_point = selectedData.bounding_box.down_a;
                            break;
                        case 22:
                            editing_point = selectedData.bounding_box.down_b;
                            break;
                        case 23:
                            editing_point = selectedData.bounding_box.down_c;
                            break;
                        case 24:
                            editing_point = selectedData.bounding_box.down_d;
                            break;
                    }
                }
                else if(change_cursor == true)
                {
                    switch (selected[0])
                    {
                        case 11:
                            selected_point = selectedData.bounding_box.up_a;
                            break;
                        case 12:
                            selected_point = selectedData.bounding_box.up_b;
                            break;
                        case 13:
                            selected_point = selectedData.bounding_box.up_c;
                            break;
                        case 14:
                            selected_point = selectedData.bounding_box.up_d;
                            break;
                        case 21:
                            selected_point = selectedData.bounding_box.down_a;
                            break;
                        case 22:
                            selected_point = selectedData.bounding_box.down_b;
                            break;
                        case 23:
                            selected_point = selectedData.bounding_box.down_c;
                            break;
                        case 24:
                            selected_point = selectedData.bounding_box.down_d;
                            break;
                    }
                }
                return selected_point;
            }          
            public void Intersect_Line(OpenGL gl, Point p, CameraParament camera_parament, SelectedData<object> selectedData, double wide, double height)
            {
                //设置缓冲区
                uint[] buffer = new uint[32];
                gl.SelectBuffer(32, buffer);
                //获取当前视口矩阵
                int[] viewport = new int[4];
                gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

                //进入渲染模式
                gl.RenderMode(OpenGL.GL_SELECT);
                //初始化名字栈
                gl.InitNames();
                //压入名字0，保证名字栈非空
                gl.PushName(0);

                //进入投影矩阵模式
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                //矩阵压栈，保存之前的投影矩阵
                gl.PushMatrix();
                //加载单位矩阵，清除之前的矩阵变换
                gl.LoadIdentity();

                //设置拾取框矩阵
                gl.PickMatrix(p.X, viewport[3] - p.Y, 10, 10, viewport);
                //设置投影矩阵，保证投影矩阵与之前一致。
                //gl.Ortho(-10, 10, -10, 10, -10, 10);
                gl.Perspective(50.0f, wide / height, 0.01, 1000);
                gl.LookAt(camera_parament.eye.X, camera_parament.eye.Y, camera_parament.eye.Z,
                    camera_parament.center.X, camera_parament.center.Y, camera_parament.center.Z,
                    camera_parament.up.X, camera_parament.up.Y, camera_parament.up.Z);

                //再次渲染，这里我认为只需要在选择模式下渲染需要选择的物体
                gl.PointSize(20.0f);
                //上面
                //为line_a命名(up_a, down_a)
                gl.LoadName(31);
                gl.Begin(OpenGL.GL_LINES);
                {
                    gl.Vertex(selectedData.bounding_box.up_a.point_x, selectedData.bounding_box.up_a.point_y, -selectedData.bounding_box.up_a.point_z);
                    gl.Vertex(selectedData.bounding_box.down_a.point_x, selectedData.bounding_box.down_a.point_y, -selectedData.bounding_box.down_a.point_z);
                }
                gl.End();
                //为line_b命名(up_b, down_b)
                gl.LoadName(32);
                gl.Begin(OpenGL.GL_LINES);
                {
                    gl.Vertex(selectedData.bounding_box.up_b.point_x, selectedData.bounding_box.up_b.point_y, -selectedData.bounding_box.up_b.point_z);
                    gl.Vertex(selectedData.bounding_box.down_b.point_x, selectedData.bounding_box.down_b.point_y, -selectedData.bounding_box.down_b.point_z);
                }
                gl.End();
                //为line_c命名(up_c, down_c)
                gl.LoadName(33);
                gl.Begin(OpenGL.GL_LINES);
                {
                    gl.Vertex(selectedData.bounding_box.up_c.point_x, selectedData.bounding_box.up_c.point_y, -selectedData.bounding_box.up_c.point_z);
                    gl.Vertex(selectedData.bounding_box.down_c.point_x, selectedData.bounding_box.down_c.point_y, -selectedData.bounding_box.down_c.point_z);
                }
                gl.End();
                //为line_d命名(up_d, down_d)
                gl.LoadName(34);
                gl.Begin(OpenGL.GL_LINES);
                {
                    gl.Vertex(selectedData.bounding_box.up_d.point_x, selectedData.bounding_box.up_d.point_y, -selectedData.bounding_box.up_d.point_z);
                    gl.Vertex(selectedData.bounding_box.down_d.point_x, selectedData.bounding_box.down_d.point_y, -selectedData.bounding_box.down_d.point_z);
                }
                gl.End();

                gl.Finish();
                gl.Flush();

                //进入投影矩阵模式
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                //矩阵出栈，恢复之前的投影矩阵
                gl.PopMatrix();

                //获取命中个数
                int num_picks = gl.RenderMode(OpenGL.GL_RENDER);
                //使用PushName()
                //uint[] selected = new uint[5];
                //int old_index = 0;
                //int now_index = 0;
                //int ID_increase = 0;
                //for(int i = 0; i < num_picks; i++)
                //{
                //    //这里-1是把初始化的0给减去，只是selected值-1，now_index值没变
                //    selected[i] = buffer[old_index] - 1;
                //    ID_increase = (int)buffer[old_index] - 1;
                //    now_index = old_index + 3 + ID_increase;                    
                //    old_index = now_index + 1;
                //}
                //使用LoadName()
                uint[] selected = new uint[5];
                for (int i = 0; i < num_picks; i++)
                {
                    selected[i] = buffer[(i + 1) * 4 - 1];
                }
                switch (selected[0])
                {
                    case 31:
                        //editing_line = new line_3D(selectedData.bounding_box.up_a, selectedData.bounding_box.down_a);
                        break;
                    case 32:
                        //editing_line = new line_3D(selectedData.bounding_box.up_b, selectedData.bounding_box.down_b);
                        break;
                    case 33:
                        //editing_line = new line_3D(selectedData.bounding_box.up_c, selectedData.bounding_box.down_c);
                        break;
                    case 34:
                        //editing_line = new line_3D(selectedData.bounding_box.up_d, selectedData.bounding_box.down_d);
                        break;
                }
            }
            public double Intersect_Triangle(TriTuple start, TriTuple direction,
                TriTuple V1, TriTuple V2, TriTuple V3)
            {
                V1.Z = -V1.Z;
                V2.Z = -V2.Z;
                V3.Z = -V3.Z;

                double t, u, v;
                TriTuple E1 = V2 - V1;
                TriTuple E2 = V3 - V1;
                TriTuple P = direction.Cross(E2);

                double det = E1.Dot(P);
                //若行列式接近于零，则射线位于三角形的平面上
                if (det > -0.0000001f && det < 0.0000001f)
                {
                    return -1;
                }
                double invDet = 1.0f / det;

                TriTuple T;
                T = start - V1;

                u = T.Dot(P) * invDet;
                //保证u>0
                if (u < 0.0f || u > 1.0f)
                {
                    return -1;
                }

                TriTuple Q = T.Cross(E1);
                //保证u+v<1
                v = direction.Dot(Q) * invDet;
                if (v < 0.0f || u + v > 1.0f)
                {
                    return -1;
                }

                t = E2.Dot(Q) * invDet;
                if (t > 0.000001f)
                {
                    ray_intersect = ray_start + ray_direction.Scalar(t);
                    return t;
                }
                return -1;
            }
            #region//传统方法：先计算与平面的交点，再判断交点是否在三角形内
            //public static bool IsInTrg(TriTuple vtxA, TriTuple vtxB, TriTuple vtxC, TriEqaResult pHit)
            //{
            //    Line_Equation l1 = new Line_Equation(vtxA, vtxB - vtxA);
            //    Line_Equation l2 = new Line_Equation(vtxC, vtxA - vtxC);
            //    Line_Equation l3 = new Line_Equation(vtxB, vtxC - vtxB);

            //    TriTuple pos = new TriTuple(pHit.x, pHit.y, pHit.z);

            //    double[] res1 = new double[3];
            //    double[] res2 = new double[3];
            //    l1.TestPoint(pos, ref res1[0], ref res2[0]);
            //    l2.TestPoint(pos, ref res1[1], ref res2[1]);
            //    l3.TestPoint(pos, ref res1[2], ref res2[2]);

            //    int i;
            //    int ca = 0, cm = 0;
            //    for (i = 0; i <= 2; i++)
            //    {
            //        if (res1[i] < 0)
            //        {
            //            cm++;
            //        }
            //        if (res1[i] >= 0)
            //        {
            //            ca++;
            //        }

            //    }

            //    if ((ca == 3) | (cm == 3))
            //    {
            //        ca = 0;
            //        cm = 0;
            //        for (i = 0; i <= 2; i++)
            //        {
            //            if (res2[i] < 0)
            //            {
            //                cm++;
            //            }
            //            if (res2[i] >= 0)
            //            {
            //                ca++;
            //            }
            //        }

            //        if ((ca == 3) | (cm == 3))
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            ca = 0; cm = 0;
            //            for (i = 0; i <= 2; i++)
            //            {
            //                if (res2[i] <= 0)
            //                {
            //                    cm++;
            //                }
            //                if (res2[i] > 0)
            //                {
            //                    ca++;
            //                }
            //            }
            //            if ((ca == 3) | (cm == 3))
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ca = 0; cm = 0;
            //        for (i = 0; i <= 2; i++)
            //        {
            //            if (res1[i] <= 0)
            //            {
            //                cm++;
            //            }
            //            if (res1[i] > 0)
            //            {
            //                ca++;
            //            }

            //        }

            //        if ((ca == 3) | (cm == 3))
            //        {
            //            ca = 0;
            //            cm = 0;
            //            for (i = 0; i <= 2; i++)
            //            {
            //                if (res2[i] < 0)
            //                {
            //                    cm++;
            //                }
            //                if (res2[i] >= 0)
            //                {
            //                    ca++;
            //                }
            //            }

            //            if ((ca == 3) | (cm == 3))
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                ca = 0; cm = 0;
            //                for (i = 0; i <= 2; i++)
            //                {
            //                    if (res2[i] <= 0)
            //                    {
            //                        cm++;
            //                    }
            //                    if (res2[i] > 0)
            //                    {
            //                        ca++;
            //                    }
            //                }
            //                if ((ca == 3) | (cm == 3))
            //                {
            //                    return true;
            //                }
            //                else
            //                { return false; }
            //            }
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //}

            //public bool Intersect(TriTuple vtxA, TriTuple vtxB, TriTuple vtxC, ref TriTuple pos)
            //{
            //    Plane_Equation curFace = new Plane_Equation(vtxA, Plane_Equation.CalcPlaneNormal(vtxA, vtxB, vtxC));
            //    TriTuple ori = ray_end - ray_start;
            //    Line_Equation curSpd = new Line_Equation(ray_start, ori);
            //    //求平面与直线的交点
            //    TriEqaResult pHit = new TriEqaResult(curSpd, curFace);

            //    if (!pHit.hasSolution)
            //    {
            //        return false;
            //    }
            //    pos = new TriTuple(pHit.x, pHit.y, pHit.z);
            //    //判断是否相交
            //    if ((pos - ray_start).Dot(ori) > 0)
            //    {
            //        return IsInTrg(vtxA, vtxB, vtxC, pHit);
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            #endregion
            public double Intersect_AABB(TriTuple start, TriTuple direction, Cube bounding_box)
            {
                //所有t的最小最大值
                double t_min = 0;
                double t_max = 10000;
                //包围盒的最小点为down_a，最大点为up_d
                double t1, t2;
                //平行于X轴
                if (Math.Abs(direction.X) < 0.000001f)
                {
                    if((start.X < bounding_box.down_a.point_x) || (start.X > bounding_box.up_d.point_x))
                    {
                        return -1;
                    }
                }
                else
                {
                    t1 = (bounding_box.down_a.point_x - start.X) / direction.X;
                    t2 = (bounding_box.up_d.point_x - start.X) / direction.X;
                    double temp;
                    if (t1 > t2)
                    {
                        temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }
                    if (t1 > t_min)
                    {
                        t_min = t1;
                    }
                    if (t2 < t_max)
                    {
                        t_max = t2;
                    }                   
                    if (t_min > t_max)
                    {
                        return -1;
                    }                   
                }
                //平行于Y轴
                if (Math.Abs(direction.Y) < 0.000001f)
                {
                    if ((start.Y < bounding_box.down_a.point_y) || (start.Y > bounding_box.up_d.point_y))
                    {
                        return -1;
                    }
                }
                else
                {
                    t1 = (bounding_box.down_a.point_y - start.Y) / direction.Y;
                    t2 = (bounding_box.up_d.point_y - start.Y) / direction.Y;
                    double temp;
                    if (t1 > t2)
                    {
                        temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }
                    if (t1 > t_min)
                    {
                        t_min = t1;
                    }
                    if (t2 < t_max)
                    {
                        t_max = t2;
                    }
                    if (t_min > t_max)
                    {
                        return -1;
                    }
                }
                //平行于Z轴
                if (Math.Abs(direction.Z) < 0.000001)
                {
                    if ((start.Z < bounding_box.down_a.point_z) || (start.Z > bounding_box.up_d.point_z))
                    {
                        return -1;
                    }
                }
                else
                {
                    t1 = (-bounding_box.down_a.point_z - start.Z) / direction.Z;
                    t2 = (-bounding_box.up_d.point_z - start.Z) / direction.Z;
                    double temp;
                    if (t1 > t2)
                    {
                        temp = t1;
                        t1 = t2;
                        t2 = temp;
                    }
                    if (t1 > t_min)
                    {
                        t_min = t1;
                    }
                    if (t2 < t_max)
                    {
                        t_max = t2;
                    }
                    if (t_min > t_max)
                    {
                        return -1;
                    }
                }
                ray_intersect = ray_start + ray_direction.Scalar(t_min);
                return t_min;
            }
            #region//利用射线与三角形相交检测实现对包围盒等立方体的检测
            //public bool Check_Intersect_Box(TriTuple start, TriTuple direction, Cube bounding_box)
            //{
            //    bool selected = false;
            //    double[] distance_face = new double[12];
            //    //前面检测
            //    distance_face[0] = Intersect_Triangle(start, direction, 
            //        bounding_box.up_c.PointToTrituple(), bounding_box.down_c.PointToTrituple(), bounding_box.up_d.PointToTrituple());
            //    distance_face[1] = Intersect_Triangle(start, direction,
            //        bounding_box.down_d.PointToTrituple(), bounding_box.down_c.PointToTrituple(), bounding_box.up_d.PointToTrituple());
            //    //后面检测
            //    distance_face[2] = Intersect_Triangle(start, direction,
            //        bounding_box.up_a.PointToTrituple(), bounding_box.down_a.PointToTrituple(), bounding_box.up_b.PointToTrituple());
            //    distance_face[3] = Intersect_Triangle(start, direction,
            //        bounding_box.down_b.PointToTrituple(), bounding_box.down_a.PointToTrituple(), bounding_box.up_b.PointToTrituple());
            //    //左面
            //    distance_face[4] = Intersect_Triangle(start, direction,
            //        bounding_box.up_a.PointToTrituple(), bounding_box.down_a.PointToTrituple(), bounding_box.up_c.PointToTrituple());
            //    distance_face[5] = Intersect_Triangle(start, direction,
            //        bounding_box.down_c.PointToTrituple(), bounding_box.down_a.PointToTrituple(), bounding_box.up_c.PointToTrituple());
            //    //右面
            //    distance_face[6] = Intersect_Triangle(start, direction,
            //        bounding_box.up_c.PointToTrituple(), bounding_box.down_c.PointToTrituple(), bounding_box.up_b.PointToTrituple());
            //    distance_face[7] = Intersect_Triangle(start, direction,
            //        bounding_box.down_b.PointToTrituple(), bounding_box.down_c.PointToTrituple(), bounding_box.up_b.PointToTrituple());
            //    //上面
            //    distance_face[8] = Intersect_Triangle(start, direction,
            //        bounding_box.up_a.PointToTrituple(), bounding_box.up_c.PointToTrituple(), bounding_box.up_b.PointToTrituple());
            //    distance_face[9] = Intersect_Triangle(start, direction,
            //        bounding_box.up_d.PointToTrituple(), bounding_box.up_c.PointToTrituple(), bounding_box.up_b.PointToTrituple());
            //    //下面
            //    distance_face[10] = Intersect_Triangle(start, direction,
            //        bounding_box.down_a.PointToTrituple(), bounding_box.down_c.PointToTrituple(), bounding_box.down_b.PointToTrituple());
            //    distance_face[11] = Intersect_Triangle(start, direction,
            //        bounding_box.down_d.PointToTrituple(), bounding_box.down_c.PointToTrituple(), bounding_box.down_b.PointToTrituple());

            //    for(int i = 0; i < distance_face.Length; i++)
            //    {
            //        if (distance_face[i] > -1)
            //        {
            //            selected = true;
            //        }
            //    }
            //    return selected;
            //}
            #endregion
            //综合所有步骤，并封装
            public SelectedData<object> Picking(OpenGL gl, Point p)
            {     
                List<SelectedData<object>> selected_models = new List<SelectedData<object>>();
                //生成射线
                Create_Ray(gl, p);
                //开始模型检测（除TIN地形模型检测采用三角形检测，其余模型一律采用AABB包围盒检测）
                double t_min;
                //TIN地形模型检测
                if(DrawTIN_Flag == true)
                {
                    int[] TIN_index = new int[2];
                    t_min = 10000;
                    for (int i = 0; i < all_tin.Count; i++)
                    {
                        for (int j = 0; j < all_tin[i].triangle_NET.Count; j++)
                        {
                            double t = Intersect_Triangle(ray_start, ray_direction,
                                all_tin[i].triangle_NET[j].V1.PointToTrituple(),
                                all_tin[i].triangle_NET[j].V2.PointToTrituple(),
                                all_tin[i].triangle_NET[j].V3.PointToTrituple());
                            if (t > 0)
                            {
                                if (t_min > t)
                                {
                                    t_min = t;
                                    TIN_index[0] = i;
                                    TIN_index[1] = j;
                                }
                            }
                        }
                    }
                    //返回最近的TIN的三角面片
                    if (t_min < 10000)
                    {
                        triangle_3D item = all_tin[TIN_index[0]].triangle_NET[TIN_index[1]];
                        SelectedData<object> selected_triangle = new SelectedData<object>(item, t_min, TIN_index);
                        selected_triangle.data_item = item;

                        selected_models.Add(selected_triangle);
                    }
                }
                if(DrawBuild_Flag == true)
                {
                    //建筑模型检测
                    //这里不像TIN有两层List，这个只有一层
                    //但是其中的建筑类型不同
                    //尽管建筑类型不同但是只需要检测总的包围盒即可
                    int[] build_index = new int[1];
                    t_min = 10000;
                    for (int i = 0; i < all_build.Count; i++)
                    {
                        double t = Intersect_AABB(ray_start, ray_direction, all_build[i].bounding_box);
                        if (t > 0)
                        {
                            if (t_min > t)
                            {
                                t_min = t;
                                build_index[0] = i;
                            }
                        }
                    }
                    //返回最近的Build类型的模型
                    if (t_min < 10000)
                    {
                        SelectedData<object> selected_build = new SelectedData<object>(all_build[build_index[0]], t_min, build_index);
                        selected_build.data_item = all_build[build_index[0]];

                        selected_models.Add(selected_build);
                    }
                }
                if(DrawRoad_Flag == true)
                {
                    //道路模型检测
                    int[] road_index = new int[1];
                    t_min = 10000;
                    for (int i = 0; i < all_road.Count; i++)
                    {
                        double t = Intersect_AABB(ray_start, ray_direction, all_road[i].bounding_box);
                        if (t > 0)
                        {
                            if (t_min > t)
                            {
                                t_min = t;
                                road_index[0] = i;
                            }
                        }
                    }
                    //返回最近的Road类型的模型
                    if (t_min < 10000)
                    {
                        SelectedData<object> selected_road = new SelectedData<object>(all_road[road_index[0]], t_min, road_index);
                        selected_road.data_item = all_road[road_index[0]];
                        selected_models.Add(selected_road);

                    }
                }
                //...

                //检测完毕，开始寻找最近的模型
                if (selected_models.Count > 0)
                {
                    t_min = selected_models.Min(item => item.distance);
                    int model_index = selected_models.FindIndex(item => item.distance == t_min);
                    return selected_models[model_index];
                }
                //未检测到选中的模型，则返回一个空类
                else
                {
                    return new SelectedData<object>();
                }

            }
            //右键显示编辑菜单识别
            public bool Show_Menu_Confine(OpenGL gl, Point p, SelectedData<object> selectedData)
            {
                Create_Ray(gl, p);
                if (selectedData.data_type[0] == "TIN")
                {
                    triangle_3D TIN_item = selectedData.data_item as triangle_3D;
                    double t = Intersect_Triangle(ray_start, ray_direction,
                        TIN_item.V1.PointToTrituple(),
                        TIN_item.V2.PointToTrituple(),
                        TIN_item.V3.PointToTrituple());
                    if (t > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    double t = Intersect_AABB(ray_start, ray_direction, selectedData.bounding_box);
                    if (t > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            public point_3D Get_GroundPoint(OpenGL gl, Point p)
            {
                Create_Ray(gl, p);
                double t = -1 * (ray_start.Z / ray_direction.Z);
                TriTuple centre_point = ray_start + t * ray_direction;
                return centre_point.TriTupleToPoint("centre_point");
            }
        }

        public class TextureControl : ModelManage
        {
            public static List<Texture[]> skybox_textures;
            public static List<Texture> terrain_textures;
            public static List<Texture> build_textures;
            public static List<Texture> road_textures;
            public static List<Texture> water_textures;
            public static List<Texture> tree_textures;
            public static List<Texture> lamp_textures;

            public TextureControl()
            {
                skybox_textures = new List<Texture[]>();
                terrain_textures = new List<Texture>();
                build_textures = new List<Texture>();
                road_textures = new List<Texture>();
                water_textures = new List<Texture>();
                tree_textures = new List<Texture>();
                lamp_textures = new List<Texture>();
            }
            //初始化天空盒纹理
            public void Initialize_Texture_Skybox(OpenGL gl)
            {
                Texture[] skybox_texture = new Texture[6];
                //添加环山湖泊背景
                Texture item1 = new Texture();
                string path = null;
                path = Application.StartupPath + "\\图标素材\\skybox\\lake_front.jpg";
                item1.Create(gl, path);
                item1.Name = path;
                skybox_texture[0] = item1;

                Texture item2 = new Texture();
                path = Application.StartupPath + "\\图标素材\\skybox\\lake_back.jpg";
                item2.Create(gl, path);
                item2.Name = path;
                skybox_texture[1] = item2;

                Texture item3 = new Texture();
                path = Application.StartupPath + "\\图标素材\\skybox\\lake_left.jpg";
                item3.Create(gl, path);
                item3.Name = path;
                skybox_texture[2] = item3;

                Texture item4 = new Texture();
                path = Application.StartupPath + "\\图标素材\\skybox\\lake_right.jpg";
                item4.Create(gl, path);
                item4.Name = path;
                skybox_texture[3] = item4;

                Texture item5 = new Texture();
                path = Application.StartupPath + "\\图标素材\\skybox\\lake_top.jpg";
                item5.Create(gl, path);
                item5.Name = path;
                skybox_texture[4] = item5;

                Texture item6 = new Texture();
                path = Application.StartupPath + "\\图标素材\\skybox\\lake_bottom.jpg";
                item6.Create(gl, path);
                item6.Name = path;
                skybox_texture[5] = item6;

                skybox_textures.Add(skybox_texture);

                //添加荒漠背景

            }
            //初始化地形纹理
            public void Initialize_Texture_TIN(OpenGL gl)
            {
                string path = Application.StartupPath + "\\图标素材\\草地纹理.jpeg";
                Texture tin = new Texture();
                tin.Create(gl, path);
                tin.Name = path;
                terrain_textures.Add(tin);

                for(int i = 0; i < all_tin.Count; i++)
                {
                    for(int j =0; j < all_tin[i].triangle_NET.Count; j++)
                    {
                        all_tin[i].triangle_NET[j].texture_index = 0;
                    }
                }              

            }
            //初始化建筑纹理
            public void Initialize_Texture_Build(OpenGL gl)
            {
                //建筑顶部
                string top_path = Application.StartupPath + "\\图标素材\\混凝土房顶纹理.jpeg";
                Texture top = new Texture();
                top.Create(gl, top_path);
                top.Name = top_path;
                build_textures.Add(top);
                //建筑周围
                string round_path = Application.StartupPath + "\\图标素材\\玻璃建筑纹理.jpeg";
                Texture round = new Texture();
                round.Create(gl, round_path);
                round.Name = round_path;
                build_textures.Add(round);
                for (int i = 0; i < all_build.Count; i++)
                {
                    if (all_build[i].build_type == "Build_1")
                    {
                        Build<object>.Build_1 build_1 = all_build[i].build_item as Build<object>.Build_1;
                        build_1.main_body.top_texture_index = 0;
                        build_1.main_body.round_texture_index = 1;
                        all_build[i].build_item = build_1;
                    }

                }
            }
            //初始化道路纹理
            public void Initialize_Texture_Road(OpenGL gl)
            {
                string path = Application.StartupPath + "\\图标素材\\马路纹理.jpeg";
                Texture road = new Texture();
                road.Create(gl, path);
                road.Name = path;
                road_textures.Add(road);
                for(int i = 0; i < all_road.Count; i++)
                {
                    if (all_road[i].road_type == "Road_1")
                    {
                        Road<object>.Road_1 road_1 = all_road[i].road_item as Road<object>.Road_1;
                        road_1.texture_index = 0;
                    }
                }
            }
            //初始化水域纹理
            public void Initialize_Texture_Water(OpenGL gl)
            {
                string path = Application.StartupPath + "\\图标素材\\河流纹理.jpeg";
                Texture water = new Texture();
                water.Create(gl, path);
                water.Name = path;
                water_textures.Add(water);
                for (int i = 0; i < all_water.Count; i++)
                {
                    all_water[i].texture_index = 0;
                }
            }
            //初始化树木纹理
            public void Initialize_Texture_Tree(OpenGL gl)
            {             
                string path = Application.StartupPath + "\\图标素材\\绿化树纹理.jpg";
                Texture tree = new Texture();
                tree.Create(gl, path);
                tree.Name = path;
                tree_textures.Add(tree);
                for (int i = 0; i < all_tree.Count; i++)
                {
                    if (all_tree[i].tree_type == "Tree_1")
                    {
                        all_tree[i].texture_index = 0;
                    }
                }
            }
            //初始化路灯纹理
            public void Initialize_Texture_Lamp(OpenGL gl)
            {
                string path = Application.StartupPath + "\\图标素材\\路灯纹理.jpeg";
                Texture lamp = new Texture();
                lamp.Create(gl, path);
                lamp.Name = path;
                lamp_textures.Add(lamp);
                for (int i = 0; i < all_lamp.Count; i++)
                {
                    if (all_lamp[i].lamp_type == "Lamp_1")
                    {
                        all_lamp[i].texture_index = 0;
                    }
                }
            }


            //修改纹理 
            public int Updata_Texture(OpenGL gl, SelectedData<object> selectedData, string texture_path)
            {
                Texture t = new Texture();
                t.Create(gl, texture_path);
                t.Name = texture_path;

                int index = -1;
                if (selectedData.data_type[0] == "TIN")
                {
                    //重复性检查
                    for(int i = 0; i < terrain_textures.Count; i++)
                    {
                        if(texture_path == terrain_textures[i].Name)
                        {
                            //重复
                            return i;
                        }
                    }
                    //未重复
                    index = terrain_textures.Count;
                    terrain_textures.Add(t);
                }
                else if (selectedData.data_type[0] == "Build")
                {
                    for(int i = 0; i < build_textures.Count; i++)
                    {
                        if(texture_path == build_textures[i].Name)
                        {
                            return i;
                        }
                    }
                    index = build_textures.Count;
                    build_textures.Add(t);
                }
                else if(selectedData.data_type[0] == "Road")
                {
                    for(int i = 0; i < road_textures.Count; i++)
                    {
                        if(texture_path == road_textures[i].Name)
                        {
                            return i;
                        }
                    }
                    index = road_textures.Count;
                    road_textures.Add(t);
                }

                return index;
            }
            
        }
    }
}
