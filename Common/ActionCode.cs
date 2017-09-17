using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        //账号相关
        None,
        Login =1,
        Register,


        //房间相关
        CreateRoom = 100,
        RoomListUnStart,


        //战绩相关
        BattleCount = 200
    }
}
