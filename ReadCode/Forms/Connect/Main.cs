

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ReadCode
{
    public partial class Main : Form
    {
        bool isConnect = false;
        Thread loop;
        public Main()
        {
            InitializeComponent();
            int nRet = CIOControllerSDK.MV_OK;
            nRet = CIOControllerSDK.MV_IO_WinIO_Init_CS();
            if (nRet != CIOControllerSDK.MV_OK)
            {
                //  LOG("win IO 初始化失败");
                CIOControllerSDK.MV_IO_WinIO_DeInit_CS();
            }
            else
            {
                //   LOG("Win IO 初始化成功");
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            int nRet = CIOControllerSDK.MV_OK;
            CIOControllerSDK.MV_IO_VERSION stVersion = new CIOControllerSDK.MV_IO_VERSION();
            CIOControllerSDK.MV_IO_GetSDKVersion_CS(ref stVersion);


        }

        void Loop()
        {

            try
            {
                while (isConnect)
                {

                    Thread.Sleep(100);
                    CheckConnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
        public int CheckConnect()
        {
            int nRet = CIOControllerSDK.MV_OK;
            try
            {

                StatusLable(lbl1, Color.WhiteSmoke);
                StatusLable(lbl2, Color.WhiteSmoke);
                StatusLable(lbl3, Color.WhiteSmoke);


                byte[] byteStatus = new byte[1024];

                nRet = CIOControllerSDK.MV_IO_GetMainInputLevel_CS(ref byteStatus[0]);
                int nGPIStatus = BitConverter.ToInt32(byteStatus, 0);

                if (CIOControllerSDK.MV_OK != nRet)
                {
                    //LOG("Getting the electrical level status failed.");
                    goto MyExit;
                }
                if ((nGPIStatus & 0x01) == 0x01)
                {
                    StatusLable(lbl1, Color.Red);
                    //LOG("Level 1 is connecting");

                }
                else
                {
                    StatusLable(lbl1, Color.Green);

                }

                if ((nGPIStatus & 0x02) == 0x02)
                {
                    StatusLable(lbl2, Color.Red);

                }
                else
                {
                    StatusLable(lbl2, Color.Green);

                }

                if ((nGPIStatus & 0x04) == 0x04)
                {
                    StatusLable(lbl3, Color.Red);
                }
                else
                {
                    StatusLable(lbl3, Color.Green);

                }
            //LOG("The status of the input electrical level is obtained.");

            MyExit:;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return nRet;

        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            isConnect = true;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;
            //GetMainGPIInputLevel();
            RunThread();
        }
        private void RunThread()
        {
            loop = new Thread(Loop);
            loop.IsBackground = true;
            loop.Start();
        }
            
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            isConnect = false;
            btnDisconnect.Enabled = false;
            btnConnect.Enabled = true;
            loop.Abort();
            StatusLable(lbl1, Color.WhiteSmoke);
            StatusLable(lbl2, Color.WhiteSmoke);
            StatusLable(lbl3, Color.WhiteSmoke);
        }

        private void StatusLable(Label lable, Color color)
        {
            //lable.BackColor = color;
            if (lable.InvokeRequired)
                lable.Invoke((MethodInvoker)delegate
                {
                    lable.BackColor = color;
                });
            else
            {
                lable.BackColor = color;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(loop==null)
            {
                return;
            }    
            else if (loop.IsAlive)
            {
                loop.Abort();
            }
        }

        private void PrintMessage(string strMessage)
        {
            string strMsg = "[" + DateTime.Now + "] " + strMessage + "\r\n";
            txtMessage.AppendText(strMsg);
        }

        private void LOG(string strMessage)
        {
            if (txtMessage.InvokeRequired)
                txtMessage.Invoke((MethodInvoker)delegate
                {
                    PrintMessage(strMessage);
                });
            else
            {
                PrintMessage(strMessage);
            }
        }
    }
}
