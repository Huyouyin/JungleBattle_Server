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
        public readonly string CONNECTSTRING = "datasource=127.0.0.1;port=3306;database=junglebattle;user=root;password=root";
        MySqlConnection conn;
        private void Connect()
        {
            conn = new MySqlConnection(CONNECTSTRING);
            try
            {
                conn.Open();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public ConnHelper()
        {
            Connect();
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
