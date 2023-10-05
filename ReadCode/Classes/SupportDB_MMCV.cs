using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DatabaseInterface_MMCV;

namespace ReadCode
{   
    public  class SupportDB_MMCV
    {

        public static bool CheckHaveLock(string LotId, string lineID, string deviceID)
        {
            try
            {
                Plasma56 plasma = new Plasma56();
                string strcheck = "";
                strcheck = plasma.CheckHaveLock(LotId, lineID, deviceID);

                if (strcheck.ToUpper().Contains(GlobVar.NoLock))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"Error CheckLock\r\n{strcheck}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excep in Function CheckHaveLock\r\n{ex.ToString()}");
                return false;
            }

        }

        public static bool CheckQualifyJig(string LotID, string lineID, string DeviceID, string jig1, string Jig2, string staffID)
        {
            try
            {
                Plasma56 plasma = new Plasma56();
                string strcheck = "";
                strcheck = plasma.CheckToolJig(LotID, lineID, DeviceID, jig1, Jig2, staffID);

                if (strcheck.ToUpper().Contains(GlobVar.OK))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"Error Check Qualify Jig\r\n{strcheck}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excep in Function Check Qualify Jig\r\n{ex.ToString()}");
                return false;
            }

        }
    }
}
