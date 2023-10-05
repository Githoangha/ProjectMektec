/***************************************************************************************************
* 
* 文件名称：IOController.cs
* 摘    要： VC3000 demo程序
*
* 当前版本：1.2.0
* 日    期：2020.12.03
* 备    注：新建
***************************************************************************************************/

/***************************************************************************************************
* 
* FileName：IOController.cs
* Abstract： VC3000 demo
*
* Current Version：1.2.0
* Date：          2020.12.03
* Remarks：None
***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HRMyTool;
using System.Threading;
using System.Runtime.InteropServices;
//using NsIOControllerSDK;
using System.Threading;


namespace ReadCode
{
    public partial class IOController : Form
    {
        CIOControllerSDK.MV_IO_SERIAL m_stSerial = new CIOControllerSDK.MV_IO_SERIAL();
        private static CIOControllerSDK.cbOutputdelegate EdgeDetectionCallBack;

        // ch:获得语言版本 | en:Get language version
        int nLCID = 0;

        Byte m_nOutputEnablePort = 0;   // ch:输出使能端口 | en:Output enable port 

        public delegate void Action();

        // ch:波特率宏定义 | en:Baud rate
        public int[] m_nBauderate = { 115200 };

        // ch:数据位宏定义 | en:Data bit
        public int[] m_nDatabits = { 8 };

        // ch:校验位定义 | en:Check bit
        public enum PARITY_SCHEME
        {
            None = 0,
        };

        // ch:停止位定义 | en:Stop bit
        public float[] m_fStopbits = { 1f, 1.5f, 2f };

        // ch:Port口定义 | en:Port definition
        public enum PORT_NUMBER
        {
            Port1 = 0x1,
            Port2 = 0x2,
            Port3 = 0x4,
            Port4 = 0x8,
            Port5 = 0x10,
            Port6 = 0x20,
            Port7 = 0x40,
            Port8 = 0x80,
        };

        public enum PORT_LIGHT_NUMBER
        {
            Port1 = 0x1,
            Port2 = 0x2,
            Port3 = 0x4,
            Port4 = 0x8,
        };


        // NPN/PNP State definition
        public enum PNP_ENABLE_STATE
        {
            PNP = 0x01, 
            NPN = 0x02,
        };


        // ch:触发沿定义 | en: Trigger edge definition
        public enum EDGE_TYPE
        {
            上升沿 = 0x01,
            下降沿 = 0x02,
        };
        public enum EDGE_TYPE_EN
        {
            Rising_Edge = 0x01,
            Falling_Edge = 0x02,
        };

        // ch:电平定义 | en: Level definition
        public enum LEVEL_TYPE
        {
            低电平 = 0, 
            高电平 = 1,
        };

        public enum LEVEL_TYPE_EN
        {
            Low_Level = 0,   
            High_Level = 1,  
        };

        // ch:光源状态定义 | en:Light source state definition 
        public enum LIGHT_STATE
        {
            常亮 = 1,
            常灭 = 2,
        };

        public enum LIGHT_STATE_EN
        {
            On = 1,    
            Off = 2,
        };

        // ch:脉冲定义 | en:Pulse definition 
        public enum PATTERN_OUT
        {
            多脉冲 = 0x05,
            单脉冲 = 0x06,
        };
        public enum PATTERN_OUT_EN
        {
            Multi_Pulse = 0x05,
            Single_Pulse = 0x06,
        };

        // ch:沿信息回传 | en: Upload Signal
        public enum EDGE_INFO_ENABLE
        {
            沿信号关闭 = 0x00,  
            沿信号开启 = 0x01,
        };
        public enum EDGE_INFO_ENABLE_EN
        {
            Disable = 0x00, 
            Enable = 0x01, 
        };

        // ch:串口通讯类别 | en: Serial communication category 
       public enum COM_NUMBER
       {
           RS232 = 0x00,
           RS485 = 0x01,
           RS422 = 0x02,
       };

       private const uint m_nMaxGlitchTime = 1000;                // ch:输入触发最大去抖时间 | en:Input trigger maximum debouncer Time 
       private const uint m_nMaxInputDelay = 1000;                // ch:输入触发最大延迟时间 | en:Input trigger maximum delay time 
       private const uint m_nMaxDurationTimeForSingle = 0xFFFF;   // ch:单脉冲模式下的最大持续时间 | en:Maximum duration in mono pulse mode
       private const uint m_nMaxDurationTimeForPWM = 0xFFFF;      // ch:多脉冲模式下的最大持续时间 | en:Maximum duration in multi pulse mode 
       private const uint m_nMaxPulseWidth = 0xFFFF;              // ch:最大脉冲宽度 | en:Maximum pulse width 
       private const uint m_nMaxPulsePeriod = 0xFFFF;             // ch:最大脉冲周期 | en:Maximum pulse period 
       private const uint m_nMaxLightTime = 0xFFFF;

        
        private const string m_strOpenCom = "打开串口";
        private const string m_strCloseCom = "关闭串口";

        private const string m_strOpenCom_EN = "Connect";
        private const string m_strCloseCom_EN = "Disconnect";
        

        private string strIOVersion = "";               // ch:IO版本号 | en:IO Version
        private string strLightVersion = "";            // ch:光源版本号 | en:Light source Version
        private string strSubversion = "";              // ch:次版本号 | en:Sub version number 

        private String strTmpCom = "";                  // ch:用于区分com10以上串口号 | en:Distinguish the serial port number above com10 
        private bool bComTen = false;                   // ch:判断是否大于com10 | en:Is it greater than com10 


        private const string m_strIOVersion = "5";     // ch:区分扩展板是否为IO板 由第三方固件定义 | en:Distinguish whether the expansion board is an IO board 
        private const string m_strLightVersion = "4";  // ch:区分扩展板是否为光源 | en:Distinguish whether the expansion board is a light source 



        // ch:沿检测回调函数 | en:Callback function along detection 
        void CallBackFunc(IntPtr handle, ref CIOControllerSDK.MV_IO_INPUT_EDGE_TYPE stParam, IntPtr pUser)
        {
            try
            {
                CIOControllerSDK.MV_IO_PORT_NUMBER enPortNum;
                int nPort = 0;
                enPortNum = (CIOControllerSDK.MV_IO_PORT_NUMBER)Convert.ToInt32(stParam.nPortNumber);
                switch (enPortNum)
                {
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_1:
                        nPort = 1;
                        break;
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_2:
                        nPort = 2;
                        break;
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_3:
                        nPort = 3;
                        break;
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_4:
                        nPort = 4;
                        break;
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_5:
                        nPort = 5;
                        break;
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_6:
                        nPort = 6;
                        break;
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_7:
                        nPort = 7;
                        break;
                    case CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_8:
                        nPort = 8;
                        break;
                    default:
                        break;
                }
                int nCounter = Convert.ToInt32(stParam.nTriggerTimes);
                int nEdge = Convert.ToInt32(stParam.enEdgeType);
                string nStrEdge;
                if (CIOControllerSDK.MV_IO_EDGE_TYPE.MV_IO_EDGE_RISING == (CIOControllerSDK.MV_IO_EDGE_TYPE)nEdge)
                {
                    nStrEdge = "Rising Edge";
                }
                else
                {
                   nStrEdge = "Falling Edge";
                }
                LOG( "Detecting" + Convert.ToString(nPort) + "change for " + Convert.ToString(nCounter, 10) + "times" + nStrEdge );
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IOController()
        {
            InitializeComponent();
           
            System.Environment.ExitCode = 1;

            // ch:获取系统语言类型 | en:Get system language type 
            nLCID = System.Globalization.CultureInfo.CurrentCulture.LCID;

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

            InitSerialWindow();
            
            InitIOWindow();

            cbComNo.Enabled = true;
            EnableSerailParams();

            //DisableIOParams();
            DisEnableIO();
            DisEnableLightParams();
            DisEnableOtherParams();

            RefreshSDKVersion();

            // ch:记录控件的初始位置和大小 | en:Records the initial position and size of the control 
            int count = MyTool.GetCtrlsCount(this) * 2 + 2;
            float[] factor = new float[count];
            int i = 0;
            factor[i++] = Size.Width;
            factor[i++] = Size.Height;
            List<Control> ctrls = MyTool.GetCtrls(this);
            foreach (Control ctrl in ctrls)
            {
                factor[i++] = ctrl.Location.X / (float)Size.Width;
                factor[i++] = ctrl.Location.Y / (float)Size.Height;
                ctrl.Tag = ctrl.Size;
            }
            Tag = factor;
        }


        private void InitSerialWindow()
        {
            /******ch:串口控制区 | en:Serial port control******/
            cbBauderate.Items.Clear();
            for (int i = 0; i < m_nBauderate.Length; ++i)
            {
                cbBauderate.Items.Add(m_nBauderate[i]);
            }
            cbBauderate.SelectedItem = 115200;

            // DataBits
            cbDataBits.Items.Clear();
            for (int i = 0; i < m_nDatabits.Length; ++i)
            {
                cbDataBits.Items.Add(m_nDatabits[i]);
            }
            cbDataBits.SelectedItem = 8;

            // StopBits
            cbStopBits.Items.Clear();
            for (int i = 0; i < m_fStopbits.Length; ++i)
            {
                cbStopBits.Items.Add(m_fStopbits[i]);
            }
            cbStopBits.SelectedItem = 1f;

            // ParityBits
            cbParityBits.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(PARITY_SCHEME)))
            {
                cbParityBits.Items.Add(item);
            }
            cbParityBits.SelectedItem = PARITY_SCHEME.None;

            if (0x0804 == nLCID)
            {
                btnOpenOrCloseCom.Text = m_strOpenCom;
            }
            else
            {
                btnOpenOrCloseCom.Text = m_strOpenCom_EN;
            }
            

            // ComName
            cbComNo.Items.Clear();
            // 扩展板串口从com7开始 1-6预留给主板和串口扩展版
            cbComNo.Items.Add("Com7");
            cbComNo.Items.Add("Com8");
            cbComNo.Items.Add("Com9");
            cbComNo.Items.Add("Com10");
            cbComNo.Items.Add("Com11");
            cbComNo.Items.Add("Com12");
            cbComNo.SelectedIndex = 0; //默认连接COM7
        }

        private void InitIOWindow()
        {
            /********ch:输入控制区 | en:Input control area ******/
            // InputPort
            cbInputPort.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(PORT_NUMBER)))
            {
                cbInputPort.Items.Add(item);
            }
            cbInputPort.SelectedItem = PORT_NUMBER.Port1;

            // InputEdgeType
            cbInputEdgeType.Items.Clear();
            if (0x0804 == nLCID)
            {
                foreach (var item in Enum.GetValues(typeof(EDGE_TYPE)))
                {
                    cbInputEdgeType.Items.Add(item);
                }
                cbInputEdgeType.SelectedItem = EDGE_TYPE.上升沿;
            }
            else
            {
                foreach (var item in Enum.GetValues(typeof(EDGE_TYPE_EN)))
                {
                    cbInputEdgeType.Items.Add(item);
                }
                cbInputEdgeType.SelectedItem = EDGE_TYPE_EN.Rising_Edge;
            }
            

            comboBoxEdgeInfoEnable.Items.Clear();
            if (0x0804 == nLCID)
            {
                foreach (var item in Enum.GetValues(typeof(EDGE_INFO_ENABLE)))
                {
                    comboBoxEdgeInfoEnable.Items.Add(item);
                }
                comboBoxEdgeInfoEnable.SelectedItem = EDGE_INFO_ENABLE.沿信号关闭;
            }
            else
            {
                foreach (var item in Enum.GetValues(typeof(EDGE_INFO_ENABLE_EN)))
                {
                    comboBoxEdgeInfoEnable.Items.Add(item);
                }
                comboBoxEdgeInfoEnable.SelectedItem = EDGE_INFO_ENABLE_EN.Disable;
            }

            // InputDelay
            tbInputDelay.Text = "0";

            // GlitchTime
            tbGlitchTime.Text = "0";

            /*******输出控制区********/
            // OutputPort
            cbOutputPort.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(PORT_NUMBER)))
            {
                cbOutputPort.Items.Add(item);
            }
            cbOutputPort.SelectedItem = PORT_NUMBER.Port1;

       
            //COM1
            cbRS1.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(COM_NUMBER)))
            {
                cbRS1.Items.Add(item);
            }
            cbRS1.SelectedItem = COM_NUMBER.RS232;
            //COM2
            cbRS2.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(COM_NUMBER)))
            {
                cbRS2.Items.Add(item);
            }
            cbRS2.SelectedItem = COM_NUMBER.RS232;
           

            cbLightPort.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(PORT_LIGHT_NUMBER)))
            {
                cbLightPort.Items.Add(item);
            }
            cbLightPort.SelectedItem = PORT_LIGHT_NUMBER.Port1;

            //cbPNPEnable
            cbPNPEnable.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(PNP_ENABLE_STATE)))
            {
                cbPNPEnable.Items.Add(item);
            }
            cbPNPEnable.SelectedItem = PNP_ENABLE_STATE.PNP;

            // cbMainPNPEnable
            cbGainPNPEnable.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(PNP_ENABLE_STATE)))
            {
                cbGainPNPEnable.Items.Add(item);
            }
            cbGainPNPEnable.SelectedItem = PNP_ENABLE_STATE.PNP;
            

            // OutputPattern
            cbOutputPattern.Items.Clear();
            if (0x0804 == nLCID)
            {
                foreach (var item in Enum.GetValues(typeof(PATTERN_OUT)))
                {
                    cbOutputPattern.Items.Add(item);
                }
                cbOutputPattern.SelectedItem = PATTERN_OUT.多脉冲;
            }
            else
            {
                foreach (var item in Enum.GetValues(typeof(PATTERN_OUT_EN)))
                {
                    cbOutputPattern.Items.Add(item);
                }
                cbOutputPattern.SelectedItem = PATTERN_OUT_EN.Multi_Pulse;
            }

            // OutputLevel
            if (0x0804 == nLCID)
            {
                foreach (var item in Enum.GetValues(typeof(LEVEL_TYPE)))
                {
                    cbValidLevel.Items.Add(item);
                }
                cbValidLevel.SelectedItem = LEVEL_TYPE.低电平;
            }
            else
            {
                foreach (var item in Enum.GetValues(typeof(LEVEL_TYPE_EN)))
                {
                    cbValidLevel.Items.Add(item);
                }
                cbValidLevel.SelectedItem = LEVEL_TYPE_EN.Low_Level;
            }
            
            // PulseDuration
            tbDurationTime.Text = "0";

            // PulsePeriod
            tbPulsePeriod.Text = "0";

            // PulseWidth
            tbPulseWidth.Text = "0";

            /********ch:光源控制区 | en:Light source control ********/
            // LightState
            rdbLightStateOn.Checked = true;

            rbRisingEdge.Checked = true;

            // LightValue
            tbLightValue.Text = "0";

            textLightTime.Text = "0";
            if (0x0804 == nLCID)
            {
                btnEdgeDetection.Text = "开启沿检测";
            }
            else
            {
                btnEdgeDetection.Text = "Enable Edge Detection";
            }

            cbPNPEnable.Enabled = false;
            label23.Enabled = false;

            /********输入电平********/
            ResetGroupBox(gbInputLevel);

            /********输出使能********/
            ResetGroupBox(gbOutputEnable);

            // 极性使能
            ResetGroupBox(groupBox8);
        }

        private void GetIOParams()
        {
            cbInputPort.SelectedItem = PORT_NUMBER.Port1;
            int inputSelectIndex = cbInputPort.SelectedIndex + 1;
            GetInputSet(inputSelectIndex);

            cbOutputPort.SelectedItem = PORT_NUMBER.Port1;
            int outputSelectIndex = cbOutputPort.SelectedIndex + 1;
            GetOutputSet(outputSelectIndex);
        }

        private void ResetGroupBox(GroupBox bBox)
        {
            foreach (Control i in bBox.Controls)
            {
                if (i is CheckBox)
                {
                    ((CheckBox)i).Checked = false;
                }
                else if (i is ComboBox)
                {
                    ((ComboBox)i).SelectedIndex = 0;
                }
                else if (i is Label)
                {
                    ((Label)i).BackColor = Color.White;
                }
            }
        }

        private void EnableIO()
        {
            gbInputSettings.Enabled = true;
            gbOutputSettings.Enabled = true;
            gbOutputEnable.Enabled = true;
            gbInputLevel.Enabled = true;
            cbPNPEnable.Enabled = true;
            label23.Enabled = true;
        }

        private void DisEnableIO()
        {
            gbInputSettings.Enabled = false;
            gbOutputSettings.Enabled = false;
            gbOutputEnable.Enabled = false;
            gbInputLevel.Enabled = false;
            cbPNPEnable.Enabled = false;
            label23.Enabled = false;
        }

        private void EnableOtherParams()
        {
            gbOtherSettings.Enabled = true;
            gbVersionSetting.Enabled = true;
        }

        private void DisEnableOtherParams()
        {
            gbOtherSettings.Enabled = false;
            gbVersionSetting.Enabled = false;
        }

        private void EnableLightParams()
        {
            gbLightCtrl.Enabled = true;
        }

        private void DisEnableLightParams()
        {
            gbLightCtrl.Enabled = false;
        }

        private void EnableSerailParams()
        {
            cbBauderate.Enabled = true;
            cbDataBits.Enabled = true;
            cbStopBits.Enabled = true;
            cbParityBits.Enabled = true;
        }

        private void DiableSerailParams()
        {
            cbBauderate.SelectedItem = 115200;
            cbBauderate.Enabled = false;

            cbDataBits.SelectedItem = 8;
            cbDataBits.Enabled = false;

            cbStopBits.SelectedItem = 1f;
            cbStopBits.Enabled = false;

            cbParityBits.SelectedItem = PARITY_SCHEME.None;
            cbParityBits.Enabled = false;
        }
       
        /// <summary>
        /// ch:获取当前SDK版本信息 | en:Get current SDK version information 
        /// </summary>
        public int RefreshSDKVersion()
        {
            int nRet = CIOControllerSDK.MV_OK;

            // ch:获取输入口参数设置信息 | en:Get input port parameter setting information 
            CIOControllerSDK.MV_IO_VERSION stVersion = new CIOControllerSDK.MV_IO_VERSION();
            int nTimeS = System.Environment.TickCount;
            do
            {
                nRet = CIOControllerSDK.MV_IO_GetSDKVersion_CS(ref stVersion);
                if (CIOControllerSDK.MV_OK == nRet)
                {
                    break;
                }
            } while (System.Environment.TickCount - nTimeS < 500);

            lbSDKVersion.Text = stVersion.nMainVersion + "." + stVersion.nSubVersion + "." + stVersion.nModifyVersion
                   + " " + stVersion.nYear.ToString("0000") + stVersion.nMonth.ToString("00") + stVersion.nDay.ToString("00");

            return nRet;
        }

        public int ComFirmwareVersion()
        {
            int nRet = CIOControllerSDK.MV_OK;
            bool bGet = false;
            CIOControllerSDK.MV_IO_VERSION stVersion = new CIOControllerSDK.MV_IO_VERSION();
            int nTimeS = System.Environment.TickCount;
            do
            {
                nRet = CIOControllerSDK.MV_IO_GetFirmwareVersion_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stVersion);
                if (CIOControllerSDK.MV_OK == nRet)
                {
                    bGet = true;
                    break;
                }

            } while (System.Environment.TickCount - nTimeS < 500);
           
            if (bGet)
            {
                return CIOControllerSDK.MV_OK;
            }
            else
            {
                return CIOControllerSDK.MV_E_UNKNOW;
            }
        }

        /// <summary>
        /// ch:获取当前固件版本信息 | en:Get current firmware version information 
        /// </summary>
        public int RefreshFirmwareVersion()
        {
            int nRet = CIOControllerSDK.MV_OK;
            bool bGet = false;
            CIOControllerSDK.MV_IO_VERSION stVersion = new CIOControllerSDK.MV_IO_VERSION();
            int nTimeS = System.Environment.TickCount;
            do
            {
                nRet = CIOControllerSDK.MV_IO_GetFirmwareVersion_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stVersion);
                if (CIOControllerSDK.MV_OK == nRet)
                {
                    bGet = true;
                    break;
                }

            } while (System.Environment.TickCount - nTimeS < 500);
            strSubversion = " ";
            lbFirmwareVersion.Text = " ";
            if (bGet)
            {
                // ch:判断IO/光源 | en:Judge IO / light source 
                strSubversion = stVersion.nMainVersion.ToString();
                
                lbFirmwareVersion.Text = stVersion.nMainVersion + "." + stVersion.nSubVersion + "." + stVersion.nModifyVersion
                   + " " + stVersion.nYear.ToString("0000") + stVersion.nMonth.ToString("00") + stVersion.nDay.ToString("00");
                return CIOControllerSDK.MV_OK;
            }
            else 
            {
                return CIOControllerSDK.MV_E_UNKNOW;
            }
        }

        /// <summary>
        /// ch:打印消息 | en:Print message  
        /// </summary>
        private void PrintMessage(string strMessage)
        {
            string strMsg = "[" + DateTime.Now + "] " + strMessage + "\r\n";
            rtbMessage.AppendText(strMsg);
        }

        /// <summary>
        /// ch:日志消息 | en:Log message  
        /// </summary>
        private void LOG(string strMessage)
        {
            this.Invoke(new Action(() => { PrintMessage(strMessage); }));
        }

        /// <summary>
        /// ch:打开关闭串口按钮点击事件 | en:Open close serial port button click event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        
        public void btnOpenOrCloseCom_Click(object sender, EventArgs e)
        {
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                strTmpCom = "";
                bComTen = false;
                if ((m_strOpenCom == btnOpenOrCloseCom.Text) || (m_strOpenCom_EN == btnOpenOrCloseCom.Text))
                {
                    btnOpenOrCloseCom.Enabled = false;
                    m_stSerial.strComName = "Com" + (cbComNo.SelectedIndex + 7);
                    if ((cbComNo.SelectedIndex + 7) >= 10)
                    {
                        bComTen = true;
                        strTmpCom = m_stSerial.strComName;
                    }
                    // 1.ch:创建句柄 | en:Create handle
                    if (IntPtr.Zero == CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]) // SelectedIndex也是从0开始
                    {
                        IntPtr handle = new IntPtr();
                        
                        nRet = CIOControllerSDK.MV_IO_CreateHandle_CS(ref handle);
                        if (CIOControllerSDK.MV_OK != nRet || IntPtr.Zero == handle)
                        {
                           LOG(m_stSerial.strComName + "Creating the handle failed.Err code:" + Convert.ToString(nRet, 16));
                        }
                        else
                        {
                           CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex] = handle;
                        }
                    }
                    else
                    {
                        LOG("Error. The serial port is open.");
                        return;
                    }

                    // 2.ch:打开串口 | en:Connect Serial 
                    nRet = CIOControllerSDK.MV_IO_Open_CS( CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref m_stSerial);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        if (bComTen)
                        {
                           LOG("The serial port " + strTmpCom + " is occupied.");
                        }
                        else
                        {
                           LOG("The serial port " + m_stSerial.strComName + " is occupied.");
                        }

                        btnOpenOrCloseCom.Enabled = true;
                        CIOControllerSDK.MV_IO_Close_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                        CIOControllerSDK.MV_IO_DestroyHandle_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                        CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex] = IntPtr.Zero;
                        return;
                    }
                    else
                    {
                        if (bComTen)
                        {
                             LOG("The serial port" + strTmpCom + "is available.");
                        }
                        else
                        {
                            LOG("The serial port" + m_stSerial.strComName + "is available.");
                        }
                    }
                    this.Invoke(new Action(() => { btnEdgeDetection.Enabled = true; }));

                    // 3.ch:获取固件版本信息 | en:Get firmware version information 
                    nRet = RefreshFirmwareVersion();
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        if (bComTen)
                        {
                           LOG(strTmpCom + "Getting the firmware version failed.");
                        }
                        else
                        {
                            LOG(m_stSerial.strComName + "Getting the firmware version failed.");
                        }
                        CIOControllerSDK.MV_IO_Close_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                        CIOControllerSDK.MV_IO_DestroyHandle_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                        CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex] = IntPtr.Zero;
                        if (0x0804 == nLCID)
                        {
                            btnOpenOrCloseCom.Text = "打开串口";
                        }
                        else
                        {
                            btnOpenOrCloseCom.Text = "Connect";
                        }
                        
                        btnOpenOrCloseCom.Enabled = true;
                        EnableSerailParams();
                        DisEnableIO();
                        DisEnableLightParams();
                        DisEnableOtherParams();
                        return;
                    }
                    else
                    {
                        if (bComTen)
                        {
                            LOG(strTmpCom + "The firmware version is obtained:" + lbFirmwareVersion.Text);
                        }
                        else
                        {
                           LOG(m_stSerial.strComName + "The firmware version is obtained:" + lbFirmwareVersion.Text);
                        }
                    }

                    //4.ch:加载参数 | en:Loading parameters 
                    if(m_strIOVersion == strSubversion)
                    {
                        //IO
                        LOG("This is an IO extended module.");
                        EnableIO();
                        int inputSelectIndex = cbInputPort.SelectedIndex + 1;
                        GetInputSet(inputSelectIndex);
                        GetOutputSet(cbOutputPort.SelectedIndex + 1);
                        CIOControllerSDK.m_hDeviceTypeList[cbComNo.SelectedIndex] = strSubversion;
                        if (0x0804 == nLCID)
                        {
                            btnOpenOrCloseCom.Text = m_strCloseCom;
                        }
                        else
                        {
                            btnOpenOrCloseCom.Text = m_strCloseCom_EN;
                        }
                        
                        btnOpenOrCloseCom.Enabled = true;
                        strIOVersion = lbFirmwareVersion.Text.ToString();

                    }
                    else if (m_strLightVersion == strSubversion)
                    {
                         LOG("This is a light source extended module.");
                        
                        EnableLightParams();
                        GetLightParams();
                        CIOControllerSDK.m_hDeviceTypeList[cbComNo.SelectedIndex] = strSubversion;
                        if (0x0804 == nLCID)
                        {
                            btnOpenOrCloseCom.Text = m_strCloseCom;
                        }
                        else
                        {
                            btnOpenOrCloseCom.Text = m_strCloseCom_EN;
                        }
                       
                        btnOpenOrCloseCom.Enabled = true;
                        strLightVersion = lbFirmwareVersion.Text.ToString();
                    }
                    else 
                    {
                        // "Unknows"
                        EnableIO();
                        int inputSelectIndex = cbInputPort.SelectedIndex + 1;
                        GetInputSet(inputSelectIndex);
                        GetOutputSet(cbOutputPort.SelectedIndex + 1);

                        EnableLightParams();
                        GetLightParams();
                        CIOControllerSDK.m_hDeviceTypeList[cbComNo.SelectedIndex] = strSubversion;
                        if (0x0804 == nLCID)
                        {
                            btnOpenOrCloseCom.Text = m_strCloseCom;
                        }
                        else
                        {
                            btnOpenOrCloseCom.Text = m_strCloseCom_EN;
                        }
                        
                        btnOpenOrCloseCom.Enabled = true;
                    }
                    
                }
                else  
                {
                    strTmpCom = "";
                    bComTen = false;
                    m_stSerial.strComName = "Com" + (cbComNo.SelectedIndex + 7);
                    if ((cbComNo.SelectedIndex + 7) >= 10)
                    {
                        bComTen = true;
                        strTmpCom = m_stSerial.strComName;
                    }
                    //ch:关闭串口 | en:Disconnect serial 
                    if (IntPtr.Zero != CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]) // SelectedIndex也是从0开始
                    {
                        if (bComTen)
                        {
                           LOG(strTmpCom + "Handle deleted.");
                        }
                        else
                        {
                           LOG(m_stSerial.strComName + "Handle deleted.");
                        }
                        CIOControllerSDK.MV_IO_Close_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                        CIOControllerSDK.MV_IO_DestroyHandle_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                        CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex] = IntPtr.Zero;

                        if (bComTen)
                        {
                            LOG(strTmpCom + " The serial port is closed.！");
                        }
                        else
                        {
                            LOG(m_stSerial.strComName + " The serial port is closed.");
                        }
                        if (0x0804 == nLCID)
                        {
                            btnOpenOrCloseCom.Text = m_strOpenCom;
                        }
                        else
                        {
                            btnOpenOrCloseCom.Text = m_strOpenCom_EN;
                        }
                    }
                    else
                    {
                         LOG("Error. The serial port is closed.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                LOG("Sending the command failed. Parameters or operations exception occurs."); 
            }
        }

        /// <summary>
        /// ch:按钮文本改变事件 | en:Button text change event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenOrCloseCom_TextChanged(object sender, EventArgs e)
        {
            if ((m_strOpenCom == btnOpenOrCloseCom.Text) || (m_strOpenCom_EN == btnOpenOrCloseCom.Text))
            {
                ////ch:关闭串口 | en:Close the serial port 
                EnableSerailParams();
                DisEnableIO();
                DisEnableLightParams();
                DisEnableOtherParams();
            }
            else
            {
                EnableOtherParams();
            }
        }

    
        /// <summary>
        /// ch:文本框数字输入检测 | en:TextBox digital input detection 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDigitalCtrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ('\r' == e.KeyChar)
            {
                TextBoxForNumberFocusLeave(sender, null);
                SendKeys.Send("{Tab}");
            }
            else
            {
                if (tbDurationTime == (TextBox)sender)
                {
                    if ((PATTERN_OUT.多脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem) || (PATTERN_OUT_EN.Multi_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
                    {
                        return;
                    }
                }
                if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// ch:输入设置应用按钮点击事件 | en:Input settings application button click event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInputSet_Click(object sender, EventArgs e)
        {
            try
            {
                CIOControllerSDK.MV_IO_SET_INPUT stInput = new CIOControllerSDK.MV_IO_SET_INPUT();
                stInput.nPort = ((UInt32)(CIOControllerSDK.MV_IO_PORT_NUMBER)cbInputPort.SelectedItem);
                stInput.nEdge = (UInt32)(CIOControllerSDK.MV_IO_EDGE_TYPE)cbInputEdgeType.SelectedItem;
                stInput.nEnable = (UInt32)(CIOControllerSDK.MV_IO_EDGE_NOTICE_STATE)comboBoxEdgeInfoEnable.SelectedItem;
                stInput.nDelayTime = UInt32.Parse(tbInputDelay.Text);
                stInput.nGlitch = UInt32.Parse(tbGlitchTime.Text);
                int nRet = CIOControllerSDK.MV_IO_SetInput_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stInput);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Sending input commands failed. Error code:" + Convert.ToString(nRet, 16)); 
                    return;
                }
                else
                {
                    LOG("The input commands are sent.");
                }
            }
            catch (System.Exception ex)
            {
                LOG("Sending the command failed. Parameters or operations exception occurs.");
                return;
            }
            finally
            {
                //int a = 10;
            }
        }

        /// <summary>
        /// ch:光源值改变事件 | en:Light source value change event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbLightValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if ("" != tbLightValue.Text)
                {
                    int nValue = Int32.Parse(tbLightValue.Text);
                    if (nValue < trackBarLight.Minimum)
                    {
                        nValue = trackBarLight.Minimum;
                        tbLightValue.Text = nValue.ToString();
                    }
                    if (nValue > trackBarLight.Maximum)
                    {
                        nValue = trackBarLight.Maximum;
                        tbLightValue.Text = nValue.ToString();
                    }
                    trackBarLight.Value = Int32.Parse(tbLightValue.Text);
                }
            }
            catch (System.Exception ex)
            {
                LOG("Sending the command failed. Parameters or operations exception occurs.");
            }
        }

        /// <summary>
        /// ch:光源值滚动条事件 | en:Light value scroll bar event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarLight_Scroll(object sender, EventArgs e)
        {
            tbLightValue.Text = trackBarLight.Value.ToString();
        }

        /// <summary>
        /// ch:清空消息 | en:Clear message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearMessage_Click(object sender, EventArgs e)
        {
            rtbMessage.Clear();
            rtbMessage.Refresh();
        }

        /// <summary>
        /// ch:保存信息 | en:Save message 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveMessage_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.RestoreDirectory = true;
                sfd.Filter = "Text|*.txt|*.ALL|*.*";
                sfd.FileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                        + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    foreach (string s in rtbMessage.Lines) sw.WriteLine(s);
                    sw.Close();
                    fs.Close();
                }
                sfd.Dispose();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// ch:输出端口选择改变 | en:Output port selection changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOutputPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ch:获取输出端口参数信息 | en:Get output port parameter information 
            if ((m_strCloseCom == btnOpenOrCloseCom.Text) || (m_strCloseCom_EN == btnOpenOrCloseCom.Text))
            {
                GetOutputSet(cbOutputPort.SelectedIndex + 1);
            }
        }

        /// <summary>
        /// ch:获取输出信息 | en:Output port selection changed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private int GetOutputSet(Int32 nPort)
        {
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                int nPortFlag = Convert.ToInt32(Math.Pow(2, (double)(nPort - 1)));

                //ch:获取输出参数 | en:Get output parameters 
                CIOControllerSDK.MV_IO_SET_OUTPUT stOutput = new CIOControllerSDK.MV_IO_SET_OUTPUT();
                stOutput.nPort = Convert.ToUInt32(nPortFlag);
                nRet = CIOControllerSDK.MV_IO_GetPortOutputParam_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stOutput);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Getting the output port:" + nPort.ToString() + "information failed. Error code:" + Convert.ToString(nRet, 16));
                    goto MyExit;
                }
                else
                {
                    LOG("The output port " + nPort.ToString() + " information is obtained.");
                }

                if (0x0804 == nLCID)  // ch:中文 | en:chinese
                {
                    cbOutputPattern.SelectedItem = (PATTERN_OUT)stOutput.nType;
                }
                else
                {
                    cbOutputPattern.SelectedItem = (PATTERN_OUT_EN)stOutput.nType;
                }

                if ((PATTERN_OUT.多脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem)||(PATTERN_OUT_EN.Multi_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
                {
                    if (0x0804 == nLCID)
                    {
                        cbValidLevel.SelectedItem = (LEVEL_TYPE)stOutput.nValidLevel;
                    }
                    else
                    {
                        cbValidLevel.SelectedItem = (LEVEL_TYPE_EN)stOutput.nValidLevel;
                    }
                     tbDurationTime.Text = stOutput.nDurationTime > m_nMaxDurationTimeForPWM ? "Max" : stOutput.nDurationTime.ToString();
                     tbPulsePeriod.Text = stOutput.nPulsePeriod.ToString();
                     tbPulseWidth.Text = stOutput.nPulseWidth.ToString();
                }
                else if ((PATTERN_OUT.单脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem) || (PATTERN_OUT_EN.Single_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
                {
                     if (0x0804 == nLCID)
                     {
                         cbValidLevel.SelectedItem = (LEVEL_TYPE)stOutput.nValidLevel;
                     }
                     else
                     {
                         cbValidLevel.SelectedItem = (LEVEL_TYPE_EN)stOutput.nValidLevel;
                     }
                     tbDurationTime.Text = stOutput.nDurationTime.ToString();
                }
                else
                {
                     LOG("The output mode is wrong.");
                     goto MyExit;
                }
            MyExit: ;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return nRet;
        }

        private int CheckOutputParam()
        {
            
            int nRet = CIOControllerSDK.MV_OK;
            UInt32 nGOPTuration = UInt32.Parse(tbDurationTime.Text);
            UInt32 nGPeriod = UInt32.Parse(tbPulsePeriod.Text);
            UInt32 nGPOPulseWidth = UInt32.Parse(tbPulseWidth.Text);

            if ((cbOutputPattern.Text == "多脉冲")||(cbOutputPattern.Text == "Multi_Pulse"))  //Multi-Pulse  Dutime > period >pulse width
            {
                if (nGPeriod > m_nMaxDurationTimeForSingle ||
                   nGPOPulseWidth > m_nMaxDurationTimeForSingle)
                {
                    MessageBox.Show("The parameter exceeds the max. value of 65535 ms.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nRet = CIOControllerSDK.MV_E_PARAMETER;
                }
             
                if (nGPeriod == 0 || nGPOPulseWidth == 0)
                {
                    MessageBox.Show("The pulse period or the pulse width of the multi-pulse cannot be 0.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nRet = CIOControllerSDK.MV_E_PARAMETER;
                }

           
                if (nGOPTuration < nGPeriod ||
                    nGOPTuration < nGPOPulseWidth ||
                    nGPeriod < nGPOPulseWidth)
                {
                    MessageBox.Show("The parameter exceeds the max. value of 65535 ms.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nRet = CIOControllerSDK.MV_E_PARAMETER;
                }
           
                // ch:Duration time 必须是周期的整数倍 | en:Duration time must be an integral multiple of the period 
                if ((0 != nGOPTuration % nGPeriod) &&
                    m_nMaxDurationTimeForPWM != nGOPTuration)
                {
                    MessageBox.Show("The pulse duration should be an integral multiple of the pulse period.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nRet = CIOControllerSDK.MV_E_PARAMETER;
                }
            
            }
            else if ((cbOutputPattern.Text == "单脉冲")||(cbOutputPattern.Text == "Single-Pulse"))
            {
                if (nGOPTuration > m_nMaxDurationTimeForSingle)
                {
                    MessageBox.Show("The parameter exceeds the max. value of 65535 ms.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    nRet = CIOControllerSDK.MV_E_PARAMETER;
                }
            }
            return nRet;
        }
        /// <summary>
        /// ch:输出设置应用 | en:Output settings application 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutputSet_Click(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_OK;
                nRet = CheckOutputParam();
                if (CIOControllerSDK.MV_E_PARAMETER == nRet)
                {
                    return;
                }
              

                CIOControllerSDK.MV_IO_SET_OUTPUT stOutput = new CIOControllerSDK.MV_IO_SET_OUTPUT();
                stOutput.nPort = (UInt32)(CIOControllerSDK.MV_IO_PORT_NUMBER)cbOutputPort.SelectedItem;
                stOutput.nType = (UInt32)(CIOControllerSDK.MV_IO_PATTERN_OUT)cbOutputPattern.SelectedItem;
                stOutput.nPulseWidth = UInt32.Parse(tbPulseWidth.Text);
                stOutput.nPulsePeriod = UInt32.Parse(tbPulsePeriod.Text);
                stOutput.nDurationTime = UInt32.Parse(tbDurationTime.Text);
                stOutput.nValidLevel = (UInt32)(CIOControllerSDK.MV_IO_LEVEL)cbValidLevel.SelectedItem;
                nRet = CIOControllerSDK.MV_IO_SetOutput_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stOutput);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Sending the command of output settings failed. Error code:" + Convert.ToString(nRet, 16)); 
                    return;
                }
                else
                {
                    LOG("The command of output settings is sent."); 
                }
            }
            catch (System.Exception ex)
            {
                 LOG("Sending the command failed. Parameters or operations exception occurs."); 
            }
            finally
            {
            }
        }

        /// <summary>
        /// ch:失去输入焦点事件 | en:Lost input focus event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxForNumberFocusLeave(object sender, EventArgs e)
        {
            if ("" == ((TextBox)sender).Text)
            {
                ((TextBox)sender).Text = "0";
            }

            UInt16 pulseWidth = UInt16.Parse(tbPulseWidth.Text);
            UInt16 pulsePeriod = UInt16.Parse(tbPulsePeriod.Text);
            UInt32 pulseDuration = 0;
            try
            {
                pulseDuration = ("Max" == tbDurationTime.Text ? m_nMaxPulsePeriod : UInt32.Parse(tbDurationTime.Text));
            }
            catch (System.Exception ex)
            {
                tbDurationTime.Text = m_nMaxPulsePeriod.ToString();
                pulseDuration = m_nMaxPulsePeriod;
                return;
            }
           
            if ((PATTERN_OUT.多脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem)||(PATTERN_OUT_EN.Multi_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
            {
                // ch:持续时间 | en:Duration Time
                if (tbDurationTime == (TextBox)sender)
                {
                    if (pulseDuration < pulsePeriod)
                    {
                        MessageBox.Show("The pulse duration should be larger than the pulse period.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbDurationTime.Text = tbPulsePeriod.Text;
                    }
                }
                // ch:脉冲周期 | en:Pulse Period 
                if (tbPulsePeriod == (TextBox)sender)
                {
                    if (pulsePeriod < pulseWidth)
                    {
                        MessageBox.Show("The pulse period should be larger than the pulse width.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbPulsePeriod.Text = tbPulseWidth.Text;
                    }

                    if (pulseDuration <= m_nMaxPulsePeriod)
                    {
                        if (pulsePeriod > pulseDuration)
                        {
                            MessageBox.Show("The pulse period should be smaller than the duration.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbPulsePeriod.Text = tbDurationTime.Text;
                        }
                    }
                    else
                    {
                        if (pulsePeriod > m_nMaxPulsePeriod)
                        {
                            MessageBox.Show("The max. pulse period is 65535 ms.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbPulsePeriod.Text = m_nMaxPulsePeriod.ToString();
                        }
                    }
                }

                // pulse Width
                if (tbPulseWidth == (TextBox)sender)
                {
                    if (pulseWidth > pulsePeriod)
                    {
                        MessageBox.Show("The pulse width should be smaller than pulse period.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbPulseWidth.Text = tbPulsePeriod.Text;
                    }
                }
            }
            else if ((PATTERN_OUT.单脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem)||(PATTERN_OUT_EN.Single_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
            {
                // Duration Time
                if (tbDurationTime == (TextBox)sender)
                {
                    if (pulseDuration > m_nMaxDurationTimeForSingle)
                    {
                        MessageBox.Show("The max. pulse duration is 65535 ms.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tbDurationTime.Text = m_nMaxDurationTimeForSingle.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// ch:输出模式选择 | en:Output mode selection 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbOutputPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != cbOutputPattern.SelectedItem)
            {
               if ((PATTERN_OUT.多脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem)||(PATTERN_OUT_EN.Multi_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
               {
                   cbValidLevel.Enabled = true;
                   tbDurationTime.Enabled = true;
                   tbPulsePeriod.Enabled = true;
                   tbPulseWidth.Enabled = true;
                   if ("" != tbPulseWidth.Text && "" != tbPulsePeriod.Text && "" != tbDurationTime.Text)
                   {
                       UInt16 pulseWidth = UInt16.Parse(tbPulseWidth.Text);
                       UInt16 pulsePeriod = UInt16.Parse(tbPulsePeriod.Text);
                       UInt32 pulseDuration = 0;
                       try
                       {
                           pulseDuration = "Max" == tbDurationTime.Text ? 0xFFFF : UInt32.Parse(tbDurationTime.Text);
                       }
                       catch (System.Exception ex)
                       {
                           tbDurationTime.Text = "Max";
                           pulseDuration = 0xFFFF;
                       }

                       if (pulsePeriod > pulseDuration)
                       {
                           pulsePeriod = (ushort)pulseDuration;
                           tbPulsePeriod.Text = pulsePeriod.ToString();
                       }
                       if (pulseWidth > pulsePeriod)
                       {
                           tbPulseWidth.Text = pulsePeriod.ToString();
                       }
                   }
               }
               else if ((PATTERN_OUT_EN.Single_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem)||(PATTERN_OUT.单脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem))
               {
                   if ("Max" == tbDurationTime.Text || UInt32.Parse(tbDurationTime.Text) > m_nMaxDurationTimeForSingle)
                   {
                       tbDurationTime.Text = m_nMaxDurationTimeForSingle.ToString();
                   }
                   tbPulsePeriod.Enabled = false;
                   tbPulseWidth.Enabled = false;
                   cbValidLevel.Enabled = true;
                   tbDurationTime.Enabled = true;
               }
            }
        }

        /// <summary>
        /// ch:脉冲宽度输入 | en:Pulse width input 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbPulseWidth_TextChanged(object sender, EventArgs e)
        {
            if (tbPulseWidth.Text == "0" || tbPulsePeriod.Text == "0")
            {
                return;
            }
            try
            {
                UInt32 pulseWidth = UInt32.Parse(tbPulseWidth.Text);
                UInt32 pulsePeriod = UInt32.Parse(tbPulsePeriod.Text);
                if (pulsePeriod == 0)
                {
                    tbPulseWidth.Text = 0.ToString();
                }
                if (pulseWidth > m_nMaxPulseWidth)
                {
                    tbPulseWidth.Text = "65535";
                }

                if (pulseWidth > pulsePeriod)
                {
                   tbPulseWidth.Text = tbPulsePeriod.Text;
                }
            }
            catch (System.Exception ex)
            {
                rtbMessage.AppendText(ex.ToString() + "\r\n");
            }
        }

        /// <summary>
        /// ch:字符串转16进制字符串 | en:String to hexadecimal string 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string StrToHex(string str)
        {
            GC.Collect();
            string strResult = string.Empty;
            byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(str);
            foreach (byte b in buffer)
            {
                strResult += b.ToString("X2") + " "; // ch:X是16进制大写格式 | en:X is in hexadecimal uppercase 
            }
            return strResult;
        }

        /// <summary>
        /// ch:判断是否十六进制格式字符串 | en:Determine whether hexadecimal format string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsHexadecimal(string str)
        {
            const string PATTERN = @"[A-Fa-f0-9]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(str, PATTERN);
        }

        /// <summary>
        ///  ch:将字符串转为16进制字符，允许中文 | en:Convert string to hexadecimal character, Chinese is allowed 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private string StrToHexStr(string s, Encoding MyEncode)
        {
            byte[] b = MyEncode.GetBytes(s);
            return ByteToHexStr(b, b.Length);
        }

        /// <summary>
        /// ch:将byte[]转为16进制字符串 | en:Convert byte [] to hexadecimal string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string ByteToHexStr(byte[] bytes, int nLen)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < nLen; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                    returnStr += " ";
                }
            }
            return returnStr;
        }

        /// <summary>
        /// ch:设置文本 | en:Set text 
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="strText"></param>
        private void SetText(Control ctrl, string strText)
        {
            ctrl.Text = strText;
        }

        /// <summary>
        /// ch:光源设置应用 | en:Light source settings 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLightSet_Click(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_OK;
                CIOControllerSDK.MV_IO_LIGHT_PARAM stParam = new CIOControllerSDK.MV_IO_LIGHT_PARAM();
                stParam.nPortNumber = (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER)(cbLightPort.SelectedItem);
                string strDurationTime = textLightTime.Text;                   
                stParam.nLightValue = UInt16.Parse(tbLightValue.Text);
                stParam.nLightState = (UInt16)(rdbLightStateOn.Checked ? CIOControllerSDK.MV_IO_LIGHTSTATE.MV_IO_LIGHTSTATE_ON : CIOControllerSDK.MV_IO_LIGHTSTATE.MV_IO_LIGHTSTATE_OFF);
                stParam.nLightEdge = (UInt16)(rbRisingEdge.Checked ? CIOControllerSDK.MV_IO_EDGE.MV_IO_EDGE_RISING : CIOControllerSDK.MV_IO_EDGE.MV_IO_EDGE_DOWN);
                if (strDurationTime == "Max" || strDurationTime == "max" || strDurationTime == "MAX" || strDurationTime == "MAx" || strDurationTime == "maX")
                {
                    stParam.nDurationTime = 0xFFFF;
                }
                else
                {
                    stParam.nDurationTime = Convert.ToUInt16(strDurationTime);
                }
                
                nRet = CIOControllerSDK.MV_IO_SetLightParam_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stParam);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Sending the command of light source settings failed. Error code:" + Convert.ToString(nRet, 16));
                    return;
                }
                else
                {
                   LOG("The command of light source settings is sent.");
                }
            }
            catch (System.Exception ex)
            {
                  LOG("Sending the command failed. Parameters or operations exception occurs.");
            }
            finally
            {
            }
        }

        private void VisionController_Resize(object sender, EventArgs e)
        {
            float[] scale = (float[])Tag;
            if (null == scale)
            {
                return;
            }
            int i = 2;
            List<Control> ctrls = MyTool.GetCtrls(this);
            foreach (Control ctrl in ctrls)
            {
                ctrl.Left = (int)(Size.Width * scale[i++]);
                ctrl.Top = (int)(Size.Height * scale[i++]);
                ctrl.Width = (int)(Size.Width / (float)scale[0] * ((Size)ctrl.Tag).Width);
                ctrl.Height = (int)(Size.Height / (float)scale[1] * ((Size)ctrl.Tag).Height);

            }
        }

        private void tbPulsePeriod_TextChanged(object sender, EventArgs e)
        {
            if (tbDurationTime.Text == "0" || tbPulsePeriod.Text == "0")
            {
                return;
            }
            try
            {
                UInt32 pulseDuration = ("Max" == tbDurationTime.Text ? 0xffff : UInt32.Parse(tbDurationTime.Text));
                UInt32 pulsePeriod = UInt32.Parse(tbPulsePeriod.Text);
                if (pulsePeriod == 0)
                {
                    tbPulsePeriod.Text = 1.ToString();
                }
                if (pulsePeriod > m_nMaxPulsePeriod)
                {
                    tbPulsePeriod.Text = "65535";
                }
                else
                {
                    if (pulseDuration <= m_nMaxPulsePeriod)
                    {
                        if (pulsePeriod > pulseDuration)
                        {
                            MessageBox.Show("The pulse period should be smaller than the duration.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tbPulsePeriod.Text = tbDurationTime.Text;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                rtbMessage.AppendText(ex.ToString() + "\r\n");
            }
        }

        private void tbDurationTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                ulong nValue = 0;
                if (isStrNum(tbDurationTime.Text, out nValue))
                {
                    if ((PATTERN_OUT.单脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem)||(PATTERN_OUT_EN.Single_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
                    {
                        // Duration Time
                        if (nValue > m_nMaxDurationTimeForSingle)
                        {
                            tbDurationTime.Text = m_nMaxDurationTimeForSingle.ToString();
                        }

                    }
                    else if ((PATTERN_OUT.多脉冲 == (PATTERN_OUT)cbOutputPattern.SelectedItem)||(PATTERN_OUT_EN.Multi_Pulse == (PATTERN_OUT_EN)cbOutputPattern.SelectedItem))
                    {
                        if (nValue > m_nMaxDurationTimeForPWM)
                        {
                            tbDurationTime.Text = m_nMaxDurationTimeForPWM.ToString();
                        }
                        if (nValue == 0)
                        {
                            tbDurationTime.Text = 0.ToString();
                        }
                    }
                }
                else
                {
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// ch:判断字符串是否为数字字符串 | en:Determine whether the string is a numeric string 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool isStrNum(string message, out ulong result)
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^\d+$");
            result = 0;
            if (rex.IsMatch(message))
            {
                result = ulong.Parse(message);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void tbInputDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ulong nValue = 0;
                if (isStrNum(tbInputDelay.Text, out nValue))
                {
                    if (nValue > m_nMaxInputDelay)
                    {
                       tbInputDelay.Text = m_nMaxInputDelay.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbGlitchTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ulong nValue = 0;
                if (isStrNum(tbGlitchTime.Text, out nValue))
                {
                    if (nValue > m_nMaxGlitchTime)
                    {
                       tbGlitchTime.Text = m_nMaxGlitchTime.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbComNo_SelectedIndexChanged(object sender, EventArgs e)
        {
             CIOControllerSDK.nComIndex = cbComNo.SelectedIndex;
            if (IntPtr.Zero == CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex])
            {
                //ch:这个串口对应的设备没有被打开，串口可以操作 | en:The device corresponding to this serial port is not opened, and the serial port can be operated 
                cbBauderate.Enabled = true;
                cbDataBits.Enabled = true;
                cbStopBits.Enabled = true;
                cbParityBits.Enabled = true;

                btnOpenOrCloseCom.Enabled = true;

                if (0x0804 == nLCID)
                {
                    btnOpenOrCloseCom.Text = m_strOpenCom;
                }
                else
                {
                    btnOpenOrCloseCom.Text = m_strOpenCom_EN;
                }
            }
            else
            {
                // ch:表明这个设备已经被打开，串口不能被操作
                // en:The device has been opened and the serial port cannot be operated 
                cbBauderate.SelectedItem = 115200; 
                cbBauderate.Enabled = false;

                cbDataBits.SelectedItem = 8; 
                cbDataBits.Enabled = false;

                cbStopBits.SelectedItem = 1f; 
                cbStopBits.Enabled = false;

                cbParityBits.SelectedItem = PARITY_SCHEME.None; 
                cbParityBits.Enabled = false;

                btnOpenOrCloseCom.Enabled = true;
                if (0x0804 == nLCID)
                {
                    btnOpenOrCloseCom.Text = m_strCloseCom; //ch:显示关闭串口 | en:Serial port close
                }
                else
                {
                    btnOpenOrCloseCom.Text = m_strCloseCom_EN;
                }

                //ch:加载各种参数 | en:Loading parameters 
                if (CIOControllerSDK.m_hDeviceTypeList[cbComNo.SelectedIndex] == m_strIOVersion)
                {
                    EnableIO();
                    DisEnableLightParams();
                    GetIOParams();
                    lbFirmwareVersion.Text = strIOVersion;
                  
                }

                if (CIOControllerSDK.m_hDeviceTypeList[cbComNo.SelectedIndex] == m_strLightVersion)
                {
                    EnableLightParams();
                    DisEnableIO();
                    GetLightParams();
                    lbFirmwareVersion.Text = strLightVersion;
                }
            }
        }

        private void VisionController_Load(object sender, EventArgs e)
        {

            // ch:初始化winio | en:Init Win IO
            int nRet = CIOControllerSDK.MV_OK;
            //nRet = CIOControllerSDK.MV_IO_WinIO_Init_CS();
            //if (nRet != CIOControllerSDK.MV_OK)
            //{
            //    LOG("win IO Init failed.");
            //    CIOControllerSDK.MV_IO_WinIO_DeInit_CS();
            //}
            //else
            //{
            //    LOG("Win IO Init success.");
            //}

            CIOControllerSDK.MV_IO_VERSION stVersion = new CIOControllerSDK.MV_IO_VERSION();
            CIOControllerSDK.MV_IO_GetSDKVersion_CS(ref stVersion);
            lbSDKVersion.Text = stVersion.nMainVersion + "." + stVersion.nSubVersion + "." + stVersion.nModifyVersion
                    + " " + stVersion.nYear.ToString("0000") + stVersion.nMonth.ToString("00") + stVersion.nDay.ToString("00");

            btnOpenOrCloseCom_TextChanged(null, null);
        }


        private void btnGetIOLevel_Click(object sender, EventArgs e)
        {
            btnGetIOLevel.Enabled = false;
            GetInputLevel();
            btnGetIOLevel.Enabled = true;
        }

        // ch:获取版本信息 | en:Get input level 
        public int GetInputLevel()
        {
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                lbIOLevel0.BackColor = Color.White;
                lbIOLevel1.BackColor = Color.White;
                lbIOLevel2.BackColor = Color.White;
                lbIOLevel3.BackColor = Color.White;
                lbIOLevel4.BackColor = Color.White;
                lbIOLevel5.BackColor = Color.White;
                lbIOLevel6.BackColor = Color.White;
                lbIOLevel7.BackColor = Color.White;
                CIOControllerSDK.MV_IO_INPUT_LEVEL stInputLevel = new CIOControllerSDK.MV_IO_INPUT_LEVEL();
                stInputLevel.nPortNumber = 0x0F;
                nRet = CIOControllerSDK.MV_IO_GetInputLevel_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stInputLevel);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Getting the electrical level status failed."); 
                    goto MyExit;
                }

                // ch:更新界面显示 | en:Update interface display 
                lbIOLevel0.BackColor = (0 == stInputLevel.nLevel0) ? Color.Green : Color.Red;
                lbIOLevel1.BackColor = (0 == stInputLevel.nLevel1) ? Color.Green : Color.Red;
                lbIOLevel2.BackColor = (0 == stInputLevel.nLevel2) ? Color.Green : Color.Red;
                lbIOLevel3.BackColor = (0 == stInputLevel.nLevel3) ? Color.Green : Color.Red;
                lbIOLevel4.BackColor = (0 == stInputLevel.nLevel4) ? Color.Green : Color.Red;
                lbIOLevel5.BackColor = (0 == stInputLevel.nLevel5) ? Color.Green : Color.Red;
                lbIOLevel6.BackColor = (0 == stInputLevel.nLevel6) ? Color.Green : Color.Red;
                lbIOLevel7.BackColor = (0 == stInputLevel.nLevel7) ? Color.Green : Color.Red;

                LOG("The status of the input electrical level is obtained.");

            MyExit: ;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return nRet;
        }

        private void btnEdgeDetection_Click(object sender, EventArgs e)
        {
            try
            {
                if ((btnEdgeDetection.Text == "Disable Edge Detection")||(btnEdgeDetection.Text == "关闭沿检测"))
                {
                    // ch:打开->关闭 | en:Open-> Close
                    int nRet = CIOControllerSDK.MV_IO_RegisterEdgeDetectionCallBack_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], null, IntPtr.Zero);

                    if (CIOControllerSDK.MV_OK == nRet)
                    {
                        if (0x0804 == nLCID)
                        {
                            this.Invoke(new Action(() => { btnEdgeDetection.Text = "开启沿检测"; }));
                        }
                        else
                        {
                            this.Invoke(new Action(() => { btnEdgeDetection.Text = "Enable Edge Detection"; }));
                        }
                        
                    }
                }
                else if ((btnEdgeDetection.Text == "Enable Edge Detection") || (btnEdgeDetection.Text == "开启沿检测"))
                {
                    // ch:关闭->开启 | en:Close ->Open
                    EdgeDetectionCallBack = new CIOControllerSDK.cbOutputdelegate(CallBackFunc);
                    int nRet = CIOControllerSDK.MV_IO_RegisterEdgeDetectionCallBack_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], EdgeDetectionCallBack, IntPtr.Zero);

                    if (CIOControllerSDK.MV_OK == nRet)
                    {
                        if (0x0804 == nLCID)
                        {
                            this.Invoke(new Action(() => { btnEdgeDetection.Text = "关闭沿检测"; }));
                        }
                        else
                        {
                            this.Invoke(new Action(() => { btnEdgeDetection.Text = "Disable Edge Detection"; }));
                        }
                    }
                    else
                    {
                        LOG("Create the event failed. Reboot the hardware."); 
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbInputPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ch:获取不同输入端口的参数 | en:Get the parameters of different input ports 
            if ((m_strCloseCom == btnOpenOrCloseCom.Text) || (m_strCloseCom_EN == btnOpenOrCloseCom.Text)) //表示设备已经打开
            {
                int inputSelectIndex = cbInputPort.SelectedIndex + 1;
                GetInputSet(inputSelectIndex);
            }
        }

        // ch:获取输入端口参数设置信息 | en:Get input port parameter settings
        private int GetInputSet(int nPort) // ch:1~8，表示8个输入端口 | en:1 ~ 8, representing 8 input ports 
        {
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                int nPortFlag = Convert.ToInt32(Math.Pow(2, (double)(nPort - 1)));

                CIOControllerSDK.MV_IO_SET_INPUT stInput = new CIOControllerSDK.MV_IO_SET_INPUT();
                stInput.nPort = Convert.ToUInt32(nPortFlag);
                nRet = CIOControllerSDK.MV_IO_GetPortInputParam_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stInput);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Getting input port " + nPort.ToString() + " information failed. Error code:" + Convert.ToString(nRet, 16));
                    goto MyExit;
                }
                // ch:更新界面显示 | en:Update interface display 
                if (0x0804 == nLCID)
                {
                    cbInputEdgeType.SelectedItem = (EDGE_TYPE)stInput.nEdge;
                    comboBoxEdgeInfoEnable.SelectedItem = (EDGE_INFO_ENABLE)stInput.nEnable;
                }
                else
                {
                    cbInputEdgeType.SelectedItem = (EDGE_TYPE_EN)stInput.nEdge;
                    comboBoxEdgeInfoEnable.SelectedItem = (EDGE_INFO_ENABLE_EN)stInput.nEnable;
                }
                
                tbInputDelay.Text = stInput.nDelayTime.ToString();
                tbGlitchTime.Text = stInput.nGlitch.ToString();

                LOG("The input port" + nPort.ToString() + " information is obtained.");
            MyExit: ;
                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return nRet;
        }


       
        private void textLightTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ulong nValue = 0;
                if (isStrNum(textLightTime.Text, out nValue))
                {
                    // ch:光源亮度持续时间 | en:Light source brightness duration 
                    if (nValue > m_nMaxLightTime)
                    {
                        MessageBox.Show("The max. light source time is 65535 ms.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textLightTime.Text = m_nMaxLightTime.ToString();
                    }
                }
                else
                {
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnResetParam_Click(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_IO_ResetParam_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                if (CIOControllerSDK.MV_OK == nRet)
                {
                    LOG("Reset.");
                }
                else
                {
                    LOG("Reseting parameters failed.");
                }
                // ch:获取光源 | en:Get light source 
                if ((m_strCloseCom == btnOpenOrCloseCom.Text) || (m_strCloseCom_EN == btnOpenOrCloseCom.Text)) //表示设备已经打开
                {
                    GetLightParams();
                }
                // ch:获取不同输入端口的参数 | en:Get the parameters of input ports 
                if ((m_strCloseCom == btnOpenOrCloseCom.Text) || (m_strCloseCom_EN == btnOpenOrCloseCom.Text)) //表示设备已经打开
                {
                    int inputSelectIndex = cbInputPort.SelectedIndex + 1;
                    GetInputSet(inputSelectIndex);
                }
                // ch:获取输出端口参数信息 | en:Get the parameters of output ports 
                if ((m_strCloseCom == btnOpenOrCloseCom.Text) || (m_strCloseCom_EN == btnOpenOrCloseCom.Text))
                {
                    GetOutputSet(cbOutputPort.SelectedIndex + 1);
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {

            try
            {
                int nRet = CIOControllerSDK.MV_IO_Reboot_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                if (CIOControllerSDK.MV_OK == nRet)
                {
                    LOG("The command of rebooting the hardware is sent.");
                    CIOControllerSDK.MV_IO_Close_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                    CIOControllerSDK.MV_IO_DestroyHandle_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                    EnableSerailParams();
                    DisEnableIO();
                    DisEnableLightParams();
                    DisEnableOtherParams();
                    //DisableIOParams();
                    if (0x0804 == nLCID)
                    {
                        btnOpenOrCloseCom.Text = m_strOpenCom;
                    }
                    else
                    {
                        btnOpenOrCloseCom.Text = m_strOpenCom_EN;
                    }
                    CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex] = IntPtr.Zero;
                    // 关闭串口
                }
                else
                {
                    LOG("Sending the command of rebooting the hardware failed.");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void cbLightPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((m_strCloseCom == btnOpenOrCloseCom.Text) || (m_strCloseCom_EN == btnOpenOrCloseCom.Text)) //表示设备已经打开
            {
                GetLightParams();
            }
        }

        private void GetLightParams()
        {
            int nRet = CIOControllerSDK.MV_OK;
            CIOControllerSDK.MV_IO_LIGHT_PARAM stParam = new CIOControllerSDK.MV_IO_LIGHT_PARAM();
            stParam.nPortNumber = (Byte)(PORT_LIGHT_NUMBER)cbLightPort.SelectedItem;

            nRet = CIOControllerSDK.MV_IO_GetLightParam_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stParam);
            if (CIOControllerSDK.MV_OK != nRet)
            {
                LOG("Sending the command of getting light source failed. Error code:" + Convert.ToString(nRet, 16));
                return;
            }
            else
            {
                LOG("The command of getting light source is sent.");
            }
            if ((1 == (int)(LIGHT_STATE)stParam.nLightState) || (1 == (int)(LIGHT_STATE_EN)stParam.nLightState))
            {
                rdbLightStateOn.Checked = true;
            }
            else
            {
                rdbLightStateOff.Checked = true;
            }

            if ((0x01 == (int)(EDGE_TYPE)stParam.nLightEdge) || (0x01 == (int)(EDGE_TYPE_EN)stParam.nLightEdge))
            {
                rbRisingEdge.Checked = true;
            }
            else
            {
                rbDownEdge.Checked = true;
            }
            textLightTime.Text = stParam.nDurationTime.ToString();
            tbLightValue.Text = stParam.nLightValue.ToString();
        }



        private void btGetGPI_Click(object sender, EventArgs e)
        {
            try
            {
                btGetGPI.Enabled = false;
                GetMainGPIInputLevel();
                btGetGPI.Enabled = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public int GetMainGPIInputLevel()
        {
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                lblGPI1.BackColor = Color.White;
                lblGPI2.BackColor = Color.White;
                lblGPI3.BackColor = Color.White;

                byte[] byteStatus = new byte[1024];

                nRet = CIOControllerSDK.MV_IO_GetMainInputLevel_CS(ref byteStatus[0]);  // 用字节数组接收动态库传过来的字符串
                int nGPIStatus = BitConverter.ToInt32(byteStatus, 0);

                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Getting the electrical level status failed.");
                    goto MyExit;
                }

               if ((nGPIStatus & 0x01) == 0x01)
                {
                    lblGPI1.BackColor = Color.Red;
                }
                else
                {
                    lblGPI1.BackColor = Color.Green;
                }

                if ((nGPIStatus & 0x02) == 0x02)
                {
                    lblGPI2.BackColor = Color.Red;
                }
                else
                {
                    lblGPI2.BackColor = Color.Green;
                }

                if ((nGPIStatus & 0x04) == 0x04)
                {
                    lblGPI3.BackColor = Color.Red;
                }
                else
                {
                    lblGPI3.BackColor = Color.Green;
                }
                LOG("The status of the input electrical level is obtained.");

            MyExit: ;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return nRet;
        }


        private void cbPNPEnable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((m_strCloseCom == btnOpenOrCloseCom.Text) || (m_strCloseCom_EN == btnOpenOrCloseCom.Text)) 
            {
                try
                {
                    int nRet = CIOControllerSDK.MV_OK;
                    int nType = (int)(CIOControllerSDK.MV_IO_PNP_TYPE)(cbPNPEnable.SelectedItem);
                    nRet = CIOControllerSDK.MV_IO_ExcutePNPEnable_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], nType);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of enabling the polarity failed. Error code:" + Convert.ToString(nRet, 16));
                        return;
                    }
                    else
                    {
                        PrintMessage("The command of enabling the polarity is sent.");

                    }
                }
                catch (System.Exception ex)
                {
                    LOG("Sending the command failed. Parameters or operations exception occurs.");
                }
            }
        }

        private void btnOutPutEnable_Click_1(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_OK;

                CIOControllerSDK.MV_IO_OUTPUT_ENABLE stParam = new CIOControllerSDK.MV_IO_OUTPUT_ENABLE();
                stParam.nPortNumber = m_nOutputEnablePort;
                stParam.nType = (Byte)CIOControllerSDK.MV_IO_ENABLE_TYPE.MV_IO_ENABLE_START;//((ENABLE_STATE.关闭使能 == (ENABLE_STATE)cbOutputEnable.SelectedItem) ? (CIOControllerSDK.MV_IO_ENABLE_TYPE.MV_IO_ENABLE_END) : (CIOControllerSDK.MV_IO_ENABLE_TYPE.MV_IO_ENABLE_START));

                nRet = CIOControllerSDK.MV_IO_SetOutputEnable_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stParam);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Sending the command of enabling output failed. Error code:" + Convert.ToString(nRet, 16));
                    return;
                }
                else
                {
                    LOG("The command of enabling output is sent.");
                }
            }
            catch (System.Exception ex)
            {
                LOG("Sending the command failed. Parameters or operations exception occurs.");
            }
        }

        private void btnOutPutUnEnable_Click(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_OK;

                CIOControllerSDK.MV_IO_OUTPUT_ENABLE stParam = new CIOControllerSDK.MV_IO_OUTPUT_ENABLE();
                stParam.nPortNumber = m_nOutputEnablePort;
                //((ENABLE_STATE.关闭使能 == (ENABLE_STATE)cbOutputEnable.SelectedItem) ? (CIOControllerSDK.MV_IO_ENABLE_TYPE.MV_IO_ENABLE_END) : (CIOControllerSDK.MV_IO_ENABLE_TYPE.MV_IO_ENABLE_START));
                stParam.nType = (Byte)CIOControllerSDK.MV_IO_ENABLE_TYPE.MV_IO_ENABLE_END;

                nRet = CIOControllerSDK.MV_IO_SetOutputEnable_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex], ref stParam);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Sending the command of disabling output failed. Error code:" + Convert.ToString(nRet, 16));
                    return;
                }
                else
                {
                    LOG("The command of disabling output is sent.");
                }
            }
            catch (System.Exception ex)
            {
                 LOG("Sending the command failed. Parameters or operations exception occurs.");
            }
        }


        private void ckbEnableOutPort0_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbEnableOutPort0.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_1);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_1));
            }
        }

        private void ckbEnableOutPort1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEnableOutPort1.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_2);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_2));
            }
            
        }

        private void ckbEnableOutPort2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbEnableOutPort2.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_3);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_3));
            }
        }

        private void ckbEnableOutPort3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbEnableOutPort3.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_4);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_4));
            }
        }

        private void ckbEnableOutPort4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ckbEnableOutPort4.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_5);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_5));
            }
        }

        private void ckbEnableOutPort5_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEnableOutPort5.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_6);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_6));
            }
        }

        private void ckbEnableOutPort6_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEnableOutPort6.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_7);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_7));
            }
        }

        private void ckbEnableOutPort7_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbEnableOutPort7.Checked)
            {
                m_nOutputEnablePort |= (Byte)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_8);
            }
            else
            {
                m_nOutputEnablePort &= (Byte)(0xFF - (int)(CIOControllerSDK.MV_IO_PORT_NUMBER.MV_IO_PORT_8));
            }
        }

        private void ckbDbgView_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbDbgView.Checked)
            {
                CIOControllerSDK.MV_IO_SetDebugView_CS(1);
            }
            else
            {
                CIOControllerSDK.MV_IO_SetDebugView_CS(0);
            }
        }

        private void btApplyCOM_Click(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_OK;
                CIOControllerSDK.MV_RS_CONFIG stComconfig = new CIOControllerSDK.MV_RS_CONFIG();

                stComconfig.nCOM1Type = (UInt32)(CIOControllerSDK.COM_NUMBER)cbRS1.SelectedItem;
                stComconfig.nCOM2Type = (UInt32)(CIOControllerSDK.COM_NUMBER)cbRS2.SelectedItem;

                nRet = CIOControllerSDK.MV_IO_SetRSConfig_CS(ref stComconfig);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    LOG("Sending the command of RS232/485/422 failed. Error code:" + Convert.ToString(nRet, 16));
                    return;
                }
                else
                {
                    LOG("The command of RS232/485/422 is sent.");
                }
            }
            catch (System.Exception ex)
            {
                LOG("Sending the command failed. Parameters or operations exception occurs.");
            }
        }


        private void cbGainPNPEnable_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_OK;
                int nType = (int)(CIOControllerSDK.MV_IO_PNP_TYPE)cbGainPNPEnable.SelectedItem;
                nRet = CIOControllerSDK.MV_IO_SetMainGPO_NPN_CS(nType);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    PrintMessage("Sending the command of enabling the polarity failed. Error code:" + Convert.ToString(nRet, 16));
                    return;
                }
                else
                {
                    PrintMessage("The command of enabling the polarity is sent.");
                }
            }
            catch (System.Exception ex)
            {
               
            }
        }

        private void btGPOLow_Click(object sender, EventArgs e)
        {
            int nRet = CIOControllerSDK.MV_OK;
            bool bSelect = false;
            try
            {
                if (ckbGPO1.Checked)
                {
                    bSelect = true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_1;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO1 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO1.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO1.ForeColor = Color.Green;
                        LOG("The command of GPO1 Low Level settings is sent.");
                    }
                    
                }
                if (ckbGPO2.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_2;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO2 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO2.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO2.ForeColor = Color.Green;
                        LOG("The command of GPO2 Low Level settings is sent.");
                    }
                    
                }
                if (ckbGPO3.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_3;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO3 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO3.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO3.ForeColor = Color.Green;
                        LOG("The command of GPO3 Low Level settings is sent.");
                    }
                }
                if (ckbGPO4.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_4;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO4 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO4.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO4.ForeColor = Color.Green;
                        LOG("The command of GPO4 Low Level settings is sent.");
                    }
                }
                if (cbGPO5.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_5;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO5 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        cbGPO5.ForeColor = Color.Black;
                    }
                    else
                    {
                        cbGPO5.ForeColor = Color.Green;
                        LOG("The command of GPO5 Low Level settings is sent.");
                    }
                }
                if (ckbGPO6.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_6;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO6 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO6.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO6.ForeColor = Color.Green;
                        LOG("The command of GPO6 Low Level settings is sent.");
                    }
                }
                if (ckbGPO7.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_7;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO7 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO7.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO7.ForeColor = Color.Green;
                        LOG("The command of GPO7 Low Level settings is sent.");
                    }
                }
                if (ckbGPO8.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_8;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO8 Low Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO8.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO8.ForeColor = Color.Green;
                        LOG("The command of GPO8 Low Level settings is sent.");
                    }
                }
                if (bSelect == false)
                {
                    LOG("Disable failed.");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                LOG("Sending the command failed. Parameters or operations exception occurs.");
            }
        }

        private void btGPOHigh_Click(object sender, EventArgs e)
        {
            int nRet = CIOControllerSDK.MV_OK;
            bool bSelect = false;
            try
            {
                if (ckbGPO1.Checked)
                {
                    bSelect = true;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_1, CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO1 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO1.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO1.ForeColor = Color.Red;
                        LOG("The command of GPO1 High Level settings is sent.");
                    }
                    
                }
                if (ckbGPO2.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_2;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO2 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO2.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO2.ForeColor = Color.Red;
                        LOG("The command of GPO2 High Level settings is sent.");
                    }
                }
                if (ckbGPO3.Checked)
                {
                    bSelect = bSelect |true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_3;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO3 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO3.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO3.ForeColor = Color.Red;
                        LOG("The command of GPO3 High Level settings is sent.");
                    } 
                }
                if (ckbGPO4.Checked)
                {
                    bSelect = bSelect |true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_4;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO4 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO4.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO4.ForeColor = Color.Red;
                        LOG("The command of GPO4 High Level settings is sent.");
                    }
                }
                if (cbGPO5.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_5;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO5 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        cbGPO5.ForeColor = Color.Black;
                    }
                    else
                    {
                        cbGPO5.ForeColor = Color.Red;
                        LOG("The command of GPO5 High Level settings is sent.");
                    }
                }
                if (ckbGPO6.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_6;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO6 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO6.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO6.ForeColor = Color.Red;
                        LOG("The command of GPO6 High Level settings is sent.");
                    }
                }
                if (ckbGPO7.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_7;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO7 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO7.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO7.ForeColor = Color.Red;
                        LOG("The command of GPO7 High Level settings is sent.");
                    }
                }
                if (ckbGPO8.Checked)
                {
                    bSelect = bSelect | true;
                    CIOControllerSDK.MV_IO_MAINPORT_NUMBER nPortIn = CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_8;
                    CIOControllerSDK.MV_GIO_LEVEL nStatus = CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH;
                    nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(nPortIn, nStatus);
                    if (CIOControllerSDK.MV_OK != nRet)
                    {
                        LOG("Sending the command of GPO8 High Level settings failed. Error code:" + Convert.ToString(nRet, 16));
                        ckbGPO8.ForeColor = Color.Black;
                    }
                    else
                    {
                        ckbGPO8.ForeColor = Color.Red;
                        LOG("The command of GPO8 High Level settings is sent.");
                    }
                }
                if(bSelect == false)
                {
                    LOG("Enable failed.");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                 LOG("Sending the command failed. Parameters or operations exception occurs.");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //frmIOCtrlUpdate frmUpdate = new frmIOCtrlUpdate(this);
            //frmUpdate.ShowDialog();
            //frmUpdate.Dispose();
        }

        private void btnSaveparam_Click(object sender, EventArgs e)
        {
            try
            {
                int nRet = CIOControllerSDK.MV_IO_Saveparam_CS(CIOControllerSDK.m_hDeviceHandleList[cbComNo.SelectedIndex]);
                if (CIOControllerSDK.MV_OK == nRet)
                {
                    LOG("The command of save param suc.");
                }
                else
                {
                    LOG("Sending the command of save param failed.");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

     
    }
}
