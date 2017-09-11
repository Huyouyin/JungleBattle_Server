using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JungleBattle_Server.Controller;

namespace JungleBattle_Server.Server
{
    /// <summary>
    /// 2017-9-6
    /// huyy
    /// 此类用于创建服务器socket
    /// </summary>
    public class GameServer
    {
        public static readonly string IP = "127.0.0.1";
        public static readonly int PORT = 9527;

        private Socket serverSocket;
        private IPEndPoint ipEndPoint;
        private List<Client> clientList;
        private ControllerManager controllerManager;

        public GameServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork , SocketType.Stream , ProtocolType.Tcp);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(IP) , PORT);
            serverSocket.Bind(ipEndPoint);
            controllerManager = new ControllerManager();
        }

        public void Start()
        {
            serverSocket.Listen(0); //开始监听端口
            Console.WriteLine("开始监听...");
            serverSocket.BeginAccept(AcceptCallBack,null);
            clientList = new List<Client>();
        }
        /// <summary>
        /// 如果多个客户端同时连进来，同时调用这个回调怎么办？？
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallBack(IAsyncResult ar)
        {
            Console.WriteLine("新客户链接进来！！");
            Socket clientSocket = serverSocket.EndAccept(ar);
            HandleNewClient(clientSocket);
            serverSocket.BeginAccept(AcceptCallBack , null);
            Console.WriteLine("继续监听...");
        }
        private void HandleNewClient(Socket newClient)
        {
            Client client = new Client(newClient , this);
            clientList.Add(client);
        }
        
        //用来处理用户请求
        public void HandleRequest(MessageData mdata,Client client)
        {
            object res = controllerManager.HandleRequest(mdata,client);
            if(res != null)
            {
                client.OnResponse(mdata, res);
            }
        }

        public void RemoveClient(Client client)
        {
            clientList.Remove(client);
        }
    }
}
