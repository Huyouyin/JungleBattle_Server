using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JungleBattle_Server.Model;
using MySql.Data.MySqlClient;

namespace JungleBattle_Server.DAO
{
    public class UserDAO
    {
        public User ExistAccount(MySqlConnection conn, string name,string pass)
        {
            string cmdStr="select * from user where username=@name and userpass=@pass ";
            MySqlCommand cmd = new MySqlCommand(cmdStr, conn);
            cmd.Parameters.AddWithValue("name" , name);
            cmd.Parameters.AddWithValue("pass" , pass);
            MySqlDataReader reader=null;
            try
            {
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    int userid = reader.GetInt32(0);
                    string username = reader.GetString(1);
                    string userpass = reader.GetString(2);
                    User user = new User(userid , username , userpass);
                    return user;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }   
            finally
            {
                if(reader != null)
                    reader.Close();
                cmd.Dispose();
            }
        }

        public bool InsertAccount(MySqlConnection conn , string name , string pass)
        {
            string cmdStr = "insert into user set username=@name ,userpass=@pass";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            cmd.Parameters.AddWithValue("name" , name);
            cmd.Parameters.AddWithValue("pass" , pass);
            try
            {
                int res = cmd.ExecuteNonQuery();
                if(res == 1)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            finally
            {
                cmd.Dispose();
            }
        }
    }
}
