using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace ReadCode
{
    public class ComboBoxItem
    {
        public ComboBoxItem() { }

        public ComboBoxItem(string pText, object pValue)
        {
            text = pText; val = pValue;
        }

        public ComboBoxItem(string pText, object pValue, Color pColor)
        {
            text = pText; val = pValue; foreColor = pColor;
        }

        string text = "";
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        object val;
        public object Value
        {
            get { return val; }
            set { val = value; }
        }

        Color foreColor = Color.Black;
        public Color ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        public override string ToString()
        {
            return text;
        }
    }

    public partial class ColorCombobox : ComboBox
    {
        public ColorCombobox()
        {
            //InitializeComponent();
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        }

        public ColorCombobox(IContainer container)
        {
            container.Add(this);

            //InitializeComponent();

            this.Items.Add(new ComboBoxItem("Green", "0", Color.Green));
            this.Items.Add(new ComboBoxItem("Blue", "0", Color.Blue));
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            if (e.Index < 0) { return; }
            e.DrawBackground();
            ComboBoxItem item = (ComboBoxItem)this.Items[e.Index];
            Brush brush = new SolidBrush(item.ForeColor);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { brush = Brushes.Yellow; }

            Color c = item.ForeColor;
            Brush b = new SolidBrush(c);
            Rectangle rect = e.Bounds;

            e.Graphics.DrawString(item.Text,
                this.Font, brush, e.Bounds.X, e.Bounds.Y);
            e.Graphics.FillRectangle(b, rect.X + 50, rect.Y + 5, rect.Width - 10, rect.Height - 10);
        }
        object selectedValue = null;
        public new Object SelectedValue
        {
            get
            {
                object ret = null;
                if (this.SelectedIndex >= 0)
                {
                    ret = ((ComboBoxItem)this.SelectedItem).Value;
                }
                return ret;
            }
            set { selectedValue = value; }
        }
        string selectedText = "";
        public new String SelectedText
        {
            get
            {
                return ((ComboBoxItem)this.SelectedItem).Text;
            }
            set { selectedText = value; }
        }
    }
}
