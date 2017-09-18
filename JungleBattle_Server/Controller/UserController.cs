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
            User user = userDAO.ExistAccount(client.GetConn() , account[0] , account[1]);

            LoginResultCode resCode = Common.LoginResultCode.Fail;
            if(user  != null)
            {
                resCode = Common.LoginResultCode.Success;
            }
            //Console.WriteLine("结果：" + resCode.ToString());
            OnResponseLogin(resCode , client , user);
        }
        //登陆响应
        private void OnResponseLogin(LoginResultCode resCode , Client client,User user)
        {
            string data = string.Empty;
            switch(resCode)
            {
                case LoginResultCode.Success:
                    data = Message.PackContentData(' ' , resCode.ToString(),user.userid.ToString(),user.userName,user.userPass);
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

        public void Logout(string data,Client client)
        {
        }
        private void OnResponseLogout(LoginResultCode resCode,Client client)
        {

        }

        //客户端注册处理
        public void Register(string data , Client client)
        {
            string[] datas = data.Split(' ');
            string name = datas[0];
            string pass = datas[1];
            RegisterResultCode resCode = RegisterResultCode.Fail;
            User user = userDAO.ExistAccount(client.GetConn() , name , pass);
            if(user == null)
            {//不存在用户
                if(userDAO.InsertAccount(client.GetConn() , name , pass))
                {//创建成功
                    user = userDAO.ExistAccount(client.GetConn() , name , pass);
                    bool insertres = usergamecountDAO.Insert(new UserGameCount(user.userid , 0 , 0) , client.GetConn());
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
            OnResponseRegister(resCode , client,user);
        }
        //注册响应
        private void OnResponseRegister(RegisterResultCode resCode , Client client,User user)
        {
            string res = resCode.ToString();
            if(RegisterResultCode.Success == resCode)
            {
                res += "," + user.userid + "," + user.userName + "," + user.userPass;
            }
            MessageData mdata = new MessageData(RequestCode.User , ActionCode.Register , res);
            client.OnResponse(mdata);
        }

        //客户端请求战斗次数
        public void BattleCount(string data,Client client)
        {
            UserGameCount ugc = usergamecountDAO.Check(int.Parse(data) , client.GetConn());
            if(ugc == null)
            {
                throw new Exception("没有找到用户的战斗数据：" + data);
            }
            OnResponseBattleCount(ugc,client);
        }

        private void OnResponseBattleCount(UserGameCount ugc , Client client)
        {
            string data = ugc.totalCount + "," + ugc.winCount;
            MessageData mdata = new MessageData(RequestCode.User , ActionCode.BattleCount , data);
            client.OnResponse(mdata);
        }
    }
}
