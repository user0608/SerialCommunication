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
using WinProjectSerialPrort.Components;

namespace WinProjectSerialPrort
{
    public partial class MainForm : Form
    {
        private MySerialPort mySerialPort;
        private string portname;
        private string receivePath;
        private Options optionsForm;
       // private int speedBaudios;
        private ChatPanel myChatPanel;
        private InputBox myInputBox;
        

        private List<String> listPathFiles;
        public delegate void HandlerReceivedMessage(string message);
        HandlerReceivedMessage loadMessageReived;
        public delegate void HandelerUpdateScroll();
        private HandelerUpdateScroll onUpdateScorll;
        
        public MainForm()
        {
            InitializeComponent();
            this.listPathFiles = new List<string>();
            this.loadMessageReived = new HandlerReceivedMessage(loadMessage);            
            this.txtPortName.SelectedIndex = 0;                        
            this.txtRatioBaudios.SelectedIndex = 6;
            this.myChatPanel = new ChatPanel(this.contentChatPanelMain.Width, this.contentChatPanelMain.Height, 0, 0);
            this.contentChatPanelMain.Controls.Clear();
            this.contentChatPanelMain.Controls.Add(this.myChatPanel);
            this.myChatPanel.changeheightPanel += new ChatPanel.ChangeheightPanel(updateScroll);

            this.myInputBox = new InputBox();
            this.myInputBox.Location = new Point(35,320);
            this.myInputBox.onFilesSend += new InputBox.HandlerSentFileClick(this.onSentFiles);
            this.myInputBox.onMessageSend += new InputBox.HandlerSentMessageClick(this.onSentMessage);
            this.Controls.Add(this.myInputBox);
            this.onUpdateScorll += new HandelerUpdateScroll(this.OnUpdateSrollAction);
            this.MaximizeBox = false;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.optionsForm = new Options();
            this.optionsForm.handlerTxtPath += new Options.HandelrtextBoxPath(this.updateFolderPath);
            this.receivePath = this.optionsForm.getFolderPath();
        }

        private int getRatio()
        {
            try
            {
                return (int)Int32.Parse(this.txtRatioBaudios.Text);
            }catch
            {
                MessageBox.Show("Ratio baudios invalid");
                this.txtRatioBaudios.SelectedIndex = 6;
                return 57600;
            }
        }
        private void updateFolderPath(string path)
        {
            this.receivePath = path;
        }
               
        private void Connect()
        {
            this.portname = txtPortName.Text;                
            this.mySerialPort = new MySerialPort(this.portname, getRatio(),this.receivePath);
            this.mySerialPort.messageIsHere += new MySerialPort.HandlerReceiveMessage(this.messageReceived);
            this.mySerialPort.tramaHire += new MySerialPort.HandlerNotifyProgress(this.onTramaHire);
            this.mySerialPort.notifyName += new MySerialPort.HandlerNotifyProgress(this.onFileNameAddToCahtPanel);
            this.mySerialPort.sendingProgress += new MySerialPort.HandlerNotifyProgress(this.upadateProgressOnChatPanel);
            
            try { 
            if (mySerialPort.Connect())
            {             
                this.lblState.Text = "OnLine "+portname;
                this.lblState.ForeColor = Color.Green;
                this.btnConnect.Text = "Disconnect";
                this.myInputBox.ComponentesState(true);         
                this.txtPortName.Enabled = false;
                this.txtRatioBaudios.Enabled = false;
                this.txtSetting.Enabled = false;
                }
                else            
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
                this.txtRatioBaudios.Enabled = true;
                this.txtSetting.Enabled = true;
                this.myInputBox.ComponentesState(false);
            }
            else
            {
                MessageBox.Show(this,"Disconnect error");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            contentChatPanelMain.AutoScroll = false;
            contentChatPanelMain.HorizontalScroll.Enabled = false;
            contentChatPanelMain.HorizontalScroll.Visible = false;
            contentChatPanelMain.HorizontalScroll.Maximum = 0;
            contentChatPanelMain.AutoScroll = true;                       
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
           

        }
     
        private void messageReceived(object oo, string message){           
            this.Invoke(this.loadMessageReived, message);         
        }
        private void loadMessage(string message)
        {
            this.myChatPanel.addNewMessage(message, "Received", true);                        

        }
        private void onFileNameAddToCahtPanel(string path, string key, float progress, float tramas)
        {
            this.myChatPanel.addNewFile(key, path, false);
        }
        private void upadateProgressOnChatPanel(string path, string key, float progress, float tramas)
        {
            this.myChatPanel.upadateProgress(key, (progress / tramas) * 100);
        }

        private void onTramaHire(string path, string key, float progress, float tramas)
        {
            if (path=="...null...")
            {
                this.myChatPanel.upadateProgress(key, (progress / tramas) * 100);
            }
            else
            {
                this.myChatPanel.addNewFile(key,path,true);
            }           
        }

        private void OnUpdateSrollAction()
        {
            this.contentChatPanelMain.VerticalScroll.Value =
            this.contentChatPanelMain.VerticalScroll.Maximum;
        }
        private void updateScroll(){ Invoke(onUpdateScorll);}

        private bool onSentMessage(string message)
        {                       
            try
            {
                this.mySerialPort.SendMessage(message);               
                this.myChatPanel.addNewMessage(message, "Me", false );              
            }
            catch
            {
            throw new Exception("Check your connection, error to send");
            }
            return true;            
        }
        private bool onSentFiles(List<string> filePaths)
        {
            this.mySerialPort.SendFiles(filePaths);
            return true;
        }      

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(this.mySerialPort!=null)this.mySerialPort.Disconnect();
        }
        int i = 0;
        private void button1_Click(object sender, EventArgs e)
        {           
        }
        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            // comboBox1 is readonly
            e.SuppressKeyPress = true;
        }

        private void folderReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {                      
            this.optionsForm.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "This project was developed by students from National university of Trujillo. \n\nDeveloped in Visual C#, for point-to-point communication between two PCs.", "UNT");
        }

        private void txtPortName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }
    }
}
