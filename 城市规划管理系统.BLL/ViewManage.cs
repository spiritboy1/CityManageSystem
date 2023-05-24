using SharpGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static 城市规划管理系统.BLL.DataEntity;

namespace 城市规划管理系统.BLL
{
    public class ViewManage
    {
        public ViewManage()
        {

        }
        public static CameraParament my_parameter;
        public class ViewController
        {
            public double speed;
            public ViewController()
            {
                TriTuple eye = new TriTuple(0f, 0f, -100f);
                TriTuple center = new TriTuple(0f, 0f, 0f);
                TriTuple up = new TriTuple(0f, 1f, 0f);
                my_parameter.eye = eye;
                my_parameter.center = center;
                my_parameter.up= up;
                speed = 0.1;
            }
            public double GetSpeed()
            {
                return speed;
            }
            public void SetSpeed(double speed)
            {
                this.speed = speed;
            }
            public CameraParament GetCamera_Parameter()
            {
                return my_parameter;
            }
            public void SetCamera_Parameter(CameraParament cameraParament)
            {
                my_parameter = cameraParament;
            }
            public void GoFront()
            {
                var diff = new TriTuple(my_parameter.center.X - my_parameter.eye.X,
                    my_parameter.center.Y - my_parameter.eye.Y,
                    0);
                var length = diff.X * diff.X + 0 + diff.Y * diff.Y;
                var radio = Math.Sqrt(speed * speed / length);
                var speedDiff = new TriTuple(diff.X * radio, diff.Y * radio, 0);//与前后不同的是这里的X,Y位置不变
                my_parameter.eye.Add(speedDiff);
                my_parameter.center.Add(speedDiff);
            }

            public void GoBack()
            {
                var diff = new TriTuple(my_parameter.center.X - my_parameter.eye.X,
                    my_parameter.center.Y - my_parameter.eye.Y,
                    0);
                var length = diff.X * diff.X + 0 + diff.Y * diff.Y;
                var radio = Math.Sqrt(speed * speed / length);
                var speedDiff = new TriTuple(-diff.X * radio, -diff.Y * radio, 0);
                my_parameter.eye.Add(speedDiff);
                my_parameter.center.Add(speedDiff);
            }

            public void GoLeft()
            {
                var diff = new TriTuple(my_parameter.center.X - my_parameter.eye.X,
                    my_parameter.center.Y - my_parameter.eye.Y,
                    0);
                var length = diff.X * diff.X + 0 + diff.Y * diff.Y;
                var radio = Math.Sqrt(speed * speed / length);
                var speedDiff = new TriTuple(diff.Y * radio, -diff.X * radio, 0);//与前后不同的是这里的X,Y交换位置 
                my_parameter.eye.Add(speedDiff);
                my_parameter.center.Add(speedDiff);
            }

            public void GoRight()
            {
                var diff = new TriTuple(my_parameter.center.X - my_parameter.eye.X,
                    my_parameter.center.Y - my_parameter.eye.Y,
                    0);
                var length = diff.X * diff.X + 0 + diff.Y * diff.Y;
                var radio = Math.Sqrt(speed * speed / length);
                var speedDiff = new TriTuple(-diff.Y * radio, diff.X * radio, 0);
                my_parameter.eye.Add(speedDiff);
                my_parameter.center.Add(speedDiff);
            }

            public void GoUp()
            {
                my_parameter.eye.Z = my_parameter.eye.Z - speed;
                my_parameter.center.Z = my_parameter.center.Z - speed;
            }

            public void GoDown()
            {
                my_parameter.eye.Z = my_parameter.eye.Z + speed;
                my_parameter.center.Z = my_parameter.center.Z + speed;
            }

            public void Turn(double turnAngle)
            {
                var diff = new TriTuple(my_parameter.center.X - my_parameter.eye.X,
                    my_parameter.center.Y - my_parameter.eye.Y,
                    0);
                var cos = Math.Cos(turnAngle);
                var sin = Math.Sin(turnAngle);
                var centerDiff = new TriTuple(diff.X * cos - diff.Y * sin,
                    diff.X * sin + diff.Y * cos,
                    0);
                my_parameter.center.X = my_parameter.eye.X + centerDiff.X;
                my_parameter.center.Y = my_parameter.eye.Y + centerDiff.Y;
            }

