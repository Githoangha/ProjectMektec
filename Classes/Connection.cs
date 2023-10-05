using ActUtlTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPlcMitsuMxCom;

namespace LineGolden_PLasma
{
    public class Connection
    {
        public ActUtlTypeClass plc = new ActUtlTypeClass();

        public void Connect()
        {
            plc.ActLogicalStationNumber = 1;
            //plc.Open();
            plc.Connect();
        }
        public void Disconnect()
        {
            if(plc!=null)
            {
                //plc.Close();
                plc.Disconnect();
            }    
        }

        
    }
}
