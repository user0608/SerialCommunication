using System;
using System.Text;
using System.Threading;
using System.IO.Ports;
namespace MySerialPortKS
{
    //Clase encargada de definir la comunicacion serial, recibir y enviar tramas.
    public class MySerialPort
    {
        private IProgress myProgress;
        private const int TRAMA_SIZE_HEAD = 5;
        private const int TRAMA_SIZE = 1024;        
        private const int BAUDIOS = 500;
        private SerialPort serialPort;
        private string portName;    

        private Thread sendProcess;
        private Thread checkPorcessOut;
        private string smsToRecieve;
        private byte[] myTrama;

        public delegate void HandlerReceiveMessage(object oo, string message);
        public event HandlerReceiveMessage messageIsHere;

        public MySerialPort(string portName)
        {
            this.portName = portName;            
            this.myTrama = new byte[TRAMA_SIZE];          
        }
    
        public bool Connect(){
            try{
                serialPort = new SerialPort(portName, BAUDIOS, Parity.Even, 8, StopBits.Two);
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
            if (this.messageIsHere != null){
                this.messageIsHere(this, recoveryMessageReceived());
            }
        }     


        //Codifica la longitud del mensaje en la cabecera de la trama head
        private void setTramaHead(string message){
            byte[] lengthMessage = ASCIIEncoding.UTF8.GetBytes((message.Length).ToString());
            int length = message.Length.ToString().Length;
            
            int index = 0;            
            for (int i = 0; i < TRAMA_SIZE_HEAD; i++) this.myTrama[i] = 48;
            for (int i = TRAMA_SIZE_HEAD - length; i < TRAMA_SIZE_HEAD; i++)
            {
                this.myTrama[i] = lengthMessage[index];
                index++;
            }
        }

        //Codifica el mensaje en la trama body.
        private void setTramaBody(string message){
            string sms = message;
            if (message.Length>(TRAMA_SIZE-TRAMA_SIZE_HEAD))
            {
                sms = sms.Substring(0, TRAMA_SIZE - TRAMA_SIZE_HEAD);
            }
            byte[] trama = ASCIIEncoding.UTF8.GetBytes(sms);            
            for (int i=TRAMA_SIZE_HEAD;i< sms.Length + TRAMA_SIZE_HEAD; i++)
            {
                this.myTrama[i] = trama[i-TRAMA_SIZE_HEAD];
            }
            
        }

        //Procedimiento de envio de las tramas
        public string Send(string message)
        {
            
            this.setTramaHead(message);
            this.setTramaBody(message);

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
            this.smsToRecieve = ASCIIEncoding.UTF8.GetString(receivedMessage, 0, TRAMA_SIZE);
            this.onMessageIsHere();
        }
        private void sendingMessage()
        {
            try
            {              
                this.serialPort.Write(this.myTrama,0,TRAMA_SIZE);                
            }
            catch
            {
                throw new Exception("Error to write message");
            }
        }

        public interface IProgress
        {
            void showProgress();
            void hideProgress();
            void setProgress(int status);
        }

    }
}
