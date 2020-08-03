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

namespace WinProjectSerialPrort
{
    public partial class MainForm : Form
    {
        private MySerialPort mySerialPort;
        private string portname;
        private int speedBaudios;
        
        public MainForm()
        {
            InitializeComponent();
            this.txtPortName.SelectedIndex = 0;
                    
        }

        private int getRatio()
        {
            try
            {
                return (int)Int32.Parse(this.txtRatioBaudios.Text);
            }catch(Exception e)
            {
                MessageBox.Show("Ratio baudios invalid");
                return 57600;
                this.txtRatioBaudios.Text = "57600";
            }
        }
               
        private void Connect()
        {
            this.portname = txtPortName.Text;                
            this.mySerialPort = new MySerialPort(this.portname, getRatio());
            this.mySerialPort.messageIsHere += new MySerialPort.HandlerReceiveMessage(this.messageReceived);
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
            this.txtMessage.Enabled = false;            
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

        private void addMessageToChat(string message,string title) 
        {
            if (message!="")
            {
                if (this.txtChat.Text == "")
                {
                    this.txtChat.Text= "[" + title + "]\n" + message; 
                }
                else
                {
                    this.txtChat.Text+= "\n\n[" + title + "]\n" + message; 
                }            
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (this.txtMessage.Text!="")
            {
                try
                {
                    string message = this.mySerialPort.Send(this.txtMessage.Text);
                    lblResponse.Text = message;
                    this.addMessageToChat(this.txtMessage.Text, "Me");
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
            this.addMessageToChat(message, "Received");
            MessageBox.Show("");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.txtChat.Text += "\n->>";                
        }
    }
}
