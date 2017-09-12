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
        private Dictionary<RequestCode , BaseController> controllerDic;
        public ControllerManager()
        {
            controllerDic = new Dictionary<RequestCode , BaseController>();
            controllerDic.Add(RequestCode.LoginRequest, new LoginController());
            controllerDic.Add(RequestCode.RegisterRequest , new RegiserController());
        }

        /// <summary>
        /// 取得控制器
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private BaseController GetController(RequestCode request)
        {
            BaseController controller;
            if(controllerDic.TryGetValue(request,out controller))
            {
                return controller;
            }
            string errorMsg = string.Format("没有找到对应Controller   RequsetCode:{0}" , request);
            throw new Exception(errorMsg);
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="mdata"></param>
        /// <returns></returns>
        public void HandleRequest(MessageData mdata,Client client)
        {
            
            BaseController controller = GetController(mdata.requsetCode);
            string methodname = mdata.actionCode.ToString();
            MethodInfo methodInfo = controller.GetType().GetMethod(methodname);
            if(methodInfo == null)
                throw new Exception("没有找到指定方法：" + methodname);
            object[] userdata = { mdata.data,client};
            methodInfo.Invoke(controller , userdata);
        }
    }
}