            public void Stagger(double staggerAngle)
            {
                var ceX = my_parameter.center.X - my_parameter.eye.X;
                var ceY = my_parameter.center.Y - my_parameter.eye.Y;
                var distanceCE = Math.Sqrt(ceX * ceX + ceY * ceY);
                var diff = new TriTuple(distanceCE, 0, my_parameter.center.Z - my_parameter.eye.Z); 
                var cos = Math.Cos(staggerAngle);
                var sin = Math.Sin(staggerAngle);
                var centerDiff = new TriTuple(diff.X * cos - diff.Z * sin,
                    0,
                    diff.X * sin + diff.Z * cos);
                my_parameter.center.Z = my_parameter.eye.Z + centerDiff.Z;
                var percent = centerDiff.X / distanceCE;
                my_parameter.center.X = my_parameter.eye.X + percent * ceX;
                my_parameter.center.Y = my_parameter.eye.Y + percent * ceY;
            }
            public void SetLook2D(OpenGL gl, CameraParament cameraParament)
            {

                gl.LookAt(cameraParament.eye.X, cameraParament.eye.Y, cameraParament.eye.Z,
                    cameraParament.center.X, cameraParament.center.Y, cameraParament.center.Z,
                    cameraParament.up.X, cameraParament.up.Y, cameraParament.up.Z);
            }
            public void SetLook3D(OpenGL gl)
            {
                gl.LookAt(my_parameter.eye.X, my_parameter.eye.Y, my_parameter.eye.Z,
                    my_parameter.center.X, my_parameter.center.Y, my_parameter.center.Z,
                    my_parameter.up.X, my_parameter.up.Y, my_parameter.up.Z);
            }
        }
        #region//三维视角移动隐藏鼠标版
        //public class Vector_compute
        //{
        //    public float x, y, z;
        //    public Vector_compute()
        //    {
        //        x = 0;
        //        y = 0;
        //        z = 0;
        //    }
        //    public Vector_compute(float x, float y, float z)
        //    {
        //        this.x = x;
        //        this.y = y;
        //        this.z = z;
        //    }
        //    //向量归一化，最简单的方法就是就直接除以向量的模长
        //    public Vector_compute normalize()
        //    {
        //        double d = Math.Sqrt(Convert.ToDouble(x * x + y * y + z * z));
        //        float length = Convert.ToSingle(d);
        //        if (length == 0)
        //        {
        //            length = 1;
        //        }
        //        x = x / length;
        //        y = y / length;
        //        z = z / length;
        //        return this;
        //    }
        //    //向量之间做叉乘操作
        //    public Vector_compute crossProduct(Vector_compute vec)
        //    {
        //        Vector_compute new_vec = new Vector_compute();
        //        new_vec.x = y * vec.z - z * vec.y;
        //        new_vec.y = z * vec.x - x * vec.z;
        //        new_vec.z = x * vec.y - y * vec.x;
        //        return new_vec;
        //    }

        //    //定义两个向量相加的操作
        //    public static Vector_compute operator +(Vector_compute v1, Vector_compute v2)
        //    {
        //        var res = new Vector_compute();
        //        res.x = v1.x + v2.x;
        //        res.y = v1.y + v2.y;
        //        res.z = v1.z + v2.z;
        //        return res;
        //    }
        //    //定义两个向量相减的操作
        //    public static Vector_compute operator -(Vector_compute v1, Vector_compute v2)
        //    {
        //        var res = new Vector_compute();
        //        res.x = v1.x - v2.x;
        //        res.y = v1.y - v2.y;
        //        res.z = v1.z - v2.z;
        //        return res;
        //    }
        //}
        //public class Camera
        //{
        //    //本类中所有有关x,y,z的变量声明和注释，均以世界坐标系为准。
        //    //水平向右为X正方向，垂直向下为Y轴正方向，垂直于显示器屏幕向外为Z轴正方向
        //    private camera_parameter my_parameter;
        //    //位置
        //    private Vector_compute my_position;
        //    //朝向  
        //    private Vector_compute my_view;
        //    //向上向量
        //    private Vector_compute my_upVector;
        //    //速度
        //    private float my_speed;
        //    //构造一个函数用来初始化参数
        //    public Camera()
        //    {
        //        Vector_compute zero = new Vector_compute(0f, 0f, 30f);
        //        Vector_compute view = new Vector_compute(0f, 0f, 0f);
        //        Vector_compute up = new Vector_compute(1f, 0f, 0f);
        //        my_position = zero;
        //        my_view = view;
        //        my_upVector = up;
        //        my_speed = 0.1f;
        //    }
        //    public float getSpeed()
        //    {
        //        return my_speed;
        //    }

