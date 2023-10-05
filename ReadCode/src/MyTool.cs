using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace HRMyTool
{
    static public class MyTool
    {
        static public int GetCtrlsCount(Form frm)
        {
            int count = 0;
            foreach (Control ctrl in frm.Controls)
            {
                count++;
                if (ctrl.Controls.Count > 0)
                {
                    count += GetCtrlsCount(ctrl);
                }
            }

            return count;
        }

        static public int GetCtrlsCount(Control ctrl)
        {
            int count = 0;
            foreach (Control subctrl in ctrl.Controls)
            {
                count++;
                if (subctrl.Controls.Count > 0)
                {
                    count += GetCtrlsCount(subctrl);
                }
            }

            return count;
        }

        static public List<Control> GetCtrls(Form frm)
        {
            List<Control> ctrls = new List<Control>();
            foreach (Control ctrl in frm.Controls)
            {
                ctrls.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    ctrls.AddRange(GetCtrls(ctrl));
                }
            }
            return ctrls;
        }

        static public List<Control> GetCtrls(Control ctrl)
        {
            List<Control> ctrls = new List<Control>();
            foreach (Control subctrl in ctrl.Controls)
            {
                ctrls.Add(subctrl);
                if (subctrl.Controls.Count > 0)
                {
                    ctrls.AddRange(GetCtrls(subctrl));
                }
            }
            return ctrls;
        }

        [DllImport("winmm")]
        static extern uint timeGetTime();
        [DllImport("winmm")]
        static extern void timeBeginPeriod(int t);
        [DllImport("winmm")]
        static extern void timeEndPeriod(int t);

    }
}





