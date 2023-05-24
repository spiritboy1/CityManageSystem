using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Quadrics;
using static 城市规划管理系统.BLL.DataEntity;

namespace 城市规划管理系统.BLL
{
    public class DataEntity
    {
        public DataEntity()
        {

        }
        //----------------------------------------------基本数据结构--------------------------------------------
        /// <summary>
        /// 三维空间点
        /// </summary>
        public class point_3D
        {
            public static string data_type;
            public string point_name;
            public double point_x;
            public double point_y;
            public double point_z;
            public point_3D()
            {
                this.point_name = "null";
                this.point_x = 0;
                this.point_y = 0;
                this.point_z = 0;
            }

            public point_3D(string point_name, double point_x, double point_y, double point_z)
            {
                this.point_name = point_name;
                this.point_x = point_x;
                this.point_y = point_y;
                this.point_z = point_z;
            }
            /// <summary>
            /// 将b的参数赋值给a，而不是引用其地址
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static point_3D operator & (point_3D a, point_3D b)
            {
                a.point_name = b.point_name;
                a.point_x = b.point_x;
                a.point_y = b.point_y;
                a.point_z = b.point_z;
                return a;
            }
            public static bool operator ==(point_3D A, point_3D B)
            {
                bool IsSame = new bool();
                if (A.point_x == B.point_x && A.point_y == B.point_y)
                    IsSame = true;
                else
                    IsSame = false;
                return IsSame;
            }

            public static bool operator !=(point_3D A, point_3D B)
            {
                return !(A == B);
            }
            public TriTuple PointToTrituple()
            {
                TriTuple t;
                t.X = this.point_x;
                t.Y = this.point_y;
                t.Z = this.point_z;
                return t;
            }
        }
        /// <summary>
        /// 三维空间线
        /// </summary>
        public class line_3D
        {
            public static string data_type;
            public string line_name;
            public point_3D point_begin;
            public point_3D point_end;
            public line_3D()
            {
                this.line_name = "null";
                this.point_begin = new point_3D();
                this.point_end = new point_3D();
            }
            public line_3D (point_3D point_begin, point_3D point_end)
            {
                this.line_name = point_begin.point_name + point_end.point_name;
                this.point_begin = point_begin;
                this.point_end = point_end;
            }
            /// <summary>
            /// 将b的参数赋值给a，而不是引用其地址
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static line_3D operator & (line_3D a, line_3D b)
            {
                a.line_name = b.line_name;
                a.point_begin = a.point_begin & b.point_begin;
                a.point_end = a.point_end & b.point_end;
                return a;
            }
            public static bool operator ==(line_3D A, line_3D B)
            {
                bool IsSame = new bool();
                if ((A.point_begin == B.point_begin && A.point_end == B.point_end) || (A.point_begin == B.point_end && A.point_end == B.point_begin))
                    IsSame = true;
                else
                    IsSame = false;
                return IsSame;
            }
            public static bool operator !=(line_3D A, line_3D B)
            {
                return !(A == B);
            }
        }
        /// <summary>
        /// 三维空间三角面
        /// </summary>
        public class triangle_3D
        {
            //以等腰三角形为例，三个顶点分别为A、B、C
            //A在上层中间，B在下层左边，C在下层右边
            public static string data_type;
            public string surface_name;
            public point_3D V1;
            public point_3D V2;
            public point_3D V3;
            //A——B
            public line_3D line_a { get; set; }
            //B——C
            public line_3D line_b { get; set; }
            //C——A
            public line_3D line_c { get; set; }
            public int texture_index { get; set; }

