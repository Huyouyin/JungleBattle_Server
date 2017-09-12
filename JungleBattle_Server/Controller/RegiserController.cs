using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JungleBattle_Server.DAO;
using Common;
using JungleBattle_Server.Server;

namespace JungleBattle_Server.Controller
{
    class RegiserController:BaseController
    {
        UserDAO userDAO;
        public RegiserController()
        {
            this.requestCode = RequestCode.RegisterRequest;
            userDAO = new UserDAO();
        }

        public void Register(string data , Client client)
        {
            string[] datas = data.Split(' ');
            string name = datas[0];
            string pass = datas[1];
            RegisterResultCode resCode = RegisterResultCode.Fail;
            if(!userDAO.ExistAccount(client.GetConn(),name,pass))
            {//不存在用户
                if(userDAO.InsertAccount(client.GetConn() , name , pass))
                {//创建成功
                    resCode = RegisterResultCode.Success;
                }
            }
            else
            {
                resCode = RegisterResultCode.AlreadyExit;
            }
            OnResponseRegister(resCode , client);
        }
        private void OnResponseRegister(RegisterResultCode resCode , Client client)
        {
            MessageData mdata = new MessageData(RequestCode.RegisterRequest , ActionCode.Register , resCode.ToString());
            client.OnResponse(mdata);
        }
    }
}
