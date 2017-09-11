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
        public object Login(string data,Client client)
        {
            string[] account=data.Split(' ');
            bool res = userDAO.VarifyAccount(client.GetConn(), account[0], account[1]);
            if (res)
                return ResultCode.Success;
            return ResultCode.Fail;
        }
    }
}