            public triangle_3D()
            {
                this.surface_name = "null";
                this.V1 = new point_3D();
                this.V2 = new point_3D();
                this.V3 = new point_3D();
                this.texture_index = -1;
            }
            public triangle_3D (point_3D V1, point_3D V2, point_3D V3)
            {            
                this.V1 = V1;
                this.V2 = V2;
                this.V3 = V3;

                this.line_a = new line_3D(V1, V2);
                this.line_b = new line_3D(V2, V3);
                this.line_c = new line_3D(V3, V1);

                this.texture_index = -1;
            }
            /// <summary>
            /// 将b的参数赋值给a，而不是引用其地址
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static triangle_3D operator & (triangle_3D a, triangle_3D b)
            {
                a.surface_name = b.surface_name;
                a.V1 = a.V1 & b.V1;
                a.V2 = a.V2 & b.V2;
                a.V3 = a.V3 & b.V3;

                a.line_a = a.line_a & b.line_a;
                a.line_b = a.line_b & b.line_b;
                a.line_c = a.line_c & b.line_c;

                a.texture_index = b.texture_index;

                return a;
            }
        }
        //摄像机及拾取类所用的数据结构
        /// <summary>
        /// 三元组
        /// </summary>
        public struct TriTuple
        {
            public double X;
            public double Y;
            public double Z;
            public TriTuple(double x, double y, double z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
            public void Add(TriTuple diff)
            {
                this.X = this.X + diff.X;
                this.Y = this.Y + diff.Y;
                this.Z = this.Z + diff.Z;
            }
            /// <summary>
            /// 向量归一
            /// </summary>
            public void Normalize()
            {
                var length = Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
                this.X = this.X / length;
                this.Y = this.Y / length;
                this.Z = this.Z / length;
            }
            //向量
            public static TriTuple operator +(TriTuple a, TriTuple b)
            {
                return new TriTuple(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            }
            public static TriTuple operator -(TriTuple a, TriTuple b)
            {
                return new TriTuple(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            }
            /// <summary>
            /// 点乘、内积、数量积
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            public double Dot(TriTuple b)
            {
                return (((this.X * b.X) + (this.Y * b.Y)) + (this.Z * b.Z));
            }
            /// <summary>
            /// 叉乘、外积、向量积
            /// </summary>
            /// <param name="b"></param>
            /// <returns></returns>
            public TriTuple Cross(TriTuple b)
            {
                TriTuple ret;
                ret.X = (this.Y * b.Z) - (this.Z * b.Y);
                ret.Y = (this.Z * b.X) - (this.X * b.Z);
                ret.Z = (this.X * b.Y) - (this.Y * b.X);
                return ret;
            }
            /// <summary>
            /// 缩放单个向量
            /// </summary>
            /// <param name="t"></param>
            /// <returns></returns>
            public TriTuple Scalar(double t)
            {
                TriTuple ret;
                ret.X = this.X * t;
                ret.Y = this.Y * t;
                ret.Z = this.Z * t;
                return ret;
            }
            /// <summary>
            /// 求XYZ轴的缩放因子
            /// </summary>
            /// <param name="t1"></param>
            /// <param name="t2"></param>
            /// <returns></returns>
            public TriTuple Scalar(TriTuple t1, TriTuple t2)
            {
                TriTuple zoom_factor = new TriTuple();
                zoom_factor.X = t2.X / t1.X;
                zoom_factor.Y = t2.Y / t1.Y;
                zoom_factor.Z = t2.Z / t1.Z;
                return zoom_factor;
            }
            /// <summary>
            /// 求两向量的距离
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static double operator & (TriTuple a, TriTuple b)
            {
                return Math.Sqrt((Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2)) + Math.Pow(b.Z - a.Z, 2));
            }
            /// <summary>
            /// 求向量的模长
            /// </summary>
            /// <param name="a"></param>
            /// <returns></returns>
            public double Modulus_Length()
            {
                return Math.Sqrt((Math.Pow(this.X, 2) + Math.Pow(this.Y, 2)) + Math.Pow(this.Z, 2));
            }
            /// <summary>
            /// 向量对应相乘
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static TriTuple operator *(TriTuple a, TriTuple b)
            {
                TriTuple result = new TriTuple();
                result.X = a.X * b.X;
                result.Y = a.Y * b.Y;
                result.Z = a.Z * b.Z;
                return result;
            }
            /// <summary>
            /// 值与向量相乘
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static TriTuple operator *(TriTuple a, double b)
            {
                TriTuple result = new TriTuple();
                result.X = a.X * b;
                result.Y = a.Y * b;
                result.Z = a.Z * b;
                return result;
            }
            /// <summary>
            /// 值与向量相乘
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static TriTuple operator *(double a, TriTuple b)
            {
                TriTuple result = new TriTuple();
                result.X = a * b.X;
                result.Y = a * b.Y;
                result.Z = a * b.Z;
                return result;
            }
            public point_3D TriTupleToPoint(string pointName)
            {
                point_3D p = new point_3D(pointName, this.X, this.Y, this.Z);
                return p;
            }
        }
        /// <summary>
        /// 摄像机参数
        /// </summary>
        public struct CameraParament
        {
            public TriTuple eye;
            public TriTuple center;
            public TriTuple up;
        }
        /// <summary>
        /// 立方体
        /// </summary>
        public class Cube
        {
            public static string data_type;
            public string cube_name;
            public point_3D up_a;
            public point_3D up_b;
            public point_3D up_c;
            public point_3D up_d;
            public point_3D down_a;
            public point_3D down_b;
            public point_3D down_c;
            public point_3D down_d;
            public point_3D centre_point;
            public double length;
            public double wide;
            public double height;
            public int top_texture_index { get; set; }
            public int round_texture_index { get; set; }
            public Cube()
            {
                this.cube_name = "null";
                this.up_a = new point_3D();
                this.up_b = new point_3D();
                this.up_c = new point_3D();
                this.up_d = new point_3D();
                this.down_a = new point_3D();
                this.down_b = new point_3D();
                this.down_c = new point_3D();
                this.down_d = new point_3D();
                this.length = 0;
                this.wide = 0;
                this.height = 0;
                this.top_texture_index = -1;
                this.round_texture_index = -1;
            }
            public Cube(string cube_name, point_3D centre_point, double length, double wide, double height)
            {
                this.cube_name = cube_name;
                //注意坐标系的正负方向，以屏幕初始化为准
                //X轴向左为正方向，Y轴向上为正方向
                //保持文本数据格式与读取数据格式一致，即从大到小
                this.up_a = new point_3D("up_a", 
                    centre_point.point_x + length / 2, 
                    centre_point.point_y + wide / 2, 
                    centre_point.point_z + height);

                this.up_b = new point_3D("up_b",
                    centre_point.point_x - length / 2,
                    centre_point.point_y + wide / 2,
                    centre_point.point_z + height);

                this.up_c = new point_3D("up_c",
                    centre_point.point_x + length / 2,
                    centre_point.point_y - wide / 2,
                    centre_point.point_z + height);

                this.up_d = new point_3D("up_d",
                    centre_point.point_x - length / 2,
                    centre_point.point_y - wide / 2,
                    centre_point.point_z + height);

                this.down_a = new point_3D("down_a",
                    centre_point.point_x + length / 2,
                    centre_point.point_y + wide / 2,
                     centre_point.point_z);

                this.down_b = new point_3D("down_b",
                    centre_point.point_x - length / 2,
                    centre_point.point_y + wide / 2,
                    centre_point.point_z);

                this.down_c = new point_3D("down_c",
                    centre_point.point_x + length / 2,
                    centre_point.point_y - wide / 2,
                     centre_point.point_z);

                this.down_d = new point_3D("down_d",
                    centre_point.point_x - length / 2,
                    centre_point.point_y - wide / 2,
                     centre_point.point_z);

                this.centre_point = new point_3D();
                this.centre_point = this.centre_point & centre_point;

                this.length = length;
                this.height = height;
                this.wide = wide;

                this.top_texture_index = -1;
                this.round_texture_index = -1;
            }
            public Cube(string cube_name, point_3D down_a, point_3D down_b, point_3D down_c, point_3D down_d, double height)
            {
                this.cube_name = cube_name;

                this.down_a = down_a;
                this.down_b = down_b;
                this.down_c = down_c;
                this.down_d = down_d;

                this.height = height;
                this.length = Math.Abs(down_a.point_x - down_b.point_x);
                this.wide = Math.Abs(down_c.point_y - down_a.point_y);

                this.up_a = new point_3D("up_a", down_a.point_x, down_a.point_y, down_a.point_z + height);
                this.up_b = new point_3D("up_b", down_b.point_x, down_b.point_y, down_b.point_z + height);
                this.up_c = new point_3D("up_c", down_c.point_x, down_c.point_y, down_c.point_z + height);
                this.up_d = new point_3D("up_d", down_d.point_x, down_d.point_y, down_d.point_z + height);

                this.centre_point = new point_3D("centre_point",
                    (down_a.point_x + down_b.point_x) / 2,
                    (down_a.point_y + down_c.point_y) / 2,
                    down_a.point_z);

                this.top_texture_index = -1;
                this.round_texture_index = -1;
            }
            public Cube(string cube_name, point_3D up_a,point_3D up_b, point_3D up_c, point_3D up_d,
                point_3D down_a, point_3D down_b, point_3D down_c, point_3D down_d)
            {
                this.cube_name = cube_name;

                this.up_a = up_a;
                this.up_b = up_b;
                this.up_c = up_c;
                this.up_d = up_d;
                this.down_a = down_a;
                this.down_b = down_b;
                this.down_c = down_c;
                this.down_d = down_d;

                this.centre_point = new point_3D("centre_point",
                    (down_a.point_x + down_b.point_x) / 2,
                    (down_a.point_y + down_c.point_y) / 2,
                    down_a.point_z);

                this.length = Math.Abs(up_b.point_x - up_a.point_x);
                this.wide = Math.Abs(up_c.point_y - up_a.point_y);
                this.height = Math.Abs(up_a.point_z - down_a.point_z);

                this.top_texture_index = -1;
                this.round_texture_index = -1;
            }
            /// <summary>
            /// 将b的参数赋值给a，而不是引用其地址
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static Cube operator &(Cube a, Cube b)
            {
                a.cube_name = b.cube_name;
                a.centre_point = b.centre_point;
                a.length = b.length;
                a.wide = b.wide;
                a.height = b.height;

                a.up_a = a.up_a & b.up_a;
                a.up_b = a.up_b & b.up_b;
                a.up_c = a.up_c & b.up_c;
                a.up_d = a.up_d & b.up_d;
                a.down_a = a.down_a & b.down_a;
                a.down_b = a.down_b & b.down_b;
                a.down_c = a.down_c & b.down_c;
                a.down_d = a.down_d & b.down_d;

                return a;
            }
        }
        /// <summary>
        /// 圆柱体
        /// </summary>
        public class Cylinder
        {
            public static string data_type;
            public string cylinder_name;
            public point_3D bottom_center;
            public double top_radius;
            public double bottom_radius;
            public double height;
        }
        /// <summary>
        /// 球体
        /// </summary>
        public class Sphere
        {
            public static string data_type;
            //球心坐标
            public string sphere_name;
            public point_3D sphere_center;
            //半径
            public float radius;
            public int slices;
            public int stacks;

        }
        //---------------------------------------------高级模型数据结构---------------------------------------------
        /// <summary>
        /// TIN地形数据模型
        /// </summary>
        public class TIN
        {
            public string TIN_name;
            //这个类型比较特殊，其子类型就是基本数据结构三角形。
            //List第一层是TIN，第二层是Triangle，且数据类型相同（都是三角形）
            //因此不像如建筑类型一样，只有一层List，且数据类型不同
            public string data_type;
            public point_3D centre_point;
            public List<triangle_3D> triangle_NET;
            public List<point_3D> point_TIN;
            public static double height_TIN;
            public TIN()
            {
                this.TIN_name = "null";
                this.centre_point = new point_3D();
                this.triangle_NET = new List<triangle_3D>();
                this.point_TIN = new List<point_3D>();
            }
            #region//狄洛尼三角剖分方法，有缺陷，部分三角形缺失
            //public List<triangle_3D> Calculate_TIN(List<point_3D> compute_point)
            //{
            //    List<point_3D> convex_buns_finish = new List<point_3D>();
            //    List<triangle_3D> T1 = new List<triangle_3D>();
            //    List<triangle_3D> T2 = new List<triangle_3D>();
            //    List<line_3D> S1 = new List<line_3D>();
            //    List<line_3D> repeat_line_3D = new List<line_3D>();
            //    point_3D basepoint = new point_3D(null, 0, 0, 0);

            //    //***************************************************************************************************
            //    //计算凸包多边形
            //    //寻找基点basepoint
            //    //画布世界坐标下x,y轴向下为正，以视觉方向的最大值为最小值
            //    if (compute_point.Count() > 0)
            //    {
            //        int n = compute_point.Count();
            //        double min_y = compute_point[0].point_y;
            //        for (int i = 1; i < n; i++)
            //        {
            //            if (compute_point[i].point_y < min_y)
            //            {
            //                min_y = compute_point[i].point_y;
            //                basepoint = compute_point[i];
            //            }
            //        }
            //        int index = compute_point.IndexOf(basepoint);
            //        compute_point.RemoveAt(index);
            //        //冒泡排序（从小到大）
            //        int end = n - 1;
            //        while (end != 0)
            //        {
            //            int flag = 0;
            //            for (int i = 1; i < end; ++i)
            //            {
            //                double Δx = compute_point[i - 1].point_x - basepoint.point_x;
            //                double Δy = compute_point[i - 1].point_y - basepoint.point_y;
            //                double ΔX = compute_point[i].point_x - basepoint.point_x;
            //                double ΔY = compute_point[i].point_y - basepoint.point_y;
            //                if (Math.Atan2(ΔY, ΔX) < Math.Atan2(Δy, Δx))
            //                {
            //                    point_3D itm = compute_point[i];
            //                    compute_point[i] = compute_point[i - 1];
            //                    compute_point[i - 1] = itm;
            //                    flag = 1;
            //                }
            //            }
            //            if (flag == 0)//这是判断点集中只有一个点的情况
            //            {
            //                break;
            //            }
            //            --end;
            //        }
            //        //标记相同的点
            //        Stack<int> del = new Stack<int>();
            //        for (int i = 0; i < n - 2; i++)
            //        {
            //            double Δx = compute_point[i].point_x - basepoint.point_x;
            //            double Δy = compute_point[i].point_y - basepoint.point_y;
            //            double ΔX = compute_point[i + 1].point_x - basepoint.point_x;
            //            double ΔY = compute_point[i + 1].point_y - basepoint.point_y;
            //            if (Math.Atan2(Δy, Δx) == Math.Atan2(ΔY, ΔX))
            //            {
            //                double s = Δx * Δx + Δy * Δy;
            //                double S = ΔX * ΔX + ΔY * ΔY;
            //                if (s < S)
            //                {
            //                    del.Push(i);
            //                }
            //            }
            //        }
            //        //剔除相同的点
            //        if (del != null)
            //        {
            //            int end1 = del.Count();
            //            for (int i = 0; i < end1; i++)
            //            {
            //                compute_point.RemoveAt(i);
            //            }
            //        }
            //        //给临时点集赋值
            //        List<point_3D> temporary_point = new List<point_3D>();
            //        int end2 = compute_point.Count();
            //        for (int i = 0; i < end2; i++)
            //        {
            //            temporary_point.Add(compute_point[i]);
            //        }
            //        //建立由凸包点构成的堆栈(初始化)
            //        //Jarvis's march 步进算法
            //        //由于点连线之后可能构成多个层级的锯齿状，因此每初始化一次都将剔除最下一层的点（几个点构成一层，大致在一条水平线上）
            //        Stack<point_3D> Convex_buns = new Stack<point_3D>();
            //        Stack<point_3D> temporary_buns = new Stack<point_3D>();
            //        Convex_buns.Push(basepoint);
            //        temporary_point.Insert(0, basepoint);
            //        temporary_point.Add(basepoint);
            //        Stack<int> finish = new Stack<int>();
            //        do
            //        {
            //            finish.Clear();
            //            for (int i = 1; i < temporary_point.Count() - 1; i++)
            //            {
            //                double x1 = temporary_point[i - 1].point_x;
            //                double x0 = temporary_point[i].point_x;
            //                double x2 = temporary_point[i + 1].point_x;
            //                double y1 = temporary_point[i - 1].point_y;
            //                double y0 = temporary_point[i].point_y;
            //                double y2 = temporary_point[i + 1].point_y;
            //                double m = (x2 - x1) * (y0 - y1) - (y2 - y1) * (x0 - x1);
            //                Convex_buns.Push(temporary_point[i]);
            //                if (m > 0)
            //                {
            //                    Convex_buns.Pop();
            //                    finish.Push(1);
            //                }
            //                else if (m < 0)
            //                {
            //                    finish.Push(0);
            //                }
            //                else
            //                {

            //                }

            //            }
            //            //将上一遍的初始化结果由Convex_buns赋给temporary_buns
            //            temporary_point.Clear();
            //            int change_end = Convex_buns.Count();
            //            for (int i = 0; i < change_end; i++)
            //            {
            //                temporary_buns.Push(Convex_buns.Pop());//倒转顺序，convex_buns从左到右是从小到大，temporary_buns从左到右是从大到小
            //            }
            //            for (int i = 0; i < change_end; i++)
            //            {
            //                temporary_point.Add(temporary_buns.Pop());
            //            }
            //            temporary_point.Add(basepoint);
            //            Convex_buns.Push(basepoint);
            //            //判断所有层级是否剔除完毕
            //            if (finish.Contains(1) == false)
            //            {
            //                break;
            //            }
            //        } while (finish.Contains(1) == true);
            //        //获取最终的凸包点
            //        int end4 = temporary_point.Count();
            //        for (int i = 0; i < end4; i++)
            //        {
            //            convex_buns_finish.Add(temporary_point[i]);
            //        }
            //        convex_buns_finish.RemoveAt(end4 - 1);
            //    }
            //    //**********************************************************************************************************
            //    //构建三角网TIN
            //    //将全部的离散点由compute_point赋给intersection_point
            //    List<point_3D> intersection_point = new List<point_3D>();
            //    int end5 = compute_point.Count();
            //    intersection_point.Add(basepoint);
            //    for (int i = 0; i < end5; i++)
            //    {
            //        intersection_point.Add(compute_point[i]);
            //    }
            //    //寻找几何中心点
            //    point_3D P0 = new point_3D(null, 0, 0, 0);
            //    int P0_index = 0;
            //    double x_total, y_total;
            //    x_total = y_total = 0;
            //    int end13 = convex_buns_finish.Count();
            //    for (int i = 0; i < end13; i++)
            //    {
            //        x_total = x_total + convex_buns_finish[i].point_x;
            //        y_total = y_total + convex_buns_finish[i].point_y;
            //    }
            //    double x_centre = x_total / end13;
            //    double y_centre = y_total / end13;
            //    double L = 0;
            //    double min_L = Math.Sqrt(Math.Pow((intersection_point[0].point_x - x_centre), 2) + Math.Pow((intersection_point[0].point_y - y_centre), 2));
            //    int end14 = intersection_point.Count();
            //    for (int i = 1; i < end14; i++)
            //    {

            //        L = Math.Sqrt(Math.Pow((intersection_point[i].point_x - x_centre), 2) + Math.Pow((intersection_point[i].point_y - y_centre), 2));
            //        if (L < min_L)
            //        {
            //            min_L = L;
            //            P0 = intersection_point[i];
            //            P0_index = i;
            //        }
            //    }
            //    centrepoint = P0;
            //    //寻找地形最低点,绘制地形图用
            //    DataDraw.draw_TINheight = Calculate_TINheight(intersection_point);
            //    //去除几何中心点
            //    intersection_point.RemoveAt(P0_index);
            //    //去除离散点中的凸包点
            //    int end6 = convex_buns_finish.Count();
            //    for (int i = 0; i < end6; i++)
            //    {
            //        for (int j = 0; j < intersection_point.Count(); j++)
            //        {
            //            if (convex_buns_finish[i].point_name == intersection_point[j].point_name)
            //            {
            //                intersection_point.RemoveAt(j);
            //                j--;
            //            }
            //        }
            //    }

            //    //生成三角形并加入T1中
            //    for (int i = 0; i < end6 - 1; i++)
            //    {
            //        line_3D la;
            //        la.line_name = P0.point_name + convex_buns_finish[i].point_name;
            //        la.point_begin = P0;
            //        la.point_end = convex_buns_finish[i];
            //        line_3D lb;
            //        lb.line_name = convex_buns_finish[i].point_name + convex_buns_finish[i + 1].point_name;
            //        lb.point_begin = convex_buns_finish[i];
            //        lb.point_end = convex_buns_finish[i + 1];
            //        line_3D lc;
            //        lc.line_name = P0.point_name + convex_buns_finish[i + 1].point_name;
            //        lc.point_begin = P0;
            //        lc.point_end = convex_buns_finish[i + 1];

            //        triangle_3D t;
            //        t.surface_name = P0.point_name + convex_buns_finish[i].point_name + convex_buns_finish[i + 1].point_name;
            //        t.line_a = la;
            //        t.line_b = lb;
            //        t.line_c = lc;
            //        T1.Add(t);
            //    }
            //    line_3D la0;
            //    la0.line_name = P0.point_name + convex_buns_finish[end6 - 1].point_name;
            //    la0.point_begin = P0;
            //    la0.point_end = convex_buns_finish[end6 - 1];
            //    line_3D lb0;
            //    lb0.line_name = convex_buns_finish[end6 - 1].point_name + convex_buns_finish[0].point_name;
            //    lb0.point_begin = convex_buns_finish[end6 - 1];
            //    lb0.point_end = convex_buns_finish[0];
            //    line_3D lc0;
            //    lc0.line_name = P0.point_name + convex_buns_finish[0].point_name;
            //    lc0.point_begin = P0;
            //    lc0.point_end = convex_buns_finish[0];
            //    triangle_3D t0;
            //    t0.surface_name = P0.point_name + convex_buns_finish[end6 - 1].point_name + convex_buns_finish[0].point_name;
            //    t0.line_a = la0;
            //    t0.line_b = lb0;
            //    t0.line_c = lc0;
            //    T1.Add(t0);
            //    //遍历离散点，生成平面三角网

            //    int end7 = intersection_point.Count();
            //    for (int i = 0; i < end7; i++)
            //    {
            //        point_3D P = intersection_point[i];
            //        for (int j = 0; j < T1.Count(); j++)
            //        {
            //            //计算三角形的外接圆圆心坐标和半径
            //            bool result = Inspection_of_outer_circle(
            //                T1[j].line_a.point_begin.point_x, T1[j].line_a.point_begin.point_y,
            //                T1[j].line_b.point_begin.point_x, T1[j].line_b.point_begin.point_y,
            //                T1[j].line_c.point_end.point_x, T1[j].line_c.point_end.point_y,
            //                P.point_x, P.point_y);
            //            if (result == true)
            //            {
            //                T2.Add(T1[j]);
            //                T1.RemoveAt(j);
            //                j--;
            //            }
            //        }

            //        //寻找T2中的公共边
            //        if (T2.Count() > 0)
            //        {
            //            int end8 = T2.Count();
            //            for (int n = 0; n < end8; n++)
            //            {
            //                S1.Add(T2[n].line_a);
            //                S1.Add(T2[n].line_b);
            //                S1.Add(T2[n].line_c);
            //            }
            //            if (S1.Count() > 0)
            //            {
            //                for (int w = 0; w < S1.Count(); w++)
            //                {
            //                    for (int m = w + 1; m < S1.Count(); m++)
            //                    {
            //                        if (S1[w].line_name == S1[m].line_name)
            //                        {
            //                            repeat_line_3D.Add(S1[w]);
            //                        }
            //                    }
            //                }
            //            }
            //            //删除重复的边（包括本体边）
            //            int end9 = repeat_line_3D.Count();
            //            for (int n = 0; n < end9; n++)
            //            {
            //                for (int m = 0; m < S1.Count(); m++)
            //                {
            //                    if (repeat_line_3D[n].line_name == S1[m].line_name)
            //                    {
            //                        S1.RemoveAt(m);
            //                        m--;
            //                    }
            //                }
            //            }
            //            //清空T2
            //            T2.Clear();
            //            repeat_line_3D.Clear();
            //        }

            //        //生成新的三角形
            //        int end10 = S1.Count();
            //        for (int n = 0; n < end10; n++)
            //        {
            //            line_3D la;
            //            la.line_name = P.point_name + S1[n].point_begin.point_name;
            //            la.point_begin = P;
            //            la.point_end = S1[n].point_begin;
            //            line_3D lc;
            //            lc.line_name = P.point_name + S1[n].point_end.point_name;
            //            lc.point_begin = P;
            //            lc.point_end = S1[n].point_end;
            //            triangle_3D t;
            //            t.surface_name = P.point_name + S1[n].point_begin.point_name + S1[n].point_end.point_name;
            //            t.line_a = la;
            //            t.line_b = S1[n];
            //            t.line_c = lc;
            //            T1.Add(t);
            //        }
            //        //清空S
            //        S1.Clear();

            //    }
            //    return T1;
            //}
            //计算建筑物数据
            #endregion
            public TIN(string TIN_name, List<point_3D> point_TIN)
            {
                this.TIN_name = TIN_name;
                this.data_type = "TIN";
                this.centre_point = Find_CentrePoint(point_TIN);
                this.triangle_NET = Calculate_TIN(point_TIN);
                this.point_TIN = point_TIN;
            }
            // 综合所有步骤并封装
            public List<triangle_3D> Calculate_TIN(List<point_3D> point_TIN)
            {
                //获得三角网的点集(T1)
                point_3D[] Matrix = Build_Matrix(point_TIN.ToArray());
                List<point_3D> T1 = Build_Initial_Trinet(Matrix);
                T1 = Build_Trinet(point_TIN.ToArray(), T1);
                T1 = Final_Trinet(T1, Matrix);
                //将点集转化为三角形集(triangle_NET)
                triangle_NET = new List<triangle_3D>();
                for (int i = 0; i < T1.Count; i += 3)
                {
                    triangle_3D triangle_item_NET = new triangle_3D(T1[i], T1[i + 1], T1[i + 2]);
                    triangle_NET.Add(triangle_item_NET);
                }

                height_TIN = Calculate_Height(point_TIN);
                return triangle_NET;
            }
            //生成初始总矩形
            private point_3D[] Build_Matrix(point_3D[] p)
            {
                double[] X = new double[p.Length];
                double[] Y = new double[p.Length];
                for (int i = 0; i < p.Length; i++)
                {
                    X[i] = p[i].point_x;
                    Y[i] = p[i].point_y;
                }
                double x_min, x_max, y_min, y_max;
                x_min = X.Min();
                x_max = X.Max();
                y_max = Y.Max();
                y_min = Y.Min();
                point_3D[] Matrix = new point_3D[4];
                Matrix[0] = new point_3D("P1", x_min - 1, y_min - 1, 0);
                Matrix[1] = new point_3D("P2", x_min - 1, y_max + 1, 0);
                Matrix[2] = new point_3D("P3", x_max + 1, y_max + 1, 0);
                Matrix[3] = new point_3D("P4", x_max + 1, y_min - 1, 0);
                return Matrix;
            }
            //生成初始三角网
            private List<point_3D> Build_Initial_Trinet(point_3D[] Matrix)
            {
                List<point_3D> T1 = new List<point_3D>
                {
                    Matrix[0],
                    Matrix[1],
                    Matrix[2],
                    Matrix[0],
                    Matrix[2],
                    Matrix[3]
                };
                return T1;
            }
            // 通过遍历离散点，生成平面三角网
            private List<point_3D> Build_Trinet(point_3D[] p, List<point_3D> T1)
            {
                List<point_3D> T2 = new List<point_3D>();
                List<line_3D> S = new List<line_3D>();

                for (int i = 0; i < p.Length; i++)
                {
                    for (int j = 0; j < T1.Count - 2; j += 3)
                    {
                        point_3D A = new point_3D(null, T1[j].point_x, T1[j].point_y, 0);
                        point_3D B = new point_3D(null, T1[j + 1].point_x, T1[j + 1].point_y, 0);
                        point_3D C = new point_3D(null, T1[j + 2].point_x, T1[j + 2].point_y, 0);

                        double x0, y0, r, lr;
                        x0 = ((B.point_y - A.point_y) * (C.point_y * C.point_y - A.point_y * A.point_y + C.point_x * C.point_x - A.point_x * A.point_x) - (C.point_y - A.point_y) * (B.point_y * B.point_y - A.point_y * A.point_y + B.point_x * B.point_x - A.point_x * A.point_x))
                            / (2 * (C.point_x - A.point_x) * (B.point_y - A.point_y) - 2 * (B.point_x - A.point_x) * (C.point_y - A.point_y));
                        y0 = ((B.point_x - A.point_x) * (C.point_x * C.point_x - A.point_x * A.point_x + C.point_y * C.point_y - A.point_y * A.point_y) - (C.point_x - A.point_x) * (B.point_x * B.point_x - A.point_x * A.point_x + B.point_y * B.point_y - A.point_y * A.point_y))
                            / (2 * (C.point_y - A.point_y) * (B.point_x - A.point_x) - 2 * (B.point_y - A.point_y) * (C.point_x - A.point_x));
                        r = Math.Sqrt((x0 - A.point_x) * (x0 - A.point_x) + (y0 - A.point_y) * (y0 - A.point_y));

                        lr = Math.Sqrt((p[i].point_x - x0) * (p[i].point_x - x0) + (p[i].point_y - y0) * (p[i].point_y - y0)); //P点到外接圆圆心的距离
                        if (lr < r)  //P点在三角形ABC外接圆的内部
                        {
                            T2.Add(T1[j]);
                            T2.Add(T1[j + 1]);
                            T2.Add(T1[j + 2]);
                            T1.RemoveAt(j + 2);
                            T1.RemoveAt(j + 1);
                            T1.RemoveAt(j);
                            j -= 3;
                        }
                    }//遍历T1中全部三角形

                    //三角形用3点排列变为3边排列
                    for (int k = 0; k < T2.Count; k++)
                    {
                        line_3D si;
                        if (k % 3 != 2)
                        {
                            si = new line_3D(T2[k], T2[k + 1]);
                        }
                        else
                        {
                            si = new line_3D(T2[k], T2[k - 2]);
                        }
                        S.Add(si);
                    }

                    //删除S中的公共边
                    for (int k = 0; k < S.Count - 1; k++)
                    {
                        int sss = 0;
                        for (int m = k + 1; m < S.Count; m++)
                        {
                            if (S[m] == S[k])
                            {
                                sss = 1;
                                S.RemoveAt(m);
                                m--;
                            }
                        }
                        if (sss == 1)
                        {
                            S.RemoveAt(k);
                            k--;
                        }
                    }

                    T2.Clear();

                    for (int k = 0; k < S.Count; k++)
                    {
                        T1.Add(S[k].point_begin);
                        T1.Add(S[k].point_end);
                        T1.Add(p[i]);
                    }
                    S.Clear();
                }//遍历所有离散点
                return T1;
            }
            // 构成不规则三角网
            private List<point_3D> Final_Trinet(List<point_3D> T1, point_3D[] Matrix)
            {
                for (int i = 0; i < T1.Count; i += 3)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (T1[i] == Matrix[j] || T1[i + 1] == Matrix[j] || T1[i + 2] == Matrix[j])
                        {
                            T1.RemoveRange(i, 3);
                            i -= 3;
                            break;
                        }
                    }
                }
                return T1;
            }
            //计算地形最高点与最低点的差值
            private double Calculate_Height(List<point_3D> point_TIN)
            {
                double draw_lowest, draw_highest;
                draw_lowest = draw_highest = 0;
                if (point_TIN != null)
                {
                    draw_lowest = point_TIN[0].point_z;
                    draw_highest = point_TIN[0].point_z;
                    for (int i = 1; i < point_TIN.Count; i++)
                    {
                        if (draw_lowest > point_TIN[i].point_z)
                        {
                            draw_lowest = point_TIN[i].point_z;
                        }
                        if (draw_highest < point_TIN[i].point_z)
                        {
                            draw_highest = point_TIN[i].point_z;
                        }
                    }
                }
                float D_value = Convert.ToSingle(draw_highest - draw_lowest);
                return D_value;
            }
            //寻找几何中心点
            public static point_3D Find_CentrePoint(List<point_3D> point_TIN)
            {
                //寻找几何中心点
                double x_total, y_total;
                x_total = y_total = 0;
                int count = point_TIN.Count;
                for (int i = 0; i < count; i++)
                {
                    x_total = x_total + point_TIN[i].point_x;
                    y_total = y_total + point_TIN[i].point_y;
                }
                double x_centre = x_total / count;
                double y_centre = y_total / count;
                point_3D centre_P = new point_3D("centre_point", x_centre, y_centre, 0);
                return centre_P;
            }
        }
        /// <summary>
        /// 建筑数据模型
        /// </summary>
        public class Build <T>
        {
            //建筑的基本属性信息
            public string build_name;
            public string build_type;
            public Cube bounding_box;//总的包围盒                            
            public point_3D centre_point;//底面中心点
            public double length; //总包围盒的长宽高
            public double wide;
            public double height;
            //具体是哪种建筑
            public T build_item;
            //所有的轮廓点
            public List<point_3D> points_Build;
            //高差，绘图调整
            public static double height_build;

