using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JungleBattle_Server.Model
{
    public class User
    {
        public int userid;
        public string userName;
        public string userPass;
        
        public User (int id,string name ,string pass)
        {
            this.userid = id;
            this.userName = name;
            this.userPass = pass;
        }
    }
}
