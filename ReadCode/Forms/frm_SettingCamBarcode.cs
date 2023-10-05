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
using System.Net.Sockets;

namespace ReadCode
{
    public partial class frm_SettingCamBarcode : Form
    {

        private Socket BarCode;

        public int ProgramID = -1;
        public int MachineIndex = 0;

        /// <summary>
        /// khởi tạo class form setting Cam Barcode
        /// </summary>
        /// <param name="PrgID"></param>
        /// <param name="MachineIndex"></param>
        public frm_SettingCamBarcode()
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
            loadProgram();
            cbProgram.SelectedValue = ProgramID;
            loadData(ProgramID);
        }

        #region event Click
        /// <summary>
        /// Button Cam 1 Connect 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnectCam1_Click(object sender, EventArgs e)
        {

            if (Connect())
            {
                btnDisconnectCam1.Enabled = true;
                btnConnectCam1.Enabled = false;
                MessageBox.Show("Connect successful.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_IPCamBarcode1.Enabled = false;
                txt_PortCamBarcode1.Enabled = false;
                cbProgram.Enabled = false;
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
                    Disconnect();
                    txt_IPCamBarcode1.Enabled = true;
                    txt_PortCamBarcode1.Enabled = true;
                    cbProgram.Enabled = true;
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
        private void Frm_SettingCamBarcode_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnDisconnectCam1.PerformClick();
        }

        private void cbProgram_SelectedValueChanged(object sender, EventArgs e)
        {
            int idprogram = Support_SQL.ToInt(cbProgram.SelectedValue);
            loadData(idprogram);
        }
        bool save()
        {
            if (cbProgram.Text.Trim() == "")
            {
                MessageBox.Show("Chưa có Program ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                DataTable dt = Support_SQL.GetTableData($"SELECT * FROM BarcodeSetting WHERE ID_Program = '{ProgramID}'");
                string sql = "";
                string Ip_Barcode = Support_SQL.ToString(txt_IPCamBarcode1.Text.Trim());
                string Port_Barcode = Support_SQL.ToString(txt_PortCamBarcode1.Text.Trim());
                int Program_ID = Support_SQL.ToInt(cbProgram.SelectedValue);
                if (dt.Rows.Count > 0)
                {
                    //Update
                    sql = $"Update BarcodeSetting Set Ip_Barcode='{Ip_Barcode}',Port_Barcode='{Port_Barcode}' where ID_Program={Program_ID}";
                }
                else
                {
                    //Insert
                    sql = $"INSERT INTO BarcodeSetting (ID_Program,Ip_Barcode,Port_Barcode) VALUES('{Program_ID}','{Ip_Barcode}','{Port_Barcode}')";
                }

                Support_SQL.ExecuteQuery(sql);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu :" + ex.Message);
            }
            return true;
        }

        /// <summary>
        /// Save Setting Barcode For Each  Machine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            if (save())
            {
                MessageBox.Show("Lưu dữ liệu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
        #region Load Data
        /// <summary>
        /// Load data theo ProgramID
        /// </summary>
        void loadData(int ID)
        {
            try
            {
                if (ID <= 0) return;
                DataTable dt = Support_SQL.GetTableData($"SELECT * FROM BarcodeSetting WHERE ID_Program = '{ID}'");
                //txtMachineIndex.Text = MachineIndex + "";
                if (dt.Rows.Count > 0)
                {
                    txt_IPCamBarcode1.Text = Support_SQL.ToString(dt.Rows[0]["Ip_Barcode"]);
                    txt_PortCamBarcode1.Text = Support_SQL.ToString(dt.Rows[0]["Port_Barcode"]);
                }
                else
                {
                    txt_IPCamBarcode1.Text = "";
                    txt_PortCamBarcode1.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void loadProgram()
        {
            DataTable dt = Support_SQL.GetTableData($"Select * from ProgramMain");
            cbProgram.DataSource = dt;
            cbProgram.DisplayMember = "ProgramName";
            cbProgram.ValueMember = "ID_Program";
        }
        #endregion
        #region Check Connect Cambarcode
        private bool Connect()
        {
            string IP = Support_SQL.ToString(txt_IPCamBarcode1.Text.Trim());
            int Port = Support_SQL.ToInt(txt_PortCamBarcode1.Text.Trim());
            if (IP == "" || Port == 0)
            {
                MessageBox.Show("Ip address and Port is null", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            try
            {
                if (BarCode != null)
                {
                    BarCode.Close();// BarCode.Dispose();
                    BarCode = null;
                }
                BarCode = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress _Ip = IPAddress.Parse(IP);
                IPEndPoint _endPoint = new IPEndPoint(_Ip, Port);
                BarCode.Connect(_endPoint);

                if (!BarCode.Connected)
                {
                    MessageBox.Show("Connect Fail.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {

                MessageBox.Show("Connect Fail." + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void Disconnect()
        {
            if (BarCode != null)
            {
                BarCode.Close();
            }
        }
        #endregion

    }
}
