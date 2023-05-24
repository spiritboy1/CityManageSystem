using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 城市规划管理系统.BLL
{
    public class LightManage
    {
        //声明光源参数
        // 光源位置
        float[] lightPosition;
        // 环境光参数
        float[] lightAmbient;
        // 漫射光参数
        float[] lightDiffuse;
        //镜面反射
        float[] lightSpecular;
        public LightManage()
        {
            lightPosition = new float[4] { 0.0f, 0.0f, 0.0f, 1.0f }; 
            lightAmbient = new float[4] { 0.0f, 0.0f, 0.0f, 1.0f };
            lightDiffuse = new float[4] { 1f, 1f, 1f, 1f };
            lightSpecular = new float[4] { 1f, 1f, 1f, 1f }; 
        }
        
        public class LightControl : LightManage
        {
            public LightControl()
            {

            }
            public void SetLight(OpenGL gl)
            {
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, lightAmbient);
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, lightDiffuse);
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, lightPosition);
                gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, lightSpecular);
                //开启光照
                gl.Enable(OpenGL.GL_LIGHTING);
                gl.Enable(OpenGL.GL_LIGHT0);
                gl.Enable(OpenGL.GL_TEXTURE_2D);
                gl.Enable(OpenGL.GL_NORMALIZE);
            }
        }

    }
}
