using System;
using System.Text;
using System.Threading;
using System.IO.Ports;
namespace MySerialPortKS
{
    //Clase encargada de definir la comunicacion serial, recibir y enviar tramas.
    public class MySerialPort
    {
        //const byte TYPE_MESSAGE = 109;
        //const byte TYPE_FILE =102;
        //const byte TYPE_PHOTO = 112;


        private const int TRAMA_SIZE_HEAD = 6;
        private const int TRAMA_SIZE = 1024;        
        private const int BAUDIOS = 57600;
        private SerialPort serialPort;
        private int speedBaudios;
        private string portName;    

        private Thread sendProcess;
        private Thread receiveMessage;
        private string smsToRecieve;
        //private byte[] myTrama;
        private Trama myTrama;

        public delegate void HandlerReceiveMessage(object oo, string message);
        public event HandlerReceiveMessage messageIsHere;

        public MySerialPort(string portName)
        {
            this.portName = portName;
            this.myTrama = new Trama();       
        }
        public MySerialPort(string portName,int speedBaudios):this(portName)
        {
            this.speedBaudios = speedBaudios;
            
        }

        public bool Connect(){
            try{
                serialPort = new SerialPort(portName, this.speedBaudios, Parity.Even, 8, StopBits.Two);
                serialPort.ReceivedBytesThreshold = TRAMA_SIZE;
                serialPort.DataReceived += new SerialDataReceivedEventHandler(receivingData);
            }catch{
                throw new Exception("Invalid port");
            }
            try{
                serialPort.Open();
                return true;
            }catch{
                return false;
            }
        }
        public bool Disconnect()
        {
            try{
                serialPort.Close();
                return true;
            }catch{
                return false;
            }
        }

          //Recopera la longitud del mensaje codificada en la cabecera de la trama
        private int recoveryLengthMessageReceived()
        {
            string lengthMessage = this.smsToRecieve.Substring(1, TRAMA_SIZE_HEAD-1);
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
            if (this.messageIsHere != null){
                this.messageIsHere(this, recoveryMessageReceived());
            }
        }     

        //Procedimiento de envio de las tramas
        public string Send(string message)
        {
            this.myTrama.SetTrama(message);

            this.sendProcess = new Thread(sendingMessage);
            if (serialPort.IsOpen){
                if (this.isBufferReady())
                {
                this.sendProcess.Start();
                return "Message was sent";
                }else{
                    throw new Exception("The buffer is not empty");
                }
            }
            else{
                throw new Exception("Is not connected");
            }
        }
        //Ejecucion de enviado de tramas en un hilo. 
        //Verifica si el puerto esta abierto
        public bool isOpen()
        {
            return this.serialPort.IsOpen;
        } 
        public bool isBufferReady()
        {
            return serialPort.BytesToWrite== 0;
        }
        public int bytesToLeaveBuffer()
        {
            if (!isBufferReady())
            {
                return this.serialPort.BytesToWrite;
            }            
            return 0;
        }
        
        //Se ejecuta cuando el evento DataReceived se desencadena. 
        private void receivingData(object o,SerialDataReceivedEventArgs args)
        {
            byte[] receivedMessage=new byte[TRAMA_SIZE];
            this.serialPort.Read(receivedMessage, 0, TRAMA_SIZE);
            if (receivedMessage[0] == Trama.TYPE_MESSAGE){
                    this.smsToRecieve = ASCIIEncoding.UTF8.GetString(receivedMessage, 0, TRAMA_SIZE);                   
                    this.receiveMessage = new Thread(receivingMessage);
                    this.receiveMessage.Start();
            }
        }
        private void receivingMessage()
        {
            this.onMessageIsHere();
        }
        private void sendingMessage()
        {
            try
            {              
                this.serialPort.Write(this.myTrama.GetTrama(),0,TRAMA_SIZE);                
            }
            catch
            {
                throw new Exception("Error to write message");
            }
            
        }
       
    }
}
