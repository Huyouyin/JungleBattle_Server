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
            int userid = int.Parse(data);
            int roomid = roomDAO.SeachWaitingRoomByUserID(client.GetConn(),userid);
            CreateRoomResultCode resCode = CreateRoomResultCode.CreateFail;
            if(roomid != -1)
            {
                resCode = CreateRoomResultCode.RepeatCreate;
            }
            else
            {
                bool res = roomDAO.InsertNewRoom(client.GetConn() , userid);
                if(res)
                {
                    resCode = CreateRoomResultCode.CreateSuccess;
                }
            }
            OnResponseCreateRoom(resCode , client);
        }

        private void OnResponseCreateRoom(CreateRoomResultCode resCode , Client client)
        {
            MessageData mdata = new MessageData(RequestCode.Room , ActionCode.CreateRoom , resCode.ToString());
            client.OnResponse(mdata);
        }

        public void RoomListUnStart(string data,Client client)
        {
            List<Room> roomList = roomDAO.SeachRoomByStatus(client.GetConn() , RoomStatus.Waiting);
            string res = "0,";
            if(roomList.Count != 0)
            {

            }
            OnResponseRoomListUnStart(res , client);
        }

        private void OnResponseRoomListUnStart(string data,Client client)
        {
            MessageData mdata = new MessageData(RequestCode.Room , ActionCode.RoomListUnStart , data);
            client.OnResponse(mdata);
        }


        
        
    }
}
