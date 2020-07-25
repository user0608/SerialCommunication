using System;
using System.Text;
using System.Threading;
using System.IO.Ports;
namespace MySerialPortKS
{
    //Clase encargada de definir la comunicacion serial, recibir y enviar tramas.
    public class MySerialPort
    {
        private const int TRAMA_SIZE_HEAD = 5;
        private const int TRAMA_SIZE_TO_SEMD=1024;
        private const int  MESSAGE_SIZE_TO_RECEIVE=1024;     
        private SerialPort serialPort;
        private string portName;
        private Thread sendProcess;               
        private string smsToRecieve;

        private byte[] myTrameHead;
        private byte[] myTrameBody;       

        public delegate void HandlerReceiveMessage(object oo, string message);
        public event HandlerReceiveMessage messageIsHere;

        public MySerialPort(string portName)
        {
            this.portName = portName;
            initializeTrama();

        }
        public bool Connect()
        {

            try
            {
                serialPort = new SerialPort(portName, 57600, Parity.Even, 8, StopBits.Two);
                serialPort.ReceivedBytesThreshold = MESSAGE_SIZE_TO_RECEIVE;
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
        //Inicializa las tramas, basadas en las constantes definidas.
        public void initializeTrama()
        {
            this.myTrameHead = new byte[TRAMA_SIZE_HEAD];
            this.myTrameBody = new byte[TRAMA_SIZE_TO_SEMD];

        }
        //Recopera la longitud del mensaje codificada en la cabecera de la trama
        private int recoveryLengthMessageReceived()
        {
            string lengthMessage = this.smsToRecieve.Substring(0, TRAMA_SIZE_HEAD);
            return (int)Int64.Parse(lengthMessage);
        }
        
        //Recopera el mensaje codificado en la trama
        private string recoveryMessageReceived()
        {
            return this.smsToRecieve.Substring(TRAMA_SIZE_HEAD,recoveryLengthMessageReceived());
        }

        //Ejecuta el el procedimiento messageIsHire, que se encuentra un nivel por encima.
        //Es controlado por el hander de la clase.
        public virtual void onMessageIsHere()
        {
            if (this.messageIsHere != null)
            {
                this.messageIsHere(this, recoveryMessageReceived());
            }
        }     


        //Codifica la longitud del mensaje en la cabecera de la trama head
        private void setTramaHead(string message)
        {
            byte[] lengthMessage = ASCIIEncoding.UTF8.GetBytes((message.Length).ToString());
            int length = message.Length.ToString().Length;
            
            int index = 0;
            
            for (int i = 0; i < TRAMA_SIZE_HEAD; i++) this.myTrameHead[i] = 48;
            for (int i = TRAMA_SIZE_HEAD - length; i < TRAMA_SIZE_HEAD; i++)
            {
                this.myTrameHead[i] = lengthMessage[index];
                index++;
            }
        }

        //Codifica el mensaje en la trama body.
        private void setTramaBody(string message)
        {
            byte[] trama = ASCIIEncoding.UTF8.GetBytes(message);            
            for (int i=0;i<message.Length;i++)
            {
                this.myTrameBody[i] = trama[i];
            }
        }

        //Procedimiento de envio de las tramas
        public string Send(string message)
        {
            
            this.setTramaHead(message);
            this.setTramaBody(message);

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
        //Ejecucion de enviado de tramas en un hilo. 
        private void sendingMessage()
        {
            try
            {
                this.serialPort.Write(this.myTrameHead,0,TRAMA_SIZE_HEAD);
                this.serialPort.Write(this.myTrameBody,0,MESSAGE_SIZE_TO_RECEIVE-TRAMA_SIZE_HEAD);                
            }
            catch
            {
                throw new Exception("Error to write message");
            }
        }
        //Verifica si el puerto esta abierto
        public bool isOpen()
        {
            return this.serialPort.IsOpen;
        } 
        
        //Se ejecuta cuando el evento DataReceived se desencadena. 
        private void receivingData(object o,SerialDataReceivedEventArgs args)
        {            
            this.smsToRecieve = serialPort.ReadExisting();
            this.onMessageIsHere();
        }
    }
}
