using System;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Threading.Tasks;

namespace MySerialPortKS
{
    //Clase encargada de definir la comunicacion serial, recibir y enviar tramas.
    public class MySerialPort
    {    
        
        private const int BAUDIOS = 57600;
        private SerialPort serialPort;
        private int speedBaudios;
        private string portName;    

        private Thread sendProcess;
        private Thread receiveMessage;
        private string smsToRecieve;     
        private Trama myTrama;

        public delegate void HandlerReceiveMessage(object oo, string message);
        public event HandlerReceiveMessage messageIsHere;

        public Thread theBufferIsReady;
        public bool processeThreadBuffer;
        public bool bufferStatus;

        /*
         * Nuevo codigo         
         */
        private ListToSend myListToSend;

        public MySerialPort(string portName)
        {
            
            
            this.portName = portName;
            this.myListToSend = new ListToSend();
            this.myListToSend.frameToSend += new ListToSend.EncodedTrame(frameReadyToSend);
        }
        public MySerialPort(string portName,int speedBaudios):this(portName)
        {
            this.speedBaudios = speedBaudios;
            
        }

        public bool Connect(){
            try{
                serialPort = new SerialPort(portName, this.speedBaudios, Parity.Even, 8, StopBits.Two);
                serialPort.ReceivedBytesThreshold = Trama.getFrameLength();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(receivingData);
            }catch{
                throw new Exception("Invalid port");
            }
            try{
                serialPort.Open();
                this.bufferStatus = true;
                this.processeThreadBuffer = true;
                this.theBufferIsReady = new Thread(isBufferReady);
                this.theBufferIsReady.Start();
                return true;
            }catch{
                return false;
            }
        }        
        public bool Disconnect()
        {
            try{
                this.processeThreadBuffer = false;
                this.myListToSend.Close();
                serialPort.Close();
                return true;
            }catch{
                return false;
            }
        }

          //Recopera la longitud del mensaje codificada en la cabecera de la trama
        private int recoveryLengthMessageReceived()
        {
            int start = Trama.FRAME_TYPE + Trama.FRAME_KEY + Trama.FRAME_EXTENSION;
            string lengthMessage = this.smsToRecieve.Substring(start,Trama.FRAME_LENGTH_DATA);
            return (int)Int64.Parse(lengthMessage);
        }       

        //Recopera el mensaje codificado en la trama
        private string recoveryMessageReceived()
        {
            return this.smsToRecieve.Substring(Trama.getFrameHeadLength(),recoveryLengthMessageReceived());
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
           Resource rec = new Resource(false);
           rec.setMessage(message);
           this.myListToSend.addStoreResource(rec);
           
            return "Message was sent";
        }

        //Ejecucion de enviado de tramas en un hilo. 
        //Verifica si el puerto esta abierto
        public bool isOpen()
        {
            return this.serialPort.IsOpen;
        } 
        public void isBufferReady()
        {
            this.bufferStatus = this.serialPort.BytesToWrite == 0;
            Thread.Sleep(5);
        }
    
       
        //Se ejecuta cuando el evento DataReceived se desencadena. 
        private void receivingData(object o,SerialDataReceivedEventArgs args)
        {
            byte[] receivedMessage=new byte[Trama.getFrameLength()];
            this.serialPort.Read(receivedMessage, 0, Trama.getFrameLength());
            if (receivedMessage[0] == Trama.TYPE_MESSAGE){
                    this.smsToRecieve = ASCIIEncoding.UTF8.GetString(receivedMessage, 0, Trama.getFrameLength());                   
                    this.receiveMessage = new Thread(receivingMessage);
                    this.receiveMessage.Start();
            }
        }

        private async void frameReadyToSend(byte[] frame)
        {
            try
            {
                await Task.Run(() => {
                    while (!this.bufferStatus) { }
                    serialPort.Write(frame, 0, Trama.getFrameLength());
                });
            }
            catch
            {
                throw new Exception("Error to write message");
            }

           // await Task.Delay(1);            
        }
        private void receivingMessage()
        {
            this.onMessageIsHere();
        }
       
    }
}
