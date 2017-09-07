using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    class Program
    {
        private readonly string IP = "192.168.6.67";
        private readonly int PORT = 9527;

        static void Main(string[] args)
        {
            Program p = new Program();
            Socket clientSocket = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
            clientSocket.Connect(p.IP , p.PORT);

            Message msg = new Message();
            
            for(int i=0;i<1000;i++)
            {
                byte[] buffer = msg.StringToBytes(i.ToString());
                clientSocket.Send(buffer);
            }


            Console.ReadKey();
            clientSocket.Close();
        }
    }
}
