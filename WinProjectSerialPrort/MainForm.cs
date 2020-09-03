using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySerialPortKS;
using MyComponets;

namespace WinProjectSerialPrort
{
    public partial class MainForm : Form
    {
        private MySerialPort mySerialPort;
        private string portname;
       // private int speedBaudios;
        private ChatPanel myChatPanel;

        private List<String> listPathFiles;

        public delegate void HandlerReceivedMessage(string message);
        HandlerReceivedMessage loadMessageReived;

        public MainForm()
        {
            InitializeComponent();
            this.listPathFiles = new List<string>();
            this.loadMessageReived = new HandlerReceivedMessage(loadMessage);            
            this.txtPortName.SelectedIndex = 0;
            this.myChatPanel = new ChatPanel(this.contentChatPanelMain.Width, this.contentChatPanelMain.Height, 0, 0);
            this.contentChatPanelMain.Controls.Clear();
            this.contentChatPanelMain.Controls.Add(this.myChatPanel);
            this.myChatPanel.changeheightPanel += new ChatPanel.ChangeheightPanel(updateScroll);

            
        }

        private int getRatio()
        {
            try
            {
                return (int)Int32.Parse(this.txtRatioBaudios.Text);
            }catch
            {
                MessageBox.Show("Ratio baudios invalid");
                this.txtRatioBaudios.Text = "57600";
                return 57600;
            }
        }
               
        private void Connect()
        {
            this.portname = txtPortName.Text;                
            this.mySerialPort = new MySerialPort(this.portname, getRatio());
            this.mySerialPort.messageIsHere += new MySerialPort.HandlerReceiveMessage(this.messageReceived);
            this.mySerialPort.tramaHire += new MySerialPort.HandlerReceiveTrama(this.onTramaHire);
            try { 
            if (mySerialPort.Connect())
            {             
                this.lblState.Text = "OnLine "+portname;
                this.lblState.ForeColor = Color.Green;
                this.btnConnect.Text = "Disconnect";
                this.txtMessage.Enabled = true;
                this.lblResponse.Text = "";
                this.txtPortName.Enabled = false;
            }else            
            {
                this.lblState.Text = "OfLine";
                this.lblState.ForeColor = Color.Red;
                this.btnConnect.Text = "Reconnect";
                MessageBox.Show(this, "Connect error");
            }
            }catch(Exception err)
            {
                this.lblState.Text = err.Message;
                this.lblState.ForeColor = Color.Red;
            }

        }
        private void Disconnect()
        {
            if(this.mySerialPort.Disconnect())
            {
                this.lblState.Text = "OfLine";
                this.lblState.ForeColor = Color.Red;
                this.btnConnect.Text = "Connect";
                this.txtPortName.Enabled = true;
            }
            else
            {
                MessageBox.Show(this,"Disconnect error");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.lblResponse.Text = "";
            contentChatPanelMain.AutoScroll = false;
            contentChatPanelMain.HorizontalScroll.Enabled = false;
            contentChatPanelMain.HorizontalScroll.Visible = false;
            contentChatPanelMain.HorizontalScroll.Maximum = 0;
            contentChatPanelMain.AutoScroll = true;            
            this.txtMessage.Enabled = false;

            this.AllowDrop = true;
            this.txtMessage.AllowDrop = true;
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.lblState.Text == "OnLine "+txtPortName.Text)
            {
                this.Disconnect();
            }
            else
            {
                this.Connect();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {



            if (this.listPathFiles.Count != 0)
            {
                this.mySerialPort.SendFiles(this.listPathFiles);
            }
            this.listPathFiles.Clear();
            if (this.txtMessage.Text != null)
            {
                try
                {
                    string message=this.mySerialPort.SendMessage(this.txtMessage.Text.Trim());                    
                    lblResponse.Text = message;
                    this.myChatPanel.addNewMessage(this.txtMessage.Text, "Me", false);
                    this.lblResponse.ForeColor = Color.Green;
                    this.txtMessage.Text = "";
                }
                catch (Exception err)
                {
                    lblResponse.Text = err.Message;
                    this.lblResponse.ForeColor = Color.Red;
                }
            }


        }

        private void txtMessage_Click(object sender, EventArgs e)
        {
            if (this.mySerialPort.isOpen())
            {
                this.lblResponse.Text = "";
            }           

        }
     
        private void messageReceived(object oo, string message){
            //this.addMessageToChat(message, "Received");
            this.Invoke(this.loadMessageReived, message);         
        }
        private void loadMessage(string message)
        {
            this.myChatPanel.addNewMessage(message, "Received", true);            
            //this.ppppp.Text = message;

        }
        private void onTramaHire(string key, float progress, float tramas, string nombre)
        {
            lblResponse.Text ="Key: "+ key+" ;Progress: "+progress.ToString() + " ;Tramas: " + tramas.ToString();
        }

        private void updateScroll()
        {
            this.contentChatPanelMain.VerticalScroll.Value = 
                this.contentChatPanelMain.VerticalScroll.Maximum;
        }
        private void txtMessage_DragDrop(object sender, DragEventArgs e)
        {
            this.txtMessage.ForeColor = Color.BlueViolet;
            string[] filesList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach(string path in filesList)
            {
                this.listPathFiles.Add(path);
            }
            this.txtMessage.Clear();
            foreach (string path in listPathFiles)
            {
                this.txtMessage.Text += string.Format(path + "{0}", Environment.NewLine);
            }
        }

        
        private void changeModeMessageBox(bool file)
        {
            if (this.listPathFiles.Count==0)
            {
                if (file)
                {
                    this.txtMessage.BackColor = Color.WhiteSmoke;
                    this.txtMessage.ForeColor = Color.Green;
                    this.txtMessage.Multiline = true;
                    this.txtMessage.Height = 60;
                    this.txtMessage.Text = string.Format("{0}                                                   Drop file...    {0}", Environment.NewLine);
                   

                }
                else
                {
                    this.txtMessage.BackColor = Color.White;
                    this.txtMessage.ForeColor = Color.Black;
                    this.txtMessage.Multiline = false;
                    this.txtMessage.Clear();
                }
            }
            else
            {
                if (file)
                {
                    this.txtMessage.BackColor = Color.WhiteSmoke;               

                }
                else
                {
                    this.txtMessage.BackColor = Color.White;                 
                }
                this.txtMessage.ForeColor = Color.BlueViolet;
            }   
        }

        private void txtMessage_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void txtMessage_DragLeave(object sender, EventArgs e)
        {
            this.changeModeMessageBox(false);
        }

        private void txtMessage_DragOver(object sender, DragEventArgs e)
        {
            this.changeModeMessageBox(true);

        }
        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(this.mySerialPort!=null)this.mySerialPort.Disconnect();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //this.lblResponse.Text=this.mySerialPort.getBytesToRead().ToString();
        }
    }
}
