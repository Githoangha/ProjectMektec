using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;
using System.Threading;

namespace LineGolden_PLasma
{
    public class CamBarcode
    {
        public string NameCam { get; set; }
        public string IpCam { get; set; }
        public int PortCam { get; set; }
        public bool IsConnected { get; set; }
        public int NumJigPlasma { get; set; }
        public bool IsComplete { get; set; }

        public string ShowTime { get; set; }

        private Socket BarcodeReader;
        ModbusClient modbusClient = new ModbusClient();
        
        Thread threadReadJig;
            
        public CamBarcode(string ip, int port, int numJig)
        {
            IpCam = ip;
            PortCam = port;
            IsConnected = false;
            NumJigPlasma = numJig;
        }
        public bool Connect_ModBus()
        {
            if (modbusClient.Connected) 
            {
                modbusClient.Disconnect();
            }
            modbusClient.IPAddress = IpCam;
            modbusClient.Port = PortCam;
            modbusClient.Connect();
            IsConnected = modbusClient.Connected;
            return IsConnected;
        }
        public bool Connect()
        {
            
            if (BarcodeReader != null) { BarcodeReader.Close(); BarcodeReader.Dispose(); BarcodeReader = null; }
            IPAddress Ip = IPAddress.Parse(IpCam);
            IPEndPoint endPoint = new IPEndPoint(Ip, PortCam);
            BarcodeReader = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            BarcodeReader.Connect(endPoint);
            IsConnected = BarcodeReader.Connected;
            return IsConnected;
        }
        public bool Disconnect_ModBus()
        {
            if (modbusClient != null) 
            {
                modbusClient.Disconnect(); 
                IsConnected = modbusClient.Connected;
                modbusClient = null; }
            return IsConnected;
        }
        public bool Disconnect()
        {
            if (BarcodeReader != null) { BarcodeReader.Close(); IsConnected = BarcodeReader.Connected; BarcodeReader.Dispose(); BarcodeReader = null; }
            return IsConnected;
        }
        public void StartReadTag()
        {
            string tempData = "";
            string tempString = "";
            IsComplete = false;
            
            //uc_Plasma.List_DataTagPlasmaInput.Clear();
            while ((uc_Plasma.List_DataTagPlasmaInput.Count < NumJigPlasma || uc_Plasma.List_DataCodeTray.Count<2) && uc_Plasma.IsExecuteFunctionReadTag)//Process lấy dữ liệu từ Barcode
            {
                //uc_Plasma.List_DataTagPlasmaInput.Clear();
                if (BarcodeReader != null && BarcodeReader.Connected)
                {
                    try
                    {
                        byte[] buffer = new byte[200];
                        BarcodeReader.Receive(buffer);
                        tempString = Encoding.ASCII.GetString(buffer).Replace("\0", "");
                        if (!uc_Plasma.IsExecuteFunctionReadTag)
                        {
                            break;
                        }
                        StringBuilder sbBarcode = new StringBuilder();
                        foreach (char c in tempString)
                        {
                            sbBarcode.Append(c);
                            if (c == ASCII.CR)
                            {
                                tempData = sbBarcode.ToString();
                                sbBarcode.Clear();
                                tempData = tempData.Replace("\0", "").Replace("\r", "").Replace("\n", "").Replace("%", "").Trim();
                                tempData = tempData.Trim();
                                if (uc_Plasma.List_DataTagPlasmaInput.Contains(tempData) || tempData.ToUpper().Trim()=="NOREAD")//Process lấy dữ liệu từ Barcode
                                {
                                    tempData = "";
                                }
                                else
                                {

                                    if(tempData.ToUpper().Contains("X4") || tempData.ToUpper().Contains("X5"))//X5B, X4B,X5A,X4A
                                    {
                                        if(!uc_Plasma.List_DataCodeTray.Contains(tempData))
                                        {
                                            uc_Plasma.List_DataCodeTray.Add(tempData);
                                        }    
                                        uc_Plasma.List_DataCodeTray = uc_Plasma.List_DataCodeTray.Distinct().ToList();
                                    }
                                    else
                                    {
                                        uc_Plasma.List_DataTagPlasmaInput.Add(tempData);//Process lấy dữ liệu từ Barcode
                                        uc_Plasma.List_DataTagPlasmaInput = uc_Plasma.List_DataTagPlasmaInput.Distinct().ToList();//Process lấy dữ liệu từ Barcode
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            IsComplete = true;
        }

        /// <summary>
        /// HuyNV Write
        /// </summary>
        public void StartReadTagNew()
        {
            string tempString = "";
            IsComplete = false;
            DateTime start = DateTime.Now;
            try
            {
                while ((uc_Plasma.List_DataTagPlasmaInput.Count < NumJigPlasma || uc_Plasma.List_DataCodeTray.Count < 2) && uc_Plasma.IsExecuteFunctionReadTag) //Không dùng
                {
                    if (BarcodeReader != null && BarcodeReader.Connected)
                    {
                        try
                        {
                            byte[] buffer = new byte[200];
                            string[] CRLF = new string[] { "\r\n" };
                            BarcodeReader.Receive(buffer);
                            tempString = Encoding.ASCII.GetString(buffer).Replace("\0", "");
                            if (!uc_Plasma.IsExecuteFunctionReadTag)
                            {
                                break;
                            }
                            int end = tempString.LastIndexOf("\r\n");
                            tempString = tempString.Substring(0, end);
                            List<string> TempData = tempString.Split(CRLF, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (var item in TempData)
                            {
                                string jig = item.Replace("\0", "").Replace("\r", "").Replace("\n", "").Replace("%", "").Trim();
                                jig = jig.Trim();
                                if (uc_Plasma.List_DataTagPlasmaInput.Contains(jig) || jig.ToUpper().Trim() == "NOREAD")//Process lấy dữ liệu từ Barcode
                                {
                                    jig = "";
                                }
                                else
                                {

                                    if (jig.ToUpper().Contains("X4") || jig.ToUpper().Contains("X5"))//X5B, X4B,X5A,X4A
                                    {
                                        if (!uc_Plasma.List_DataCodeTray.Contains(jig))
                                        {
                                            uc_Plasma.List_DataCodeTray.Add(jig);
                                        }
                                        uc_Plasma.List_DataCodeTray = uc_Plasma.List_DataCodeTray.Distinct().ToList();
                                    }
                                    else
                                    {
                                        uc_Plasma.List_DataTagPlasmaInput.Add(jig);//Process lấy dữ liệu từ Barcode
                                        uc_Plasma.List_DataTagPlasmaInput = uc_Plasma.List_DataTagPlasmaInput.Distinct().ToList();//Process lấy dữ liệu từ Barcode
                                    }

                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                IsComplete = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DateTime stop = DateTime.Now;
                ShowTime = $"thời gian xử lý thực tế {(stop - start).TotalSeconds}";
            }
            
        }

     
        public void SendSignal(byte[] bytesend)
        {
            if (BarcodeReader.Connected && BarcodeReader != null) BarcodeReader.Send(bytesend);
        }
        public void SendSignal_Modbus(byte[] bytesend)
        {
            if (modbusClient.Connected && modbusClient != null) 
                modbusClient.sendData= bytesend;
        }
    }
}
