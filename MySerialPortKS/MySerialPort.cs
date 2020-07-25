using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace MySerialPortKS
{

    public class MySerialPort
    {
        private ReceiveMessageAction receiveMessageAction;
        private SerialPort serialPort;
        private string portName;
        private Thread sendProcess;        
        private string smsToSend;
        private string smsToRecieve;

        public delegate void HandlerReceiveMessage(object oo, string message);
        public event HandlerReceiveMessage messageIsHere;

        public MySerialPort(string portName, ReceiveMessageAction receiveMessageAction)
        {
            this.portName = portName;
            this.receiveMessageAction = receiveMessageAction;
            

        }

        public bool Connect()
        {
                        
            try
            {
            serialPort = new SerialPort(portName, 57600, Parity.Even, 8, StopBits.Two);
            serialPort.ReceivedBytesThreshold = 5;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(receivingData);
                
            }
            catch
            {
                throw new Exception("Invalid port");
            }
            try
            {
                serialPort.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Disconnect()
        {
            try
            {
                serialPort.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Send(string message)
        {
            this.smsToSend = message;
            sendProcess = new Thread(sendingMessage);
            if (serialPort.IsOpen)
            {
                this.sendProcess.Start();
                return "Message was sent";
            }
            else
            {
                throw new Exception("Is not connected");
            }
        }
        private void sendingMessage()
        {
            try
            {
                this.serialPort.Write(smsToSend);                
            }
            catch
            {
                throw new Exception("Error to write message");
            }
        }
        public bool isOpen()
        {
            return this.serialPort.IsOpen;
        }
        private string receive()
        {
            return serialPort.ReadExisting();          
        }
        private void receivingData(object o,SerialDataReceivedEventArgs args)
        {
            receiveMessageAction.messageReceived(this.receive());
        }
    }
}
