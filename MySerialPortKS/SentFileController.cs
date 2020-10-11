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
        public delegate void HandlerNotifyFileProgress(string key,string path,float totalFrames,float progress);
        public  EncodedTrame frameToSend;
        public HandlerNotifyFileProgress notifyFile;
        public HandlerNotifyFileProgress notifyFileProgress;


        private bool bufferIsEmpty;
        private List<Resource> storeResource;
        private List<Resource> processingResource;
        private Thread processesSend;
       
        public SentFileController(string portname){
            this.storeResource = new List<Resource>();
            this.processingResource = new List<Resource>();
            this.initCouterKey(portname);
            this.bufferIsEmpty = true;          
            this.processState = true;
            processesSend = new Thread(this.sendDataProcess);
            this.processesSend.Start();
        }
        private void initCouterKey(string port)
        {
            switch (port)
            {
                case "COM1":this.indexCounter = 100; break;
                case "COM2":this.indexCounter = 200; break;
                case "COM3":this.indexCounter = 300; break;
                case "COM4":this.indexCounter = 400; break;
                case "COM5":this.indexCounter = 500; break;
                case "COM6":this.indexCounter = 600; break;
                case "COM7":this.indexCounter = 700; break;
                case "COM8":this.indexCounter = 800; break;
                case "COM9":this.indexCounter = 900; break;
            }
        }
        private void notifyProgress(string key, float totalFrame, float progress)
        {
            if (notifyFileProgress != null) this.notifyFileProgress(key,"...null..." ,totalFrame, progress);
        }

        public void addStoreResource(Resource file)
        {
            Resource f = file;
            f.setIndex(indexCounter);           
            if (this.indexCounter >= 999)
                this.indexCounter = 100;
            f.notifyProgress += new Resource.HandlerNotifyProgress(this.notifyProgress);
           
            Boolean _lockTeken = false;
            Monitor.Enter(storeResource, ref _lockTeken);
            try
            {
                this.storeResource.Add(f);
                if (this.notifyFile != null&&f.isFile()) this.notifyFile(indexCounter.ToString(),f.getPath(),0,0);
                this.indexCounter++;
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
                        Thread.Sleep(4);                         
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
        
        public async void Close()
        {
            this.processState = false;
            await Task.Run(() => processesSend.Join());
            foreach (Resource r in this.processingResource)
            {
                r.CloseStream();
            }
            foreach (Resource r in this.storeResource)
            {
                r.CloseStream();
            }
            this.processingResource.Clear();
            this.storeResource.Clear();

        }

    }
}
