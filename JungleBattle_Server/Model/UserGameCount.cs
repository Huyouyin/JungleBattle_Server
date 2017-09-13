using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JungleBattle_Server.Model
{
    public class UserGameCount
    {
        public int userID;
        public int winCount;
        public int totalCount;

        public UserGameCount(int id,int win,int total)
        {
            userID = id;
            winCount = win;
            totalCount = total;
        }
    }
}
