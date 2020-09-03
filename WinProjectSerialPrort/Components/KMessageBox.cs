using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MyComponets
{

   
    public class KMessageBox:Form
    {
        private const int WIDTH_FORM = 310;
        private const int HEIGHT_FORM = 220;
        private const int WIDTH_LBL_MESSAGE = 260;
        private const int HEIGHT_LBL_MESSAGE = 90;

        public delegate void ActionClick(Object oo);
        //public event ActionClick clickAccept;
        public static int MESSAGE_TYPE_SUCCESS=876789;
        public static int MESSAGE_TYPE_WARNING = 463574;
        public static int MESSAGE_TYPE_ERROR = 383223;
        private Point  flocation;
        private Label lblMessage;
        private Button btnAccept;

        KMessageBox(Point fatherLocation, string message,string title)
        {
            this.flocation = fatherLocation;
            this.Text = title;
            this.initialize(message);
        }

        private void initialize(string message)
        {

            this.lblMessage = new Label();
            this.lblMessage.Text = message;
            this.lblMessage.Size = new Size(WIDTH_LBL_MESSAGE,HEIGHT_LBL_MESSAGE);
            int num = (WIDTH_FORM - WIDTH_LBL_MESSAGE) / 2;
            this.lblMessage.Location = new Point(num,num);

            this.btnAccept = new Button();
            this.btnAccept.Text = "Accept";
            this.btnAccept.Location = new Point((WIDTH_FORM - this.btnAccept.Width) / 2, HEIGHT_FORM - (num*3+this.btnAccept.Height));
            this.btnAccept.Click += new EventHandler(onClickAccept);                        

            this.Size = new Size(WIDTH_FORM, HEIGHT_FORM);
            this.Location = flocation;


            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ShowIcon = false;

            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnAccept);
        }

        public static void show(Point fatherLocation, string message, string title)
        {
            KMessageBox kmessagebox = new KMessageBox(fatherLocation, message, title);
            kmessagebox.Show();
        }
        public void onClickAccept(object sender,Object o)
        {
            this.Dispose();
        }
               
    }
}
