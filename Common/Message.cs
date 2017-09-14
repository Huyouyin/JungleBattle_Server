using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 2017-9-14
    /// huyy
    /// 此类用于处理要发送的消息和接受到的消息
    /// </summary>
    public class Message
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

        //读取消息
        private bool Read(Action<MessageData> handleMesssage)
        {
            int msglength = BitConverter.ToInt32(receiveMsg , 0);
            if(msglength > currentMsgLength)
            {
                return false;
            }
            int requestcode = BitConverter.ToInt32(receiveMsg , MSG_HEAD_LENGTH);
            int actioncode = BitConverter.ToInt32(receiveMsg , MSG_HEAD_LENGTH * 2);
            string msg = Encoding.UTF8.GetString(receiveMsg , MSG_HEAD_LENGTH * 3 , msglength - MSG_HEAD_LENGTH * 2);
            MessageData md = new MessageData(requestcode , actioncode , msg);
            //Console.WriteLine("收到一条消息：" + md.requsetCode.ToString() + "," + md.actionCode.ToString() + "," + md.data);
            handleMesssage(md);
            UpdateCache(msglength + MSG_HEAD_LENGTH);
            return true;
        }


        //数据打包
        public static byte[] PackData(MessageData mdata)
        {
            byte[] requestBuffer = BitConverter.GetBytes((int)mdata.requsetCode);
            byte[] actionBuffer = BitConverter.GetBytes((int)mdata.actionCode);
            byte[] strBuffer = Encoding.UTF8.GetBytes(mdata.data);
            byte[] lengthBuffer = BitConverter.GetBytes(strBuffer.Length + MSG_HEAD_LENGTH * 2);

            byte[] buffer = new byte[strBuffer.Length + 12];
            Array.Copy(lengthBuffer , 0 , buffer , 0 , lengthBuffer.Length);
            Array.Copy(requestBuffer , 0 , buffer , MSG_HEAD_LENGTH , requestBuffer.Length);
            Array.Copy(actionBuffer , 0 , buffer , MSG_HEAD_LENGTH*2 , actionBuffer.Length);
            Array.Copy(strBuffer , 0 , buffer , MSG_HEAD_LENGTH*3 , strBuffer.Length);
            return buffer;
        }
        //链接字符串
        public static string PackContentData(char sparateChar , params string[] datas)
        {
            string content = string.Empty;
            for(int i = 0; i < datas.Length; i++)
            {
                content += datas[i];
                content += sparateChar;
            }
            return content;
        }
    }
    public class MessageData
        {
            public RequestCode requsetCode;
            public ActionCode actionCode;
            public string data;
            public MessageData(int request , int action , string data)
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
