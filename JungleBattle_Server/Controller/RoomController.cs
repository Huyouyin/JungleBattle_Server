using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using JungleBattle_Server.DAO;
using JungleBattle_Server.Model;
using JungleBattle_Server.Server;

namespace JungleBattle_Server.Controller
{
    class RoomController:BaseController
    {
        RoomDAO roomDAO = new RoomDAO();
        public RoomController()
        {
            this.requestCode = RequestCode.Room;
        }
        public void CreateRoom(string data , Client client)
        {
            string ownername = data;
            int roomid = roomDAO.SeachWaitingRoomByUserName(client.GetConn(), ownername);
            CreateRoomResultCode resCode = CreateRoomResultCode.CreateFail;
            if(roomid != -1)
            {
                resCode = CreateRoomResultCode.RepeatCreate;
            }
            else
            {
                bool res = roomDAO.InsertNewRoom(client.GetConn() , ownername);
                if(res)
                {
                    roomid = roomDAO.SeachWaitingRoomByUserName(client.GetConn() , ownername);
                    if(roomid != -1)
                    {
                        resCode = CreateRoomResultCode.CreateSuccess;
                    }
                }
            }
            OnResponseCreateRoom(resCode , client,roomid);
        }

        private void OnResponseCreateRoom(CreateRoomResultCode resCode , Client client,int roomid =-1)
        {
            string res = resCode.ToString();
            if(roomid != -1)
            {
                res += "," + roomid;
            }
            MessageData mdata = new MessageData(RequestCode.Room , ActionCode.CreateRoom ,res);
            client.OnResponse(mdata);
        }

        public void RoomListUnStart(string data,Client client)
        {
            List<Room> roomList = roomDAO.SeachRoomByStatus(client.GetConn() , RoomStatus.Waiting);
            StringBuilder sb = new StringBuilder();
            sb.Append(roomList.Count+"_");
            if(roomList.Count != 0)
            {
                foreach(Room r in roomList)
                {
                    sb.Append(r.roomId);
                    sb.Append(',');
                    sb.Append(r.ownerName);
                    sb.Append('|');
                }
            }
            OnResponseRoomListUnStart(sb.ToString() , client);
        }

        private void OnResponseRoomListUnStart(string data,Client client)
        {
            MessageData mdata = new MessageData(RequestCode.Room , ActionCode.RoomListUnStart , data);
            client.OnResponse(mdata);
        }


        
        
    }
}
