using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum LoginResultCode                         //登陆返回
    {
        Success=0,
        Fail
    }
    public enum RegisterResultCode                      //注册返回
    {
        Success=0,
        Fail,
        AlreadyExit        //用户已经存在
    }
    public enum CreateRoomResultCode                          //创建房间返回
    {
        CreateSuccess,
        CreateFail,
        RepeatCreate        //重复创建
    }
    
}
