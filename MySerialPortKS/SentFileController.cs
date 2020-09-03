using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MySerialPortKS
{
    class SentFileController
    {
       
        private int indexCounter;
        private bool processState;
        public delegate void EncodedTrame(byte[] frame);
        public delegate void DelegateStatusResource(Resource file);
        public  EncodedTrame frameToSend;
        //public DelegateStatusResource statusOfSendFile;
        
        
        private bool bufferIsEmpty;
        private List<Resource> storeResource;
        private List<Resource> processingResource;
        private Thread processesSend;
       
        public SentFileController(){
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
            
            Boolean _lockTeken = false;
            Monitor.Enter(storeResource, ref _lockTeken);
            try
            {
                this.storeResource.Add(f);
            }
            finally
            {
                if (_lockTeken)
                {
                    Monitor.Exit(storeResource);
                }
            }
        }
        void sendDataProcess()
        {
            while (processState)
            {
                if (storeResource.Count != 0)
                {
                    if (storeResource.Count != 0)
                    {
                        Boolean _lockTeken = false;
                        Monitor.Enter(storeResource, ref _lockTeken);
                        try
                        {
                            this.processingResource.Add(storeResource[0]);
                            this.storeResource.RemoveAt(0);
                        }
                        finally
                        {
                            if (_lockTeken)
                            {
                                Monitor.Exit(storeResource);
                            }
                        }                      
                    }
                }
                if (processingResource.Count != 0)
                {
                    foreach (Resource rec in processingResource)
                    {
                        byte[] trama = rec.getTrama();
                        if (this.frameToSend != null) this.frameToSend(trama);
                       // if (this.statusOfSendFile != null) this.statusOfSendFile(rec);
                        Thread.Sleep(100);                         
                        if (rec.isCompleted())
                        {
                            rec.CloseStream();
                            this.processingResource.Remove(rec);
                            break;
                        }
                    }
                }
                else
                {
                    Thread.Sleep(100);                    
                }
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
