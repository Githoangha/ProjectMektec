using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace ReadCode
{
    public partial class ucViewImage : UserControl
    {
        private HWindow _Window = null;
        public ucViewImage()
        {
            InitializeComponent();
            WindowView.HKeepAspectRatio = false;
            _Window = WindowView.HalconWindow;
            _Window.SetDraw("margin");
            _Window.SetLineWidth(3);
        }
        public void ViewImage(HImage image)
        {
            SmartSetPart(image, WindowView);
            image.DispImage(_Window);

        }
        public void DispObj(HObject obj, string color)
        {
            _Window.SetColor(color);
            _Window.DispObj(obj);
        }
        public void ViewResult(string name, double row, double col)
        {
            _Window.DispText(name, "image", row, col, new HTuple(), new HTuple(), new HTuple());
        }
        public void ViewResult(HTuple lstName, HTuple lstRow, HTuple lstCol, HTuple color)
        {
            _Window.DispText(lstName, "image", lstRow, lstCol, color, new HTuple(), new HTuple());
        }

        public void SaveImage(string codeJig, string nameImage,int Index)
        {
            string Path =@"D:\\9999.SaveImage_Missing_NoRead";
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            Path = Path + "\\" + codeJig + "-" + DateTime.Now.ToString("yyyyMMdd_HHmm");
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
            string fileName = Path + @"\\CAM_"+Index+ " - " + nameImage + "_" + DateTime.Now.ToString("dd_MM_yyyy HH_mm_ss") + ".bmp";
            _Window.DumpWindow("bmp", fileName);
        }

        public static void SmartSetPart(HObject Image, HSmartWindowControl HSWindow)
        {
            if (Image == null || Image.Key == IntPtr.Zero)
                return;

            HTuple width = null;
            HTuple height = null;
            try
            {
                HOperatorSet.GetImageSize(Image, out width, out height);
            }
            catch (HalconException ex)
            {
                return;
            }

            if (width == null || width.Length <= 0 || height == null || height.Length <= 0)
                return;
            HSWindow.HalconWindow.GetWindowExtents(out int row, out int col, out int wdWidth, out int wdHeight);

            int imgRow, imgCol;
            int imgWidth, imgHeight;
            //
            if (((float)wdHeight / (float)wdWidth) > ((float)height / (float)width))
            {
                imgCol = 0;
                imgWidth = width;
                imgHeight = wdHeight * imgWidth / wdWidth;
                imgRow = (height - imgHeight) / 2;
            }
            else
            {
                imgRow = 0;
                imgHeight = height;
                imgWidth = wdWidth * imgHeight / wdHeight;
                imgCol = (width - imgWidth) / 2;
            }


            HSWindow.HalconWindow.SetPart(new HTuple(imgRow), new HTuple(imgCol), new HTuple(imgHeight + imgRow),
                new HTuple(imgWidth + imgCol));
        }
    }
}
