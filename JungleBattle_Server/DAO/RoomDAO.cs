using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using JungleBattle_Server.Model;
using MySql.Data.MySqlClient;

namespace JungleBattle_Server.DAO
{
    public class RoomDAO
    {
        public bool InsertNewRoom(MySqlConnection conn,string username)
        {
            string cmdStr = "insert into room set username = @username,status = 0";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            cmd.Parameters.AddWithValue("username" , username);

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
        public List<Room> SeachRoomByStatus(MySqlConnection conn,RoomStatus status)
        {
            List<Room> roomlist = new List<Room>();

            string cmdStr = "select * from room where status = @status";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            Console.WriteLine("status=" + status);
            cmd.Parameters.AddWithValue("status" , (int)status);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    int roomid = reader.GetInt32(0);
                    string ownername = reader.GetString(1);
                    Room room = new Room(roomid , ownername);
                    roomlist.Add(room);
                }
                Console.WriteLine("f房间个数：" + roomlist.Count);
                return roomlist;
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
        
        /// <summary>
        /// 查询玩家已经建立的未开始游戏的房间
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int SeachWaitingRoomByUserName(MySqlConnection conn , string username)
        {
            string cmdStr = "select * from room where username = @username and status = 0";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            cmd.Parameters.AddWithValue("username" , username);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    int roomid = reader.GetInt32(0);
                    return roomid;
                }
                return -1;
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
