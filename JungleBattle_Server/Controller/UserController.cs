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
    class UserController:BaseController
    {
        UserDAO userDAO;
        UserGameCountDAO usergamecountDAO;
        public UserController()
        {
            this.requestCode = RequestCode.User;
            userDAO = new UserDAO();
            usergamecountDAO = new UserGameCountDAO();
        }

        //客户端登陆处理
        public void Login(string data , Client client)
        {

            string[] account = data.Split(' ');
            int userid = userDAO.ExistAccount(client.GetConn() , account[0] , account[1]);

            LoginResultCode resCode = Common.LoginResultCode.Fail;
            UserGameCount ugc = null;
            if(userid != -1)
            {
                resCode = Common.LoginResultCode.Success;
                ugc = usergamecountDAO.Check(userid , client.GetConn());
                if(ugc == null)
                {
                    throw new Exception("在usergamecount中找不到此用户：userid=" + userid);
                }
            }
            //Console.WriteLine("结果：" + resCode.ToString());
            OnResponseLogin(resCode , client , ugc);
        }
        //登陆响应
        private void OnResponseLogin(LoginResultCode resCode , Client client , UserGameCount ugc)
        {
            string data = string.Empty;
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
            MessageData mdata = new MessageData(RequestCode.User , ActionCode.Login , data);
            client.OnResponse(mdata);
        }
        //客户端注册处理
        public void Register(string data , Client client)
        {
            string[] datas = data.Split(' ');
            string name = datas[0];
            string pass = datas[1];
            RegisterResultCode resCode = RegisterResultCode.Fail;
            int userid = userDAO.ExistAccount(client.GetConn() , name , pass);
            if(userid == -1)
            {//不存在用户
                if(userDAO.InsertAccount(client.GetConn() , name , pass))
                {//创建成功
                    userid = userDAO.ExistAccount(client.GetConn() , name , pass);
                    bool insertres = usergamecountDAO.Insert(new Model.UserGameCount(userid , 0 , 0) , client.GetConn());
                    if(insertres)
                    {
                        resCode = RegisterResultCode.Success;
                    }
                    else
                    {
                        throw new Exception("插入表usergamecount失败");
                    }
                }
            }
            else
            {
                resCode = RegisterResultCode.AlreadyExit;
            }
            OnResponseRegister(resCode , client);
        }
        //注册响应
        private void OnResponseRegister(RegisterResultCode resCode , Client client)
        {
            MessageData mdata = new MessageData(RequestCode.User , ActionCode.Register , resCode.ToString());
            client.OnResponse(mdata);
        }
    }
}