            public Build()
            {

            }
            public Build(Build_1 build_1)
            {
                this.build_name = build_1.main_body.cube_name;
                this.build_type = "Build_1";
                this.points_Build = new List<point_3D>();
                for(int i = 0; i < build_1.point_Build_1.Count; i++)
                {
                    this.points_Build.Add(build_1.point_Build_1[i]);
                }
                this.bounding_box = Create_BoundingBox(points_Build);
                this.centre_point = new point_3D();
                this.centre_point = this.centre_point & build_1.main_body.centre_point;
                this.length = this.bounding_box.length;
                this.wide = this.bounding_box.wide;
                this.height = this.bounding_box.height;
                //this.build_item = build_1;这里只能通过实例化的Build对象去赋值
            }
            //....其他类型的建筑实例化

            /// <summary>
            /// 所有不同种类建筑的中心点
            /// </summary>
            /// <param name="building"></param>
            /// <returns></returns>
            public static point_3D Find_CentrePoint(List<Build<object>> building)
            {
                double x_total, y_total;
                x_total = y_total = 0;
                int count = building.Count;
                for (int i = 0; i < count; i++)
                {
                    x_total = x_total + building[i].centre_point.point_x;
                    y_total = y_total + building[i].centre_point.point_y;
                }
                double x_centre = x_total / count;
                double y_centre = y_total / count;
                point_3D centre_point = new point_3D("centre_point", x_centre, y_centre, 0);
                return centre_point;
            }
            /// <summary>
            /// 创建包围盒
            /// </summary>
            /// <param name="model_points"></param>
            /// <returns></returns>
            public Cube Create_BoundingBox(List<point_3D> model_points)
            {
                double offset = 0.5;
                int count = model_points.Count;
                double max_x, max_y, max_z;
                double min_x, min_y, min_z;
                max_x = min_x = model_points[0].point_x;
                max_y = min_y = model_points[0].point_y;
                max_z = min_z = model_points[0].point_z;

                //寻找模型的最大与最小的X值,Y值,Z值
                for (int i = 0; i < count; i++)
                {
                    if (max_x < model_points[i].point_x)
                    {
                        max_x = model_points[i].point_x;
                    }
                    if (max_y < model_points[i].point_y)
                    {
                        max_y = model_points[i].point_y;
                    }
                    if (max_z < model_points[i].point_z)
                    {
                        max_z = model_points[i].point_z;
                    }
                    if (min_x > model_points[i].point_x)
                    {
                        min_x = model_points[i].point_x;
                    }
                    if (min_y > model_points[i].point_y)
                    {
                        min_y = model_points[i].point_y;
                    }
                    if (min_z > model_points[i].point_z)
                    {
                        min_z = model_points[i].point_z;
                    }
                }

                point_3D up_a = new point_3D("up_a", max_x + offset, max_y + offset, max_z + offset);
                point_3D up_b = new point_3D("up_b", min_x - offset, max_y + offset, max_z + offset);
                point_3D up_c = new point_3D("up_c", max_x + offset, min_y - offset, max_z + offset);
                point_3D up_d = new point_3D("up_d", min_x - offset, min_y - offset, max_z + offset);

                point_3D down_a = new point_3D("down_a", max_x + offset, max_y + offset, min_z - offset);
                point_3D down_b = new point_3D("down_b", min_x - offset, max_y + offset, min_z - offset);
                point_3D down_c = new point_3D("down_c", max_x + offset, min_y - offset, min_z - offset);
                point_3D down_d = new point_3D("down_d", min_x - offset, min_y - offset, min_z - offset);

                Cube cube = new Cube("BoundingBox", up_a, up_b, up_c, up_d, down_a, down_b, down_c, down_d);
                return cube;
            }
            /// <summary>
            /// 最简单的建筑
            /// </summary>
            public class Build_1 : Build<T>
            {
                public Cube main_body; 
                public List<point_3D> point_Build_1;
                public Build_1()
                {
                    this.main_body = new Cube();
                    this.point_Build_1 = new List<point_3D>();
                }
                public Build_1(Cube main_body)
                {
                    this.main_body = new Cube();
                    this.main_body = this.main_body & main_body;
                    this.point_Build_1 = ModelToPoints();                                  
                }
                /// <summary>
                /// Build_1模型分解
                /// </summary>
                /// <param name="build_1"></param>
                /// <returns></returns>
                public List<point_3D> ModelToPoints()
                {
                    List<point_3D> points = new List<point_3D>();
                    points.Add(main_body.up_a);
                    points.Add(main_body.up_b);
                    points.Add(main_body.up_c);
                    points.Add(main_body.up_d);
                    points.Add(main_body.down_a);
                    points.Add(main_body.down_b);
                    points.Add(main_body.down_c);
                    points.Add(main_body.down_d);
                    return points;
                }
                /// <summary>
                /// 将b的参数赋值给a，而不是引用其地址
                /// </summary>
                /// <param name="a"></param>
                /// <param name="b"></param>
                /// <returns></returns>
                public static Build_1 operator & (Build_1 a, Build_1 b)
                {
                    a.main_body = a.main_body & b.main_body;
                    for(int i = 0; i < b.point_Build_1.Count; i++)
                    {
                        a.point_Build_1[i] = b.point_Build_1[i];
                    }
                    return a;
                }
            }

