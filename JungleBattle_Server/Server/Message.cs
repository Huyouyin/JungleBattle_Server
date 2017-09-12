using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace JungleBattle_Server.Server
{
    /// <summary>
    /// 2017-9-6
    /// huyy
    /// 此类用于处理要发送的消息和接受到的消息
    /// </summary>
    class Message
    {
        private static readonly int MAX_RECEIVE_LENGTH = 1024; //能够接受消息的最大长度
        private static readonly int MSG_HEAD_LENGTH = 4;  //接收到信息头长度
        public byte[] receiveMsg;  //收到的字节缓存
        public int currentMsgLength;   //当前缓存长度
        public int msgRemainLength;     //当前缓存剩余长度


        public Message()
        {
            receiveMsg = new byte[MAX_RECEIVE_LENGTH];
            currentMsgLength = 0;
            msgRemainLength = MAX_RECEIVE_LENGTH - currentMsgLength;
        }
        
        public void UpdateCacheLength(int amount)
        {
            currentMsgLength += amount;
            msgRemainLength = MAX_RECEIVE_LENGTH - currentMsgLength;
        }

        /// <summary>
        /// 左移多少位
        /// </summary>
        /// <param name="length"></param>
        private void UpdateCache(int count)
        {
            Array.Copy(receiveMsg , count , receiveMsg , 0 , MAX_RECEIVE_LENGTH - count);
            UpdateCacheLength(-count);
        }
        //解析消息
        public void ParseMessage(Action<MessageData> handleMesssage)
        {
            while(currentMsgLength > MSG_HEAD_LENGTH)
            {
                if(!Read(handleMesssage))
                    break;
            }
        }

        private bool Read(Action<MessageData> handleMesssage)
        {
            int msglength = BitConverter.ToInt32(receiveMsg , 0);
            if(msglength > currentMsgLength)
            {
                return false;
            }
            int requestcode = BitConverter.ToInt32(receiveMsg , MSG_HEAD_LENGTH);
            int actioncode = BitConverter.ToInt32(receiveMsg , MSG_HEAD_LENGTH * 2);
            string msg = Encoding.UTF8.GetString(receiveMsg , MSG_HEAD_LENGTH * 3 , msglength-MSG_HEAD_LENGTH*2);
            MessageData md = new MessageData(requestcode , actioncode , msg);
            Console.WriteLine("收到一条消息："+md.requsetCode.ToString() +","+ md.actionCode.ToString() +","+ md.data);
            handleMesssage(md);
            UpdateCache(msglength + MSG_HEAD_LENGTH);
            return true;
        }

        //数据打包，用于响应客户端
        public static byte[] PackData(MessageData mdata)
        {
            byte[] requestBuffer = BitConverter.GetBytes((int)mdata.requsetCode);//请求
            byte[] dataBuffer = Encoding.UTF8.GetBytes(mdata.data);//数据
            byte[] lengthBuffer = BitConverter.GetBytes(dataBuffer.Length + MSG_HEAD_LENGTH);//总长度

            byte[] buffer = lengthBuffer.Concat(requestBuffer).Concat(dataBuffer).ToArray();
            return buffer;
        }
    }

    public class MessageData
    {
        public RequestCode requsetCode;
        public ActionCode actionCode;
        public string data;
        public MessageData(int request,int action,string data)
        {
            this.requsetCode = (RequestCode)request;
            this.actionCode = (ActionCode)action;
            this.data = data;
        }
        public MessageData(RequestCode request , ActionCode action , string data)
        {
            this.requsetCode = request;
            this.actionCode = action;
            this.data = data;
        }
    }
}
