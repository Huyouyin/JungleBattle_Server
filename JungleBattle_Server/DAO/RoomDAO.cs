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
        public bool InsertNewRoom(MySqlConnection conn,int ownerid)
        {
            string cmdStr = "insert into room set ownerid=@ownerid,status = 0";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            cmd.Parameters.AddWithValue("roomid" , ownerid);

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
            cmd.Parameters.AddWithValue("status" , (int)status);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    int roomid = reader.GetInt32(0);
                    int ownerid = reader.GetInt32(1);
                    reader.GetInt32(2);
                    reader.GetInt32(3);
                    Room room = new Room(roomid , ownerid);
                    roomlist.Add(room);
                }
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
        public int SeachWaitingRoomByUserID(MySqlConnection conn , int id)
        {
            string cmdStr = "select * from room where roomownerid = @id and status = 0";
            MySqlCommand cmd = new MySqlCommand(cmdStr , conn);
            cmd.Parameters.AddWithValue("id" , id);
            MySqlDataReader reader = null;
            try
            {
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    int roomid = reader.GetInt32(0);
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
