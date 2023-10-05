using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadCode
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string licenseFile = Path.Combine(Application.StartupPath, "License\\license.lic");
            //if (File.Exists(licenseFile))
            //{
            //    bool isActivated = RTCLicense.License.cLicense.CheckLicense(licenseFile,
            //        Application.ProductName) == 1;
            //    if (!isActivated)
            //    {
            //        MessageBox.Show("You do not have Lincese to run the program.\nPlease check again.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("You do not have Lincese to run the program.\nPlease check again.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            initCulturalFormattingChanges();
            //Application.Run(new Main());
            //Application.Run(new IOController());
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(fvi.OriginalFilename));
            if (processes.Length > 1)
            {
                MessageBox.Show("Chương trình Before Plasma đang chạy. Xin vui lòng kiểm tra lại!", "RTC - Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                frm_Login newfrm = new frm_Login();
                if (newfrm.ShowDialog() == DialogResult.OK)
                {

                    Application.Run(new frm_Main());
                    newfrm.Close();
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
