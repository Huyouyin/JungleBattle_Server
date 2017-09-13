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
        UserGameCountDAO usergameDAO;

        public RegiserController()
        {
            this.requestCode = RequestCode.RegisterRequest;
            userDAO = new UserDAO();
            usergameDAO = new UserGameCountDAO();
        }

        public void Register(string data , Client client)
        {
            string[] datas = data.Split(' ');
            string name = datas[0];
            string pass = datas[1];
            RegisterResultCode resCode = RegisterResultCode.Fail;
            int userid = userDAO.ExistAccount(client.GetConn() , name , pass);
            if(userid==-1)
            {//不存在用户
                if(userDAO.InsertAccount(client.GetConn() , name , pass))
                {//创建成功
                    userid = userDAO.ExistAccount(client.GetConn() , name , pass);
                    bool insertres = usergameDAO.Insert(new Model.UserGameCount(userid , 0 , 0) , client.GetConn());
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
        private void OnResponseRegister(RegisterResultCode resCode , Client client)
        {
            MessageData mdata = new MessageData(RequestCode.RegisterRequest , ActionCode.Register , resCode.ToString());
            client.OnResponse(mdata);
        }
    }
}
