using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Task
    {
        public string       _taskNo             { get; private set; }
        public string       _userNo             { get; private set; }
        public Code         _taskStatusCode     { get; private set; }
        public Code         _taskKindCode       { get; private set; }
        public Code         _taskGroupCode      { get; private set; }
        public string       _taskName           { get; private set; }
        public TimeSpan?    _planTime           { get; private set; }
        public TimeSpan?    _resultTime         { get; private set; }
        public string       _memo               { get; private set; }
        public DateTime?    _createYmd          { get; private set; }
        public DateTime?    _updateYmd          { get; private set; }
        public DateTime?    _todoYmd            { get; private set; }
        public DateTime?    _finishedYmd        { get; private set; }

        public Task(string taskNo, string userNo, Code taskStatusCode = null, Code taskKindCode = null, Code taskGroupCode = null,
            string taskName = "", TimeSpan? planTime = null, TimeSpan? resultTime = null, string memo = "",
            DateTime? createYmd = null, DateTime? updateYmd = null, DateTime? todoYmd = null, DateTime? finishedYmd = null)
        {
            _taskNo             = taskNo;
            _userNo             = userNo;
            _createYmd          = createYmd;
            _updateYmd          = updateYmd;
            _todoYmd            = todoYmd;
            _finishedYmd        = finishedYmd;  
            _taskStatusCode     = taskStatusCode;
            _taskKindCode       = taskKindCode;
            _taskGroupCode      = taskGroupCode;
            _taskName           = taskName;
            _planTime           = planTime;
            _resultTime         = resultTime;
            _memo               = memo;
        }
    }
}
