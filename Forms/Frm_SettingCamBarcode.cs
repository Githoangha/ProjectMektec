using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LineGolden_PLasma
{
    public partial class Frm_SettingCamBarcode : Form
    {
        // Khởi tạo thư viện lưu tên Reader và cfg của reader đó
        public int ProgramID = -1;
        public int PlasmaIndex = 0;
        

        public Frm_SettingCamBarcode()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Form Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_SettingCamBarcode_Load(object sender, EventArgs e)
        {
            loadProgramName();
        }

        #region event Click
        /// <summary>
        /// Button Cam 1 Connect 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnectCam1_Click(object sender, EventArgs e)
        {

            try
            {

                btnDisconnectCam1.Enabled = true;
                btnConnectCam1.Enabled = false;
            }
            catch
            {
                btnDisconnectCam1.Enabled = false;
                btnConnectCam1.Enabled = true;
            }
        }

        /// <summary>
        /// Button Cam 1 Disconnect 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnectCam1_Click(object sender, EventArgs e)
        {
            if (!btnConnectCam1.Enabled && btnDisconnectCam1.Enabled)
            {
                try
                {

                    btnDisconnectCam1.Enabled = false;
                    btnConnectCam1.Enabled = true;
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }


        /// <summary>
        /// Save Setting Barcode For Each Plasma Machine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Support_SQL.GetTableData($"SELECT * FROM {PLS.NameTable} WHERE {PLS.ID}={ProgramID} AND {PLS.PlasmaIndex}={PlasmaIndex}");
                string sqlString = "";
                if (dt.Rows.Count > 0)
                {

                    sqlString = $"UPDATE {PLS.NameTable} SET " +
                                               $"{PLS.Ip_Barcode}='{txt_IPCamBarcode1.Text.Trim()}',{PLS.Port_Barcode}='{txt_PortCamBarcode1.Text.Trim()}'" +
                                               $"WHERE {PLS.ID}={ProgramID} AND {PLS.PlasmaIndex}={PlasmaIndex}";
                }
                else 
                {
                    sqlString = $"INSERT INTO {PLS.NameTable} ({PLS.ID},{PLS.PlasmaIndex},{PLS.Ip_Barcode},{PLS.Port_Barcode}) VALUES" +
                                              $"({ProgramID},{PlasmaIndex},'{txt_IPCamBarcode1.Text.Trim()}','{txt_PortCamBarcode1.Text.Trim()}')";
                }
                Support_SQL.ExecuteQuery(sqlString);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region Function Support
        /// <summary>
        /// Get name program
        /// </summary>
        void loadProgramName()
        {
            try
            {
                DataTable dt = Support_SQL.GetTableData($"SELECT * FROM PlasmaSetting WHERE ID_Program = '{ProgramID}' AND PlasmaIndex={PlasmaIndex}");
                txtProgram.Text = Lib.ToString(Support_SQL.ExecuteScalar($"SELECT ProgramName from ProgramMain WHERE ID_Program = '{ProgramID}'"));
                txtPlasmaIndex.Text = PlasmaIndex + "";
                if (dt.Rows.Count > 0)
                {
                    txt_IPCamBarcode1.Text = Lib.ToString(dt.Rows[0]["Ip_Barcode"]);
                    txt_PortCamBarcode1.Text = Lib.ToString(dt.Rows[0]["Port_Barcode"]);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        private void Frm_SettingCamBarcode_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnDisconnectCam1.PerformClick();
        }

      


    }
}
