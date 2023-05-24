
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static 城市规划管理系统.BLL.DataEntity;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Quadrics;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;
using System.Drawing;
using SharpGL.SceneGraph.Lighting;
using System.Drawing.Imaging;

namespace 城市规划管理系统.BLL
{
    public class DataDraw
    {
        public DataDraw()
        {

        }
        public void Draw_SkyBox(OpenGL gl, Cube skybox, Texture[] skybox_texture)
        {
            //启用纹理映射
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Color(1f, 1f, 1f);
            //绘制立方体并绑定纹理
            //前
            skybox_texture[0].Bind(gl);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(1, 1, 1);
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(skybox.up_c.point_x, skybox.up_c.point_y, -skybox.up_c.point_z);
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(skybox.down_c.point_x, skybox.down_c.point_y, -skybox.down_c.point_z);
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(skybox.down_d.point_x, skybox.down_d.point_y, -skybox.down_d.point_z);
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(skybox.up_d.point_x, skybox.up_d.point_y, -skybox.up_d.point_z);
            }
            gl.End();
            //后
            skybox_texture[1].Bind(gl);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(1, 1, 1);
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(skybox.up_a.point_x, skybox.up_a.point_y, -skybox.up_a.point_z);
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(skybox.down_a.point_x, skybox.down_a.point_y, -skybox.down_a.point_z);
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(skybox.down_b.point_x, skybox.down_b.point_y, -skybox.down_b.point_z);
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(skybox.up_b.point_x, skybox.up_b.point_y, -skybox.up_b.point_z);
            }
            gl.End();
            //左
            skybox_texture[2].Bind(gl);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(1, 1, 1);
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(skybox.up_a.point_x, skybox.up_a.point_y, -skybox.up_a.point_z);
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(skybox.down_a.point_x, skybox.down_a.point_y, -skybox.down_a.point_z);
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(skybox.down_c.point_x, skybox.down_c.point_y, -skybox.down_c.point_z);
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(skybox.up_c.point_x, skybox.up_c.point_y, -skybox.up_c.point_z);
            }
            gl.End();
            //右
            skybox_texture[3].Bind(gl);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(1, 1, 1);
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(skybox.up_d.point_x, skybox.up_d.point_y, -skybox.up_d.point_z);
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(skybox.down_d.point_x, skybox.down_d.point_y, -skybox.down_d.point_z);
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(skybox.down_b.point_x, skybox.down_b.point_y, -skybox.down_b.point_z);
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(skybox.up_b.point_x, skybox.up_b.point_y, -skybox.up_b.point_z);
            }
            gl.End();
            //上
            skybox_texture[4].Bind(gl);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(1, 1, 1);
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(skybox.up_a.point_x, skybox.up_a.point_y, -skybox.up_a.point_z);
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(skybox.up_c.point_x, skybox.up_c.point_y, -skybox.up_c.point_z);
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(skybox.up_d.point_x, skybox.up_d.point_y, -skybox.up_d.point_z);
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(skybox.up_b.point_x, skybox.up_b.point_y, -skybox.up_b.point_z);
            }
            gl.End();
            //下
            skybox_texture[5].Bind(gl);
            gl.Begin(OpenGL.GL_QUADS);
            gl.Normal(1, 1, 1);
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(skybox.down_a.point_x, skybox.down_a.point_y, -skybox.down_a.point_z);
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(skybox.down_c.point_x, skybox.down_c.point_y, -skybox.down_c.point_z);
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(skybox.down_d.point_x, skybox.down_d.point_y, -skybox.down_d.point_z);
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(skybox.down_b.point_x, skybox.down_b.point_y, -skybox.down_b.point_z);
            }
            gl.End();
        }
        public void Draw_BaseGrid(OpenGL gl)
        {
            //关闭纹理映射
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            //绘制过程
            gl.Translate(0.0f, 0.0f, 0.0f);
            gl.PushAttrib(OpenGL.GL_CURRENT_BIT);  //保存当前属性
            gl.PushMatrix();//压入堆栈
            gl.LineWidth(1.0f);
            gl.Color(1.0f, 1.0f, 1.0f);
            //在X,Y平面上绘制网格
            for (int i = -10000; i <= 10000; i = i + 10)
            {
                //绘制线
                gl.Begin(OpenGL.GL_LINES);
                {
                    //Y轴方向
                    gl.Vertex(-10000.0f, i * 1.0f, 0.0f);
                    gl.Vertex(10000.0f, i * 1.0f, 0.0f);
                    //X轴方向
                    gl.Vertex(i * 1.0f, -10000.0f, 0.0f);
                    gl.Vertex(i * 1.0f, 10000.0f, 0.0f);
                }
                gl.End();
            }            
            gl.PopMatrix();
            gl.PopAttrib();
        }
        public void Draw_Axis(OpenGL gl)
        {
            //关闭纹理映射
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            //绘制轴线
            gl.Translate(0.0f, 0.0f, 0.0f);
            gl.Color(1f, 0f, 0f);
            gl.LineWidth(3.0f);
            gl.Begin(OpenGL.GL_LINES);
            {
                gl.Vertex(-10000.0f, 0.0f, 0.0f);
                gl.Vertex(10000.0f, 0.0f, 0.0f);
            }
            gl.End();
            gl.Color(0f, 0f, 1f);
            gl.Begin(OpenGL.GL_LINES);
            {
                gl.Vertex(0.0f, -10000.0f, 0.0f);
                gl.Vertex(0.0f, 10000.0f, 0.0f);
            }            
            gl.End();
        }
        public void Draw_TIN(OpenGL gl, triangle_3D item_TIN, double height_correction)
        {
            //启用纹理映射
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Color(1f, 1f, 1f);
            if (item_TIN.texture_index != -1)
            {
                ModelManage.TextureControl.terrain_textures[item_TIN.texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_TRIANGLES);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(item_TIN.V1.point_x, item_TIN.V1.point_y, -(item_TIN.V1.point_z));
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(item_TIN.V2.point_x, item_TIN.V2.point_y, -(item_TIN.V2.point_z));
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(item_TIN.V3.point_x, item_TIN.V3.point_y, -(item_TIN.V3.point_z));
            }
            gl.End();
            gl.Color(1f, 1f, 1f);
        }
        public void Draw_Build_1(OpenGL gl, Build<object>.Build_1 build_1, double height_correction)
        {
            //启用纹理映射
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Color(1f, 1f, 1f);
            //前后左右同一纹理
            //前面
            if (build_1.main_body.round_texture_index != -1)
            {
                ModelManage.TextureControl.build_textures[build_1.main_body.round_texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(build_1.main_body.up_c.point_x, build_1.main_body.up_c.point_y, -(build_1.main_body.up_c.point_z - height_correction));
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(build_1.main_body.down_c.point_x, build_1.main_body.down_c.point_y, -(build_1.main_body.down_c.point_z - height_correction));
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(build_1.main_body.down_d.point_x, build_1.main_body.down_d.point_y, -(build_1.main_body.down_d.point_z - height_correction));
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(build_1.main_body.up_d.point_x, build_1.main_body.up_d.point_y, -(build_1.main_body.up_d.point_z - height_correction));
            }
            gl.End();
            //后面
            if (build_1.main_body.round_texture_index != -1)
            {
                ModelManage.TextureControl.build_textures[build_1.main_body.round_texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(build_1.main_body.up_a.point_x, build_1.main_body.up_a.point_y, -(build_1.main_body.up_a.point_z - height_correction));
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(build_1.main_body.down_a.point_x, build_1.main_body.down_a.point_y, -(build_1.main_body.down_a.point_z - height_correction));
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(build_1.main_body.down_b.point_x, build_1.main_body.down_b.point_y, -(build_1.main_body.down_b.point_z - height_correction));
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(build_1.main_body.up_b.point_x, build_1.main_body.up_b.point_y, -(build_1.main_body.up_b.point_z - height_correction));
            }
            gl.End();

            //左面
            if (build_1.main_body.round_texture_index != -1)
            {
                ModelManage.TextureControl.build_textures[build_1.main_body.round_texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(build_1.main_body.up_a.point_x, build_1.main_body.up_a.point_y, -(build_1.main_body.up_a.point_z - height_correction));
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(build_1.main_body.down_a.point_x, build_1.main_body.down_a.point_y, -(build_1.main_body.down_a.point_z - height_correction));
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(build_1.main_body.down_c.point_x, build_1.main_body.down_c.point_y, -(build_1.main_body.down_c.point_z - height_correction));
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(build_1.main_body.up_c.point_x, build_1.main_body.up_c.point_y, -(build_1.main_body.up_c.point_z - height_correction));
            }
            gl.End();

            //右面
            if (build_1.main_body.round_texture_index != -1)
            {
                ModelManage.TextureControl.build_textures[build_1.main_body.round_texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(build_1.main_body.up_d.point_x, build_1.main_body.up_d.point_y, -(build_1.main_body.up_d.point_z - height_correction));
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(build_1.main_body.down_d.point_x, build_1.main_body.down_d.point_y, -(build_1.main_body.down_d.point_z - height_correction));
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(build_1.main_body.down_b.point_x, build_1.main_body.down_b.point_y, -(build_1.main_body.down_b.point_z - height_correction));
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(build_1.main_body.up_b.point_x, build_1.main_body.up_b.point_y, -(build_1.main_body.up_b.point_z - height_correction));
            }
            gl.End();

            //上面单独固定纹理
            //上面
            if (build_1.main_body.top_texture_index != -1)
            {
                ModelManage.TextureControl.build_textures[build_1.main_body.top_texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }                  
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(build_1.main_body.up_a.point_x, build_1.main_body.up_a.point_y, -(build_1.main_body.up_a.point_z - height_correction));
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(build_1.main_body.up_c.point_x, build_1.main_body.up_c.point_y, -(build_1.main_body.up_c.point_z - height_correction));
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(build_1.main_body.up_d.point_x, build_1.main_body.up_d.point_y, -(build_1.main_body.up_d.point_z - height_correction));
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(build_1.main_body.up_b.point_x, build_1.main_body.up_b.point_y, -(build_1.main_body.up_b.point_z - height_correction));
            }
            gl.End();

            //下面不绘制纹理
            //下面
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(build_1.main_body.down_a.point_x, build_1.main_body.down_a.point_y, -(build_1.main_body.down_a.point_z - height_correction));
                gl.Vertex(build_1.main_body.down_c.point_x, build_1.main_body.down_c.point_y, -(build_1.main_body.down_c.point_z - height_correction));
                gl.Vertex(build_1.main_body.down_d.point_x, build_1.main_body.down_d.point_y, -(build_1.main_body.down_d.point_z - height_correction));
                gl.Vertex(build_1.main_body.down_b.point_x, build_1.main_body.down_b.point_y, -(build_1.main_body.down_b.point_z - height_correction));
            }
            gl.End();
        }
        public void Draw_Road_1(OpenGL gl, Road<object>.Road_1 road_1, double height_correction)
        {
            //启用纹理映射
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Color(1f, 1f, 1f);
            if (road_1.texture_index != -1)
            {
                ModelManage.TextureControl.road_textures[road_1.texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                gl.TexCoord(-10.0, 0.0);
                gl.Vertex(road_1.a.point_begin.point_x, road_1.a.point_begin.point_y, -(road_1.a.point_begin.point_z - height_correction));
                gl.TexCoord(-10.0, 1.0);
                gl.Vertex(road_1.b.point_begin.point_x, road_1.b.point_begin.point_y, -(road_1.b.point_begin.point_z - height_correction));
                gl.TexCoord(10.0, 1.0);
                gl.Vertex(road_1.b.point_end.point_x, road_1.b.point_end.point_y, -(road_1.b.point_end.point_z - height_correction));
                gl.TexCoord(10.0, 0.0);
                gl.Vertex(road_1.a.point_end.point_x, road_1.a.point_end.point_y, -(road_1.a.point_end.point_z - height_correction));
            }
            gl.End();
        }
        public void Draw_Road_2(OpenGL gl, Road<object>.Road_2 road_2, double height_correction)
        {
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Color(1f, 1f, 1f);
            if (road_2.texture_index != -1)
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                for(int i = 0; i < road_2.curve_a.Count; i++)
                {
                    gl.Vertex(road_2.curve_a[i].point_x, road_2.curve_a[i].point_y, road_2.curve_a[i].point_z);
                }
                for (int i = road_2.curve_b.Count - 1; i > -1; i--)
                {
                    gl.Vertex(road_2.curve_b[i].point_x, road_2.curve_b[i].point_y, road_2.curve_b[i].point_z);
                }
            }
            gl.End();
        }
        public void Draw_ControlPoint(OpenGL gl, List<point_3D> control_point)
        {
            //开启平滑
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            //关闭纹理映射
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PointSize(20.0f);
            gl.Begin(OpenGL.GL_POINTS);
            {
                gl.Color(1f, 0f, 0f);
                for(int i = 0; i < control_point.Count; i++)
                {
                    gl.Vertex(control_point[i].point_x, control_point[i].point_y, control_point[i].point_z);
                }
            }
            gl.End();
        }
        public void Draw_Water(OpenGL gl, Water water)
        {
            //开启平滑
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            //关闭纹理映射
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Color(0.58f, 0.82f, 0.86f);
                for (int i = 0; i < water.points_Water.Count; i++)
                {
                    gl.Vertex(water.points_Water[i].point_x, water.points_Water[i].point_y, -water.points_Water[i].point_z);
                }
            }
            gl.End();
        }
        public void Draw_Tree(OpenGL gl, Tree tree)
        {
            //开启纹理映射
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Color(1f, 1f, 1f);
            if (tree.texture_index != -1)
            {
                ModelManage.TextureControl.tree_textures[tree.texture_index].Bind(gl);
                gl.Begin(OpenGL.GL_QUADS);
                gl.Normal(1, 1, 1);
            }
            else
            {
                gl.Begin(OpenGL.GL_LINE_LOOP);
            }
            {
                gl.TexCoord(0.0, 0.0);
                gl.Vertex(tree.points_Tree[0].point_x, tree.points_Tree[0].point_y, -tree.points_Tree[0].point_z);
                gl.TexCoord(0.0, 1.0);
                gl.Vertex(tree.points_Tree[2].point_x, tree.points_Tree[0].point_y, -tree.points_Tree[2].point_z);
                gl.TexCoord(1.0, 1.0);
                gl.Vertex(tree.points_Tree[3].point_x, tree.points_Tree[3].point_y, -tree.points_Tree[3].point_z);
                gl.TexCoord(1.0, 0.0);
                gl.Vertex(tree.points_Tree[1].point_x, tree.points_Tree[1].point_y, -tree.points_Tree[1].point_z);
            }
            gl.End();
        }
        public void Draw_Lamp(OpenGL gl, Lamp lamp, int scale)
        {
            //从下往上绘制
            gl.Color(0.25f, 0.41f, 0.87f);
            Cube cube = new Cube("foundation", lamp.centre_point, 0.5 * scale, 0.5 * scale, 0.25 * scale);
            Draw_Cube(gl, cube, 0);
            gl.Color(1.0f, 1.0f, 1.0f);
            Draw_Cylinder(gl, new point_3D("lampstandard", lamp.centre_point.point_x, lamp.centre_point.point_y, -(lamp.centre_point.point_z + 4.25 * scale)), 0.125 * scale, 0.125 * scale, 4.25 * scale, 20, 20);
            gl.Color(0.95f, 0.86f, 0.70f);
            Draw_Sphere(gl, new point_3D("bulb", lamp.centre_point.point_x, lamp.centre_point.point_y, -(lamp.centre_point.point_z + 4.5 * scale)), 0.25 * scale, 20, 20);
        }
        private void Draw_Cube(OpenGL gl, Cube cube, double height_correction)
        {
            //前面
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(cube.up_c.point_x, cube.up_c.point_y, -(cube.up_c.point_z - height_correction));
                gl.Vertex(cube.down_c.point_x, cube.down_c.point_y, -(cube.down_c.point_z - height_correction));
                gl.Vertex(cube.down_d.point_x, cube.down_d.point_y, -(cube.down_d.point_z - height_correction));
                gl.Vertex(cube.up_d.point_x, cube.up_d.point_y, -(cube.up_d.point_z - height_correction));
            }
            gl.End();
            //后面
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(cube.up_a.point_x, cube.up_a.point_y, -(cube.up_a.point_z - height_correction));
                gl.Vertex(cube.down_a.point_x, cube.down_a.point_y, -(cube.down_a.point_z - height_correction));
                gl.Vertex(cube.down_b.point_x, cube.down_b.point_y, -(cube.down_b.point_z - height_correction));
                gl.Vertex(cube.up_b.point_x, cube.up_b.point_y, -(cube.up_b.point_z - height_correction));
            }
            gl.End();
            //左面
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(cube.up_a.point_x, cube.up_a.point_y, -(cube.up_a.point_z - height_correction));
                gl.Vertex(cube.down_a.point_x, cube.down_a.point_y, -(cube.down_a.point_z - height_correction));
                gl.Vertex(cube.down_c.point_x, cube.down_c.point_y, -(cube.down_c.point_z - height_correction));
                gl.Vertex(cube.up_c.point_x, cube.up_c.point_y, -(cube.up_c.point_z - height_correction));
            }
            gl.End();
            //右面
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(cube.up_d.point_x, cube.up_d.point_y, -(cube.up_d.point_z - height_correction));
                gl.Vertex(cube.down_d.point_x, cube.down_d.point_y, -(cube.down_d.point_z - height_correction));
                gl.Vertex(cube.down_b.point_x, cube.down_b.point_y, -(cube.down_b.point_z - height_correction));
                gl.Vertex(cube.up_b.point_x, cube.up_b.point_y, -(cube.up_b.point_z - height_correction));
            }
            gl.End();
            //上面
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(cube.up_a.point_x, cube.up_a.point_y, -(cube.up_a.point_z - height_correction));
                gl.Vertex(cube.up_c.point_x, cube.up_c.point_y, -(cube.up_c.point_z - height_correction));
                gl.Vertex(cube.up_d.point_x, cube.up_d.point_y, -(cube.up_d.point_z - height_correction));
                gl.Vertex(cube.up_b.point_x, cube.up_b.point_y, -(cube.up_b.point_z - height_correction));
            }
            gl.End();
            //下面
            gl.Begin(OpenGL.GL_QUADS);
            {
                gl.Vertex(cube.down_a.point_x, cube.down_a.point_y, -(cube.down_a.point_z - height_correction));
                gl.Vertex(cube.down_c.point_x, cube.down_c.point_y, -(cube.down_c.point_z - height_correction));
                gl.Vertex(cube.down_d.point_x, cube.down_d.point_y, -(cube.down_d.point_z - height_correction));
                gl.Vertex(cube.down_b.point_x, cube.down_b.point_y, -(cube.down_b.point_z - height_correction));
            }
            gl.End();
        }
        //参数说明：
        //slices表示球体纵向分割的数目（经线）,主要是设置绘制的细密度，stacks表示球体横向分割的数目（纬线）
        private void Draw_Sphere(OpenGL gl, point_3D centre_point, double radius, int slices, int stacks)
        {
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PushMatrix();
            gl.Translate(centre_point.point_x, centre_point.point_y, centre_point.point_z);
            var sphere = gl.NewQuadric();
            gl.QuadricDrawStyle(sphere, OpenGL.GL_QUADS);
            gl.QuadricNormals(sphere, OpenGL.GLU_NONE);
            gl.QuadricOrientation(sphere, (int)OpenGL.GLU_SMOOTH);
            gl.QuadricTexture(sphere, (int)OpenGL.GLU_FALSE);
            gl.Sphere(sphere, radius, slices, stacks);
            gl.DeleteQuadric(sphere);
            gl.PopMatrix();
        }
        private void Draw_Cylinder(OpenGL gl, point_3D centre_point,double top_radius, double bottom_radius, double height, int slices, int stacks)
        {
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            gl.PushMatrix();
            gl.Translate(centre_point.point_x, centre_point.point_y, centre_point.point_z);
            var cylinder = gl.NewQuadric();
            gl.QuadricDrawStyle(cylinder, OpenGL.GL_QUADS);
            gl.QuadricNormals(cylinder, OpenGL.GLU_SMOOTH);
            gl.QuadricOrientation(cylinder, (int)OpenGL.GLU_OUTSIDE);
            gl.QuadricTexture(cylinder, (int)OpenGL.GLU_FALSE);
            gl.Cylinder(cylinder, bottom_radius, top_radius, height, slices, stacks);
            gl.End();
            gl.PopMatrix();
        }
        //重绘并高亮显示边框和顶点
        //三角形高亮
        public void Highlight_Selected(OpenGL gl, triangle_3D triangle, double height_correction)
        {
            //放大并高亮顶点
            //开启平滑
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            //关闭纹理映射
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            //开启混合（透明度）
            //gl.Enable(OpenGL.GL_BLEND);
            //gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            //gl.Hint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_NICEST);
            //设置点的尺寸要在Begin之前
            gl.PointSize(20.0f);
            gl.Begin(OpenGL.GL_POINTS);
            {
                gl.Color(1f, 0f, 0f);
                gl.Vertex(triangle.V1.point_x, triangle.V1.point_y, -(triangle.V1.point_z - height_correction));
                gl.Vertex(triangle.V2.point_x, triangle.V2.point_y, -(triangle.V2.point_z - height_correction));
                gl.Vertex(triangle.V3.point_x, triangle.V3.point_y, -(triangle.V3.point_z - height_correction));
            }
            gl.End();
            //高亮边框
            gl.Color(0.5f, 1f, 1f);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Vertex(triangle.V1.point_x, triangle.V1.point_y, -(triangle.V1.point_z - height_correction));
                gl.Vertex(triangle.V2.point_x, triangle.V2.point_y, -(triangle.V2.point_z - height_correction));
                gl.Vertex(triangle.V3.point_x, triangle.V3.point_y, -(triangle.V3.point_z - height_correction));
            }
            gl.End();
        }
        //立方体高亮
        public void Highlight_Selected(OpenGL gl, Cube bounding_box)
        {
            //放大并高亮顶点
            //开启平滑
            gl.Enable(OpenGL.GL_POINT_SMOOTH);
            //关闭纹理映射
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            //开启混合（透明度）
            //gl.Enable(OpenGL.GL_BLEND);
            //gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            //gl.Hint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_NICEST);
            //设置点的尺寸要在Begin之前
            gl.PointSize(20.0f);
            gl.Begin(OpenGL.GL_POINTS);
            {
                gl.Color(1f, 0f, 0f);
                //上面
                gl.Vertex(bounding_box.up_a.point_x, bounding_box.up_a.point_y, -bounding_box.up_a.point_z);
                gl.Vertex(bounding_box.up_b.point_x, bounding_box.up_b.point_y, -bounding_box.up_b.point_z);
                gl.Vertex(bounding_box.up_c.point_x, bounding_box.up_c.point_y, -bounding_box.up_c.point_z);
                gl.Vertex(bounding_box.up_d.point_x, bounding_box.up_d.point_y, -bounding_box.up_d.point_z);
                //下面
                gl.Vertex(bounding_box.down_a.point_x, bounding_box.down_a.point_y, -bounding_box.down_a.point_z);
                gl.Vertex(bounding_box.down_b.point_x, bounding_box.down_b.point_y, -bounding_box.down_b.point_z);
                gl.Vertex(bounding_box.down_c.point_x, bounding_box.down_c.point_y, -bounding_box.down_c.point_z);
                gl.Vertex(bounding_box.down_d.point_x, bounding_box.down_d.point_y, -bounding_box.down_d.point_z);
            }
            gl.End();

            //高亮边框
            gl.Color(0.5f, 1f, 1f);
            //gl.LineWidth(3.0f);
            //前面
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Vertex(bounding_box.up_c.point_x, bounding_box.up_c.point_y, -bounding_box.up_c.point_z);
                gl.Vertex(bounding_box.down_c.point_x, bounding_box.down_c.point_y, -bounding_box.down_c.point_z);
                gl.Vertex(bounding_box.down_d.point_x, bounding_box.down_d.point_y, -bounding_box.down_d.point_z);
                gl.Vertex(bounding_box.up_d.point_x, bounding_box.up_d.point_y, -bounding_box.up_d.point_z);
            }
            gl.End();

            //后面
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Vertex(bounding_box.up_a.point_x, bounding_box.up_a.point_y, -bounding_box.up_a.point_z);
                gl.Vertex(bounding_box.down_a.point_x, bounding_box.down_a.point_y, -bounding_box.down_a.point_z);
                gl.Vertex(bounding_box.down_b.point_x, bounding_box.down_b.point_y, -bounding_box.down_b.point_z);
                gl.Vertex(bounding_box.up_b.point_x, bounding_box.up_b.point_y, -bounding_box.up_b.point_z);
            }
            gl.End();

            //左面
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Vertex(bounding_box.up_a.point_x, bounding_box.up_a.point_y, -bounding_box.up_a.point_z);
                gl.Vertex(bounding_box.down_a.point_x, bounding_box.down_a.point_y, -bounding_box.down_a.point_z);
                gl.Vertex(bounding_box.down_c.point_x, bounding_box.down_c.point_y, -bounding_box.down_c.point_z);
                gl.Vertex(bounding_box.up_c.point_x, bounding_box.up_c.point_y, -bounding_box.up_c.point_z);
            }
            gl.End();

            //右面
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Vertex(bounding_box.up_d.point_x, bounding_box.up_d.point_y, -bounding_box.up_d.point_z);
                gl.Vertex(bounding_box.down_d.point_x, bounding_box.down_d.point_y, -bounding_box.down_d.point_z);
                gl.Vertex(bounding_box.down_b.point_x, bounding_box.down_b.point_y, -bounding_box.down_b.point_z);
                gl.Vertex(bounding_box.up_b.point_x, bounding_box.up_b.point_y, -bounding_box.up_b.point_z);
            }
            gl.End();

            //上面
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Vertex(bounding_box.up_a.point_x, bounding_box.up_a.point_y, -bounding_box.up_a.point_z);
                gl.Vertex(bounding_box.up_c.point_x, bounding_box.up_c.point_y, -bounding_box.up_c.point_z);
                gl.Vertex(bounding_box.up_d.point_x, bounding_box.up_d.point_y, -bounding_box.up_d.point_z);
                gl.Vertex(bounding_box.up_b.point_x, bounding_box.up_b.point_y, -bounding_box.up_b.point_z);
            }
            gl.End();

            //下面
            gl.Begin(OpenGL.GL_LINE_LOOP);
            {
                gl.Vertex(bounding_box.down_a.point_x, bounding_box.down_a.point_y, -bounding_box.down_a.point_z);
                gl.Vertex(bounding_box.down_c.point_x, bounding_box.down_c.point_y, -bounding_box.down_c.point_z);
                gl.Vertex(bounding_box.down_d.point_x, bounding_box.down_d.point_y, -bounding_box.down_d.point_z);
                gl.Vertex(bounding_box.down_b.point_x, bounding_box.down_b.point_y, -bounding_box.down_b.point_z);
            }
            gl.End();
        }

    }
}
