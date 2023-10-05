using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.IO;
using AYNETTEK.UHFReader;
using System.Text.RegularExpressions;
using System.Net;
namespace LineGolden_PLasma
{
    public partial class Frm_RFID : Form
    {
        FrmAutoSize AUTOSIZE = new FrmAutoSize();
        public static string SelectedEpc = "";//EPC selected by dataGridview
        private System.Threading.Timer ShowStatistics;
        CheckBox[] ants = null;
        List<byte> WorkAntPlan = new List<byte>();//working antenna
        int TagCnt = 0;//number of different labels
        private const byte FIRST_READ_PORT = 0;
        long StartTime = 0;//Starting time
        long ExecutedTime = 0;//Existing execution time
        long ColorationInterval = 3000;//Timing with datetime.ticks, ticks/10000 is milliseconds
        private ReadMode_enum ReaderMode = ReadMode_enum.AT_MODE;
        private int ReadSingleTime;
        R2KReader objreader = new R2KReader();
        MyTagList TagList = new MyTagList();
       /// <summary>
       /// Khởi tạo contructor
       /// </summary>
        public Frm_RFID(R2KReader _objreader)
        {
            InitializeComponent();
            objreader = _objreader;
            objreader.SetEnableKeepAlive(true);
            //objreader.tcpDealKeepAliveEvent += new R2KReader.TcpDealKeepAliveEvent(RfidTcpDisconnectEvent);
        }
        /// <summary>
        /// Set auto size for Form RFID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_RFID_Resize(object sender, EventArgs e)
        {
            AUTOSIZE.controlAutoSize(this);
        }
        /// <summary>
        /// Return Object R2KReader đang sử dụng
        /// </summary>
        /// <returns></returns>
        public R2KReader get_objreader()
        {
            return this.objreader;
        }
        /// <summary>
        /// Load form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            AUTOSIZE.controllInitializeSize(this);
            ants = new CheckBox[] { checkBox_Ant1, checkBox_Ant2, checkBox_Ant3, checkBox_Ant4};
            //Bind stats operations to timers
            if (ShowStatistics != null)
            {
                ShowStatistics.Dispose();
            }
            ShowStatistics = new System.Threading.Timer((TimerCallback)ShowStatisticsInfo, null, Timeout.Infinite, 100);
        }
        /// <summary>
        /// Close Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (objreader!=null)
                {
                   // objreader.Dispose();
                }
                this.Dispose();
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region EVENT handling
        void objreader_TagRead(object sender, AutoReadEventArgs Args)
        {
            this.BeginInvoke((EventHandler)delegate
            {
                TagShow(this, Args);
            });
        }
        private void TagShow(object obj, AutoReadEventArgs tag)
        {
            try
            {
                TagCnt++;//增加标签数
                la_TagCount.Text = TagCnt.ToString();
                lock (gdvTagShow)
                {
                    //在list中根据EPC查找标签
                    int index = TagList.myFind(tag.EPC);
                    if (index == -1)//未找到
                    {
                        TagList.mylist.Add(new MyTag(tag));
                        //显示信息，并更改当前行的背景颜色
                        string tagEPC = Tools.HexStringToString(tag.EPC);
                        //serial number, EPC, counter, RSSI, antenna port, channel number
                        gdvTagShow.Rows.Add(TagList.mylist.Count, tagEPC, tag.GetAntStr(), tag.readCnt.ToString(), tag.RSSI.ToString());
                        gdvTagShow.Rows[TagList.mylist.Count - 1].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                    }
                    else
                    {

                        //更新信息
                        gdvTagShow[2, index].Value = tag.GetAntStr();
                        gdvTagShow[3, index].Value = Convert.ToInt32(gdvTagShow[3, index].Value) + tag.readCnt;
                        gdvTagShow[4, index].Value = tag.RSSI.ToString();

                        //更改标签读取时间和行背景颜色
                        TagList.mylist[index].GottenTime = tag.time;
                        gdvTagShow.Rows[index].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                    }
                }         
            }
            catch
            {
            }
        }
        #endregion

        //clear display
        private void buttonClear_Click(object sender, EventArgs e)
        {
            TagList.mylist.Clear();
            gdvTagShow.Rows.Clear();
            StatisticsInit();
        }
        #region Process Static Time
        //display initialization or clear static time
        private void StatisticsInit()
        {
            TagCnt = 0;                     
            ExecutedTime = 0;
            StartTime = Environment.TickCount;
        }

