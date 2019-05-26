using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CodeData
    {
        private string _logInUserNo { get; set; }

        public CodeData(string logInUserNo)
        {
            _logInUserNo    = logInUserNo;
        }

        #region グループコード
        #region タスクグループ更新
        /// <summary>
        /// ユーザ毎のタスクグループ名を更新する。
        /// </summary>
        /// <param name="taskGroupCode">タスクグループコードインスタンス</param>
        /// <returns></returns>
        public void TaskGroupMstUpdate(Code taskGroupCode)
        {
            DataBaceAccess dbAccess = new DataBaceAccess();
            SqlConnection connection = dbAccess.GetSqlSvrConnect();
            SqlCommand command = connection.CreateCommand();

            try
            {
                // データベースの接続開始
                connection.Open();

                // SQLの準備
                StringBuilder query = new StringBuilder();

                query.AppendLine("UPDATE MST_USER_TASK_GROUP_CODE ");
                query.AppendLine("SET ");
                query.AppendLine("    TASK_GROUP_NAME   = @TASK_GROUP_NAME ");
                query.AppendLine("WHERE ");
                query.AppendLine("    USER_NO           = @USER_NO ");
                query.AppendLine("AND TASK_GROUP_CODE   = @TASK_GROUP_CODE ");

                command.CommandText = query.ToString();

                command.Parameters.Add(new SqlParameter("@TASK_GROUP_NAME", taskGroupCode._name));
                command.Parameters.Add(new SqlParameter("@USER_NO", _logInUserNo));
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", taskGroupCode._code));

                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
        #endregion
    }
}