            //...

        }
        //道路模型
        public class Road<T>
        {
            //道路的基本属性信息
            public string road_name;
            public string road_type;
            //作为道路而言没必要设置包围盒，但是为了统一模型管理。可以将道路的包围盒的高度设置为1
            public Cube bounding_box;//总的包围盒               
            public point_3D centre_point;//底面中心点
            public double length; //总包围盒的长宽高
            public double wide;
            public double height;
            //具体是哪种建筑
            public T road_item;
            //所有的轮廓点
            public List<point_3D> points_Road;
            //高差，绘图调整
            public static double height_road;
            public int texture_index { get; set; }
            public Road()
            {

            }
            public Road(string road_name, Road_1 road_1)
            {
                this.road_name = road_name;
                this.road_type = "Road_1";
                this.points_Road = new List<point_3D>();
                for (int i = 0; i < road_1.point_Road_1.Count; i++)
                {
                    this.points_Road.Add(road_1.point_Road_1[i]);
                }
                this.bounding_box = new Cube();
                this.bounding_box = this.bounding_box & Create_BoundingBox(this.points_Road);
                this.centre_point = new point_3D();
                this.centre_point = this.centre_point & this.bounding_box.centre_point;
                this.length = this.bounding_box.length;
                this.wide = this.bounding_box.wide;
                this.height = this.bounding_box.height;
                //this.road_item = road_1;
            }
            public Road(string road_name, Road_2 road_2)
            {
                this.road_name = road_name;
                this.road_type = "Road_2";
                this.points_Road = new List<point_3D>();
                for (int i = 0; i < road_2.point_Road_2.Count; i++)
                {
                    this.points_Road.Add(road_2.point_Road_2[i]);
                }
                this.bounding_box = new Cube();
                this.bounding_box = this.bounding_box & Create_BoundingBox(this.points_Road);
                this.centre_point = new point_3D();
                this.centre_point = this.centre_point & this.bounding_box.centre_point;
                this.length = this.bounding_box.length;
                this.wide = this.bounding_box.wide;
                this.height = this.bounding_box.height;
            }
            //...

