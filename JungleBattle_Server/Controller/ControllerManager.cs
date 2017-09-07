using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using JungleBattle_Server.Server;

namespace JungleBattle_Server.Controller
{
    class ControllerManager
    {
        private Dictionary<RequesetCode , BaseController> controllerDic;
        public ControllerManager()
        {
            controllerDic = new Dictionary<RequesetCode , BaseController>();
        }

        private BaseController GetController(RequesetCode request)
        {
            BaseController controller;
            if(controllerDic.TryGetValue(request,out controller))
            {
                return controller;
            }
            string errorMsg = string.Format("没有找到对应Controller   RequsetCode:{0}" , request);
            throw new Exception(errorMsg);
        }

        public object HandleMessageData(MessageData data)
        {
            BaseController controller = GetController(data.requsetCode);
            string methodname = data.actionCode.ToString();
            MethodInfo methodInfo = controller.GetType().GetMethod(methodname);
            if(methodInfo == null)
                throw new Exception("没有找到指定方法：" + methodname);
            string[] userdata = { data.data };
            object res = methodInfo.Invoke(controller , userdata);
            return res;
        }
    }
}
