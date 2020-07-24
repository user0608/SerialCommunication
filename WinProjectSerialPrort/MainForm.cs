using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WinProjectSerialPrort
{
    public partial class MainForm : Form
    {
        private MySerialPort mySerialPort;
        private string portname;
        public MainForm()
        {
            InitializeComponent();
            this.portname = "COM1";
            this.mySerialPort = new MySerialPort(portname);
        }

        private void Connect()
        {

            if (mySerialPort.Connect())
            {             
                this.lblState.Text = "OnLine "+portname;
                this.lblState.ForeColor = Color.Green;
                this.btnConnect.Text = "Disconnect";
                this.txtMessage.Enabled = true;
                this.lblResponse.Text = "";                
            }else            
            {
                this.lblState.Text = "OfLine";
                this.lblState.ForeColor = Color.Red;
                this.btnConnect.Text = "Reconnect";
                MessageBox.Show(this, "Connect error");
            }
        }
        private void Disconnect()
        {
            if(this.mySerialPort.Disconnect())
            {
                this.lblState.Text = "OfLine";
                this.lblState.ForeColor = Color.Red;
                this.btnConnect.Text = "Connect";
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
            if (this.lblState.Text == "OnLine COM1")
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
            try
            {
            string message = this.mySerialPort.Send(this.txtMessage.Text);
                lblResponse.Text = message;                
                this.lblResponse.ForeColor = Color.Green;
                this.txtMessage.Text = "";
            }
            catch(Exception err)
            {
                lblResponse.Text = err.Message;
                this.lblResponse.ForeColor = Color.Red;
            }

        }

        private void txtMessage_Click(object sender, EventArgs e)
        {
            if (this.mySerialPort.isOpen())
            {
                this.lblResponse.Text = "";
            }
        }
    }
}