            public Cube Create_BoundingBox(List<point_3D> model_points)
            {
                double offset = 0.5;
                int count = model_points.Count;
                double max_x, max_y;
                double min_x, min_y;
                max_x = min_x = model_points[0].point_x;
                max_y = min_y = model_points[0].point_y;
                //寻找模型的最大与最小的X值,Y值,Z值
                for (int i = 0; i < count; i++)
                {
                    if (max_x < model_points[i].point_x)
                    {
                        max_x = model_points[i].point_x;
                    }
                    if (max_y < model_points[i].point_y)
                    {
                        max_y = model_points[i].point_y;
                    }
                    if (min_x > model_points[i].point_x)
                    {
                        min_x = model_points[i].point_x;
                    }
                    if (min_y > model_points[i].point_y)
                    {
                        min_y = model_points[i].point_y;
                    }
                }
                point_3D down_a = new point_3D("down_a", max_x + offset, max_y + offset, model_points[0].point_z - offset);
                point_3D down_b = new point_3D("down_b", min_x - offset, max_y + offset, model_points[0].point_z - offset);
                point_3D down_c = new point_3D("down_c", max_x + offset, min_y - offset, model_points[0].point_z - offset);
                point_3D down_d = new point_3D("down_d", min_x - offset, min_y - offset, model_points[0].point_z - offset);

                Cube cube = new Cube("BoundingBox", down_a, down_b, down_c, down_d, 1);
                return cube;
            }
            //直线
            public class Road_1 : Road<T>
            {
                public line_3D a;
                public line_3D b;
                public List<point_3D> point_Road_1;
                public Road_1()
                {
                    this.a = new line_3D();
                    this.b = new line_3D();
                    this.point_Road_1 = new List<point_3D>();
                }
                public Road_1(line_3D a, line_3D b)
                {
                    this.a = new line_3D();
                    this.a = this.a & a;
                    this.b = new line_3D();
                    this.b = this.b & b;
                    this.point_Road_1 = ModelToPoints();
                }
                public Road_1(point_3D centre_point, double length, double wide)
                {
                    point_3D a_start = new point_3D("a_start", centre_point.point_x + length / 2, centre_point.point_y + wide / 2, centre_point.point_z);
                    point_3D a_end = new point_3D("a_end", centre_point.point_x - length / 2, centre_point.point_y + wide / 2, centre_point.point_z);
                    point_3D b_start = new point_3D("b_start", centre_point.point_x + length / 2, centre_point.point_y - wide / 2, centre_point.point_z);
                    point_3D b_end = new point_3D("b_end", centre_point.point_x - length / 2, centre_point.point_y - wide / 2, centre_point.point_z);

                    this.a = new line_3D(a_start, a_end);
                    this.b = new line_3D(b_start, b_end);

                    this.point_Road_1 = ModelToPoints();
                }
                public List<point_3D> ModelToPoints()
                {
                    List<point_3D> points = new List<point_3D>();
                    points.Add(a.point_begin);
                    points.Add(a.point_end);
                    points.Add(b.point_begin);
                    points.Add(b.point_end);
                    return points;
                }
            }
            //曲线
            public class Road_2 : Road<T>
            {
                public point_3D control_point_a;
                public List<point_3D> curve_a;
                public point_3D control_point_b;
                public List<point_3D> curve_b;
                public List<point_3D> point_Road_2;
                public Road_2()
                {
                    this.control_point_a = new point_3D();
                    this.curve_a = new List<point_3D>();
                    this.control_point_b = new point_3D();
                    this.curve_b = new List<point_3D>();
                    this.point_Road_2 = new List<point_3D>();
                }
                public Road_2(point_3D[] start_point, point_3D[] middle_point, point_3D[] end_point)
                {                   
                    this.curve_a = new List<point_3D>();
                    this.curve_a = Calculate_Curve(start_point[0], middle_point[0], end_point[0]);
                    this.curve_b = new List<point_3D>();
                    this.curve_b = Calculate_Curve(start_point[1], middle_point[1], end_point[1]);
                    this.point_Road_2 = new List<point_3D>();
                    this.point_Road_2 = ModelToPoints();
                }
                private List<point_3D> Calculate_Curve(point_3D start_point, point_3D middle_point, point_3D end_point)
                {
                    List<point_3D> points = new List<point_3D>();
                    for(int i = 1; i < 11; i++)
                    {
                        double t = i / 10.0;
                        double x = Math.Pow((1 - t), 2) * start_point.point_x + 2 * t * (1 - t) * middle_point.point_x + Math.Pow(t, 2) * end_point.point_x;
                        double y = Math.Pow((1 - t), 2) * start_point.point_y + 2 * t * (1 - t) * middle_point.point_y + Math.Pow(t, 2) * end_point.point_y;
                        point_3D item = new point_3D("curve_point", x, y, middle_point.point_z);
                        points.Add(item);
                    }
                    return points;
                }
                public List<point_3D> ModelToPoints()
                {
                    List<point_3D> points = new List<point_3D>();
                    for(int i = 0; i < curve_a.Count; i++)
                    {
                        points.Add(curve_a[i]);
                    }
                    for (int i = 0; i < curve_b.Count; i++)
                    {
                        points.Add(curve_b[i]);
                    }
                    return points;
                }
            }
        }
        //水域模型
        public class Water
        {
            //水域基本属性信息
            public string water_name;
            public string water_type;
            //水域的详细信息
            //所有的轮廓点
            public List<point_3D> points_Water;
            public int texture_index { get; set; }
            public Water()
            {
                this.water_name = "null";
                this.water_type = "null";
                this.points_Water = new List<point_3D>();
                this.texture_index = -1;
            }
            public Water (string water_name, string water_type, List<point_3D> water)
            {
                this.water_name = water_name;
                this.water_type = water_type;
                this.points_Water = new List<point_3D>();
                for(int i = 0; i < water.Count; i++)
                {
                    points_Water.Add(water[i]);
                }
                this.texture_index = -1;
            }
        }
        //树木模型
        public class Tree
        {
            //树木基本属性信息
            public string tree_name;
            public string tree_type;
            //水域的详细信息
            //模型中心点
            public point_3D centre_point;
            public double height;
            //所有的轮廓点
            public List<point_3D> points_Tree;
            public int texture_index { get; set; }
            public Tree()
            {
                this.tree_name = "null";
                this.tree_type = "null";
                this.centre_point = new point_3D();
                this.height = 0;
                this.points_Tree = new List<point_3D>();
                this.texture_index = -1;
            }
            public Tree(string tree_name, string tree_type, point_3D centre_point, double height)
            {
                this.tree_name = tree_name;
                this.tree_type = tree_type;
                this.centre_point = new point_3D();
                this.centre_point = this.centre_point & centre_point;
                this.height = height;
                this.points_Tree = new List<point_3D>();
                points_Tree.Add(new point_3D("up_left", centre_point.point_x + 1, centre_point.point_y, centre_point.point_z + height));
                points_Tree.Add(new point_3D("up_right", centre_point.point_x - 1, centre_point.point_y, centre_point.point_z + height));
                points_Tree.Add(new point_3D("down_left", centre_point.point_x + 1, centre_point.point_y, centre_point.point_z));
                points_Tree.Add(new point_3D("down_right", centre_point.point_x - 1, centre_point.point_y, centre_point.point_z));
            }
        }
        //路灯模型
        public class Lamp
        {
            //路灯基本属性信息
            public string lamp_name;
            public string lamp_type;
            //水域的详细信息
            //模型中心点
            public point_3D centre_point;
            public double height;
            //所有的轮廓点
            public List<point_3D> points_Lamp;
            public int texture_index { get; set; }
            public Lamp()
            {
                this.lamp_name = "null";
                this.lamp_type = "null";
                this.centre_point = new point_3D();
                this.height = 0;
                this.points_Lamp = new List<point_3D>();
                this.texture_index = -1;
            }
            public Lamp(string lamp_name, string lamp_type, point_3D centre_point, double height)
            {
                this.lamp_name = lamp_name;
                this.lamp_type = lamp_type;
                this.centre_point = new point_3D();
                this.centre_point = this.centre_point & centre_point;
                this.height = height;               
            }
        }

