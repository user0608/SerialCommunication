using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySerialPortKS
{
    public class Trama
    {

        public static byte TYPE_MESSAGE = 109;
        public static byte TYPE_FILE = 102;
        private static string EXTENSION_MESSAGE = "smssm";
  
        public static int FRAME_DATA = 1024;
        public static int FRAME_NUMBER = 5;
        public static int FRAME_NUM_FRAMES = 5;
        public static int FRAME_LENGTH_DATA = 4;
        public static int FRAME_EXTENSION = 5;
        public static int FRAME_KEY = 3;
        public static int FRAME_TYPE = 1;

              
        private byte[] myTrama;
        public Trama()
        {
            this.myTrama = new byte[getFrameLength()];
        }

       public static int getFrameLength()
       {
            return
                FRAME_DATA +
                getFrameHeadLength();
       }
        public static int getFrameHeadLength()
        {
            return
                FRAME_NUMBER +
                FRAME_NUM_FRAMES +
                FRAME_LENGTH_DATA +
                FRAME_EXTENSION +
                FRAME_KEY +
                FRAME_TYPE;
        }

        private byte[] convertStringToBytes(string data)
        {
            return ASCIIEncoding.UTF8.GetBytes(data);
        }

        private void setFieldsHead(byte[] data,int lengthBackFields)
        {           
            int index = 0;
            for (int i = lengthBackFields - data.Length; i < lengthBackFields; i++)
            {
                this.myTrama[i] = data[index];
                index++;
            }
        }

        private void setTramaHead(byte[] lengthData,byte[] number,byte[] numFrames,byte[] extension,byte[] key, byte type)
        {
            //Clear data of the frame head.            
            for (int i = 0; i < getFrameHeadLength(); i++) this.myTrama[i] = 48;            

            //set type of frame
            this.myTrama[0] = type;
            
            int lengthBackFields = FRAME_TYPE + FRAME_KEY;
            this.setFieldsHead(key, lengthBackFields);

            lengthBackFields += FRAME_EXTENSION;
            this.setFieldsHead(extension, lengthBackFields);

            lengthBackFields+= FRAME_LENGTH_DATA;
            this.setFieldsHead(lengthData, lengthBackFields);

            lengthBackFields+= FRAME_NUM_FRAMES;
            this.setFieldsHead(numFrames, lengthBackFields);

            lengthBackFields+=FRAME_NUMBER;
            this.setFieldsHead(number, lengthBackFields);
        }

        //Codifica el mensaje en la trama body.
        private void setTramaBody(byte[] message)
        {
            int lengthMessage = message.Length;
            if (lengthMessage > FRAME_DATA) lengthMessage = FRAME_DATA;
            for (int i = getFrameHeadLength(); i < lengthMessage + getFrameHeadLength(); i++)
            {
                this.myTrama[i] = message[i - getFrameHeadLength()];
            }
        }

        public void SetTrama(string message,int key)
        {
            this.SetTrama(this.convertStringToBytes(message),Trama.TYPE_MESSAGE,key.ToString(),EXTENSION_MESSAGE,1,1);
        }
        public void SetTrama(byte[] data,byte type,string key,string extension,int numFrames,int number)
        {           
            byte[] bytesKey = this.convertStringToBytes(key);
            byte[] bytesExtencion = this.convertStringToBytes(extension);
            byte[] bytesNumFrames= this.convertStringToBytes(numFrames.ToString());
            byte[] bytesNumber= this.convertStringToBytes(number.ToString());
            byte[] bytesLengthData = this.convertStringToBytes(data.Length.ToString());
            this.setTramaHead(bytesLengthData, bytesNumber, bytesNumFrames, bytesExtencion,bytesKey,type);
            this.setTramaBody(data);
        }
        public byte[] GetTrama()
        {
            return this.myTrama;
        }

    }
}
