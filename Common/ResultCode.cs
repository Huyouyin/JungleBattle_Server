using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum LoginResultCode
    {
        Success=0,
        Fail
    }
    public enum RegisterResultCode
    {
        Success=0,
        Fail,
        AlreadyExit,        //用户已经存在
        InsertFailed        //插入用户局数失败
    }
}
