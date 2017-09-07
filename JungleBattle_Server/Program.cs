using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JungleBattle_Server.Server;

namespace JungleBattle_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer server = new GameServer();
            server.Start();

            Console.ReadKey();
        }
    }
}
