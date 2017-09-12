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
        AlreadyExit
    }
}
