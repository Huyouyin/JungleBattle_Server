using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using JungleBattle_Server.DAO;
using JungleBattle_Server.Server;

namespace JungleBattle_Server.Controller
{
    class LoginController:BaseController
    {
        UserDAO userDAO;
        public LoginController()
        {
            this.requestCode = RequestCode.LoginRequest;
            userDAO = new UserDAO();
        }
        public void Login(string data,Client client)
        {

            string[] account=data.Split(' ');
            bool res = userDAO.ExistAccount(client.GetConn(), account[0], account[1]);

            LoginResultCode resCode = Common.LoginResultCode.Fail;
            if(res)
            {
                resCode = Common.LoginResultCode.Success;
            }
            //Console.WriteLine("结果：" + resCode.ToString());
            OnResponseLogin(resCode , client);
        }
        private void OnResponseLogin(LoginResultCode resCode,Client client)
        {
            MessageData mdata = new MessageData(RequestCode.LoginRequest , ActionCode.Login , resCode.ToString());
            client.OnResponse(mdata);
        }
    }
}
