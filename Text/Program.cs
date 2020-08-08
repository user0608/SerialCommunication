using System;
using WinProjectSerialPrort;
using System.Windows.Forms;
namespace Text
{
    class Program
    {
        static void Main(string[] args)
        {
             ChatPanel panel=new ChatPanel(1,2,2,3); 
            string cadena = "Indica que algo o alguien ocupa completamente algún punto del espacio que separa dos cosas o está situado en medio de ellas; puede referirse tanto al espacio físico como al tiempo.";
           
            Console.WriteLine(panel.formatTextMessage(cadena, 0, 0));
        }
    }
}
