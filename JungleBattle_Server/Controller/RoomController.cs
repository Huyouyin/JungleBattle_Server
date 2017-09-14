using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using JungleBattle_Server.Server;

namespace JungleBattle_Server.Controller
{
    class RoomController:BaseController
    {
        public RoomController()
        {
            this.requestCode = RequestCode.Room;
        }
        public void CreateRoom(string data , Client client)
        {
            Console.WriteLine(data);
        }
    }
}
