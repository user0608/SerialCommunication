using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySerialPortKS
{
    public class Frame
    {

        public static byte TYPE_MESSAGE = 109;
        public static byte TYPE_FILE = 102;

        private static string EXTENSION_MESSAGE = "smssm";
  
        public static int FRAME_DATA = 1024;
        public static int FRAME_NUMBER = 5;
        public static int FRAME_NUM_FRAMES = 5;
        public static int FRAME_LENGTH_DATA = 4;     
        public static int FRAME_KEY = 3;
        public static int FRAME_TYPE = 1;

              
        private byte[] myFrame;
        public Frame()
        {
            this.myFrame = new byte[getFrameLength()];
        }

       public static int getFrameLength()
       {
            return(
                FRAME_DATA +
                getFrameHeadLength());
       }
        public static int getFrameHeadLength()
        {
            return(
                FRAME_NUMBER +
                FRAME_NUM_FRAMES +
                FRAME_LENGTH_DATA +           
                FRAME_KEY +
                FRAME_TYPE);
        }

        private byte[] convertStringToBytes(string data)
        {           
            return ASCIIEncoding.UTF8.GetBytes(data.Trim());
        }

        private void setFieldsHead(byte[] data,int lengthBackFields)
        {           
            int index = 0;
            for (int i = lengthBackFields - data.Length; i < lengthBackFields; i++)
            {
                this.myFrame[i] = data[index];
                index++;
            }
        }

        private void setFrameHead(byte[] lengthData,byte[] number,byte[] numFrames,byte[] extension,byte[] key, byte type)
        {
            //Clear data of the frame head.            
            for (int i = 0; i < getFrameHeadLength(); i++) this.myFrame[i] = ASCIIEncoding.ASCII.GetBytes("0")[0];            

            //set type of frame
            this.myFrame[0] = type;
            
            int lengthBackFields = FRAME_TYPE + FRAME_KEY;
            this.setFieldsHead(key, lengthBackFields);
            lengthBackFields+= FRAME_LENGTH_DATA;
            this.setFieldsHead(lengthData, lengthBackFields);
            lengthBackFields+= FRAME_NUM_FRAMES;
            this.setFieldsHead(numFrames, lengthBackFields);
            lengthBackFields+=FRAME_NUMBER;
            this.setFieldsHead(number, lengthBackFields);
        }

        //Codifica el mensaje en la trama body.
        private void setFrameBody(byte[] message,int dataLenght)
        {            
            if (dataLenght > FRAME_DATA) dataLenght = FRAME_DATA;
            for (int i = getFrameHeadLength(); i < dataLenght + getFrameHeadLength(); i++)
            {
                this.myFrame[i] = message[i - getFrameHeadLength()];
            }
        }

        public void SetFrame(string message, int key)
        {
            this.SetFrame(
                this.convertStringToBytes(message)
                , message.Length,
                Frame.TYPE_MESSAGE,
                key.ToString(), 
                EXTENSION_MESSAGE, 1, 1);
        }
        public void SetFrame(byte[] data,int dataLength,byte type,string key,string extension,float numFrames,float number)
        {           
            byte[] bytesKey = this.convertStringToBytes(key);
            byte[] bytesExtencion = this.convertStringToBytes(extension);
            byte[] bytesNumFrames= this.convertStringToBytes(numFrames.ToString());
            byte[] bytesNumber= this.convertStringToBytes(number.ToString());
            byte[] bytesLengthData = this.convertStringToBytes(dataLength.ToString());
            this.setFrameHead(bytesLengthData, bytesNumber, bytesNumFrames, bytesExtencion,bytesKey,type);
            this.setFrameBody(data,dataLength);
        }
        public byte[] GetFrame()
        {
            return this.myFrame;
        }



        public static void Main(String[] args)
        {


            for (int i=0;i<100; i++)
            {                
                Frame tram = new Frame();
                Console.WriteLine(tram.convertStringToBytes("hola" + i.ToString()).Length);               
                
            }

        }
    }
}
