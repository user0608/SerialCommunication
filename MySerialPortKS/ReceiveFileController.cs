using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySerialPortKS
{
    class ReceiveFileController
    {
        //private List<KFile> filesToWrite;
        private Dictionary<string, KFile> filesToWrite;

        public ReceiveFileController()
        {
            filesToWrite = new Dictionary<string, KFile>();
        }
        public void InitFileStream(string path,string key)
        {
            KFile file = KFile.NewKFile(path, KFile.WRITE_MODE);
            this.filesToWrite.Add(key, file);
        }
        public  void WriteFile(string key, byte[] data,int lengthData,int numFrames,int progress)
        {
            try {               
               KFile file = this.filesToWrite[key];           
            if (numFrames == progress)
            {
                file.Write(data, lengthData); 
                file.Close();
                this.filesToWrite.Remove(key);
                return;
            }
                file.Write(data, lengthData);            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " - no stream");
            }
        }
    }
}
