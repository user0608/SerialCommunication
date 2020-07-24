using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace WinProjectSerialPrort
{
    class MySerialPort {
        private SerialPort serialPort;


        public MySerialPort(string portName)
        {
            serialPort = new SerialPort(portName, 9600, Parity.Even, 8, StopBits.Two);

        }

        public bool Connect() {

            try {

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
            if (serialPort.IsOpen) {
                try {

                    this.serialPort.Write(message);
                    return "Message was sent";
                }
                catch {
                    throw new Exception ("Error to write message");
                }

            }
            else {
                throw new Exception("Is not connected");
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

            } else {
                throw new Exception("Is not connected");
            }
        }
    }
}
