using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication15
{
    public partial class Scroll : UserControl
    {
        int counter = 0;
        int strlen;
        int Locx = 0,//for send x1(location of label)
            Locxr = 0,//for send x2(panel width)
            Locy = 0;//for send y1(location of label)
        
        int status=1;
        public Scroll()
        {
            InitializeComponent();
        }
        private void Scroll_Load(object sender, EventArgs e)
        {
            int locx1 = lblText.Location.X;//int of x of lbl
            int locy1 = lblText.Location.Y;//int of y of lbl
            int locx2 = this.panel1.Width;//width of panel
            Locx = locx1;
            Locxr = locx2;
            Locy = locy1;
            lblText.Text = "Mohammad Javad Arshiyan";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (status == 0)
            {
                scrollRight();
            }
            if (status == 1)
            {
                scrollLeft();
            }
        }
        public void scrollLeft()
        {

            if ((counter++) != this.panel1.Width + strlen)
            {
                lblText.Location = new Point((((Locx) + counter)), Locy);
            }
            else
            {
                counter =(- (strlen));
            }

        }//Scroll lbl from left to right
        public void scrollRight()
        {
            lblText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            if (this.panel1.Width + strlen != (counter++))
            {
                lblText.Location = new Point(((Locxr- counter)), Locy);
            }
            else
            {
                counter = 0;
            }

        }//Scroll lbl from right to left
        public void lbl(string lbl)
        {
            lblText.Text = lbl;
            strlen=lbl.Length;
        }//Set Text to lbl
        public void lbl()
        {
            lblText.Text = "Mohammad Javad Arshiyan";
            strlen = lblText.Text.Length;
        }//Set Text to lbl(for empty)
        public void Speed(int delay)
        {
            timer1.Interval = delay;
        }//set Speed between 1 to 1000
        public void LorR(int bol)
        {
            status = bol;
        }//set scroll to left(1) or right(0)
    }
}
