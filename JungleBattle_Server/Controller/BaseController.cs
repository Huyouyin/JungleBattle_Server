﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace JungleBattle_Server.Controller
{
    abstract class BaseController
    {
        public virtual object DefaultHandleRequset(string data)
        {
            return null;
        }
    }
}
