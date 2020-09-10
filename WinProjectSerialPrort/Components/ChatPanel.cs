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
        const int PADD_LBL_MESSAGE_Y_BOTTOM = 8;


        const int SPACE_BETWEEN_MESSAGEBOX = 4;
        const int HEIGHT_LINE_PX = 13;
        private List<ItemChat> historyChatList;
        private int position_y;
        private int position_x;
        private int width_panel;
        private int height_panel;    

        private Dictionary<string,ItemChat> chatFileItems;
        private delegate void HandlerUpdate(string key, float progress);
        private delegate void HandelerADD(ItemChat item);
        private delegate void HandelerADDMessage(string message, string title, bool received);
        private delegate void HandelerAddNewFile(string key, string path, bool received);


        private HandelerADDMessage actionAddMessage;
        private HandelerAddNewFile actionAddNewFile;
        private HandlerUpdate update;
        private HandelerADD add;


        public delegate void ChangeheightPanel();
        public ChangeheightPanel changeheightPanel;

        public ChatPanel(int width, int height,int x,int y)
        {
            this.historyChatList = new List<ItemChat>();
            this.chatFileItems = new Dictionary<string, ItemChat>();
            this.width_panel = width;
            this.height_panel = height;
            this.MinimumSize = new Size(width, height);
            this.Location = new Point(x, y);
            this.BorderStyle= System.Windows.Forms.BorderStyle.Fixed3D;
            this.position_y = 9;
            this.position_x = 10;
            this.update += new HandlerUpdate(this.up);
            this.add += new HandelerADD(this.addFile);
            this.actionAddMessage += new HandelerADDMessage(this.onAddNewMessage);
            this.actionAddNewFile += new HandelerAddNewFile(this.onAddNewFile);
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
        public void onAddNewMessage(string message, string title, bool received)
        {
            Size size = this.calcSizeBoxMensaje(message);
            Point point = this.calcPosittionBoxMessage(received, size.Width, size.Height);
            ItemChat item = new ItemChat(size, point, message, received);
            this.Controls.Add(item);
            if (position_y > this.Height)
            {
                this.Height = position_y;
            }
            if (this.changeheightPanel != null)
            {
                this.changeheightPanel();
            }
        }
        public void addNewMessage(string message, string title, bool received)
        {
            Invoke(this.actionAddMessage,message,title,received);
        }
        public void onAddNewFile(string key, string path, bool received)
        {
            Size size = this.calcSizeBoxMensaje(path);
            Point point = this.calcPosittionBoxMessage(received, size.Width, size.Height);
            ItemChat item = new ItemChat(size, point, path, received, true);
            this.chatFileItems.Add(key, item);
            if (this.add != null) Invoke(this.add, item);
            if (position_y > this.Height)
            {
                this.Height = position_y;
            }
            if (this.changeheightPanel != null)
            {
                this.changeheightPanel();
            }

        }
        public void addNewFile(string key,string path,bool received)
        {
            Invoke(this.actionAddNewFile, key,path, received);
        }
        public void addFile(ItemChat item)
        {
            this.Controls.Add(item);
        }
        public void upadateProgress(string key, float progress)
        {
            Invoke(this.update, key, progress);
        }
        public void up(string key,float progress)
        {
            if (this.chatFileItems != null) this.chatFileItems[key].updateProgress(progress);
        }

        public class ItemChat : Panel
        {
            private bool received;
         
            private Label message;
            private Label progress;


            public ItemChat(Size sise, Point point, string message, bool received)
               :this(sise,point,message,received,false)
            {                
            }
            public ItemChat(Size sise,Point point,string path,bool received,bool file)
            {
                this.Size = sise;
                this.Location = point;                
                this.received = received;
                this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.initializeComponents(path,received, file);
            }          
            public void updateProgress(float progress)
            {
                double prog = ((int)(progress*100))/ 100.0;
                if(this.progress!=null )this.progress.Text = (prog).ToString()+"%";
            }
            
            private void initializeComponents(string message,bool received,bool file)
            {   if (file)
                {
                    this.progress = new Label();
                    this.progress.Text = "0%";
                    this.progress.AutoSize = true;
                }
                this.message = new Label();
                this.message.Text = message;
                this.message.Location = new Point(PADD_LBL_MENSAJE_X,PADD_LBL_MESSAGE_Y_TOP);               
                this.message.Size =new Size(
                    this.Size.Width-2*PADD_LBL_MENSAJE_X - (file?50:0),
                    this.Size.Height- PADD_LBL_MESSAGE_Y_TOP- PADD_LBL_MESSAGE_Y_BOTTOM);
                this.message.AutoSize = false;
                if (file)this.progress.Location = new Point(this.Size.Width -40, PADD_LBL_MESSAGE_Y_TOP);
               
                if (received)
                {
                    this.BackColor = Color.White;
                }
                else
                {
                    this.BackColor = Color.Teal;
                    this.ForeColor = Color.White;                   
                    this.message.ForeColor = Color.White;
                }
                this.Controls.Add(this.message);
                if(file)this.Controls.Add(this.progress);
            }
        }
    }    

}
