using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using 城市规划管理系统.BLL;
using static 城市规划管理系统.BLL.DataEntity;
using System.Web;

namespace 城市规划管理系统.DAL
{
    public class DataManageSQL
    {
        public bool YesOrNo = false;
        public static string mydbs = "Data Source=(local);Initial Catalog=城市规划管理系统;Integrated Security=True";
        public string mycom;
        public string mycom1;
        public string mycom4;
        public string mycom5;
        public static string table_name;
        public static string column_name;        
        public static string target_name;

        
        //设置参数
        public void Parameter_Set(string tableName, string columnName, string targetName)
        {
            table_name = tableName;
            column_name = columnName;
            target_name = targetName;
            mycom = "select * from " + table_name + " where " + column_name + "='" + target_name + "'";//where='要查询的项'就是要加单引号
            mycom1 = "select * from " + table_name + " order by " + column_name + "";
            mycom4 = "Delete from " + table_name + " where " + column_name + "='" + target_name + "'";//where='要查询的项'就是要加单引号
            mycom5 = "Delete from " + table_name + "";
        }
        public void Parameter_Clear()
        {
            table_name = null;
            column_name = null;
            target_name = null;
            mycom = null;
        }
    }
    public class DataInquireSQL : DataManageSQL
    {            
        public DataSet Inquire_Loading(string tableName, string columnName, string targetName)
        {
            Parameter_Set(tableName, columnName, targetName);
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            SqlCommand cmd = new SqlCommand(mycom1, con); //执行命令不一样          
            SqlDataAdapter sda = new SqlDataAdapter(mycom1, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            return ds;
        }
        public DataSet Inquire(string tableName, string columnName, string targetName)
        {
            Parameter_Set(tableName, columnName, targetName);
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            SqlCommand cmd = new SqlCommand(mycom, con);//执行命令不一样
            SqlDataAdapter sda = new SqlDataAdapter(mycom, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            return ds;
        }
        public int Inquire_ModelNumber(string tableName, string columnName, string targetName)
        {
            Parameter_Set(tableName, columnName, targetName);
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            SqlCommand cmd = new SqlCommand(mycom1, con); //执行命令不一样          
            SqlDataAdapter sda = new SqlDataAdapter(mycom1, con);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            int model_number = ds.Tables[0].Rows.Count;
            return model_number;
        }
    }

    public class DataAddSQL : DataManageSQL
    {
        public bool Add_User(long userAccount, string userPassword, string eMail, string userName, string userSex, int userAge, DateTime userBrithday)
        {
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            string mycom2 = "insert into 账户信息 values('" + userAccount + "','" + userPassword + "','" + eMail + "','" + userName + "','" + userSex + "','" + userAge + "','" + userBrithday + "')";
            SqlCommand cmd = new SqlCommand(mycom2, con);
            int i = cmd.ExecuteNonQuery();
            if(i != 0)
            {
                YesOrNo = true;
            }

            return YesOrNo;
        }
        public bool Add_ModelOverview(int data_index, string data_name, string data_type)
        {
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            string mycom2 = "insert into 数据总览 values('" + data_index + "','" + data_name + "','" + data_type + "')";
            SqlCommand cmd = new SqlCommand(mycom2, con);
            int i = cmd.ExecuteNonQuery();
            if (i != 0)
            {
                YesOrNo = true;
            }
            return YesOrNo;
        }
        public bool Add_TIN(int data_index, TIN tin)
        {
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            string centre_point = null;
            centre_point = "(" + tin.centre_point.point_x.ToString("0.000") + " " + tin.centre_point.point_y.ToString("0.000") + " " + tin.centre_point.point_z.ToString("0.000") + ")";
            string point_TIN = null;
            for(int n = 0;n < tin.point_TIN.Count; n++)
            {
                string point_item = "(" + tin.point_TIN[n].point_x.ToString("0.000") + " " + tin.point_TIN[n].point_y.ToString("0.000") + " " + tin.point_TIN[n].point_z.ToString("0.000") + ")";
                if(n != tin.point_TIN.Count - 1)
                {
                    point_item = point_item + ", ";
                }
                point_TIN = point_TIN + point_item;
            }
            string triangle_TIN = null;
            for(int n = 0;n < tin.triangle_NET.Count; n++)
            {
                string triangle_V1 = tin.triangle_NET[n].V1.point_x.ToString("0.000") + " " + tin.triangle_NET[n].V1.point_y.ToString("0.000") + " " + tin.triangle_NET[n].V1.point_z.ToString("0.000");
                string triangle_V2 = tin.triangle_NET[n].V2.point_x.ToString("0.000") + " " + tin.triangle_NET[n].V2.point_y.ToString("0.000") + " " + tin.triangle_NET[n].V2.point_z.ToString("0.000");
                string triangle_V3 = tin.triangle_NET[n].V3.point_x.ToString("0.000") + " " + tin.triangle_NET[n].V3.point_y.ToString("0.000") + " " + tin.triangle_NET[n].V3.point_z.ToString("0.000");
                string triangle_item = "((" + triangle_V1 + "," + triangle_V2 + "," + triangle_V3 + "," + triangle_V1 + "))";
                if(n != tin.triangle_NET.Count - 1)
                {
                    triangle_item = triangle_item + ", ";
                }
                triangle_TIN = triangle_TIN + triangle_item;
            }
            string mycom2 = "insert into TIN values('" + data_index + "','" + tin.TIN_name + "',GEOMETRY::Parse('POINT"+ centre_point +"') ,GEOMETRY::Parse('MULTIPOINT("+ point_TIN + ")'), GEOMETRY::Parse('MULTIPOLYGON(" + triangle_TIN + ")'))";
            SqlCommand cmd = new SqlCommand(mycom2, con);
            int i = cmd.ExecuteNonQuery();
            if (i != 0)
            {
                YesOrNo = true;
            }
            return YesOrNo;
        }
        public bool Add_Build(int data_index, Build<object> build)
        {
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            string centre_point = null;
            centre_point = "(" + build.centre_point.point_x.ToString("0.000") + " " + build.centre_point.point_y.ToString("0.000") + " " + build.centre_point.point_z.ToString("0.000") + ")";
            string points_Build = null;
            for (int n = 0; n < build.points_Build.Count; n++)
            {
                string point_item = "(" + build.points_Build[n].point_x.ToString("0.000") + " " + build.points_Build[n].point_y.ToString("0.000") + " " + build.points_Build[n].point_z.ToString("0.000") + ")";
                if (n != build.points_Build.Count - 1)
                {
                    point_item = point_item + ", ";
                }
                points_Build = points_Build + point_item;
            }
            string Build_item = null;
            string Build_height = null;
            if (build.build_type == "Build_1")
            {
                string down_a = (build.build_item as Build<object>.Build_1).main_body.down_a.point_x.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_a.point_y.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_a.point_z.ToString("0.000");
                string down_b = (build.build_item as Build<object>.Build_1).main_body.down_b.point_x.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_b.point_y.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_b.point_z.ToString("0.000");
                string down_c = (build.build_item as Build<object>.Build_1).main_body.down_c.point_x.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_c.point_y.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_c.point_z.ToString("0.000");
                string down_d = (build.build_item as Build<object>.Build_1).main_body.down_d.point_x.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_d.point_y.ToString("0.000") + " " + (build.build_item as Build<object>.Build_1).main_body.down_d.point_z.ToString("0.000");
                Build_item = "((" + down_a + "," + down_b + "," + down_d + "," + down_c + "," + down_a + "))";
                Build_height = (build.build_item as Build<object>.Build_1).main_body.height.ToString("0.000");
            }            
            string mycom2 = "insert into Build values('" + data_index + "','" + build.build_name + "','" + build.build_type + "',GEOMETRY::Parse('POINT" + centre_point + "') ,'" + build.length + "','" + build.wide + "','" + build.height + "',GEOMETRY::Parse('MULTIPOINT(" + points_Build + ")'), GEOMETRY::Parse('MULTIPOLYGON(" + Build_item + ")'),'" + Build_height + "')";
            SqlCommand cmd = new SqlCommand(mycom2, con);
            int i = cmd.ExecuteNonQuery();
            if (i != 0)
            {
                YesOrNo = true;
            }
            return YesOrNo;
        }
        public bool Add_Road(int data_index, Road<object> road)
        {
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            string centre_point = null;
            centre_point = "(" + road.centre_point.point_x.ToString("0.000") + " " + road.centre_point.point_y.ToString("0.000") + " " + road.centre_point.point_z.ToString("0.000") + ")";
            string points_Road = null;
            for (int n = 0; n < road.points_Road.Count; n++)
            {
                string point_item = "(" + road.points_Road[n].point_x.ToString("0.000") + " " + road.points_Road[n].point_y.ToString("0.000") + " " + road.points_Road[n].point_z.ToString("0.000") + ")";
                if (n != road.points_Road.Count - 1)
                {
                    point_item = point_item + ", ";
                }
                points_Road = points_Road + point_item;
            }
            string Road_item = null;
            if (road.road_type == "Road_1")
            {
                string a_start = (road.road_item as Road<object>.Road_1).a.point_begin.point_x.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).a.point_begin.point_y.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).a.point_begin.point_z.ToString("0.000");
                string a_end = (road.road_item as Road<object>.Road_1).a.point_end.point_x.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).a.point_end.point_y.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).a.point_end.point_z.ToString("0.000");
                string b_start = (road.road_item as Road<object>.Road_1).b.point_begin.point_x.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).b.point_begin.point_y.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).b.point_begin.point_z.ToString("0.000");
                string b_end = (road.road_item as Road<object>.Road_1).b.point_end.point_x.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).b.point_end.point_y.ToString("0.000") + " " + (road.road_item as Road<object>.Road_1).b.point_end.point_z.ToString("0.000");
                string line_a = "(" + a_start + "," + a_end +")";
                string line_b = "(" + b_start + "," + b_end +")";
                Road_item = line_a + "," + line_b;
            }
            string mycom2 = "insert into Road values('" + data_index + "','" + road.road_name + "','" + road.road_type + "',GEOMETRY::Parse('POINT" + centre_point + "') ,'" + road.length + "','" + road.wide + "','" + road.height + "',GEOMETRY::Parse('MULTIPOINT(" + points_Road + ")'), GEOMETRY::Parse('MULTILINESTRING(" + Road_item + ")'))";
            SqlCommand cmd = new SqlCommand(mycom2, con);
            int i = cmd.ExecuteNonQuery();
            if (i != 0)
            {
                YesOrNo = true;
            }
            return YesOrNo;
        }
    }
    public class DataUpdataSQL : DataManageSQL
    {
        public bool Updata_User(long userAccount, string userPassword, string eMail, string userName, string userSex, int userAge, DateTime userBrithday, string targetAccount)
        {
            string mycom3 = "Update 账户信息 set 账号='" + userAccount + "',密码='" + userPassword + "',邮箱='" + eMail + "',姓名='" + userName + "',性别='" + userSex + "',年龄='" + userAge + "',生日='" + userBrithday + "' where 账号='" + targetAccount + "'";
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            SqlCommand cmd = new SqlCommand(mycom3, con);
            var result = cmd.ExecuteNonQuery();
            if (result != 0)
            {
                YesOrNo = true;
            }
            else
            {
                YesOrNo = false;
            }
            con.Close();

            return YesOrNo;
        }

    }
    public class DataDeleteSQL : DataManageSQL
    {
        public bool Delete_User(string tableName, string columnName, string targetName)
        {
            Parameter_Set(tableName, columnName, targetName);
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();          
            SqlCommand cmd = new SqlCommand(mycom4, con);
            var result = cmd.ExecuteNonQuery();
            if (result != 0)
            {
                YesOrNo = true;
            }
            else
            {
                YesOrNo = false;
            }
            con.Close();
            return YesOrNo;
        }
        public bool Delete_AllModel(string tableName, string columnName, string targetName)
        {
            Parameter_Set(tableName, columnName, targetName);
            SqlConnection con = new SqlConnection(mydbs);
            con.Open();
            SqlCommand cmd = new SqlCommand(mycom5, con);
            var result = cmd.ExecuteNonQuery();
            if (result != 0)
            {
                YesOrNo = true;
            }
            else
            {
                YesOrNo = false;
            }
            con.Close();
            return YesOrNo;
        }
    }
}
