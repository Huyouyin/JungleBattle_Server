using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JungleBattle_Server.Model;
using MySql.Data.MySqlClient;

namespace JungleBattle_Server.DAO
{
    class UserGameCountDAO
    {
        public bool Insert(UserGameCount usergame,MySqlConnection conn)
        {
            string cmdStr = "insert into usergamecount set userid=@id ,wincount=@win,totalcount=@total";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            cmd.Parameters.AddWithValue("id" , usergame.userID);
            cmd.Parameters.AddWithValue("win" , usergame.winCount);
            cmd.Parameters.AddWithValue("total" , usergame.totalCount);
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

        public UserGameCount Check(int userid,MySqlConnection conn)
        {
            string cmdStr = "select * from usergamecount where userid=@id";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            cmd.Parameters.AddWithValue("id" , userid);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    int win = reader.GetInt32("wincount");
                    int total = reader.GetInt32("totalcount");
                    return new UserGameCount(userid,win,total);
                }
                return null;
            }
            catch(Exception e)
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
    }
}