        //show statistics time
        private void ShowStatisticsInfo(object obj)//Statistics every 100ms
        {
            //Shield cross-thread call checking
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            //show statistics
            long UsedTime = (Environment.TickCount - StartTime) + ExecutedTime;
            TimeSpan timespan = new TimeSpan(UsedTime * 10000);
            lb_timeCnt.Text = string.Format("{0}:{1}:{2}:{3}.{4}",
            timespan.Days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10);
            //background color change
            //The color gradually becomes darker, and changes every ColorationInterval/4, and the ColorationInterval reaches the deepest (red)
            for (int i = 0; i < gdvTagShow.Rows.Count; i++)
            {
                if (gdvTagShow.Rows[i].DefaultCellStyle.BackColor != Color.Red)
                {
                    if (DateTime.Compare(TagList.mylist[i].GottenTime.AddMilliseconds(ColorationInterval), DateTime.Now) < 0)
                    {
                        gdvTagShow.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        continue;
                    }
                    if (DateTime.Compare(TagList.mylist[i].GottenTime.AddMilliseconds(ColorationInterval * 5 / 6), DateTime.Now) < 0)
                    {
                        gdvTagShow.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 64, 64);
                        continue;
                    }
                    if (DateTime.Compare(TagList.mylist[i].GottenTime.AddMilliseconds(ColorationInterval * 4 / 6), DateTime.Now) < 0)
                    {
                        gdvTagShow.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 128);
                        continue;
                    }
                    if (DateTime.Compare(TagList.mylist[i].GottenTime.AddMilliseconds(ColorationInterval / 2), DateTime.Now) < 0)
                    {
                        gdvTagShow.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 192);
                        continue;
                    }
                }
            }
        }
        #endregion



        #region DataGridView right-click menu
        //DataGridView右键
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {     
            if (Bt_ReadTag_On_Off.Text == "停止" )//读卡时屏蔽
            {
                return;
            }
            if (e.Button == MouseButtons.Right)//右键
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (gdvTagShow.Rows[e.RowIndex].Selected == false)
                    {
                        gdvTagShow.ClearSelection();
                        gdvTagShow.Rows[e.RowIndex].Selected = true;
                    }

                    //只允许选中一行
                    if (gdvTagShow.SelectedRows.Count > 1)
                    {
                        foreach (DataGridViewRow row in gdvTagShow.SelectedRows)
                        {
                            if (row.Index != e.RowIndex)
                            {
                                row.Selected = false;
                            }
                        }
                    }

                    //提取选中行的EPC
                    SelectedEpc = (string)gdvTagShow[1, e.RowIndex].Value;
                    selectIndex = TagList.myFind(SelectedEpc);
                }
            }
        }
        int selectIndex = 0;
        private void aCCESSToolStripMenuItem_Click(object sender, EventArgs e)
        {
         //   Access tag_acc = new Access( objreader,TagList.mylist[selectIndex]);
         //   tag_acc.ShowDialog();
        }
        #endregion


        #region "single card read"

        #region Datagridview变色
        DateTime lastSeenTagTimeStamp = DateTime.Now;
        #endregion

        private Thread ReadThread = null;
        private bool isInventory = true;

        
        /*public Status_enum AutoReadTag(ref TagReadData[] TagData,ref UInt32 TagCnt)
        {      
            List<TagReadData> tags = new List<TagReadData>();     
            ModbusStatus_enum retval = 0;
            UInt16 TagOnlineFlag = 0;
            UInt16 TagNum = 0;
            UInt16 ReadCacheCnt = 0;//读取标签缓存池的次数
            byte[] data = new byte[256];
            TagCnt = 0;
            if (TagData != null)
            {
                TagData = null;
            }
            try
            {
                retval = objreader.Modbus_ReadMultipleRegister(ModbusAddr.GetModbusAddr("TagOnlineFlag"), 2, ref data);
                if (retval != ModbusStatus_enum.SUCCESS)
                {
                    return Status_enum.TOCOM_ERROR;
                }
                TagOnlineFlag = Tools.ByteToU16(data, 0);
                TagNum = Tools.ByteToU16(data, 2);
            }
            catch
            {            
            }


            if ((TagOnlineFlag == 1) && (TagNum <= 200))
            {
                try
                {
                    while (TagNum > 0)
                    {

                        //每次最多读20个标签
                        UInt16 OnceReadTagNum = 0;
                        if (TagNum <= 10)
                        {
                            OnceReadTagNum = TagNum;
                            TagNum = 0;
                        }
                        else
                        {
                            OnceReadTagNum = 10;
                            TagNum -= 10;
                        }
                        //有标签在线
                        retval = objreader.Modbus_ReadMultipleRegister((UInt16)(ModbusAddr.GetModbusAddr("TagList") + ReadCacheCnt * 10*10), (UInt16)(OnceReadTagNum * 10), ref data);
                  
                        if (retval != ModbusStatus_enum.SUCCESS)
                        { 
                            return Status_enum.TOCOM_ERROR;                        
                        }

                        if (data.Length != OnceReadTagNum * 20)
                        { 
                            return Status_enum.TOCOM_ERROR;                        
                        }
                        for (int i = 0; i < OnceReadTagNum; i++)
                        {
                            TagReadData ReadData = new TagReadData();
                            ReadData.TagIndex = data[i * 20 + 0];
                           // if ((ReadData.TagIndex == 0) || (ReadData.TagIndex > 250)) continue;
                            ReadData.RSSI = ((sbyte)data[i * 20 + 1]).ToString();
                            ReadData.EPCTID_WordLen = data[i * 20 + 2];
                            ReadData.ant = data[i * 20 + 3];
                            if (ReadData.EPCTID_WordLen > 8) ReadData.EPCTID_WordLen = 8;
                            ReadData.EPC = Tools.ByteToHexString(data, i * 20 + 4, ReadData.EPCTID_WordLen * 2);
                            ReadData.time = DateTime.Now;
                            ReadData.readCnt = 1;
                            //Take diff of base time and last seen tag time
                            TimeSpan diffbwTwoTagsTime = ReadData.time.Subtract(lastSeenTagTimeStamp);
                            if (diffbwTwoTagsTime.TotalMilliseconds < 1)
                            {
                                //Adding the difference
                                ReadData.time = ReadData.time.Subtract(diffbwTwoTagsTime);
                                //adding 1 miilisecond to bas time
                                ReadData.time = ReadData.time.AddMilliseconds(1);
                            }
                            //Set last seen tag timestamp                                    
                            lastSeenTagTimeStamp = ReadData.time;
                            TagCnt++;
                            tags.Add(ReadData);
                        }
                        ReadCacheCnt++;                    
                    }                  
                }
                catch
                {
              
                }
                
            }
            TagData = new TagReadData[TagCnt];
            Array.Copy(tags.ToArray(), 0, TagData, 0, TagData.Length);
            return Status_enum.SUCCESS;
        }*/

        //异步线程触发开始按钮
        private bool Asynchronous_Bt_ReadTag_On_Off()
        {
            try
            {
                this.BeginInvoke((EventHandler)delegate
                {             
                    Bt_ReadTag_On_Off_Click(null, null);
                });
            }
            catch 
            {
              
                return false;
            }
            return true;
        }


        //触发读卡
        private void StartReadTag(object obj)
        {
            Status_enum retval;
            if (ReaderMode == ReadMode_enum.AUTOREAD_LINEBODY)
            {
                objreader.SetLineBodyAutoRead(AutoReadSwitch_enum.START);
            }
            objreader.SetClearTagCache();
            while (isInventory)
            {
                TagReadData[] tags = null;
                UInt32 Cnt = 0;
                
                if (ReaderMode == ReadMode_enum.AT_MODE)
                { 
                    //触发读卡模式，需demo主动发送读卡指令
                    retval = objreader.SetTriggerRead();
                    if (retval != Status_enum.SUCCESS)
                    {
                        SetStatus("Failed to perform answer card reading！", Color.Red);
                    }
                    else
                    {
                        SetStatus("Execute acknowledgment card reading success！", Color.Black);
                    }                       
                }
                Thread.Sleep(100 * ReadSingleTime * WorkAntPlan.Count + 50);
                objreader.AutoReadTag(ref tags, ref Cnt);//读取标签数据

                if (tags != null)
                { 
                    if (tags.Length != 0)
                    {          
                        AddResult(tags);                  
                    }                
                }
                objreader.SetClearTagCache();
            }
        }
        private void StopThreadReadTag()
        {
            isInventory = false;
            if (ReaderMode == ReadMode_enum.AUTOREAD_LOGISTICS)
            {
                AutoReadDataUploadSwitch(AutoReadSwitch_enum.STOP);//中止自动读卡（物流）数据喷发
                //删除事件，避免事件冲突
                objreader.AutoReadHandler -= new AYNETTEK.UHFReader.EventHandler<AutoReadEventArgs>(objreader_TagRead);
            }
            else
            {
                
                if(ReaderMode == ReadMode_enum.AUTOREAD_LINEBODY)
                {
                    objreader.SetLineBodyAutoRead(AutoReadSwitch_enum.STOP);//停止自动读卡(线体)
                }
                if (ReadThread != null && ReadThread.IsAlive)
                {
                    ReadThread.Abort();
                }
            }
        }

        private void AddResult(TagReadData[] tags)
        {          
            this.BeginInvoke((EventHandler)delegate{
                TagShow(this, tags);
            });
          
        }


        //天线盘点到的标签分类显示
        private void AntHaveReadTag()
        {
            UInt32 TagNum_Ant1=0;
            UInt32 TagNum_Ant2=0;
            UInt32 TagNum_Ant3=0;
            UInt32 TagNum_Ant4=0;
            label_UniqueTagNum.Text = TagList.mylist.Count.ToString();  
            foreach(MyTag tag in TagList.mylist)
            {
                if (tag.AntPort == 1) TagNum_Ant1++;
                if (tag.AntPort == 2) TagNum_Ant2++;
                if (tag.AntPort == 3) TagNum_Ant3++;
                if (tag.AntPort == 4) TagNum_Ant4++;           
            }
            label_Ant1.Text = TagNum_Ant1.ToString();
            label_Ant2.Text = TagNum_Ant2.ToString();
            label_Ant3.Text = TagNum_Ant3.ToString();
            label_Ant4.Text = TagNum_Ant4.ToString();
        }



        private void TagShow(object obj, TagReadData[] data)
        {
            try
            {
                //TagOnlineNum.Text = data.Length.ToString().PadLeft(4, '0');
                foreach (TagReadData tag in data)
                { 
                    TagCnt++;//增加标签数
                    la_TagCount.Text = TagCnt.ToString();
                    lock (gdvTagShow)
                    {
                        //在list中根据EPC查找标签
                        int index = TagList.myFind(tag.EPC);  
                        if (index == -1)//未找到
                        {
                            TagList.mylist.Add(new MyTag(tag));
                            //显示信息，并更改当前行的背景颜色
                            //序号，EPC，计数器，RSSI，天线端口，信道号
                            string tempTagEPC = Tools.HexStringToString(tag.EPC);
                            gdvTagShow.Rows.Add(TagList.mylist.Count, tempTagEPC, tag.GetAntStr(), tag.readCnt.ToString(), tag.RSSI.ToString());
                            gdvTagShow.Rows[TagList.mylist.Count - 1].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                        }
                        else
                        {
                            //ShowCntOnAnt((byte)TagList.mylist[index].AntPort, (byte)(tag.ant));
                            //TagList.mylist[index].AntMark((int)tag.ant);//记录读取过的天线号和当前读取的天线号
                            TagList.mylist[index].TagIndexRefresh(tag.TagIndex);
                            //更新信息
                                         
                            gdvTagShow[2, index].Value = tag.GetAntStr();
                            
                            gdvTagShow[3, index].Value = Convert.ToInt32(gdvTagShow[3, index].Value) + tag.readCnt;
                            gdvTagShow[4, index].Value = tag.RSSI.ToString();
                        
                            //更改标签读取时间和行背景颜色
                            TagList.mylist[index].GottenTime = tag.time;
                            gdvTagShow.Rows[index].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
                        }
                        AntHaveReadTag();

                    }                
                }               
            }
            catch (Exception e)
            {
                SetStatus(e.ToString(), Color.Red);
                SetStatus("Label list display error", Color.Red);
            }
        }

        #endregion
        //Set the card reader antenna
        private Status_enum ReadTag_SetAntCfg()
        {
            Status_enum retval;

            WorkAntPlan.Clear();
            for (int i = 0; i < ants.Length; i++)
            {
                if (ants[i].Checked)
                {
                    WorkAntPlan.Add(((byte)(i + 1)));
                }
            }
            retval = objreader.SetWorkAntPlan(WorkAntPlan);
            return retval;
        }

        //Get card reader mode
        private bool ReadTag_GetReadMode()
        {
            Status_enum retval;
            UInt16 WordMode = 0;
            retval = objreader.GetLogisticsWorkMode(ref WordMode);
            if (retval != Status_enum.SUCCESS)
            {
                SetStatus("Failed to get working mode parameter", Color.Red);
                return false;
            }

            if (WordMode == 2) 
            {
                ReaderMode = ReadMode_enum.AUTOREAD_LOGISTICS; 
            }
            else if (WordMode == 1)
            {
                ReaderMode = ReadMode_enum.AUTOREAD_LINEBODY;
            }
            else
            {
                ReaderMode = ReadMode_enum.AT_MODE;
            }

            if (ReaderMode == ReadMode_enum.AUTOREAD_LOGISTICS)
            {
                label_ReadMode.Text = "Auto-Logistics";
                label_ReadMode.ForeColor = Color.OrangeRed;
            }
            else if (ReaderMode == ReadMode_enum.AUTOREAD_LINEBODY)
            {
                label_ReadMode.Text = "Auto-Line Body";
                label_ReadMode.ForeColor = Color.Black;
                UInt16 ReadTime = 0;
                retval = objreader.GetReadSingleTime(ref ReadTime);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to get working mode parameter", Color.Red);
                    return false;
                }
                ReadSingleTime = ReadTime;
            }
            else
            {
                label_ReadMode.Text = "Answer mode";
                label_ReadMode.ForeColor = Color.Black;
                UInt16 ReadTime = 0;
                retval = objreader.GetReadSingleTime(ref ReadTime);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to get working mode parameter", Color.Red);
                    return false;
                }
                ReadSingleTime = ReadTime;

            }
            return true;
        }


        private void StopReadTag()
        {
            //stop timer，record elapsed time
            ShowStatistics.Change(Timeout.Infinite, 100);
            ExecutedTime += Environment.TickCount - StartTime;
            Bt_ReadTag_On_Off.Text = "Start";
            groupBox_AntSelect.Enabled = true;      
            SetStatus("Card reading stopped", Color.Black);
            StopThreadReadTag();
            
          
        }

        //Start to read card data automatically
        private bool AutoReadDataUploadSwitch(AutoReadSwitch_enum StartOrStop)
        {   
            Status_enum retval;
            retval = objreader.SetLogisticsAutoReadDataUpload(StartOrStop);
            if (retval != Status_enum.SUCCESS)
            {
                SetStatus("The automatic card reading data eruption setting failed!", Color.Red);
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Tab READ TAGS
        private void Bt_ReadTag_On_Off_Click(object sender, EventArgs e)
        {
            if (Bt_ReadTag_On_Off.Text == "Start")
            {
                groupBox_AntSelect.Enabled = false;       
                //读卡时禁止部分操作              
                Bt_ReadTag_On_Off.Text = "Stop";
                SetStatus("Reading card", Color.Black);
                ReadTag_GetReadMode();//获取读写器工作模式
                if (ReaderMode == ReadMode_enum.AUTOREAD_LOGISTICS)
                {
                    //自动读卡模式(物流)
                    AutoReadDataUploadSwitch(AutoReadSwitch_enum.START);//开启自动读卡数据喷发
                    objreader.AutoReadHandler += new AYNETTEK.UHFReader.EventHandler<AutoReadEventArgs>(objreader_TagRead);
                }
                else
                {
                    //线体自动读卡 和 应答读卡模式
                    ReadTag_SetAntCfg();//配置天线   
                    if (ReadThread != null && ReadThread.IsAlive)
                    {
                        ReadThread.Abort();
                    }
                    isInventory = true;
                    ReadThread = new Thread(StartReadTag);
                    ReadThread.IsBackground = true;
                    ReadThread.Start(100);
                }
                //启动定时器，记录起始时间
                ShowStatistics.Change(0, 100);
                StartTime = Environment.TickCount;

            }
            else
            {
                StopReadTag();
            }
        }

        private void bt_Clear_Click(object sender, EventArgs e)
        {
            TagCnt = 0;
            if (Bt_ReadTag_On_Off.Text == "Clear")
            {
                StartTime = 0;//开始时间
                ExecutedTime = 0;//已有的执行时间     
                lb_timeCnt.Text = "0:0:0:0.0";
            }
                                 
            //TagOnlineNum.Text = (0).ToString().PadLeft(4, '0');
            la_TagCount.Text = (0).ToString().PadLeft(2, '0'); 
            gdvTagShow.Rows.Clear();
            TagList.mylist.Clear();
        }

        #endregion 
        private void AccessInfoClear()
        {
            TagAccessListSelection.Items.Clear();
            TagAccessListSelection.Text = "Select label";
            AccessAnt1.Checked = true;
            comboBox_accessType.SelectedIndex = 0;
            textBox_AccessOffset.Text = "2";
            textBox_AccessLen.Text = "6";
            textBoxDatas_access.Text = "";
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (objreader.ConnectStatus != ConnectStatusEnum.CONNECTED)
            {
                /*Connection not established, page switching is not allowed*/
                if (this.tabControl1.SelectedIndex != 0)
                {
                    SetStatus("Connection not established, window switching is not allowed!", Color.Black);
                    this.tabControl1.SelectedIndex = 0;
                }
                return;
            }
            switch (this.tabControl1.SelectedIndex)
            {
                case 0://Inventory label
                    //if ((Bt_ReadTag_On_Off.Text == "停止") && ((ReaderMode == ReadMode_enum.AUTOREAD_LOGISTICS) || (ReaderMode == ReadMode_enum.AUTOREAD_LINEBODY)))
                    //{
                    //    Bt_ReadTag_On_Off_Click(null, null);
                    //}
                    //break;
                case 1://Memory access
                    if (Bt_ReadTag_On_Off.Text == "Stop")
                    {
                        StopReadTag();
                    }
                    Thread.Sleep(50);
                    AccessInfoClear();
                    for (int i = 0; i < TagList.mylist.Count; i++)
                    {
                        TagAccessListSelection.Items.Add(TagList.mylist[i].Epc);
                    }
                    break;
                case 2://Parameter settings
                    if (Bt_ReadTag_On_Off.Text == "Stop")
                    {
                        StopReadTag();
                    }
                    Thread.Sleep(50);
                    button_ParamRefresh_Click(null, null);
                    break;
                case 3://Extended parameters
                    if (Bt_ReadTag_On_Off.Text == "Stop")
                    {
                        StopReadTag();
                    }
                    Thread.Sleep(50);
                    //button_ExpandParamRefresh_Click(null, null);
                    break;
                case 4://Online upgrade
                    if (Bt_ReadTag_On_Off.Text == "Stop")
                    {
                        StopReadTag();
                    }
                    break;
                default:
                    break;

            }
        }
        private void button_accessClear_Click(object sender, EventArgs e)
        {
            textBoxDatas_access.Text = "";    
        }

        private void button_accessExec_Click(object sender, EventArgs e)
        {
            if (TagAccessListSelection.Text == "null" && (radioButton_Select.Checked == true))
            {
                SetStatus("Please select a tag in the tag list!", Color.Red);
                return;
            }
            if (comboBox_accessType.Text == "Read")
            {
                AccessRead();
            }
            else if (comboBox_accessType.Text == "Write")
            {
                AccessWrite();
            }
            else if (comboBox_accessType.Text == "WriteEPC")
            {
                AccessWriteEPC();
            }
            else
            {
                SetStatus("Wrong action option!", Color.Red);
            }
        }

        //Get a valid access antenna
        public int GetAccessAnt()
        {
          
            if (AccessAnt4.Checked == true)
            {
                return (0x01<<3);
            }
            else if (AccessAnt3.Checked == true)
            {
                return (0x01 << 2);
            }
            else if (AccessAnt2.Checked == true)
            {
                return (0x01 << 1);
            }
            else
            {
                return 0x01;
            }
        }


        #region Read tag memory

        /*  public bool AccessReadTagMemory(int TagIndex,int Ant,byte bank,int offset,int wordlen)
          {
              bool retval = true;
              int pos =0;
              byte[] datas = new byte[256];
              objreader.AccessCmdFlow++;
              datas[pos++] = (byte)(objreader.AccessCmdFlow >> 8); datas[pos++] = (byte)(objreader.AccessCmdFlow & 0x00ff);
              datas[pos++] = 0; datas[pos++] = (byte)ReaderCmd_enum.ReadTagMemory;
              datas[pos++] = (byte)(TagIndex >>8); datas[pos++] = (byte)(TagIndex & 0x00ff);
              datas[pos++] = 0; datas[pos++] = (byte)(Ant & 0x00ff);
              datas[pos++] = 0; datas[pos++] = (byte)(bank & 0x00ff);
              datas[pos++] = (byte)(offset >> 8); datas[pos++] = (byte)(offset & 0x00ff);
              datas[pos++] = (byte)(wordlen >> 8); datas[pos++] = (byte)(wordlen & 0x00ff);
              if (objreader.Modbus_WriteMultipleRegister(ModbusAddr.GetModbusAddr("AccessTagCmdFlow_Req"), datas, (UInt16)(pos/2)) != ModbusStatus_enum.SUCCESS)
              {
                  retval = false;

              }
              return retval;
          }*/



        /// <summary>
        /// Read Tag Memory
        /// </summary>
        /// <param name="bank"></param>
        private void ReadTagMemory(Gen2.Bank bank)
        {
            Status_enum retval;       
            byte[] data = new byte[256];
            AccessTagMask TagMask = new AccessTagMask();
            try
            {
                if (textBox_AccessOffset.Text.Equals("") || textBox_AccessLen.Text.Equals(""))
                {
                    SetStatus("Offset word address or read word length cannot be empty！", Color.Red);
                    return;
                }
                int ReadWordlen = int.Parse(textBox_AccessLen.Text);            
                int ReadOffset = int.Parse(textBox_AccessOffset.Text);                          
                int ant = GetAccessAnt();
                AccessMask_enum TagListIndex;
    
                if (radioButton_FirstResponse.Checked == true)
                {
                    //Take the label of the first response
                    TagListIndex = AccessMask_enum.NO_MASK;
                }
                else
                {
                    //Filter by tags
                    if (TagAccessListSelection.Text == "Select label")
                    {
                        SetStatus("Please select a label to filter！", Color.Red);
                        return;                    
                    }

                    TagListIndex = AccessMask_enum.ENABLE_MASK;
                    TagMask.Mask_Bank = Gen2.Bank.EPC;
                    TagMask.Mask_WordPtr = 2;//The first 4 bytes of the EPC area are CRC and PC code, followed by the EPC code body
                    TagMask.Mask_WordLen = (UInt16)(TagAccessListSelection.Text.Length / 4);//Word unit
                    TagMask.Mask_Data = Tools.HexStringToByte(TagAccessListSelection.Text, 0, TagAccessListSelection.Text.Length/2);//Byte unit



                }
                

                if (ReadWordlen == 0)
                {
                    SetStatus("The read word length cannot be 0！", Color.Red);
                    return;
                }
                if (ReadWordlen > 32)
                {
                    ReadWordlen = 32;
                    textBox_AccessLen.Text = "32";//Read up to 32 words at a time
                }

                if (TagListIndex == AccessMask_enum.ENABLE_MASK)
                {
                    retval = objreader.SetAccessMask(TagMask);
                    if (retval != Status_enum.SUCCESS)
                    {
                        textBoxDatas_access.Text = "Failed to send mask condition!";
                        SetStatus("Failed to send mask condition!", Color.Red);
                    }
                }

                retval = objreader.SetReBootAutoReadMulAnt((UInt16)ant, Save_enum.No_Save);
                if (retval != Status_enum.SUCCESS)
                {               
                    SetStatus("天数设置失败!", Color.Red);
                    return;
                }
                Thread.Sleep(50);
                retval = objreader.AccessReadTagMemory(TagListIndex, ant, bank, ReadOffset, ReadWordlen, ref data);
                if (retval != Status_enum.SUCCESS)
                {
                    textBoxDatas_access.Text = "Read failed！";
                    SetStatus("Tag memory read failed!", Color.Red);
                }
                else
                {
                    // TODO: 06/06/2022 Hoang sửa do cần hiển thị String thay vì Hexastring
                    string temp = Tools.ByteToHexString(data, 0, ReadWordlen * 2, "");
                    textBoxDatas_access.Text = Tools.HexStringToString(temp);
                    SetStatus("Tag memory read successfully！", Color.Green);
                }
            }
            catch (Exception e)
            {
                SetStatus("Failed to read, the input data format is not correct!", Color.Red);
                MessageBox.Show(e.Message);
            }
        }


        void AccessRead()
        {
            switch (comboBox_MemoryArea.Text)
            { 
                case "EPCArea":
                    ReadTagMemory(Gen2.Bank.EPC);
                    break;
                case "TIDArea":
                    ReadTagMemory(Gen2.Bank.TID);
                    break;
                case "USERArea":
                    ReadTagMemory(Gen2.Bank.USER);               
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Write tag memory
        private void AccessWrite()
        {
            switch (comboBox_MemoryArea.Text)
            {
                case "EPCArea":

                    WriteTagMemory(Gen2.Bank.EPC);
                    break;
                case "USERArea":
                    WriteTagMemory(Gen2.Bank.USER);
                    break;
                default:
                    break;
            }            
        }
        //发送写标签内存请求帧
        /*public bool AccessWriteTagMemory(int TagIndex, int Ant, byte bank, int offset, int wordlen,byte[] writedata)
        {
            bool retval = true;
            int pos = 0;
            byte[] datas = new byte[256];
            objreader.AccessCmdFlow++;
            datas[pos++] = (byte)(objreader.AccessCmdFlow >> 8); datas[pos++] = (byte)(objreader.AccessCmdFlow & 0x00ff);
            datas[pos++] = 0; datas[pos++] = (byte)ReaderCmd_enum.WriteTagMemory;
            datas[pos++] = 0; datas[pos++] = (byte)(TagIndex & 0x00ff);
            datas[pos++] = 0; datas[pos++] = (byte)(Ant & 0x00ff);
            datas[pos++] = 0; datas[pos++] = (byte)(bank & 0x00ff);
            datas[pos++] = (byte)(offset >> 8); datas[pos++] = (byte)(offset & 0x00ff);
            datas[pos++] = (byte)(wordlen >> 8); datas[pos++] = (byte)(wordlen & 0x00ff);
            for (int i = 0; i < wordlen*2; i++)
            {
                datas[pos++] = writedata[i];
            }
            if (objreader.Modbus_WriteMultipleRegister(ModbusAddr.GetModbusAddr("AccessTagCmdFlow_Req"), datas, (UInt16)(pos/2)) != ModbusStatus_enum.SUCCESS)
            {
                retval = false;
            }
            return retval;
        }*/

        private void WriteTagMemory(Gen2.Bank bank)
        {
            Status_enum retval;
            byte[] datas = new byte[256];
            AccessTagMask TagMask = new AccessTagMask();
            try
            {
                if (textBox_AccessOffset.Text.Equals("") || textBox_AccessLen.Text.Equals(""))
                {
                    SetStatus("Offset word address or write word length cannot be empty！", Color.Red);
                    return;
                }
                int ReadWordlen = int.Parse(textBox_AccessLen.Text);
                int ReadOffset = int.Parse(textBox_AccessOffset.Text);
                AccessMask_enum TagListIndex;
       

                if (radioButton_FirstResponse.Checked == true)
                {
                    //The label of the first response
                    TagListIndex = AccessMask_enum.NO_MASK;
                }
                else
                {
                    //Filter method
                    if (TagAccessListSelection.Text == "Select label")
                    {
                        SetStatus("Please select a label to filter！", Color.Red);
                        return;
                    }
                    TagListIndex = AccessMask_enum.ENABLE_MASK;
                    TagListIndex = AccessMask_enum.ENABLE_MASK;
                    TagMask.Mask_Bank = Gen2.Bank.EPC;
                    TagMask.Mask_WordPtr = 2;//The first 4 bytes of the EPC area are CRC and PC code, followed by the EPC code body
                    TagMask.Mask_WordLen = (UInt16)(TagAccessListSelection.Text.Length / 4);//2 characters are a byte
                    TagMask.Mask_Data = Tools.HexStringToByte(TagAccessListSelection.Text, 0, TagAccessListSelection.Text.Length/2);

                }

              
                int ant = GetAccessAnt();
                if (ReadWordlen == 0)
                {
                    SetStatus("Write word length cannot be 0！", Color.Red);
                    return;
                }
                if (ReadWordlen > 32)
                {
                    ReadWordlen = 32;
                    textBox_AccessLen.Text = "32";//Read up to 32 words at a time
                }



                string tmp = textBoxDatas_access.Text.ToString();
                if (tmp.Replace(" ", "").Length == 0)
                {
                    MessageBox.Show("Write content cannot be empty");
                    return;
                }
                byte[] writedata = Tools.HexStringToByte(textBoxDatas_access.Text, 0, ReadWordlen*2);
                if (writedata.Length < ReadWordlen * 2)
                {
                    MessageBox.Show("Insufficient data!");
                    return;
                }

                retval = objreader.SetReBootAutoReadMulAnt((UInt16)ant, Save_enum.No_Save);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to set the number of days!", Color.Red);
                    return;
                }
                Thread.Sleep(50);
                retval = objreader.AccessWriteTagMemory(TagListIndex, ant, bank, ReadOffset, ReadWordlen, writedata);
                if (retval == Status_enum.SUCCESS)
                {
                    //Access request completed   
                    SetStatus("Tag memory write succeeded！", Color.Green);
                }
                else
                {
                    SetStatus("Tag memory write failed！", Color.Red);
                }
            }
            catch (Exception e)
            {
                SetStatus("Write failed, the input data format is not correct!", Color.Red);
                MessageBox.Show(e.Message);
            }
        }
        #endregion


        #region Write EPC number

        private void AccessWriteEPC()
        {
            Status_enum retval;
            byte[] datas = new byte[256];
            try
            {
                string datastr = textBoxDatas_access.Text.ToString();

                //datastr.Replace(" ", "");
                if ((datastr.Length==0) || (datastr==null))
                {
                    MessageBox.Show("Write content cannot be empty");
                    return;
                }
                datastr = datastr.Replace(" ", "");
                if (datastr.Length % 4 != 0)
                {
                    MessageBox.Show("The byte length of the write content must be an even number！");
                    SetStatus("The data length does not meet the rules!", Color.Red);
                    return;
                }
                //TODO: Hoang edit 26/05/2022 Do cần ghi kiểu String thay vì Hexa
                string datatemp = Tools.StringToHexString(datastr);
                byte[] writedata = Tools.HexStringToByte(datatemp, 0, datatemp.Length / 2);//datastr is a string, two characters represent a byte
                //
                int ant = GetAccessAnt();
                retval = objreader.SetReBootAutoReadMulAnt((UInt16)ant, Save_enum.No_Save);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to set the number of days!", Color.Red);
                    return;
                }
                Thread.Sleep(50);

                retval = objreader.WriteTagEPC(ant, writedata, writedata.Length/2);            
                if (retval == Status_enum.SUCCESS)
                {
                    //Access request completed   
                    SetStatus("EPC number written successfully！", Color.Green);
                }
                else
                {
                    SetStatus("Failed to write EPC number！", Color.Red);
                }      
            }
            catch
            {
                SetStatus("Format error, failed to write EPC number！", Color.Red);
            }

   
        }
        #endregion

        private void comboBox_accessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_accessType.Text)
            { 
                case "Read":
                    textBoxDatas_access.Enabled = false;
                    comboBox_MemoryArea.Enabled = true;
                    textBox_AccessOffset.Enabled = true;
                    textBox_AccessLen.Enabled = true; 
                    if(comboBox_MemoryArea.Items.Count == 2)
                    {
                        comboBox_MemoryArea.Items.Add("TIDArea");    
                    }   
                    break;
                case "Write":
                    textBoxDatas_access.Enabled = true;
                    comboBox_MemoryArea.Enabled = true;
                    textBox_AccessOffset.Enabled = true;
                    textBox_AccessLen.Enabled = true; 
                    comboBox_MemoryArea.Items.Remove("TIDArea");
                    if (comboBox_MemoryArea.Text == "EPCArea")
                    {
                        textBox_AccessOffset.Text = "2";
                    }
                    break;
                case "WriteEPC":
                    textBoxDatas_access.Enabled = true;
                    comboBox_MemoryArea.Enabled = false;
                    textBox_AccessOffset.Enabled = false;
                    textBox_AccessLen.Enabled = false;
                    if (radioButton_Select.Checked == true)
                    {
                        if (TagAccessListSelection.Text != "Select label")
                        {
                            textBoxDatas_access.Text = Tools.strToSpaceStr(TagAccessListSelection.Text);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Network parameter settings
        /// </summary>
        private void NetCfgSet()
        {
            Status_enum retval;
            NetCfg cfg = new NetCfg();
            try
            {
                if (textBox_IpAddr.Text.Trim().Replace(" ", "") == "" || textBox_SubMask.Text.Replace(" ", "") == ""
                    || textBox_GateWay.Text.Trim().Replace(" ", "") == "")
                {
                    SetStatus("Network parameters cannot be empty！", Color.Red);
                    return;
                }
                cfg.ip = IPAddress.Parse(textBox_IpAddr.Text);
                cfg.mask = IPAddress.Parse(textBox_SubMask.Text);
                cfg.gateway = IPAddress.Parse(textBox_GateWay.Text);

                retval = objreader.SetNetParam(cfg);
                if (retval == Status_enum.SUCCESS)
                {
                    SetStatus("The network parameters are set successfully！", Color.Green);
                }
                else
                {
                    SetStatus("Network parameter setting failed！", Color.Red);
                }
                SetStatus("The network parameters are set successfully！", Color.Green);
            }
            catch
            {
                SetStatus("Incorrect network parameter format！", Color.Red);
            }
        }

        private bool ReBootAutoReadAntSet()
        {
            Status_enum retval;
            UInt16 MulAntport = 0;
            try
            {
                if (checkBox_SaveANT1.Checked == true) MulAntport |= (0x01 << 0);
                if (checkBox_SaveANT2.Checked == true) MulAntport |= (0x01 << 1);
                if (checkBox_SaveANT3.Checked == true) MulAntport |= (0x01 << 2);
                if (checkBox_SaveANT4.Checked == true) MulAntport |= (0x01 << 3);

                retval = objreader.SetReBootAutoReadMulAnt(MulAntport, Save_enum.Save);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Antenna save failed！", Color.Red);
                    return false;
                }      
                return true;
            }
            catch
            {
                SetStatus("Card reading parameter setting failed！", Color.Red);
                return false;
            }      
        }

        /// <summary>
        /// Antenna settings
        /// </summary>
        /// <returns></returns>
        private bool AntennaPowerCfgSet()
        {
            Status_enum retval;
            byte[] datas = new byte[256];
            UInt16 AntReadPower = 0;
            UInt16 AntWritePower = 0;


            try
            {
                AntReadPower = Convert.ToUInt16(Convert.ToSingle(textBox_ReadPower_Ant1.Text) * 10);//Power value magnified 10 times
                AntWritePower = Convert.ToUInt16(Convert.ToSingle(textBox_WritePower_Ant1.Text) * 10);//Power value magnified 10 times
                if (objreader.ProductModel_value == ReaderProductModel.UR_F511)
                {
                    if ((AntReadPower < 130) || (AntReadPower > 270))
                    {
                        MessageBox.Show("The power value is out of bounds, the normal range value is 13~27dBm");
                        return false;
                    }
                }
                else
                {
                    if ((AntReadPower > 300) || (AntWritePower > 300))
                    {
                        MessageBox.Show("The power value is out of bounds, the normal range value is 0~30dBm");
                        return false;
                    }    
                }


                retval = objreader.SetAntennaReadPower(AntReadPower, AntWritePower);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Power parameter setting failed！", Color.Red);
                    return false;
                }

            }
            catch
            {
                SetStatus("Antenna parameter format is incorrect！", Color.Red);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Mode related parameter configuration
        /// </summary>
        /// <returns></returns>
        private bool ModeCfgSet()
        {
            Status_enum retval;
            byte[] datas = new byte[256];
            try
            {
                UInt16 ReadSingleTime = Convert.ToUInt16(textBox_SingleTime.Text);
                retval = objreader.SetReadSingleTime(ReadSingleTime);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Trigger card reading time setting failed！", Color.Red);
                    return false;
                }
                UInt16 TagCacheTime = Convert.ToUInt16(textBox_CacheTime.Text);
                retval = objreader.SetTagCacheTime(TagCacheTime);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Label cache time setting failed！", Color.Red);
                    return false;
                }
                SetStatus("Card reading related parameters are set successfully！", Color.Green);
            }
            catch
            {
                SetStatus("Card reading related parameter setting failed！", Color.Red);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Display of network parameters
        /// </summary>
        /// <returns></returns>
        private bool NetcfgSetInfoDisp()
        {
            Status_enum retval;
            try
            {
                /*******************************************/
                //Display of network parameters
                NetCfg cfg = new NetCfg();

                retval = objreader.GetNetParam(ref cfg);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to get network parameters", Color.Red);
                    return false;
                }
                textBox_IpAddr.Text = cfg.ip.ToString();
                textBox_SubMask.Text = cfg.mask.ToString();
                textBox_GateWay.Text = cfg.gateway.ToString();
                textBox_MACAddr.Text = Tools.MacToString(cfg.mac);
                return true;
                /*******************************************/
            }
            catch
            {
                SetStatus("Failed to get network parameters", Color.Red);
                return false;
            }        
        }


        /// <summary>
        /// Card reading related parameters display
        /// </summary>
        /// <returns></returns>
        private bool ReadCfgSetInfoDisp()
        {
            Status_enum retval;
            try
            {
                /*******************************************/
                //Power
                UInt16 ReadPower = 0;
                UInt16 WritePower = 0;
                retval = objreader.GetAntennaReadPower(ref ReadPower, ref WritePower);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to get power parameters", Color.Red);
                    return false;
                }
                textBox_ReadPower_Ant1.Text = (ReadPower * 0.1).ToString();
                textBox_WritePower_Ant1.Text = (WritePower * 0.1).ToString();


                UInt16 ReadTime = 0;
                retval = objreader.GetReadSingleTime(ref ReadTime);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to obtain execution time for triggering card reading", Color.Red);
                    return false;
                }
                textBox_SingleTime.Text = ReadTime.ToString();//Execution time for triggering card reading


                UInt16 TagCacheTime = 0;
                retval = objreader.GetTagCacheTime(ref TagCacheTime);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to get tag cache time", Color.Red);
                    return false;
                }
                textBox_CacheTime.Text = TagCacheTime.ToString();//Tag cache time
                return true;
                /******************************************/
            }
            catch
            {
                SetStatus("读卡相关参数获取失败", Color.Red);
                return false;
            }        
        }

        /// <summary>
        /// Antenna parameter acquisition for automatic card reading
        /// </summary>
        /// <returns></returns>
        private bool RebootAutoReadAntSetInfoDisp()
        {

            Status_enum retval;
            try
            {
                /*******************************************/
                //Antenna parameter used for automatic card reading
                UInt16 MulAntport = 0;
                retval = objreader.GetReBootAutoReadMulAnt(ref MulAntport);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to acquire the antenna that is automatically enabled at power-on", Color.Red);
                    return false;
                }
                if (((MulAntport >> 0) & 0x01) == 0x01) checkBox_SaveANT1.Checked = true; else checkBox_SaveANT1.Checked = false;
                if (((MulAntport >> 1) & 0x01) == 0x01) checkBox_SaveANT2.Checked = true; else checkBox_SaveANT2.Checked = false;
                if (((MulAntport >> 2) & 0x01) == 0x01) checkBox_SaveANT3.Checked = true; else checkBox_SaveANT3.Checked = false;
                if (((MulAntport >> 3) & 0x01) == 0x01) checkBox_SaveANT4.Checked = true; else checkBox_SaveANT4.Checked = false;
                
                /******************************************/
                return true;

            }
            catch
            {
                SetStatus("Failed to acquire the antenna that is automatically enabled at power-on", Color.Red);
                return false;
            }    
        }

        /// <summary>
        /// Inventory object parameter acquisition
        /// </summary>
        /// <returns></returns>
        private bool InventoryObjectSetInfoDisp()
        {
            Status_enum retval;
            try
            {
                UInt16 AntennaCheck = 0;
                retval = objreader.GetAntennaCheck(ref AntennaCheck);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Failed to obtain antenna detection parameters", Color.Red);
                    return false;
                }
                //comboBox_AntCheckEnable.SelectedIndex = AntennaCheck == 1 ? 1 : 0;//Antenna Detection Enable

                UInt16 InventoryTIDEnable = 0;
                UInt16 InventoryTIDAddr = 0;
                UInt16 InventoryTIDLen = 0;
                retval = objreader.GetInventoryTIDSet(ref InventoryTIDEnable, ref InventoryTIDAddr, ref InventoryTIDLen);
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("Inventory TID parameter acquisition failed", Color.Red);
                    return false;
                }
                //comboBox_InventoryTIDEnable.SelectedIndex = InventoryTIDEnable == 1 ? 1 : 0;
                //textBox_InventoryTIDAddr.Text = InventoryTIDAddr.ToString();
                //textBox_InventoryTIDLen.Text = InventoryTIDLen.ToString();
                return true;
            }
            catch
            {
                SetStatus("Inventory object parameter acquisition failed", Color.Red);
                return false;
            }
         
        }


        /// <summary>
        /// Get working mode parameters
        /// </summary>
        /// <returns></returns>
        private bool WordModeSetInfoDisp()
        {
            Status_enum retval;
            UInt16 WordMode=0;
            retval = objreader.GetLogisticsWorkMode(ref WordMode);
            if (retval != Status_enum.SUCCESS)
            {
                SetStatus("Failed to get working mode parameter", Color.Red);
                return false;      
            }
            
            comboBox_ReadWordMode.SelectedIndex = WordMode;//Reader working mode
            if (WordMode == 2)
            {
                ReaderMode = ReadMode_enum.AUTOREAD_LOGISTICS;
            }
            else if (WordMode == 1)
            {
                ReaderMode = ReadMode_enum.AUTOREAD_LINEBODY;
            }
            else
            {
                ReaderMode = ReadMode_enum.AT_MODE;
            }

            return true;
            
        }
        /// <summary>
        /// Get Mac Address
        /// </summary>
        public void GetMac()
        {
            byte[] mac = new byte[6];
            Status_enum retval = objreader.GetMac(ref mac);
            if (retval == Status_enum.SUCCESS)
            {
                textBox_setMac.Text = Tools.MacToString(mac);
                SetStatus("Mac address set successfully！", Color.Green);
            }
            else
            {
                SetStatus("Mac address setting failed！", Color.Red);
            }
        }

        /// <summary>
        /// Event Click Button Refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ParamRefresh_Click(object sender, EventArgs e)
        {
            NetcfgSetInfoDisp();
            RebootAutoReadAntSetInfoDisp();
            WordModeSetInfoDisp();
            GetMac();
            if ((ReaderMode == ReadMode_enum.AT_MODE) || (ReaderMode == ReadMode_enum.AUTOREAD_LINEBODY))
            {
                GroupBox_ReadCardParam.Enabled = true;
                groupBox_AutoReadAnt.Enabled = true;
                ReadCfgSetInfoDisp();

            }
            else
            {
                GroupBox_ReadCardParam.Enabled = false;
                groupBox_AutoReadAnt.Enabled = false;
            }
        }


        #region Online update
        private string FileName = "";
        private bool IsUpgrading = false;
        private void bt_BeginUpdate_Click(object sender, EventArgs e)
        {
            if (FileName == "" || !File.Exists(FileName))
            {
                SetStatus("Please reselect the file!", Color.Red);
                return;
            }
            if (IsUpgrading)
            {
                SetStatus("Is being upgraded...", Color.Red);
                return;
            }

            SetStatus("Start the upgrade..", Color.Black);
            SetStatus("Start the upgrade..", Color.Green);
            System.Threading.Thread upgradeThread = new System.Threading.Thread(UpgradeProcessThread);
            upgradeThread.IsBackground = true;
            upgradeThread.Start(FileName);
        }
        public bool Check_reading()
        {
            if (Bt_ReadTag_On_Off.Text == "Start")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void UpdatabuttonStatus(bool Enable)
        {
            this.BeginInvoke((EventHandler)delegate
            {
                //bt_updatefile.Enabled = Enable;
                //bt_BeginUpdate.Enabled = Enable;
            });
        }

        void UpgradeProcessThread(object obj)
        {
            if(Check_reading())
            {
                SetStatus("Upgrade terminated: Please stop reading tags!", Color.Red);
                return;
            }
            //SetStatus("Current program version:" + ReaderFirmwareText.Text, Color.Green);
            SetStatus("During upgrade...", Color.Green);
            IsUpgrading = true;
            UpdatabuttonStatus(false);
            UpgradeProcess(FileName);
        }

        
        private bool ReadUpdataStatus(UInt16 PkgID)
        {
            byte[] datas = new byte[2];
            UInt16 i = 10;
            UInt16 Status;
            while ((i--)>0)
            {
                Thread.Sleep(5);
                //objreader.Modbus_ReadMultipleRegister(ModbusAddr.GetModbusAddr("UpgradeData_status"), 1, ref datas);         
                objreader.Modbus_ReadMultipleRegister(0xDF22, 1, ref datas);
                Status = Tools.ByteToU16(datas, 0);
                if (PkgID == Status)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Quy trình nâng cấp
        /// </summary>
        /// <param name="obj"></param>
        private void UpgradeProcess(object obj)
        {
            string file = (string)obj;
            FileStream binfs = new FileStream(file, FileMode.Open);
            System.IO.FileInfo finfo = new FileInfo(file);
            BinaryReader bin = new BinaryReader(binfs);

            Byte[] buf = new Byte[128];
            UInt16 data_len;
            UInt16 pkgID;
            long total_len = finfo.Length + buf.Length;
            long writed_len = 0;
            int retry = 0;
            bool err = false;
            //同步开始：发送数据包编号0x0000数据包,直到收到应答
            data_len = 0;
            pkgID = 0x0000;
            writed_len = 0;
            Tools.U16ToByteArray(pkgID, ref buf, 0);    //填写本次数据包编码
            Tools.U16ToByteArray(data_len, ref buf, 2); //填写本次数据包长度
            //if (objreader.Modbus_WriteMultipleRegister(ModbusAddr.GetModbusAddr("UpgradeData"), buf, 34) == ModbusStatus_enum.SUCCESS)
            if (objreader.Modbus_WriteMultipleRegister(0xDF00, buf, 34) == ModbusStatus_enum.SUCCESS)
            {
                if (ReadUpdataStatus(pkgID) == false)
                {
                    SetStatus("数据包:" + pkgID.ToString() + ",升级失败...", Color.Red);
                    return;
                }
            }
            else
            {
                SetStatus("数据包:" + pkgID .ToString()+ ",升级失败...", Color.Red);
                return;
            }
            data_len = 64;
            //开始下载过程
            while (data_len > 0)
            {
                pkgID++;
                //清空buffer
                for (int i = 0; i < buf.Length; i++)
                {
                    buf[i] = 0xFF;
                }
                //获取64个字节的bin文件数据
                data_len = (UInt16)bin.Read(buf, 4, 64);
                if (data_len <= 0) continue;

                writed_len += data_len;
                for (retry = 5; retry > 0; retry--)
                {
                    try
                    {
                        Tools.U16ToByteArray(pkgID, ref buf, 0);    //填写本次数据包编码
                        Tools.U16ToByteArray(data_len, ref buf, 2); //填写本次数据包长度
                        //if (objreader.Modbus_WriteMultipleRegister(ModbusAddr.GetModbusAddr("UpgradeData"), buf, 34) == ModbusStatus_enum.SUCCESS)
                        if (objreader.Modbus_WriteMultipleRegister(0xDF00, buf, 34) == ModbusStatus_enum.SUCCESS)
                        {
                            if (ReadUpdataStatus(pkgID) == true)
                            {
                                break;                            
                            }                       
                        }
                    }
                    catch (Exception exp)
                    {
                        exp.ToString();
                    }
         
                }
                if (retry <= 0)
                {
                    //如果尝试5次不成功,则返回错误.更新失败
                    err = true;
                }
                int persent = (int)(((double)writed_len / (double)total_len) * 100);
            }

            if (!err)
            {
                pkgID = 0xFFFF;
                Tools.U16ToByteArray(pkgID, ref buf, 0);    //填写本次数据包编码
                Tools.U16ToByteArray(data_len, ref buf, 2); //填写本次数据包长度
                //if (objreader.Modbus_WriteMultipleRegister(ModbusAddr.GetModbusAddr("UpgradeData"), buf, 34) == ModbusStatus_enum.SUCCESS)
                if (objreader.Modbus_WriteMultipleRegister(0xDF00, buf, 34) == ModbusStatus_enum.SUCCESS)
                {
                    if (ReadUpdataStatus(pkgID) == false)
                    {
                        SetStatus("数据包:" + pkgID.ToString() + ",升级失败...", Color.Red);
                        return;
                    }
                    err = false;
                }
                else
                {
                    err = true;
                }
            }

            //下载成功，返回
            binfs.Close();
            IsUpgrading = false; 
            if (err)
                SetStatus("升级失败...", Color.Red);
            else
            {
                SetStatus("升级成功...", Color.Green);
                SetStatus("开始重启设备...", Color.Green);
                // Tools.U16ToByteArray(0x9527, ref buf, 0);   
                //if (objreader.Modbus_WriteMultipleRegister(ModbusAddr.GetModbusAddr("SystemReset"), buf, 1) != ModbusStatus_enum.SUCCESS)
                Status_enum retval = objreader.SystemReset();
                if (retval != Status_enum.SUCCESS)
                {
                    SetStatus("升级终止:重启失败！", Color.Red);
                    UpdatabuttonStatus(true);
                    return;
                }
                rebootDemo();
                Thread.Sleep(1000);
                SetStatus("新软件版本:" + objreader.FirmwareVersion_str, Color.Green);
                SetStatus("升级成功", Color.Green);
            }
            UpdatabuttonStatus(true);
        }
        private bool rebootDemo()
        {
            try
            {
                Thread.Sleep(8000);
                this.BeginInvoke((EventHandler)delegate
                {
                    //if (objreader.commtype == CommType_enum.TCP)
                    //{
                    //    //断开连接
                    //    this.button_Disconnect_TCP_Click(null, null);

                    //    //重新建立连接
                    //    this.button_Connect_TCP_Click(null, null);
                    //}
                    //else
                    //{
                    //    //断开连接
                    //    this.button_Disconnect_USART_Click(null, null);

                    //    //重新建立连接
                    //    this.button_Connect_USART_Click(null, null);    
                    //}

                });
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
            return true;
        }
        #endregion


        private void radioButton_FirstResponse_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_FirstResponse.Checked == false)
            {
                TagAccessListSelection.Enabled = true;             
            }
            else
            {
                TagAccessListSelection.Enabled = false;              
            }
        }
        #region Event Control Tab page Setting
        private void button_netcfgSet_Click(object sender, EventArgs e)
        {
            NetCfgSet();//Network parameter settings   
        }

        private void button_ReadCfgSet_Click(object sender, EventArgs e)
        {
            bool retval1, retval2;
            retval1 = ModeCfgSet();
            retval2 = AntennaPowerCfgSet();
            if ((retval1 == true) && (retval2 == true))
            {
                SetStatus("Card reading related parameters are set successfully！", Color.Green);
            }
        }

        private void button_RebootCfgSet_Click(object sender, EventArgs e)
        {
            bool retval;
            retval = ReBootAutoReadAntSet();
            if (retval)
            {
                SetStatus("Antenna set successfully！", Color.Green);
            }

        }
        #endregion
        //private void button_InventoryObjectSet_Click(object sender, EventArgs e)
        //{
        //    Status_enum retval;
        //    try
        //    {
        //        UInt16 AntennaCheck = 0;
        //        AntennaCheck = Convert.ToUInt16(comboBox_AntCheckEnable.SelectedIndex);
        //        retval = objreader.SetAntennaCheck(AntennaCheck);
        //        if (retval != Status_enum.SUCCESS)
        //        {
        //            SetStatus("Antenna detection enable setting failed！", Color.Red);
        //            return;
        //        }


        //        UInt16 InventoryTIDEnable = Convert.ToUInt16(comboBox_InventoryTIDEnable.SelectedIndex);
        //        UInt16 InventoryTIDAddr = Convert.ToUInt16(textBox_InventoryTIDAddr.Text);
        //        UInt16 InventoryTIDLen = Convert.ToUInt16(textBox_InventoryTIDLen.Text);
        //        retval = objreader.SetInventoryTID(InventoryTIDEnable, InventoryTIDAddr, InventoryTIDLen);
        //        if (retval != Status_enum.SUCCESS)
        //        {
        //            SetStatus("Inventory TID parameter setting failed！", Color.Red);
        //            return;
        //        }
        //        SetStatus("Inventory object set successfully", Color.Green);

        //    }
        //    catch
        //    {
        //        SetStatus("Inventory object setup failed", Color.Red);
        //    }
        //}

        private void button_ModeSet_Click(object sender, EventArgs e)
        {
            Status_enum retval;
            UInt16 workmode = 0;
            workmode = Convert.ToUInt16(comboBox_ReadWordMode.SelectedIndex);
            retval = objreader.SetLogisticsWorkMode(workmode);
            if (retval != Status_enum.SUCCESS)
            {
                SetStatus("Card reading mode setting failed！", Color.Red);
                return;
            }
            else
            {
                SetStatus("Card reading mode set successfully！", Color.Green);
                button_ParamRefresh_Click(null,null);
            }
        }


        private void comboBox_MemoryArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox_MemoryArea.Text == "EPC区") && (comboBox_accessType.Text == "写"))
            {
                textBox_AccessOffset.Text = "2";
            }
            else
            {
                textBox_AccessOffset.Text = "0";
            }
        }
        private void button_setMac_Click(object sender, EventArgs e)
        {
            //SetMac
            byte[] mac = Tools.StringToMac(textBox_setMac.Text.Replace(" ", ""));
            Status_enum retval = objreader.SetMac(mac);
            if (retval == Status_enum.SUCCESS)
            {
                SetStatus("MAC address set successfully！", Color.Green);
            }
            else
            {
                SetStatus("MAC address setting failed！", Color.Red);
            }
        }

        public void SetStatus(string str, Color col)
        {

            this.BeginInvoke((EventHandler)delegate
            {
                toolStripStatusLabelStatus.Text = "status：" + str;
                toolStripStatusLabelStatus.ForeColor = col;
            });
        }
        private void ReadTag_Click(object sender, EventArgs e)
        {

        }

    }
}
