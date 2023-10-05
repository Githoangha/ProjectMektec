using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineGolden_PLasma
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            initCulturalFormattingChanges();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo Plasma = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Plasma.OriginalFilename));
            if (processes.Length > 1)
            {
                MessageBox.Show("Chương trình Before Plasma đang chạy. Xin vui lòng kiểm tra lại!", "RTC - Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Frm_Login newfrm = new Frm_Login();
                if (newfrm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Frm_Main());
                }
            }

        }
        private static void initCulturalFormattingChanges()
        {
            CultureInfo cultureDefinition = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cultureDefinition.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            cultureDefinition.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            cultureDefinition.DateTimeFormat.LongTimePattern = "HH:mm:ss";
            cultureDefinition.DateTimeFormat.LongDatePattern = "yyyy/MM/dd";
            Thread.CurrentThread.CurrentCulture = cultureDefinition;
        }
    }
}
