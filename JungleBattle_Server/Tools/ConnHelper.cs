using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JungleBattle_Server.Tools
{
    /// <summary>
    /// 2017-9-8
    /// huyy
    /// 此类用于和数据库的交互
    /// </summary>
    public class ConnHelper
    {
        private readonly string CONNECTSTRING = "datasource=127.0.0.1;port=3306;database=junglebattle;user=root;password=root";
        MySqlConnection conn;

        public MySqlConnection Conn
        {
            get { return conn; }
        }
        public MySqlConnection Connect()
        {
            conn = new MySqlConnection(CONNECTSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch(Exception e)
            {
                throw e;
            }
        }        

        public void Close()
        {
            if(conn!=null)
            {
                conn.Close();
            }
        }
    }
}
