using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadCode
{
    public partial class frm_Data : Form
    {
        public frm_Data()
        {
            InitializeComponent();
        }

        private void frm_Data_Load(object sender, EventArgs e)
        {
            LoadDataWaiting();
        }
        void LoadDataWaiting()
        {
            string sqlString = "Select * from ReadCode where StatusUpload='WAITING'";
            DataTable dt = Support_SQL.GetTableData(sqlString);
            if (dt.Rows.Count > 0)
            {
                grdData.DataSource = dt;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < grvData.RowCount; i++)
            //{
            //    int ID = Support_SQL.ToInt(grvData.GetRowCellValue(i, colID));
            //    string code_TagJigNonePCS = Support_SQL.ToString(grvData.GetRowCellValue(i, colCode_TagJigNonePCS));
            //    string code_TagJigHavePCS= Support_SQL.ToString(grvData.GetRowCellValue(i, colCode_TagJigHavePCS));
            //    if (ID > 0)
            //    {
            //        //UploadData to ->Server
            //        bool uploadDataPlasma = false;

            //        if (ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(5000), () =>
            //        {
            //            uploadDataPlasma = SupportDB_MMCV.Upload_DataPlasma(c_varGolbal.LineID, c_varGolbal.RouteID, c_varGolbal.DeviceID, c_varGolbal.MPN, c_varGolbal.StaffID, c_varGolbal.LotID, code_TagJigHavePCS, code_TagJigNonePCS, c_varGolbal.List_DataCode);
            //        }) && uploadDataPlasma)
            //        {
            //            Support_SQL.SaveStateUploadByID(ID,"OK"); 
            //             //uploadDataPlasma = true;
            //        }
            //        else
            //        {
            //            //uploadDataPlasma = false;
            //            Support_SQL.SaveStateUploadByID(ID, "WAITING"); 
            //        }
            //    }
            //}
        }
        /// <summary>
        /// Timeout lệnh thực thi
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="codeBlock"></param>
        /// <returns></returns>
        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                Task task = Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
    }
}
