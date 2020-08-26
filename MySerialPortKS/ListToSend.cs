using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MySerialPortKS
{
    class ListToSend
    {
        public static object locker = new Object();
        private int indexCounter;
        private bool processState;
        public delegate void EncodedTrame(byte[] frame);
        public delegate void DelegateStatusResource(Resource file);
        public  EncodedTrame frameToSend;
        public DelegateStatusResource statusOfSendFile;
        
        
        private bool bufferIsEmpty;
        private List<Resource> storeResource;
        private List<Resource> processingResource;
        private Thread processesSend;
       
        public ListToSend(){
            this.storeResource = new List<Resource>();
            this.processingResource = new List<Resource>();
            this.bufferIsEmpty = true;
            this.indexCounter = 100;
            this.processState = true;
            processesSend = new Thread(this.sendDataProcess);
            this.processesSend.Start();
        }

        public void addStoreResource(Resource file)
        {
            Resource f = file;
            f.setIndex(indexCounter);
            this.indexCounter++;
            if (this.indexCounter >= 999)
                this.indexCounter = 100;
            this.storeResource.Add(f);
        }

        void sendDataProcess()
        {
            while (processState)
            {
                if (storeResource.Count != 0)
                {
                    if (storeResource.Count != 0)
                    {
                        this.processingResource.Add(storeResource[0]);
                        this.storeResource.RemoveAt(0);
                    }
                }

                if (processingResource.Count != 0)
                {

                    foreach (Resource rec in processingResource)
                    {
                                               
                        byte[] trama = rec.getTrama();
                        if (this.frameToSend != null) this.frameToSend(trama);                        
                        if (this.statusOfSendFile != null) this.statusOfSendFile(rec);
                        Thread.Sleep(3);
                        if (rec.isCompleted())
                        {
                            this.processingResource.Remove(rec);
                            break;
                        }
                    }
                }
                else Thread.Sleep(100);
            }
        }
        public void setStateBuffer(bool state) {
            this.bufferIsEmpty = state;
        }
        public void Close()
        {
            this.processState = false;
        }

    }
}
