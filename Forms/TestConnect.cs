using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActUtlTypeLib;
using NPlcMitsuMxCom;


namespace LineGolden_PLasma
{
    public partial class TestConnect : Form
    {
        #region  variable

        public ActUtlTypeClass PLC_Fx3 = new ActUtlTypeClass();


        public NPlcMitsuMxLib PLC = new NPlcMitsuMxLib(MX_COMMUNICATION_TYPE.UTL_TYPE);
        public int iret;
        private bool IsConnect = false;
        private Thread Main_Thread;
        //System.Windows.Forms.Timer PCAlive = new System.Windows.Forms.Timer();
        #endregion
        public TestConnect()
        {
            InitializeComponent();
            //PCAlive.Interval = 1000;
            //PCAlive.Tick += PCAlive_Tick;

        }


        private void TestConnect_Load(object sender, EventArgs e)
        {
            btnDisconnect.Enabled = btnRead.Enabled = btnWrite.Enabled = false;
        }
        #region Button Click
        private void btnConnect_Click(object sender, EventArgs e)
        {

            if (Lib.ToInt(txtStationNumber.Text.Trim()) <= 0)
            {
                MessageBox.Show("Local Station Number is Fail");
                return;
            }
            try
            {
                //PLC_Fx3.ActLogicalStationNumber = Lib.ToInt(txtStationNumber.Text.Trim());
                //PLC_Fx3.Open();
                int values=PLC.Open(Lib.ToInt(txtStationNumber.Text.Trim()));
                if (values == 0)
                {
                    IsConnect = true;
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = btnRead.Enabled = btnWrite.Enabled = true;
                    //PCAlive.Start();
                    Start_Thread();
                }
                else
                {
                    MessageBox.Show("Connect is Fail");
                }
               
               
            }
            catch (Exception ex)
            {

                MessageBox.Show("Local Station Number is Fail" + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (PLC_Fx3 != null)
            {
                //PCAlive.Stop();
                Stop_Thread();
                //PLC_Fx3.Close();
                PLC.Close();
                IsConnect = false;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = btnRead.Enabled = btnWrite.Enabled = false;
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            try
            {
                PLC_Fx3.GetDevice(txtAddressTrigger.Text.Trim(), out int Data);
                txtValuesAddress.Text = Lib.ToString(Data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        int bitConvert(object obj)
        {
            if (Lib.ToString(obj).Trim() != "" && Lib.ToInt(obj) != 0)
            {
                return 1;
            }
            return 0;
        }
        public List<int> Encode(string S)
        {
            List<int> list = new List<int>();
            foreach (char c in S)
            {
                list.Add(Lib.ToInt(c));
            }
            return list;
        }
        public string Decode(List<int> list)
        {
            string result = "";



            return result;

        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                string Address = txtAddressTrigger.Text.Trim();

                string AddressCon = Address.Substring(0, 1);

                switch (AddressCon.ToUpper())
                {
                    case "M":
                        int Values = bitConvert(txtValuesAddress.Text);
                        //PLC_Fx3.SetDevice(Address, Values);
                        if(Values>0)
                            PLC.WriteBit(Address, true);
                        else
                            PLC.WriteBit(Address, true);
                        break;
                    case "D":
                        int S = Lib.ToInt(txtValuesAddress.Text.Trim());
                        if (S >= 0)
                        {
                            PLC_Fx3.WriteDeviceBlock(Address, 8, S);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        #endregion

        #region Thread AlivePLC

        bool flagPCAlive = false;


        /// <summary>
        /// User Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PCAlive_Tick(object sender, EventArgs e)
        {
            if (flagPCAlive == false)
            {
                PLC_Fx3.SetDevice("M112", 1);
                flagPCAlive = true;
            }
            else
            {
                PLC_Fx3.SetDevice("M112", 0);
                flagPCAlive = false;
            }
        }

        private void Start_Thread()
        {
            Main_Thread = new Thread(new ThreadStart(Alive_PLC));
            Main_Thread.IsBackground = true;
            Main_Thread.Start();
        }
        private void Stop_Thread()
        {
            if (Main_Thread != null)
            {
                Main_Thread.Abort();
            }
        }

        /// <summary>
        /// User Thread
        /// </summary>
        private void Alive_PLC()
        {
            while (IsConnect)
            {
                Thread.Sleep(1000);
                if (flagPCAlive == false)
                {
                    //PLC_Fx3.SetDevice("M112", 1);
                    int values=PLC.WriteBit("M112", true);
                     
                    flagPCAlive = true;
                }
                else
                {
                    //PLC_Fx3.SetDevice("M112", 0);
                    int values=PLC.WriteBit("M112", false);
                    flagPCAlive = false;
                }
            }
        }
        #endregion

        private void txtStationNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            Ping pinger = new Ping();
            try
            {
                string IP = txtIP.Text;
                if (IP.Trim() == "") return;
                PingReply reply = pinger.Send(IP, 3000);

                if (reply.Status == IPStatus.Success)
                {
                    lbResult.Text = $"Ping {IP} Success........!";
                }
                else
                {
                    lbResult.Text = $"Ping {IP} Error..........?";
                }
            }
            catch (PingException ex)
            {
                lbResult.Text = ex.Message;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

        }
    }
}
