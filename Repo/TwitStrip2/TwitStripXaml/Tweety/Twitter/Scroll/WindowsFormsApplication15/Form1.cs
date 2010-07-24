using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication15
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            scroll1.LorR(1);
            
            scroll1.Speed(20);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            scroll1.lbl(textBox1.Text);
        }

    }
}
