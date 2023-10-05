using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadCode
{
    public class WorkerThreadAwaitVC3000
    {
        //int nRet = CIOControllerSDK.MV_OK;
        private Thread Thread_Await_VC3000;

        public  WorkerThreadAwaitVC3000()
        {
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
        public void Main_Thread()
        {
            Thread_Await_VC3000 = new Thread(new ThreadStart(StartWorkerThread));
            Thread_Await_VC3000.IsBackground = true;
            Thread_Await_VC3000.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Set_NPN_Enable()
        {
            try
            {
                int nRet = CIOControllerSDK.MV_OK;
                int nType = (int)IOController.PNP_ENABLE_STATE.NPN;//or PNP
                nRet = CIOControllerSDK.MV_IO_SetMainGPO_NPN_CS(nType);
                if (CIOControllerSDK.MV_OK != nRet)
                {
                    MessageBox.Show("NPN Enable is Fail");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// Trigger Run
        /// </summary>
        /// <returns></returns>
        public bool IsTrigger()
        {
            try
            {
                byte[] byteStatus = new byte[1024];
                int nRet = nRet = CIOControllerSDK.MV_IO_GetMainInputLevel_CS(ref byteStatus[0]);
                int nGPIStatus = BitConverter.ToInt32(byteStatus, 0);

                if (CIOControllerSDK.MV_OK != nRet)
                    return false;

                bool isTrigger = (nGPIStatus & 0x01) == 0x01;
                //bool isHaveJig1 = (nGPIStatus & 0x02) == 0x02;
                bool isHaveJig2 = (nGPIStatus & 0x04) == 0x04;

                return isTrigger && isHaveJig2;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Check Sensor
        /// </summary>
        /// <returns></returns>
        public bool IsSensor()
        {
            try
            {
                byte[] byteStatus = new byte[1024];
                int nRet = nRet = CIOControllerSDK.MV_IO_GetMainInputLevel_CS(ref byteStatus[0]);
                int nGPIStatus = BitConverter.ToInt32(byteStatus, 0);

                if (CIOControllerSDK.MV_OK != nRet)
                    return false;
                bool isHaveJig2 = (nGPIStatus & 0x04) == 0x04;

                return isHaveJig2;
            }
            catch
            {
                return false;
            }
        }
        public bool IsButton()
        {
            try
            {
                byte[] byteStatus = new byte[1024];
                int nRet = nRet = CIOControllerSDK.MV_IO_GetMainInputLevel_CS(ref byteStatus[0]);
                int nGPIStatus = BitConverter.ToInt32(byteStatus, 0);

                if (CIOControllerSDK.MV_OK != nRet)
                    return false;
                bool Isbutton = (nGPIStatus & 0x01) == 0x01;

                return Isbutton;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Bật đèn
        /// </summary>
        /// <returns></returns>
        public bool LightOn()
        {
            Set_NPN_Enable();
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_3,CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_HIGH);
                if (CIOControllerSDK.MV_OK != nRet)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Tắt đèn
        /// </summary>
        /// <returns></returns>
        public bool LightOff()
        {
            Set_NPN_Enable();
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                nRet = CIOControllerSDK.MV_IO_SetMainOutputLevel_CS(CIOControllerSDK.MV_IO_MAINPORT_NUMBER.MV_MAINIO_PORT_3, 
                    CIOControllerSDK.MV_GIO_LEVEL.MV_GIO_LEVEL_LOW);
                if (CIOControllerSDK.MV_OK != nRet)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Chạy thread chờ VC3000 gửi tính hiệu IO 
        /// </summary>
        public void StartWorkerThread()
        {
            CIOControllerSDK.MV_IO_VERSION stVersion = new CIOControllerSDK.MV_IO_VERSION();
            CIOControllerSDK.MV_IO_GetSDKVersion_CS(ref stVersion);
            try
            {
                while (c_varGolbal.IsRun)
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

        /// <summary>
        /// Dừng thread 
        /// </summary>
        public void StopWorkerThread()
        {
            try
            {
                if (Thread_Await_VC3000 != null)
                {
                    Thread_Await_VC3000.Abort();
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                throw;
            }
            
        }
        public int CheckConnect()
        {
            int nRet = CIOControllerSDK.MV_OK;
            try
            {
                byte[] byteStatus = new byte[1024];
                nRet = CIOControllerSDK.MV_IO_GetMainInputLevel_CS(ref byteStatus[0]);
                int nGPIStatus = BitConverter.ToInt32(byteStatus, 0);

                if (CIOControllerSDK.MV_OK != nRet)
                {
                    goto MyExit;
                }
                if ((nGPIStatus & 0x01) == 0x01)
                {
                   c_varGolbal.BitIO1 = c_varGolbal._isProduct = true;//c_varGolbal._isProduct=
                }
                if ((nGPIStatus & 0x02) == 0x02)
                {
                    c_varGolbal.BitIO2 = true;
                }
                if ((nGPIStatus & 0x04) == 0x04)
                {
                    c_varGolbal.BitIO3 = true;
                }
            MyExit:;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return nRet;

        }
    }
}
