using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    class Message
    {
        private readonly int MAX_RECEIVE_LENGTH = 1024; //能够接受消息的最大长度
        private readonly int MSG_HEAD_LENGTH = 4;  //接收到信息头长度
        public byte[] receiveMsg;  //收到的字节缓存
        public Queue<string> msgQueue;
        public int currentMsgLength;   //当前缓存长度
        public int msgRemainLength;     //当前缓存剩余长度


        public Message()
        {
            receiveMsg = new byte[MAX_RECEIVE_LENGTH];
            msgQueue = new Queue<string>();
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
            Array.Copy(receiveMsg , count , receiveMsg , 0 , currentMsgLength - count);
            UpdateCacheLength(-count);
        }

        public void ParseMessage()
        {
            while(currentMsgLength > MSG_HEAD_LENGTH)
            {
                if(!Read())
                    break;
            }
        }
        private bool Read()
        {
            int msglength = BitConverter.ToInt32(receiveMsg , 0);
            if(currentMsgLength > msglength + MSG_HEAD_LENGTH)
            {
                string msg = Encoding.UTF8.GetString(receiveMsg , MSG_HEAD_LENGTH , msglength);
                msgQueue.Enqueue(msg);
                UpdateCache(msglength + MSG_HEAD_LENGTH);
                return true;
            }
            return false;
        }

        public byte[] StringToBytes(string str)
        {
            int length = str.Length;
            byte[] head = BitConverter.GetBytes(length);
            byte[] context = Encoding.UTF8.GetBytes(str);
            byte[] buffer = new byte[context.Length+ MSG_HEAD_LENGTH];
            Array.Copy(head , 0 , buffer , 0 , MSG_HEAD_LENGTH);
            Array.Copy(context , 0 , buffer , MSG_HEAD_LENGTH , context.Length);
            return buffer;
        }

        public string GetMessage()
        {
            if(msgQueue.Count > 0)
                return msgQueue.Dequeue();
            Console.WriteLine("msgQueue个数为0");
            return null;
        }
    }
}
