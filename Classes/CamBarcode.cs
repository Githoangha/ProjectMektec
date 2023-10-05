using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EasyModbus;

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

        private Socket BarcodeReader;
        ModbusClient modbusClient = new ModbusClient();
       

            
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
            while ((uc_Plasma.List_DataTagPlasmaInput.Count < NumJigPlasma || uc_Plasma.List_DataCodeTray.Count<2) && uc_Plasma.IsExecuteFunctionReadTag)
            {
                //uc_Plasma.List_DataTagPlasmaInput.Clear();
                if (BarcodeReader != null && BarcodeReader.Connected)
                {
                    try
                    {
                        byte[] buffer = new byte[1000];
                        BarcodeReader.Receive(buffer);
                        tempString = Encoding.ASCII.GetString(buffer).Replace("\0", "");
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
                                if (uc_Plasma.List_DataTagPlasmaInput.Contains(tempData) || tempData.ToUpper().Trim()=="NOREAD" )
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
                                        uc_Plasma.List_DataTagPlasmaInput.Add(tempData);
                                        uc_Plasma.List_DataTagPlasmaInput = uc_Plasma.List_DataTagPlasmaInput.Distinct().ToList();
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
            string tempData = "";
            string tempString = "";
            while (c_varGolbal.IsRun) //Không dùng
            {
                if (BarcodeReader != null && BarcodeReader.Connected)
                {
                    try
                    {
                        byte[] buffer = new byte[1000];
                        BarcodeReader.Receive(buffer);
                        tempString = Encoding.ASCII.GetString(buffer);
                        tempData = Encoding.ASCII.GetString(buffer).Replace("\0", "").Replace("\u0002", "").Replace("\u0003", "").Replace("\n", "").Replace("\r", "");
                        if (uc_Plasma.List_DataTagPlasmaInput.Contains(tempData) || tempData.ToUpper()=="NOREAD")
                        {
                            tempData = "";
                        }
                        else
                        {
                            uc_Plasma.List_DataTagPlasmaInput.Add(tempData);
                            uc_Plasma.List_DataTagPlasmaInput = uc_Plasma.List_DataTagPlasmaInput.Distinct().ToList();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

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
