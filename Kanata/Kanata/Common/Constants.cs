using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class Constants
    {
        #region タスクステータス
        /// <summary>
        /// タスクステータス
        /// </summary>
        public static class TaskStatus
        {
            // タスクステータスカラム名
            public static readonly string TASK_STATUS_CODE  = "TASK_STATUS_CODE";
            public static readonly string TASK_STATUS_NAME  = "TASK_STATUS_NAME";

            // 「全て」(00)
            public static readonly string ALL_00 = "00";

            // 「未」(01)
            public static readonly string UNCOMPLETED_01    = "01";

            // 「済」(10)
            public static readonly string COMPLETED_10      = "10";

            // 「削除」(20)
            public static readonly string DELETE_20         = "20";
        }
        #endregion

        #region タスク種別
        /// <summary>
        /// タスク種別
        /// </summary>
        public static class TaskKind
        {
            // タスク種別カラム名
            public static readonly string TASK_KIND_CODE    = "TASK_KIND_CODE";
            public static readonly string TASK_KIND_NAME    = "TASK_KIND_NAME";

            // 「全て」(00)
            public static readonly string ALL_00 = "00";

            // 「個人」(01)
            public static readonly string PERSONAL_01       = "01";
        }
        #endregion

        #region タスクグループ
        /// <summary>
        /// タスクグループ
        /// </summary>
        public static class TaskGroup
        {
            // タスク種別カラム名
            public static readonly string TASK_GROUP_CODE   = "TASK_GROUP_CODE";
            public static readonly string TASK_GROUP_NAME   = "TASK_GROUP_NAME";
            
            // 「全て」(00)
            public static readonly string ALL_00 = "00";
        }
        #endregion
    }
}
