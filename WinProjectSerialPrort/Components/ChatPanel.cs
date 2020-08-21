using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyComponets
{
    public class ChatPanel:Panel
    {
        
        const int LENGTH_MESSAGE =69;
        const int PXWORD = 5;

        /// <summary>
        ///padding de la etiqueta que mostarra el mensaje. 
        /// </summary>
        const int PADD_LBL_MENSAJE_X = 6;
        const int PADD_LBL_MESSAGE_Y_TOP = 6;
        const int PADD_LBL_MESSAGE_Y_BOTTOM = 10;


        const int SPACE_BETWEEN_MESSAGEBOX = 13;
        const int HEIGHT_LINE_PX = 13;
        private List<ItemChat> historyChatList;
        private int position_y;
        private int position_x;
        private int width_panel;
        private int height_panel;

        public delegate void ChangeheightPanel();
        public ChangeheightPanel changeheightPanel;

        public ChatPanel(int width, int height,int x,int y)
        {
            this.historyChatList = new List<ItemChat>();
            this.width_panel = width;
            this.height_panel = height;
            this.MinimumSize = new Size(width, height);
            this.Location = new Point(x, y);
            this.BorderStyle= System.Windows.Forms.BorderStyle.Fixed3D;
            this.position_y = 9;
            this.position_x = 10;
            
        }
        private Size calcSizeBoxMensaje(string mensaje)
        {            
            int length_mensaje = mensaje.Length;
            int width = LENGTH_MESSAGE * PXWORD + 2 * PADD_LBL_MENSAJE_X;
            int height = (length_mensaje / LENGTH_MESSAGE)* HEIGHT_LINE_PX;
            if (length_mensaje % LENGTH_MESSAGE != 0)
                height+= HEIGHT_LINE_PX;
            return new Size(width, height+PADD_LBL_MESSAGE_Y_TOP+PADD_LBL_MESSAGE_Y_BOTTOM);
        }
        private Point calcPosittionBoxMessage(bool received,int widthMessageBox,int heightMessageBox)
        {
            int x = 0;
            int y = 0;
            if (received){
                x = this.position_x;           
            }
            else{
                x = this.width_panel - this.position_x - widthMessageBox;
            }
            y = this.position_y;
            this.position_y += (heightMessageBox + SPACE_BETWEEN_MESSAGEBOX);
            return new Point(x,y);
        }   
        
        public void addNewMessage(string message, string title, bool received)
        {
            Size size = this.calcSizeBoxMensaje(message);
            Point point = this.calcPosittionBoxMessage(received, size.Width, size.Height);
            ItemChat item = new ItemChat(size, point, message, title, received);
            this.historyChatList.Add(item);
            this.Controls.Add(item);
            if (position_y >this.Height)
            {
                this.Height = position_y;
            }
            if (this.changeheightPanel!=null)
            {
                this.changeheightPanel();
            }
        }
        public class ItemChat : Panel
        {
            private bool received;
            private Label title;
            private Label message;      
            public ItemChat(Size sise,Point point,string message,string title,bool received)
            {
                this.Size = sise;
                this.Location = point;                
                this.received = received;
                this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.initializeComponents(message, title,received);
            }
            private void initializeComponents(string message,string title,bool received)
            {

                this.title = new Label();
                this.title.Location = new Point(PADD_LBL_MENSAJE_X,0);
                this.title.Text = title;

                this.message = new Label();
                this.message.Text = message;
                this.message.Location = new Point(PADD_LBL_MENSAJE_X,PADD_LBL_MESSAGE_Y_TOP);
                this.message.Size =new Size(
                    this.Size.Width-2* PADD_LBL_MENSAJE_X,
                    this.Size.Height- PADD_LBL_MESSAGE_Y_TOP- PADD_LBL_MESSAGE_Y_BOTTOM);
                this.message.AutoSize = false;
                if (received)
                {
                    this.BackColor = Color.White;
                }
                else
                {
                    this.BackColor = Color.Teal;
                    this.ForeColor = Color.White;
                    this.title.ForeColor = Color.White; 
                    this.message.ForeColor = Color.White;
                }

             //   this.Controls.Add(this.title);
                this.Controls.Add(this.message);
            }
        }
    }    

}
