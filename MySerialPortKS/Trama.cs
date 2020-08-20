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
        
        private const int TRAMA_SIZE_HEAD = 6;
        private const int TRAMA_SIZE = 1024;
        private const int BAUDIOS = 57600;
        private byte[] myTrama;


        public Trama()
        {
            this.myTrama = new byte[TRAMA_SIZE];
        }

        private void setTramaHead(byte[] message, byte tipo)
        {
            //byte[] lengthMessage = ASCIIEncoding.UTF8.GetBytes((message.Length).ToString());
            byte[] lengthMessage = ASCIIEncoding.UTF8.GetBytes((message.Length).ToString());
            int length = message.Length.ToString().Length;
            for (int i = 0; i < TRAMA_SIZE_HEAD; i++) this.myTrama[i] = 48;
            this.myTrama[0] = tipo;
            int index = 0;
            for (int i = TRAMA_SIZE_HEAD - length; i < TRAMA_SIZE_HEAD; i++)
            {
                this.myTrama[i] = lengthMessage[index];
                index++;
            }
        }

        //Codifica el mensaje en la trama body.
        private void setTramaBody(byte[] message)
        {
            int lengthMessage = message.Length;
            if (lengthMessage > (TRAMA_SIZE - TRAMA_SIZE_HEAD)) lengthMessage = TRAMA_SIZE - TRAMA_SIZE_HEAD;
            for (int i = TRAMA_SIZE_HEAD; i < lengthMessage + TRAMA_SIZE_HEAD; i++)
            {
                this.myTrama[i] = message[i -TRAMA_SIZE_HEAD];
            }
        }

        public void SetTrama(string message)
        {
            this.SetTrama(message, EXTENSION_MESSAGE);
        }
        public void SetTrama(string message,string extension)
        {            
            byte[] bytesMessage = ASCIIEncoding.UTF8.GetBytes(message);
            this.setTramaHead(bytesMessage, TYPE_MESSAGE);
            this.setTramaBody(bytesMessage);
        }
        public byte[] GetTrama()
        {
            return this.myTrama;
        }

    }
}