        //--------------------------------------------------其他----------------------------------------------------
        /// <summary>
        /// 拾取到返回的信息数据结构
        /// </summary>
        public class SelectedData <T> where T : class, new()
        {
            //基本属性信息
            public string data_name;
            public string[] data_type;       
            public double distance;          
            public Cube bounding_box;
            public int[] data_index;
            //具体数据
            public T data_item;
            public List<point_3D> data_points;
            public SelectedData()
            {
                this.data_name = "null";
                this.data_type = new string[1];
                this.data_type[0] = "null";
                this.data_index = new int[1];
                this.distance = -1;
                this.bounding_box = new Cube("Bounding_Box", new point_3D("centre_point", 0, 0, 0), 0, 0, 0);
                this.data_item = default(T);
                this.data_points = new List<point_3D>();
            }
            public SelectedData(triangle_3D tin, double distance, int[] data_index)
            {
                this.data_name = tin.surface_name;
                this.data_type = new string[1];
                this.data_type[0] = "TIN";
                this.distance = distance;
                this.bounding_box = null;
                this.data_index = data_index;
                //this.data_item = tin;这里只能通过实例化的SelectedData对象去赋值
            }
            public SelectedData(Build<object> build, double distance, int[] data_index)
            {
                this.data_name = build.build_name;
                this.data_type = new string[2];
                this.data_type[0] = "Build";
                this.data_type[1] = build.build_type;
                this.distance = distance;
                this.bounding_box = build.bounding_box;//拾取类没必要重新开辟内存空间去赋值，直接索引地址即可
                this.data_index = data_index;
                this.data_points = build.points_Build;
                //this.data_item =build;这里只能通过实例化的SelectedData对象去赋值
            }
            public SelectedData(Road<object> road, double distance, int[] data_index)
            {
                this.data_name = road.road_name;
                this.data_type = new string[2];
                this.data_type[0] = "Road";
                this.data_type[1] = road.road_type;
                this.distance = distance;
                this.bounding_box = road.bounding_box;
                this.data_index = data_index;
                this.data_points = road.points_Road;
            }
            /// <summary>
            /// 将b的参数赋值给a，而不是引用其地址
            /// </summary>
            /// <param name="a"></param>
            /// <returns></returns>
            public static SelectedData<T> operator &(SelectedData<T> a, SelectedData<T> b)
            {
                a.data_name = b.data_name;
                a.data_type = b.data_type;
                a.distance = b.distance;
                if (b.data_type[0] == "TIN")
                {
                    a.bounding_box = b.bounding_box;
                }
                else
                {
                    a.bounding_box = a.bounding_box & b.bounding_box;
                }
                a.data_index = b.data_index;
                //由于有不同的类型，需要实例化对象去根据情况赋值
                T item = new T();
                item = b.data_item;
                a.data_item = item;
                //a.data_item = b.data_item;
                if(b.data_type[0] != "TIN")
                {
                    if (a.data_points.Count > 0)
                    {
                        for (int i = 0; i < b.data_points.Count; i++)
                        {
                            a.data_points[i] = b.data_points[i];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < b.data_points.Count; i++)
                        {
                            a.data_points.Add(b.data_points[i]);
                        }
                    }
                }
                return a;
            }            

