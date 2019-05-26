using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class User
    {
        public string   _userNo      { get; private set; }
        public string   _userId      { get; private set; }
        public string   _userName    { get; private set; }
        public int      _overTime    { get; private set; }
        public int      _percentTime { get; private set; }
        public int      _workTime    { get; private set; }

        public User(string userNo, string userId = "", string userName = "",
                    int overTime = 0, int percentTime = 0, int workTime = 0)
        {
            _userNo         = userNo;
            _userId         = userId;
            _userName       = userName;
            _overTime       = overTime;
            _percentTime    = percentTime;
            _workTime       = workTime;
        }
    }
}