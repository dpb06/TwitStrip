using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Timers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Core;

namespace Marquee_ScreenSaver
{
    public partial class Marquee : Form
    {


        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            //Getter methods
            public int Left
            { get { return left; } }
            public int Top
            { get { return top; } }
            public int Right
            { get { return right; } }
            public int Bottom
            { get { return bottom; } }
            public int Width
            { get { return right - left; } }
            public int Height
            { get { return bottom - top; } }
        }
        [DllImport("user32.DLL", EntryPoint = "SetParent")]
        private static extern long SetParent(long Child, long parent);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.DLL", EntryPoint = "InvalidateRect")]
        private static extern int InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

        [DllImport("user32.DLL", EntryPoint = "IsWindowVisible")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private System.Timers.Timer timer1 = null;
        private System.Timers.Timer timer2 = null;
   

        //newer message: nextPos+1 --> nextPos+n gives the nth message 
        //Pos after nextPos, update nextPos:  nextPos = (nextPos++) % _Max_Messages
        private static int _Max_Messages = 30;

        private static List<Result> FriendsTimeline = Twitter.GetFriendsTimeline("sad","asd");


        private int CurrentCount = 0;// max of _Max_Messages(when CurrentCount == _Max_Messages 
        // NextPos will start wrapping around)
        private string[] Messages = new string[_Max_Messages];

        //Postion on the graphic. is X - X_Positions[NextIndex];
        //new String s goes at CurrentXLimit which is stored at XPositions[NextIndex]
        private int[] XPositions = new int[_Max_Messages];

        private string[] Sender = new string[_Max_Messages];// name of sender?id?
        //Where to store the next message
        //nextIndex = (nextIndex++) % _Max_Messages
        private int NextIndex = 0;
        //CurrentXLimit += (int)(10+ g.MeasureString( s , this.Font).Width);
        private int CurrentXLimit = 0;// the far right side of any strings if drawn in the current font(where to add to)


        //Eventually store the image urls in here when we have the actual twitter going
        private string[] SenderImageUrls = new string[_Max_Messages];


        //With this version because int's are used may need to make sure that X is not larger than about 4,000,000,000 (lol)
        //however X Will keep on growing, eventually(long time later) reaching the limit.

        //also because of the scrolling text the latest Twitts may not be obvious
        //have two Twit lines? 1 scrolling(will be able to jump back and forth easy, as well as jumping forward and back messages)
        //                     1 static   (also movable maybe but restarts when the twits refresh?)

        private static Boolean TEST = false;


        private static IntPtr parent = new IntPtr(0);
        string DisplayString = "TextTicker for SimpleTwit";
        private int X = 0;
        private int Y = 0;
        Random rand = new Random();
        private int m_speed = 50;
        private bool location_random = false;
        private Color FColor;
        private Color BColor;

        private ArrayList colorArray = new ArrayList();
        private CheckBox checkBox1;
        private Panel panel1;
        private Button button1;
        private TextBox textBox1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Marquee()
        {
            colorArray.Add(Color.YellowGreen);
            colorArray.Add(Color.Blue);
            colorArray.Add(Color.Chocolate);
            colorArray.Add(Color.DarkGreen);
            colorArray.Add(Color.Gold);
            colorArray.Add(Color.Khaki);
            colorArray.Add(Color.MintCream);
            colorArray.Add(Color.LightCoral);
            colorArray.Add(Color.Purple);
            colorArray.Add(Color.Yellow);
            colorArray.Add(Color.Black);
            colorArray.Add(Color.Azure);
            colorArray.Add(Color.Firebrick);
            colorArray.Add(Color.DarkMagenta);
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }



        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.timer1 = new System.Timers.Timer();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.SynchronizingObject = this;
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.checkBox1.Location = new System.Drawing.Point(3, 39);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(217, 29);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Show Status Update";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 75);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(339, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(549, 38);
            this.textBox1.TabIndex = 2;
            // 
            // Marquee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(900, 100);
            this.ControlBox = false;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1200, 100);
            this.Name = "Marquee";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Marquee";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Marquee_Load);
            this.Click += new System.EventHandler(this.Marquee_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Marquee_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Marquee_KeyPress);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Marquee_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
            foreach (Result result in FriendsTimeline)
            {
                result.
            }

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] cmdList = Environment.CommandLine.Split(' ');
            // First Command parm is the exe name
            // Second Param is the mode ..
            string s = "";

            for (int i = 0; i < cmdList.Length; i++)
                s += cmdList[i] + "  ";

            //if (cmdList[1].IndexOf("/c") >= 0)
            //{
            //    Application.Run(new Options());
            //    return;
            //}
            parent = IntPtr.Zero;

            if (cmdList[1] == "/p")	// Options 
                parent = (IntPtr)int.Parse(cmdList[2]);

            Application.Run(new Marquee());
            
        }
        private void Marquee_Load(object sender, System.EventArgs e)
        {
            // Starts in here when the program starts
            LoadSettings();

        
            if (IntPtr.Zero != parent)
            {
                this.Hide();
                timer2 = new System.Timers.Timer();
                timer2.Interval = m_speed;
                timer2.Elapsed += new System.Timers.ElapsedEventHandler(PREVIEW);
                timer2.Start();
                timer2.Enabled = true;

                RECT rc = new RECT();
                if (GetClientRect(parent, ref rc))
                {
                    X = rc.right;
                    Y = rc.Bottom / 2;
                    Graphics g = Graphics.FromHwnd(parent);
                    SizeF sf = g.MeasureString(DisplayString, this.Font);
                }
            }
            else
            {//set refresh rates and start timer
                timer1.Interval = m_speed;
                timer1.Start();
                timer1.Enabled = true;

                if (timer2 != null)
                {
                    timer2.Stop();
                    timer2.Enabled = false;
                }
               

                X = this.ClientRectangle.Right;
                Y = this.ClientRectangle.Bottom + 10;
            }
        }

        private void Marquee_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        { }

        private void Marquee_Click(object sender, System.EventArgs e)
        {


            LoadOptions();
            LoadSettings();
        }

        private void LoadOptions()
        {
          //  Options o = new Options();
          //  DialogResult dr = o.ShowDialog();
          //  if (dr == DialogResult.Cancel)
            //    Application.Exit();

            //DisplayString = o.DisplayText.Text;

            //Invalidate();// tells the graphics? (something anyway) that ther is refreshing to do
        }

        private void PREVIEW(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!IsWindowVisible(parent))
                Application.Exit();

            if (IntPtr.Zero != parent) //if pointer isnt pointing to Zero(where empty pointers point)
            {
                RECT rc = new RECT();
                if (GetClientRect(parent, ref rc))
                {   // creates a Graphics object to attatch to the parent which is pointed to(the window)
                    Graphics g = Graphics.FromHwnd(parent);
                    //WILL NEED THIS! measures the size(Height x Width) for the drawer
                    SizeF sf = g.MeasureString(DisplayString, this.Font);
                    X--; // step the string left one step
                    if (X <= Convert.ToInt32(-sf.Width)) // if the string starts more than the string length off the page
                    // then the whole string has left the page, 
                    //   so start it again in another position(Y is random)
                    //( do we want more tweets to have already started going by now?)
                    {
                        X = rc.Right;
                        if (location_random)
                        {
                            Y = rand.Next(rc.Bottom);
                            if (Y < Convert.ToInt32(sf.Height))
                                Y = Convert.ToInt32(sf.Height);
                        }
                    }
                    //					InvalidateRect(parent , ref rc , true); // this line says that the drawn rect is out of date
                    g.Clear(BColor);
                    g.DrawString(DisplayString, this.Font, new SolidBrush(FColor), new Point(X, Y), StringFormat.GenericDefault);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Marquee_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //////////////////////////////////////////

            Graphics g = e.Graphics;
            if (TEST)
            {
                Messages[0] = "Hello ";
                Messages[1] = "World ";
                Messages[2] = "lol ";
                CurrentCount = 3;
                NextIndex = 3;

                XPositions[0] = 0;// just making sure i get the value not the pointer
                XPositions[1] = XPositions[0] + (int)(10 + g.MeasureString(Messages[0], this.Font).Width);
                XPositions[2] = XPositions[1] + (int)(10 + g.MeasureString(Messages[1], this.Font).Width);
                CurrentXLimit = XPositions[2] + (int)(10 + g.MeasureString(Messages[2], this.Font).Width);

            }
            else
            {
                Messages = DisplayString.Split(' ');
                XPositions[0] = 0;
                for (int i = 1; i < Messages.Length; i++)
                    XPositions[i] = XPositions[i - 1] + (int)(10 + g.MeasureString(Messages[i - 1], this.Font).Width);
                CurrentCount = Messages.Length;
            }
            Rectangle rc = e.ClipRectangle;
            if (true)//TEST)
            {
                for (int i = 0; i < CurrentCount; i++)
                    g.DrawString(Messages[i], this.Font, new SolidBrush(FColor), new Point(XPositions[i] + X, Y), StringFormat.GenericDefault);
            }
            else
                g.DrawString(DisplayString, this.Font, new SolidBrush(FColor), new Point(X, Y), StringFormat.GenericDefault);
        }

        /// <summary>
        /// What to do when the Timer ticks.
        /// Currently shifts X(the position of the first string) by 1
        /// 
        /// and if needed resets Y and X to new positions
        /// (will need to decide on when to do this)
        /// </summary>
        /// <param name="sender">The current timer, options such as when to tick can be changed
        /// in designer(right click timer1 and change properties)</param>
        /// <param name="e"></param>
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            SizeF sf = g.MeasureString(DisplayString, this.Font);
            X--;

          //  if (!location_random)
            //    Y = textBox1.Location.Y / 2;

            if (X <= Convert.ToInt32(-sf.Width))
            {
                X = this.ClientRectangle.Right;
                if (location_random)
                {
                    Y = rand.Next(this.ClientRectangle.Bottom);
                    if (Y < Convert.ToInt32(sf.Height))
                        Y = Convert.ToInt32(sf.Height);
                }
            }
            Invalidate(this.ClientRectangle, false);	// this section needs to be redrawn on the next sweep
        }

        //what to do if a key is pressed when window selected
        private void Marquee_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //if ENTER or ESC are pressed (use this when typing status?)
            if (e.KeyChar == 13 || e.KeyChar == 27)
                Application.Exit();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0082)
            {
                Application.Exit();
            }
            base.WndProc(ref m);
        }
        //
        private void LoadSettings()
        {
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey sk1 = rk.OpenSubKey("SOFTWARE\\Marquee_ScreenSaver");
            if (sk1 == null)
            {
                this.ForeColor = Color.Khaki;
                this.BackColor = Color.Black;
                location_random = false;
            }
            else
            {
                int backColorSelectedItem = int.Parse((string)sk1.GetValue("bcolor"));
                int foreColorSelectedItem = int.Parse((string)sk1.GetValue("fcolor"));
                string m_text = (string)sk1.GetValue("text");
                string m_font = (string)sk1.GetValue("font");
                int m_fontSize = int.Parse((string)sk1.GetValue("fontSize"));
                m_speed = int.Parse((string)sk1.GetValue("speed"));
                int m_pos = int.Parse((string)sk1.GetValue("pos"));
                if (m_pos == 1)
                    location_random = true;

                this.Font = new Font(m_font, m_fontSize);
                FColor = this.ForeColor = (Color)colorArray[foreColorSelectedItem];
                BColor = this.BackColor = (Color)colorArray[backColorSelectedItem];
                DisplayString = m_text;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                textBox1.Visible = true;
            }
            else
                textBox1.Visible = false;
            
          //  DialogResult dr = o.ShowDialog();
          //  if (dr == DialogResult.Cancel)
            //    Application.Exit();

            //DisplayString = o.DisplayText.Text;

            //Invalidate();
        }

        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                Twitter.UpdateStatus(textBox1.Text, SettingHelper.UserName, SettingHelper.Password);
            if (e.KeyChar == 27)
                Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
