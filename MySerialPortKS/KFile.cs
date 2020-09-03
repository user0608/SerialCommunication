using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MySerialPortKS
{
    class KFile
    {
        public static int READ_MODE = 2943;
        public static int WRITE_MODE = 3432;

        public static int TAM_DATA = 1024;

        private string path;
        private string name;
        private string extension;
        private long numBytes;
        private long progress;
        
        private FileStream myStream;
        private BinaryReader myReader;
        private BinaryWriter myWriter;

        private  KFile(string path)
        {
            
        }
        public static KFile NewKFile(string path,int mode)
        {
            KFile file = new KFile(path);
            if (KFile.READ_MODE == mode)
            {
                file.path = path;
                file.extension = Path.GetExtension(path);
                file.name = Path.GetFileName(path);
                file.progress = -1;
                file.myStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                file.myReader = new BinaryReader(file.myStream);
                file.numBytes = file.myStream.Length;
            }
            if (KFile.WRITE_MODE == mode)
            {
                file.myStream = new FileStream(path, FileMode.Create, FileAccess.Write);
                file.myWriter = new BinaryWriter(file.myStream);
            }
            return file;
        }     
         public void Close(){
            
            if(this.myStream!=null) this.myStream.Close();
            if (this.myReader != null) this.myReader.Close();
            if (this.myWriter != null) this.myWriter.Close();

         }
        public FileResponse Read()
        {
            if (this.myReader!=null)
            {
                if (this.progress==-1)
                {
                    byte[] data = ASCIIEncoding.UTF8.GetBytes(this.GetFileName());
                    this.progress++;
                    return new FileResponse(data,data.Length,false);

                }else
                {
                    byte[] data = new byte[TAM_DATA];
                    int length = myReader.Read(data, 0, TAM_DATA);
                    this.progress++;
                    return new FileResponse(data,length, length < TAM_DATA);
                }
            }
            else
            {
                throw new ArgumentNullException("Error to read, the file is write mode");
            }
           
        }
        public float GetProgress()
        {
            return this.progress;
        }
        public  void Write(byte[] data,int length)
        {
            if (this.myWriter != null)
            {                
                this.myWriter.Write(data, 0, length);                
            }
            else
            {
                throw new ArgumentNullException("Error to read, the file is read mode");
            } 
        }
        public float GetNumBytes()
        {
            return this.numBytes;
        }
        public string GetFileName()
        {
            return this.name;
        }
        public string GetFilePath()
        {
            return this.path;
        }
        public string GetFileExtension()
        {
            return this.extension;
        }

        public class FileResponse
        {
            byte[] bytes;
            int length;
            bool completed;
            public FileResponse(byte[] bytes,int length,bool completed)
            {
                this.bytes=bytes;
                this.length = length;
                this.completed = completed;
            }
            public byte[] GetBytes()
            {
                return this.bytes;
            }
            public int GetLength()
            {
                return this.length;
            }
            public bool IsCompleted()
            {
                return this.completed;
            }

        }

    }
}
