using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadCode.Forms
{
    public partial class frmSetting_PLC : Form
    {
        public frmSetting_PLC()
        {
            InitializeComponent();
        }


        private void frmSetting_PLC_Load(object sender, EventArgs e)
        {

        }

        void LoadData()
        {

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Lưu dữ liệu vào db;
                string sql_select = "Select * from SettingPLC";
                DataTable dt = Support_SQL.GetTableData(sql_select);
                if(dt.Rows.Count<=0)
                {
                    //Nếu chưa có cho insert
                   // string sql_insert= "Insert into SettingPLC ()"+
                     //                  $"('{}','{}','{}')"
                                      
                }
                else
                {
                    //Nếu có rồi thì update lại 
                }





                //Set lại các dữ liêu dã thay đổi của class SettingPLC

                //SettingPLC.IP_PLC=
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
