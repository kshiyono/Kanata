using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Task
    {
        public string TASK_NO { get; set; }
        public string USER_NO { get; set; }
        public DateTime CREATE_YMD { get; set; }
        public DateTime UPDATE_YMD { get; set; }
        public DateTime TODO_YMD { get; set; }
        public DateTime FINISHED_YMD { get; set; }
        public string PERCENT_TIME { get; set; }
        public string TASK_STATUS_CODE { get; set; }
        public string TASK_STATUS_NAME { get; set; }
        public string TASK_KIND_CODE { get; set; }
        public string TASK_KIND_NAME { get; set; }
        public string TASK_GROUP_CODE { get; set; }
        public string TASK_GROUP_NAME { get; set; }
        public string TASK_NAME { get; set; }
        public TimeSpan PLAN_TIME { get; set; }
        public TimeSpan RESULT_TIME { get; set; }
        public string MEMO { get; set; }
    }
}
