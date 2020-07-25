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
        private SerialPort serialPort;
        private string portName;
        private Thread sendProcess;
        private Thread recieveProcess;
        private string smsToSend;


        public MySerialPort(string portName)
        {
            this.portName = portName;
            

        }

        public bool Connect()
        {

            
            try
            {
            serialPort = new SerialPort(portName, 9600, Parity.Even, 8, StopBits.Two);
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
        public string Recieve()
        {
            if (serialPort.IsOpen)
            {
                try
                {

                    string message = this.serialPort.ReadExisting();
                    return message;
                }
                catch
                {
                    throw new Exception("Error to read the message");
                }

            }
            else
            {
                throw new Exception("Is not connected");
            }
        }
    }
}
