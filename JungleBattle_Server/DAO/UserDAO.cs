using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JungleBattle_Server.DAO
{
    public class UserDAO
    {
        public bool VarifyAccount(MySqlConnection conn, string name,string pass)
        {
            string cmdStr="select * from user where name= @name and pass=@pass ";
            MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("pass", pass);
            int res = cmd.ExecuteNonQuery();
            if(res > 0)
            {
                return true;
            }
            return false;
        }
    }
}