        //    //旋转摄像机
        //    public void rotateView(float angle, float x, float y, float z)
        //    {
        //        Vector_compute newView = new Vector_compute();
        //        //计算方向向量
        //        Vector_compute view = my_view - my_position;
        //        //计算 sin 和cos值
        //        float cosTheta = (float)Math.Cos(angle);
        //        float sinTheta = (float)Math.Sin(angle);

        //        //计算旋转向量的x值
        //        newView.x = (cosTheta + (1 - cosTheta) * x * x) * view.x;
        //        newView.x = newView.x + ((1 - cosTheta) * x * y - z * sinTheta) * view.y;
        //        newView.x = newView.x + ((1 - cosTheta) * x * z + y * sinTheta) * view.z;

        //        //计算旋转向量的y值
        //        newView.y = ((1 - cosTheta) * x * y + z * sinTheta) * view.x;
        //        newView.y = newView.y + (cosTheta + (1 - cosTheta) * y * y) * view.y;
        //        newView.y = newView.y + ((1 - cosTheta) * y * z - x * sinTheta) * view.z;

        //        //计算旋转向量的y值
        //        newView.z = ((1 - cosTheta) * x * z - y * sinTheta) * view.x;
        //        newView.z = newView.z + ((1 - cosTheta) * y * z + x * sinTheta) * view.y;
        //        newView.z = newView.z + (cosTheta + (1 - cosTheta) * z * z) * view.z;

        //        //更新摄像机的方向
        //        my_view = my_position + newView;
        //    }

        //    //显示或者隐藏光标
        //    [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        //    public extern static int ShowCursor(int show);

        //    //获得光标位置
        //    [DllImport("user32.dll", CharSet = CharSet.Auto)]
        //    public static extern bool GetCursorPos(out Point pt);

        //    //设置光标位置
        //    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        //    private static extern int SetCursorPos(int x, int y);

        //    // 用于保存旋转角度
        //    float lastRotY;

        //    float currentRotY;

        //    public void setViewByMouse(int middleX, int middleY)
        //    {

        //        //声明鼠标坐标
        //        Point mouse_position;
        //        //声明摄像机旋转角度 
        //        float angle_X;
        //        float angle_Y;
        //        // 得到当前鼠标位置
        //        GetCursorPos(out mouse_position);
        //        // 设置鼠标位置在屏幕中心（如果不加这一句就会不停的在X方向上旋转）           
        //        SetCursorPos(middleX, middleY);
        //        // 得到鼠标移动方向
        //        angle_X = (float)((middleX - mouse_position.X)) / 1000.0f;
        //        angle_Y = (float)((middleY - mouse_position.Y)) / 1000.0f;
        //        lastRotY = currentRotY;
        //        // 跟踪摄像机上下旋转角度
        //        currentRotY = currentRotY + angle_Y;
        //        // 如果上下旋转弧度大于1.0,我们截取到1.0并旋转
        //        if (currentRotY > 1.0f)
        //        {
        //            currentRotY = 1.0f;
        //            // 根据保存的角度旋转方向
        //            if (lastRotY != 1.0f)
        //            {
        //                // 通过叉积找到与旋转方向垂直的向量
        //                Vector_compute vAxis = my_view - my_position;
        //                vAxis = vAxis.crossProduct(my_upVector);
        //                vAxis = vAxis.normalize();
        //                ///旋转
        //                rotateView(1.0f - lastRotY, vAxis.x, vAxis.y, vAxis.z);
        //            }
        //        }
        //        // 如果旋转弧度小于-1.0,则也截取到-1.0并旋转
        //        else if (currentRotY < -1.0f)
        //        {
        //            currentRotY = -1.0f;
        //            if (lastRotY != -1.0f)
        //            {
        //                // 通过叉积找到与旋转方向垂直的向量
        //                Vector_compute vAxis = my_view - my_position;
        //                vAxis = vAxis.crossProduct(my_upVector);
        //                vAxis = vAxis.normalize();
        //                rotateView(-1.0f - lastRotY, vAxis.x, vAxis.y, vAxis.z);
        //            }
        //        }
        //        // 否则就旋转angleY度
        //        else
        //        {
        //            // 找到与旋转方向垂直向量
        //            Vector_compute vAxis = my_view - my_position;
        //            vAxis = vAxis.crossProduct(my_upVector);
        //            vAxis = vAxis.normalize();
        //            rotateView(angle_Y, vAxis.x, vAxis.y, vAxis.z);
        //        }
        //        //总是左右旋转摄像机
        //        //因为这里的摄像机位置是X轴正方向，Z轴正方向，Y轴为0
        //        //参数为（1，0，0）时，摄像机顶部指向Z轴，在ZY轴平面旋转
        //        //参数为（0，1，0）时，摄像机顶部指向Y轴，在XZ轴平面旋转
        //        //参数为（0，0，1）时，摄像机顶部指向Z轴，在XY轴平面旋转
        //        rotateView(angle_X, 0, 0, 1);
        //    }

