using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data
{
    public class TaskData
    {
        #region タスク管理画面　タスク一覧取得処理
        public List<Task> LoadForm_TaskList_Select(Flg_Task_Select flgTaskSelect)
        {
            // タスクインスタンス生成
            List<Task> taskList         = null;
            Task selectedTaskForList    = null;
            Code taskStatusCode         = null;
            Code taskKindCode           = null;
            Code taskGroupCode          = null;

            // 共通部品を生成
            DataBaceAccess dbAccess = new DataBaceAccess();

            // 共通部品からSQLserverとの接続オブジェクトを取得
            SqlConnection connection = dbAccess.GetSqlSvrConnect();

            SqlCommand command = connection.CreateCommand();
            DataTable dt = new DataTable();

            try
            {
                // データベースの接続開始
                connection.Open();

                // SQLの準備
                StringBuilder query = new StringBuilder();

                query.AppendLine("SELECT");
                query.AppendLine("    T_TL.TASK_NO ");
                query.AppendLine("  , T_TL.USER_NO ");
                query.AppendLine("  , T_TL.CREATE_YMD ");
                query.AppendLine("  , T_TL.UPDATE_YMD ");
                query.AppendLine("  , T_TL.TODO_YMD ");
                query.AppendLine("  , T_TL.FINISHED_YMD ");
                query.AppendLine("  , T_TL.TASK_STATUS_CODE ");
                query.AppendLine("  , M_TSC.TASK_STATUS_NAME ");
                query.AppendLine("  , T_TL.TASK_KIND_CODE ");
                query.AppendLine("  , M_UTKC.TASK_KIND_NAME ");
                query.AppendLine("  , T_TL.TASK_GROUP_CODE ");
                query.AppendLine("  , M_UTGC.TASK_GROUP_NAME ");
                query.AppendLine("  , T_TL.TASK_NAME ");
                query.AppendLine("  , T_TL.PLAN_TIME ");
                query.AppendLine("  , T_TL.RESULT_TIME ");
                query.AppendLine("  , T_TL.MEMO ");
                // タスクリストテーブル
                query.AppendLine("FROM TRN_TASK_LIST T_TL ");
                // タスクステータスコードマスタ
                query.AppendLine("INNER JOIN MST_TASK_STATUS_CODE       M_TSC ");
                query.AppendLine("  ON T_TL.TASK_STATUS_CODE        =   M_TSC.TASK_STATUS_CODE ");
                // タスク種別コードマスタ
                query.AppendLine("INNER JOIN MST_USER_TASK_KIND_CODE    M_UTKC ");
                query.AppendLine("  ON  T_TL.USER_NO                =   M_UTKC.USER_NO ");
                query.AppendLine("  AND T_TL.TASK_KIND_CODE         =   M_UTKC.TASK_KIND_CODE ");
                // タスクグループコードマスタ
                query.AppendLine("INNER JOIN MST_USER_TASK_GROUP_CODE   M_UTGC ");
                query.AppendLine("  ON  T_TL.USER_NO                =   M_UTGC.USER_NO ");
                query.AppendLine("  AND T_TL.TASK_GROUP_CODE        =   M_UTGC.TASK_GROUP_CODE ");

                #region 検索条件

                // ユーザーNo
                query.AppendLine("WHERE T_TL.USER_NO                =   @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", flgTaskSelect.USER_NO));

                // 検索日付条件
                if (flgTaskSelect.DAY_FROM != new DateTime(0))
                {
                    query.AppendLine("AND T_TL.TODO_YMD             >=  @DAY_FROM ");
                    command.Parameters.Add(new SqlParameter("@DAY_FROM", flgTaskSelect.DAY_FROM));
                }
                if (flgTaskSelect.DAY_TO != new DateTime(0))
                {
                    query.AppendLine("AND T_TL.TODO_YMD             <=  @DAY_TO ");
                    command.Parameters.Add(new SqlParameter("@DAY_TO", flgTaskSelect.DAY_TO));
                }
                // タスクステータスコード条件
                if (flgTaskSelect.TASK_STATUS_CODE != Constants.TaskStatus.ALL_00)
                {
                    query.AppendLine("AND T_TL.TASK_STATUS_CODE     =   @TASK_STATUS_CODE ");
                    command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", flgTaskSelect.TASK_STATUS_CODE));
                }
                // タスク種別コード条件
                if (flgTaskSelect.TASK_KIND_CODE   != Constants.TaskKind.ALL_00)
                {
                    query.AppendLine("AND T_TL.TASK_KIND_CODE       =   @TASK_KIND_CODE ");
                    command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", flgTaskSelect.TASK_KIND_CODE));
                }
                // タスクグループコード条件
                if (flgTaskSelect.TASK_GROUP_CODE  != Constants.TaskGroup.ALL_00)
                {
                    query.AppendLine("AND T_TL.TASK_GROUP_CODE      =   @TASK_GROUP_CODE ");
                    command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", flgTaskSelect.TASK_GROUP_CODE));
                }
                #endregion

                query.AppendLine("ORDER BY TODO_YMD, TASK_STATUS_CODE ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataReader reader = command.ExecuteReader();

                // タスクインスタンスの生成
                taskList = new List<Task>();

                // 結果を表示します。
                while (reader.Read())
                {
                    taskStatusCode  = new Code((string)reader.GetValue(6), (string)reader.GetValue(7));
                    taskKindCode    = new Code((string)reader.GetValue(8), (string)reader.GetValue(9));
                    taskGroupCode   = new Code((string)reader.GetValue(10), (string)reader.GetValue(11));

                    selectedTaskForList = new Task(
                        taskNo: (string)reader.GetValue(0),
                        userNo: (string)reader.GetValue(1),
                        taskStatusCode: taskStatusCode,
                        taskKindCode: taskKindCode,
                        taskGroupCode: taskGroupCode,
                        taskName: (string)reader.GetValue(12),
                        planTime: (TimeSpan)reader.GetValue(13),
                        resultTime: (TimeSpan)reader.GetValue(14),
                        memo: (string)reader.GetValue(15),
                        createYmd: (DateTime)reader.GetValue(2),
                        updateYmd: (DateTime)reader.GetValue(3),
                        todoYmd: (DateTime)reader.GetValue(4),
                        finishedYmd: (DateTime)reader.GetValue(5)
                    );

                    taskList.Add(selectedTaskForList);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                // データベースの接続終了
                connection.Close();
            }

            return taskList;
        }
        #endregion

        #region　タスク管理画面　タスク追加処理
        public void Task_Insert(Task taskAdd)
        {
            // 共通部品を生成
            DataBaceAccess dbAccess = new DataBaceAccess();

            // 共通部品からSQLserverとの接続オブジェクトを取得
            SqlConnection connection = dbAccess.GetSqlSvrConnect();
            SqlCommand command = connection.CreateCommand();

            try
            {
                // データベースの接続開始
                connection.Open();

                // SQLの準備
                StringBuilder query = new StringBuilder();

                query.AppendLine("INSERT ");
                query.AppendLine("    INTO TRN_TASK_LIST ");
                query.AppendLine("( ");
                query.AppendLine("    TASK_NO ");
                query.AppendLine("  , USER_NO ");
                query.AppendLine("  , CREATE_YMD ");
                query.AppendLine("  , UPDATE_YMD ");
                query.AppendLine("  , TODO_YMD ");
                query.AppendLine("  , FINISHED_YMD ");
                query.AppendLine("  , TASK_STATUS_CODE ");
                query.AppendLine("  , TASK_KIND_CODE ");
                query.AppendLine("  , TASK_GROUP_CODE ");
                query.AppendLine("  , TASK_NAME ");
                query.AppendLine("  , PLAN_TIME ");
                query.AppendLine("  , RESULT_TIME ");
                query.AppendLine("  , MEMO ");
                query.AppendLine(") ");
                query.AppendLine("VALUES ");
                query.AppendLine("( ");
                query.AppendLine("    @TASK_NO ");
                query.AppendLine("  , @USER_NO ");
                query.AppendLine("  , @CREATE_YMD ");
                query.AppendLine("  , @UPDATE_YMD ");
                query.AppendLine("  , @TODO_YMD ");
                query.AppendLine("  , @FINISHED_YMD ");
                query.AppendLine("  , @TASK_STATUS_CODE ");
                query.AppendLine("  , @TASK_KIND_CODE ");
                query.AppendLine("  , @TASK_GROUP_CODE ");
                query.AppendLine("  , @TASK_NAME ");
                query.AppendLine("  , @PLAN_TIME ");
                query.AppendLine("  , @RESULT_TIME ");
                query.AppendLine("  , @MEMO ");
                query.AppendLine(") ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@TASK_NO", taskAdd._taskNo));
                command.Parameters.Add(new SqlParameter("@USER_NO", taskAdd._userNo));
                command.Parameters.Add(new SqlParameter("@CREATE_YMD", taskAdd._createYmd));
                command.Parameters.Add(new SqlParameter("@UPDATE_YMD", taskAdd._updateYmd));
                command.Parameters.Add(new SqlParameter("@TODO_YMD", taskAdd._todoYmd));
                command.Parameters.Add(new SqlParameter("@FINISHED_YMD", taskAdd._finishedYmd));
                command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", taskAdd._taskStatusCode._code));
                command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", taskAdd._taskKindCode._code));
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", taskAdd._taskGroupCode._code));
                command.Parameters.Add(new SqlParameter("@TASK_NAME", taskAdd._taskName));
                command.Parameters.Add(new SqlParameter("@PLAN_TIME", taskAdd._planTime));
                command.Parameters.Add(new SqlParameter("@RESULT_TIME", taskAdd._resultTime));
                command.Parameters.Add(new SqlParameter("@MEMO", taskAdd._memo));

                // SQLの実行
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                // データベースの接続終了
                connection.Close();
            }
        }
        #endregion

        #region　タスク管理画面　タスク番号取得処理
        public string Get_TaskNo_Max(string user_no)
        {
            // 
            string task_no_max = null;

            // 共通部品を生成
            DataBaceAccess dbAccess = new DataBaceAccess();

            // 共通部品からSQLserverとの接続オブジェクトを取得
            SqlConnection connection = dbAccess.GetSqlSvrConnect();
            SqlCommand command = connection.CreateCommand();
            DataTable dt = new DataTable();

            try
            {
                // データベースの接続開始
                connection.Open();

                // SQLの準備
                StringBuilder query = new StringBuilder();

                query.AppendLine("SELECT");
                query.AppendLine("    RIGHT('0000000' + CONVERT(VARCHAR,ISNULL(MAX(TASK_NO), 0) + 1), 7) ");
                query.AppendLine("FROM TRN_TASK_LIST ");
                query.AppendLine("WHERE USER_NO            =   @USER_NO ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@USER_NO", user_no));

                // SQLの実行
                SqlDataReader reader = command.ExecuteReader();

                // 結果を表示します。
                while (reader.Read())
                {
                    task_no_max = (string)reader.GetValue(0);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                // データベースの接続終了
                connection.Close();
            }

            return task_no_max;
        }
        #endregion

        #region タスク更新画面　１タスク取得処理
        public Task LoadForm_Task_Select(string taskNo, string loginUserNo)
        {
            // タスクインスタンス生成
            Task taskReturn             = null;
            Code taskStatusCode         = null;
            Code taskKindCode           = null;
            Code taskGroupCode          = null;

            // 共通部品を生成
            DataBaceAccess dbAccess = new DataBaceAccess();

            // 共通部品からSQLserverとの接続オブジェクトを取得
            SqlConnection connection = dbAccess.GetSqlSvrConnect();

            SqlCommand command = connection.CreateCommand();

            try
            {
                // データベースの接続開始
                connection.Open();

                // SQLの準備
                StringBuilder query = new StringBuilder();

                query.AppendLine("SELECT");
                query.AppendLine("    T_TL.TASK_NO ");
                query.AppendLine("  , T_TL.USER_NO ");
                query.AppendLine("  , T_TL.CREATE_YMD ");
                query.AppendLine("  , T_TL.UPDATE_YMD ");
                query.AppendLine("  , T_TL.TODO_YMD ");
                query.AppendLine("  , T_TL.FINISHED_YMD ");
                query.AppendLine("  , T_TL.TASK_STATUS_CODE ");
                query.AppendLine("  , M_TSC.TASK_STATUS_NAME ");
                query.AppendLine("  , T_TL.TASK_KIND_CODE ");
                query.AppendLine("  , M_UTKC.TASK_KIND_NAME ");
                query.AppendLine("  , T_TL.TASK_GROUP_CODE ");
                query.AppendLine("  , M_UTGC.TASK_GROUP_NAME ");
                query.AppendLine("  , T_TL.TASK_NAME ");
                query.AppendLine("  , T_TL.PLAN_TIME ");
                query.AppendLine("  , T_TL.RESULT_TIME ");
                query.AppendLine("  , T_TL.MEMO ");
                // タスクリストテーブル
                query.AppendLine("FROM TRN_TASK_LIST T_TL ");
                // タスクステータスコードマスタ
                query.AppendLine("INNER JOIN MST_TASK_STATUS_CODE   M_TSC ");
                query.AppendLine("  ON T_TL.TASK_STATUS_CODE        =   M_TSC.TASK_STATUS_CODE ");
                // タスク種別コードマスタ
                query.AppendLine("INNER JOIN MST_USER_TASK_KIND_CODE    M_UTKC ");
                query.AppendLine("  ON  T_TL.USER_NO                =   M_UTKC.USER_NO ");
                query.AppendLine("  AND T_TL.TASK_KIND_CODE         =   M_UTKC.TASK_KIND_CODE ");
                // タスクグループコードマスタ
                query.AppendLine("INNER JOIN MST_USER_TASK_GROUP_CODE   M_UTGC ");
                query.AppendLine("  ON T_TL.USER_NO                 =   M_UTGC.USER_NO ");
                query.AppendLine("  AND T_TL.TASK_GROUP_CODE        =   M_UTGC.TASK_GROUP_CODE ");
                // 検索条件
                query.AppendLine("WHERE T_TL.TASK_NO                =   @TASK_NO ");
                command.Parameters.Add(new SqlParameter("@TASK_NO", taskNo));
                query.AppendLine("  AND T_TL.USER_NO                =   @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", loginUserNo));

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read() == true)
                { 
                    // 値のセット
                    taskStatusCode  = new Code((string)reader.GetValue(6), (string)reader.GetValue(7));
                    taskKindCode    = new Code((string)reader.GetValue(8), (string)reader.GetValue(9));
                    taskGroupCode   = new Code((string)reader.GetValue(10), (string)reader.GetValue(11));

                    taskReturn = new Task(
                        taskNo: (string)reader.GetValue(0),
                        userNo: (string)reader.GetValue(1),
                        taskStatusCode: taskStatusCode,
                        taskKindCode: taskKindCode,
                        taskGroupCode: taskGroupCode,
                        taskName: (string)reader.GetValue(12),
                        planTime: (TimeSpan)reader.GetValue(13),
                        resultTime: (TimeSpan)reader.GetValue(14),
                        memo: (string)reader.GetValue(15),
                        createYmd: (DateTime)reader.GetValue(2),
                        updateYmd: (DateTime)reader.GetValue(3),
                        todoYmd: (DateTime)reader.GetValue(4),
                        finishedYmd: (DateTime)reader.GetValue(5)
                    );
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                // データベースの接続終了
                connection.Close();
            }

            return taskReturn;
        }
        #endregion

        #region タスク更新画面　タスク更新処理
        public void Task_Update(Task taskUpdate)
        {
            // 共通部品を生成
            DataBaceAccess dbAccess = new DataBaceAccess();

            // 共通部品からSQLserverとの接続オブジェクトを取得
            SqlConnection connection = dbAccess.GetSqlSvrConnect();
            SqlCommand command = connection.CreateCommand();

            try
            {
                // データベースの接続開始
                connection.Open();

                // SQLの準備
                StringBuilder query = new StringBuilder();

                query.AppendLine("UPDATE TRN_TASK_LIST ");
                query.AppendLine("SET ");
                query.AppendLine("    UPDATE_YMD        = @UPDATE_YMD ");
                if(taskUpdate._taskStatusCode._code == Constants.TaskStatus.COMPLETED_10)
                    query.AppendLine("  , FINISHED_YMD      = @FINISHED_YMD ");
                query.AppendLine("  , TODO_YMD          = @TODO_YMD ");
                query.AppendLine("  , TASK_STATUS_CODE  = @TASK_STATUS_CODE ");
                query.AppendLine("  , TASK_KIND_CODE    = @TASK_KIND_CODE ");
                query.AppendLine("  , TASK_GROUP_CODE   = @TASK_GROUP_CODE ");
                query.AppendLine("  , TASK_NAME         = @TASK_NAME ");
                query.AppendLine("  , PLAN_TIME         = @PLAN_TIME ");
                query.AppendLine("  , RESULT_TIME       = @RESULT_TIME ");
                query.AppendLine("  , MEMO              = @MEMO ");
                query.AppendLine("WHERE ");
                query.AppendLine("    TASK_NO           = @TASK_NO ");
                query.AppendLine("AND USER_NO           = @USER_NO ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@TASK_NO", taskUpdate._taskNo));
                command.Parameters.Add(new SqlParameter("@USER_NO", taskUpdate._userNo));
                command.Parameters.Add(new SqlParameter("@UPDATE_YMD", taskUpdate._updateYmd));
                command.Parameters.Add(new SqlParameter("@TODO_YMD", taskUpdate._todoYmd));
                if (taskUpdate._taskStatusCode._code == Constants.TaskStatus.COMPLETED_10)
                    command.Parameters.Add(new SqlParameter("@FINISHED_YMD", taskUpdate._finishedYmd));
                command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", taskUpdate._taskStatusCode._code));
                command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", taskUpdate._taskKindCode._code));
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", taskUpdate._taskGroupCode._code));
                command.Parameters.Add(new SqlParameter("@TASK_NAME", taskUpdate._taskName));
                command.Parameters.Add(new SqlParameter("@PLAN_TIME", taskUpdate._planTime));
                command.Parameters.Add(new SqlParameter("@RESULT_TIME", taskUpdate._resultTime));
                command.Parameters.Add(new SqlParameter("@MEMO", taskUpdate._memo ?? ""));

                // SQLの実行
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                // データベースの接続終了
                connection.Close();
            }
        }
        #endregion

        #region タスク更新画面　タスク中断処理
        public void Task_Stop(Task taskStop)
        {
            DataBaceAccess dbAccess = new DataBaceAccess();
            SqlConnection connection = dbAccess.GetSqlSvrConnect();
            SqlCommand command = connection.CreateCommand();

            try
            {
                connection.Open();
                StringBuilder query = new StringBuilder();

                query.AppendLine("UPDATE TRN_TASK_LIST ");
                query.AppendLine("SET ");
                query.AppendLine("      RESULT_TIME         = @RESULT_TIME ");
                query.AppendLine("  ,   MEMO                = @MEMO ");
                query.AppendLine("WHERE ");
                query.AppendLine("    TASK_NO           = @TASK_NO ");
                query.AppendLine("AND USER_NO           = @USER_NO ");

                command.CommandText = query.ToString();

                command.Parameters.Add(new SqlParameter("@TASK_NO", taskStop._taskNo));
                command.Parameters.Add(new SqlParameter("@USER_NO", taskStop._userNo));
                command.Parameters.Add(new SqlParameter("@RESULT_TIME", taskStop._resultTime));
                command.Parameters.Add(new SqlParameter("@MEMO", taskStop._memo ?? ""));

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

        #region タスク更新画面　タスク削除処理
        public void Task_Delete(Task taskDelete)
        {
            // 共通部品を生成
            DataBaceAccess dbAccess = new DataBaceAccess();

            // 共通部品からSQLserverとの接続オブジェクトを取得
            SqlConnection connection = dbAccess.GetSqlSvrConnect();
            SqlCommand command = connection.CreateCommand();

            try
            {
                // データベースの接続開始
                connection.Open();

                // SQLの準備
                StringBuilder query = new StringBuilder();

                query.AppendLine("UPDATE TRN_TASK_LIST ");
                query.AppendLine("SET ");
                query.AppendLine("      UPDATE_YMD          = @UPDATE_YMD ");
                query.AppendLine("  ,   TASK_STATUS_CODE    = @TASK_STATUS_CODE ");
                query.AppendLine("  ,   MEMO                = @MEMO ");
                query.AppendLine("WHERE ");
                query.AppendLine("    TASK_NO           = @TASK_NO ");
                query.AppendLine("AND USER_NO           = @USER_NO ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@TASK_NO", taskDelete._taskNo));
                command.Parameters.Add(new SqlParameter("@USER_NO", taskDelete._userNo));
                command.Parameters.Add(new SqlParameter("@UPDATE_YMD", taskDelete._updateYmd));
                command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", taskDelete._taskStatusCode._code));
                command.Parameters.Add(new SqlParameter("@MEMO", taskDelete._memo ?? ""));

                // SQLの実行
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                throw;
            }
            finally
            {
                // データベースの接続終了
                connection.Close();
            }
        }
        #endregion
    }
}
