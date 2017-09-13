using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using JungleBattle_Server.DAO;
using JungleBattle_Server.Server;
using JungleBattle_Server.Model;

namespace JungleBattle_Server.Controller
{
    class LoginController:BaseController
    {
        UserDAO userDAO;
        UserGameCountDAO usergamecountDAO;
        public LoginController()
        {
            this.requestCode = RequestCode.LoginRequest;
            userDAO = new UserDAO();
            usergamecountDAO = new UserGameCountDAO();
        }
        public void Login(string data,Client client)
        {

            string[] account=data.Split(' ');
            int userid = userDAO.ExistAccount(client.GetConn(), account[0], account[1]);

            LoginResultCode resCode = Common.LoginResultCode.Fail;
            UserGameCount ugc = null;
            if(userid!=-1)
            {
                resCode = Common.LoginResultCode.Success;
                ugc = usergamecountDAO.Check(userid , client.GetConn());
                if(ugc == null)
                {
                    throw new Exception("在usergamecount中找不到此用户：userid=" + userid);
                }
            }
            //Console.WriteLine("结果：" + resCode.ToString());
            OnResponseLogin(resCode , client,ugc);
        }
        private void OnResponseLogin(LoginResultCode resCode,Client client,UserGameCount ugc)
        {
            string data=string.Empty;
            switch(resCode)
            {
                case LoginResultCode.Success:
                    data = Message.PackContentData(',' , resCode.ToString() , ugc.totalCount.ToString() , ugc.winCount.ToString());
                    break;
                case LoginResultCode.Fail:
                    data = Message.PackContentData(',' , resCode.ToString());
                    break;
                default:
                    break;
            }
            MessageData mdata = new MessageData(RequestCode.LoginRequest , ActionCode.Login , data);
            client.OnResponse(mdata);
        }
    }
}
