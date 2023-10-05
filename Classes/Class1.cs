using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Drawing;

namespace LineGolden_PLasma.Classes
{
    public struct View
    {
        public decimal length;
        public decimal width;
        public decimal height;

        public void DefaultView()
        {
            length = 0;
            width = 0;
            height = 0;
        }
        public void SetView(decimal length, decimal width, decimal height)
        {
            this.length = length;
            this.width = width;
            this.height = height;
        }
        public void Draw2D(decimal length, decimal width)
        {

        }
        public void Draw3D(decimal length, decimal width, decimal height)
        {  
           
            
        }
        public void Test()
        {
            Bitmap bitmap = new Bitmap(1000, 800, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
        }
    }
}
