using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using JungleBattle_Server.Tools;

namespace JungleBattle_Server.Server
{
    /// <summary>
    /// 2017-9-6
    /// huyy
    /// 此类用于和客户端做交互
    /// </summary>
    public class Client
    {

        private Socket clientSocket;
        private GameServer server;
        private Message msg;
        private ConnHelper connHelper;

        public Client(Socket _clientSocket,GameServer server)
        {
            clientSocket = _clientSocket;
            this.server = server;
            msg = new Message();
            connHelper = new ConnHelper();
            //string str = string.Format("欢迎链接!!   IP:{0}      Port:{1}" , GameServer.IP , GameServer.PORT);
            //byte[] buffer = msg.StringToBytes(str);
            //Send(buffer);
            BeginReceive();
        }

        private void Send(byte[] buffer)
        {
            clientSocket.Send(buffer);
        }

        private void BeginReceive()
        {
            clientSocket.BeginReceive(msg.receiveMsg , msg.currentMsgLength , msg.msgRemainLength , SocketFlags.None , ReceiveCallBack , null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            Console.WriteLine("------------------------------------------------------------------------------------------");

            int amount = 0;
            try
            {
                amount = clientSocket.EndReceive(ar);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                Console.WriteLine("远程客户端已经强制断开链接");
                Quit();
                return;
            }
            msg.UpdateCacheLength(amount);
            msg.ParseMessage(ProcessMessageData);
            BeginReceive();
        }
        //处理收到消息
        private void ProcessMessageData(MessageData mdata)
        {
            server.HandleRequest(mdata,this);
        }
        //处理响应
        public void HandleRequestResult(MessageData mdata, object requestResult)
        {
            OnResponse(mdata.requsetCode , requestResult);
        }

        private void OnResponse(RequestCode request,object responseData)
        {
            byte[] buffer = Message.PackData(request , responseData as string);
            Send(buffer);
        }

        private void Quit()
        {
            clientSocket.Close();
            connHelper.Close();
            server.RemoveClient(this);
        }
    }
}
