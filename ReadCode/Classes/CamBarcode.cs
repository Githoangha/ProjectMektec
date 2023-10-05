using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ReadCode
{
    public class CamBarcode
    {
        //public string NameCam { get; set; }
        public string IpCam { get; set; }
        public int PortCam { get; set; }
        public bool IsConnected { get; set; }
        //public int NumJigPlasma { get; set; }
        public bool IsComplete { get; set; }

        private Thread Thread_TCP;

        private Socket BarcodeReader;
        public CamBarcode()
        {
            //this.NameCam = NameCam;
            this.IpCam = IpCam;
            this.PortCam = PortCam;
        }
        public CamBarcode(string ip, int port)//string name,
        {
            //NameCam = name;
            IpCam = ip;
            PortCam = port;
            IsConnected = false;
            //NumJigPlasma = numJig;
        }
        public void Run_Thread_TCP()
        {
            Thread_TCP = new Thread(new ThreadStart(StartReadTag));
            Thread_TCP.IsBackground = true;
            Thread_TCP.Start();
        }
        public void Stop_Thread_TCP()
        {
            if(Thread_TCP.IsAlive)
            {
                Thread_TCP.Abort();
            }    
        }
        public bool Connect()
        {
            if (BarcodeReader != null) { BarcodeReader.Close(); BarcodeReader.Dispose(); BarcodeReader = null; }
            BarcodeReader = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress Ip = IPAddress.Parse(IpCam);
            IPEndPoint endPoint = new IPEndPoint(Ip, PortCam);
            BarcodeReader.Connect(endPoint);
            IsConnected = BarcodeReader.Connected;
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
            
            IsComplete = true;
        }


        public void SendSignal(byte[] bytesend)
        {
            if (BarcodeReader.Connected && BarcodeReader != null) BarcodeReader.Send(bytesend);
        }
    }
}
