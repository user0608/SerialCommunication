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
        public delegate void HandlerNotifyProgress(string key,float totalFrame,float progress);
        public HandlerNotifyProgress notifyProgress;

        private KFile fileToSend;
        private KFile.FileResponse response;


        private int key;        
        private Frame trama;

        
        

        public Resource(bool file)
        {
            this.trama = new Frame();
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
        public string getPath()
        {
            if (this.file) return this.message;
            return "";
        }
        public void SetFile(KFile file)
        {
            this.fileToSend = file;
            this.message = file.GetFileName();
        }

        public bool isCompleted()
        {
            if (!this.file)
            {
                return true;
            }
            else
            {
                return this.response.IsCompleted();
            }
        }
        public bool isFile()
        {
            return this.file;
        }
        public byte[] getTrama()
        {
            if (this.file)
            {              
                float numFrames = (UInt64)(fileToSend.GetNumBytes()+1023) / 1024;
                this.response = this.fileToSend.Read();
                this.trama.SetFrame(
                    this.response.GetBytes(),
                    this.response.GetLength(),
                    Frame.TYPE_FILE,
                    getIndex().ToString(),
                    this.fileToSend.GetFileExtension(),
                    numFrames, 
                    fileToSend.GetProgress()
                    );                
                if (this.notifyProgress != null) this.notifyProgress(this.getIndex().ToString(),numFrames, fileToSend.GetProgress());
                return this.trama.GetFrame();
            }
            else
            {
                this.trama.SetFrame(this.message,this.key);
                return this.trama.GetFrame();
            }
        }
        public void CloseStream()
        {
            if(this.file)this.fileToSend.Close();
        }
        public void setIndex(int index)
        {
            this.key = index;
        }
        public int getIndex()
        {
            return this.key;
        }
    }
}
