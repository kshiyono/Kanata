using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Flg
    {
        public virtual string USER_NO { get; set; }
        public virtual DateTime DAY_FROM { get; set; }
        public virtual DateTime DAY_TO { get; set; }
        public virtual string TASK_STATUS_CODE { get; set; }
        public virtual string TASK_KIND_CODE { get; set; }
        public virtual string TASK_GROUP_CODE { get; set; }
        public virtual string CHART_INTERVAL { get; set; }
    }

    public class Flg_Task_Select : Flg
    {
        public override string USER_NO { get; set; }
        public override DateTime DAY_FROM { get; set; }
        public override DateTime DAY_TO { get; set; }
        public override string TASK_STATUS_CODE { get; set; }
        public override string TASK_KIND_CODE { get; set; }
        public override string TASK_GROUP_CODE { get; set; }
    }

    public class Flg_Chart_Select : Flg
    {
        public override string USER_NO { get; set; }
        public override DateTime DAY_FROM { get; set; }
        public override DateTime DAY_TO { get; set; }
        public override string TASK_KIND_CODE { get; set; }
        public override string TASK_GROUP_CODE { get; set; }
        public override string CHART_INTERVAL { get; set; }
    }
}
