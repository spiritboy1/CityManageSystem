using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static 城市规划管理系统.BLL.DataDisplay.CommandDisplay;
using static 城市规划管理系统.BLL.DataEntity;
using static 城市规划管理系统.BLL.ModelManage;

namespace 城市规划管理系统.BLL
{
    public class DataDisplay
    {
        public DataDisplay()
        {

        }
        public class PropertySetting
        {
            //----------------------------------------------基本信息----------------------------------------------
            private string model_name;
            private string model_type;
            private string list_name;
            private int model_index;
            [CategoryAttribute("基本信息"), DefaultValueAttribute("null"), ReadOnlyAttribute(true), DescriptionAttribute("模型名称")]
            public string ModelName
            {
                get { return model_name; }
                set { model_name = value; }
            }
            [CategoryAttribute("基本信息"), DefaultValueAttribute("null"), ReadOnlyAttribute(true), DescriptionAttribute("模型类型")]
            public string ModelType
            {
                get { return model_type; }
                set { model_type = value; }
            }
            [CategoryAttribute("基本信息"), DefaultValueAttribute("null"), ReadOnlyAttribute(true), DescriptionAttribute("模型所属集合名称")]
            public string ListName
            {
                get { return list_name; }
                set { list_name = value; }
            }
            [CategoryAttribute("基本信息"), ReadOnlyAttribute(true), DescriptionAttribute("模型所属集合索引")]
            public int ModelIndex
            {
                get { return model_index; }
                set { model_index = value; }
            }
            //------------------------------------------------详细信息-----------------------------------------
            public string centre_point;
            [CategoryAttribute("详细信息"), ReadOnlyAttribute(true), DescriptionAttribute("模型中心点")]
            public string CentrePoint
            {
                get { return centre_point; }
                set { centre_point = value; }
            }
            public double length;
            [CategoryAttribute("详细信息"), ReadOnlyAttribute(true), DefaultValueAttribute("0"), DescriptionAttribute("包围盒长度")]
            public double Length
            {
                get { return length; }
                set { length = value; }
            }
            public double wide;
            [CategoryAttribute("详细信息"), ReadOnlyAttribute(true), DefaultValueAttribute("0"), DescriptionAttribute("包围盒宽度")]
            public double Wide
            {
                get { return wide; }
                set { wide = value; }
            }
            public double height;
            [CategoryAttribute("详细信息"), ReadOnlyAttribute(true), DefaultValueAttribute("0"), DescriptionAttribute("包围盒高度")]
            public double Height
            {
                get { return height; }
                set { height = value; }
            }

            public void Display_Information()
            {
                if (Pick.selected_data.data_type != null)
                {
                    //基本信息
                    model_name = selected_data.data_name;
                    model_type = selected_data.data_type.ToString();
                    if (selected_data.data_type[0] == "TIN")
                    {
                        list_name = "all_tin";
                    }
                    else if (selected_data.data_type[0] == "Build")
                    {
                        list_name = "all_build";
                    }
                    else if (selected_data.data_type[0] == "Road")
                    {
                        list_name = "all_road";
                    }

                    if (selected_data.data_type[0] == "TIN")
                    {
                        model_index = selected_data.data_index[1];
                    }
                    else
                    {
                        model_index = selected_data.data_index[0];
                    }
                    //详细信息
                    if (selected_data.data_type[0] != "TIN")
                    {
                        centre_point = selected_data.bounding_box.centre_point.point_x.ToString() + ","
                            + selected_data.bounding_box.centre_point.point_y.ToString() + ","
                            + selected_data.bounding_box.centre_point.point_z.ToString(); ;
                        length = selected_data.bounding_box.length;
                        wide = selected_data.bounding_box.wide;
                        height = selected_data.bounding_box.height;
                    }
                }

            }
        }
        public class CommandDisplay
        {          
            public delegate void AddModel_Tpye_Changed();
            public static event AddModel_Tpye_Changed addmodel_type_changed;
           
            private static string[] addmodel_type;
            public static string[] AddModel_Type
            {
                get { return addmodel_type; }
                set
                {                    
                    if (addmodel_type != value)
                    {
                        addmodel_type = value;
                        addmodel_type_changed();
                    }
                    addmodel_type = value;
                }
            }
        }
    }
}