            public void Initialize_SelectedData()
            {
                this.data_name = "null";
                this.data_type = new string[1];
                this.data_type[0] = "null";
                this.data_index = new int[1];
                this.distance = -1;
                this.bounding_box = new Cube("Bounding_Box", new point_3D("centre_point", 0, 0, 0), 0, 0, 0);
                this.data_item = default(T);
                this.data_points = new List<point_3D>();
            }
           
        }
        public class AddModel<T> where T : class, new()
        {
            //基本属性信息
            public string data_name;
            public string[] data_type;
            public int[] data_index;
            //具体数据
            public T data_item;
            public List<point_3D> data_points;

            public AddModel()
            {
                this.data_name = "null";
                this.data_type = new string[1];
                this.data_type[0] = "null";
                this.data_index = new int[1];
                this.data_item= default(T);
                this.data_points= new List<point_3D>();
            }

            public AddModel(Build<object> build, int[] data_index)
            {
                this.data_name = build.build_name;
                this.data_type = new string[2];
                this.data_type[0] = "Build";
                this.data_type[1] = build.build_type;
                this.data_index = data_index;
                //this.data_item = build.build_item;
                this.data_points = build.points_Build;              
            }
            public AddModel(Road<object> road, int[] data_index)
            {
                this.data_name = road.road_name;
                this.data_type = new string[2];
                this.data_type[0] = "Road";
                this.data_type[1] = road.road_type;
                this.data_index = data_index;
                //this.data_item = road.road_item;
                this.data_points = road.points_Road;
            }
            public void Initialize_AddModle()
            {
                this.data_name = "null";
                this.data_type = new string[1];
                this.data_type[0] = "null";
                this.data_index = new int[1];
                this.data_item = default(T);
                this.data_points = new List<point_3D>();
            }
        }
    }
}