        //    //按A或D键向左或者向右
        //    public void move_X_Camera(float speed)
        //    {
        //        Vector_compute move_X = new Vector_compute();
        //        Vector_compute cross = my_view - my_position;
        //        cross = cross.crossProduct(my_upVector);
        //        //归一化向量
        //        move_X = cross.normalize();
        //        //这里的x,y都得加上，这样不管朝向哪个方向都可以前进或者后退
        //        //加上z不可以朝下前进或者朝上前进，暂时搁置
        //        //根据速度更新摄像机的位置
        //        my_position.x = my_position.x + move_X.x * speed;
        //        my_position.y = my_position.y + move_X.y * speed;
        //        //根据速度更新摄像机的视点位置
        //        my_view.x = my_view.x + move_X.x * speed;
        //        my_view.y = my_view.y + move_X.y * speed;
        //    }
        //    //按W或S键前进或者后退
        //    public void move_Y_Camera(float speed)
        //    {
        //        //计算方向向量
        //        Vector_compute vector = my_view - my_position;
        //        //单位化
        //        vector = vector.normalize();
        //        //这里的x,y都得加上，这样不管朝向哪个方向都可以向左或者向右
        //        //根据速度更新摄像机的位置
        //        my_position.x = my_position.x + vector.x * speed;
        //        my_position.y = my_position.y + vector.y * speed;
        //        //根据速度更新摄像机的视点位置
        //        my_view.x = my_view.x + vector.x * speed;
        //        my_view.y = my_view.y + vector.y * speed;
        //    }
        //    public void move_Z_Camera(float speed)
        //    {

        //        my_position.z = my_position.z + speed;
        //        my_view.z = my_view.z + speed;
        //    }
        //    public camera_parameter GetCamera_Parameter()
        //    {
        //        my_parameter.position = my_position;
        //        my_parameter.view = my_view;
        //        my_parameter.upVector = my_upVector;
        //        return my_parameter;
        //    }
        //    public void SetCamera_Parameter(
        //        float my_position_x, float my_position_y, float my_position_z,
        //            float my_view_x, float my_view_y, float my_view_z,
        //            float my_upVector_x, float my_upVector_y, float my_upVector_z)
        //    {
        //        my_parameter.position.x = my_position_x;
        //        my_parameter.position.y = my_position_y;
        //        my_parameter.position.z = my_position_z;
        //        my_parameter.view.x = my_view_x;
        //        my_parameter.view.y = my_view_y;
        //        my_parameter.view.z = my_view_z;
        //        my_parameter.upVector.x = my_upVector_x;
        //        my_parameter.upVector.y = my_upVector_y;
        //        my_parameter.upVector.z = my_upVector_z;
        //    }

        //    public void setLook2D(OpenGL gl, float my_position_x, float my_position_y, float my_position_z,
        //            float my_view_x, float my_view_y, float my_view_z,
        //            float my_upVector_x, float my_upVector_y, float my_upVector_z)
        //    {
        //        gl.LookAt(my_position_x, my_position_y, my_position_z,
        //            my_view_x, my_view_y, my_view_z,
        //            my_upVector_x, my_upVector_y, my_upVector_z);
        //    }
        //    //由于旋转方法和获取并设置鼠标的方法相互调用，导致在三维模式中Camera类中的参数只在这个类中改变
        //    //因从三维模式这个设置摄像机的方法不能具有传入参数
        //    public void setLook3D(OpenGL gl)
        //    {
        //        gl.LookAt(my_position.x, my_position.y, my_position.z
        //            , my_view.x, my_view.y, my_view.z
        //            , my_upVector.x, my_upVector.y, my_upVector.z);
        //    }
        //}
        #endregion

    }
}
