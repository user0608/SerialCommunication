using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySerialPortKS
{
    class Resource
    {
        private bool file;
        private string message;

        private int index;
        private Trama trama;
        
        public Resource(bool file)
        {
            this.trama = new Trama();
            this.file = file;
        }
        public void setMessage(string message)
        {
            if (!this.file)
            {
                this.message = message;
            }
            else
            {
                throw new Exception("File mode actived");
            }
        }

        public bool isCompleted()
        {
            if (!this.file)
            {
                return true;
            }
            else
            {
                   return false;
            }
        }

        public byte[] getTrama()
        {
            if (this.file)
            {
                return new byte[1];
            }
            else
            {
                this.trama.SetTrama(this.message,this.index);
                return this.trama.GetTrama();
            }
        }

        public void setIndex(int index)
        {
            this.index = index;
        }
        public int getIndex()
        {
            return this.index;
        }
    }
}
