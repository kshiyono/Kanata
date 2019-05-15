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
            List<Task> taskList = null;
            Task first_Task = null;

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
                query.AppendLine("  , T_TL.TODO_YMD ");
                query.AppendLine("  , T_TL.TASK_STATUS_CODE ");
                query.AppendLine("  , M_TSC.TASK_STATUS_NAME ");
                query.AppendLine("  , T_TL.TASK_KIND_CODE ");
                query.AppendLine("  , M_UTKC.TASK_KIND_NAME ");
                query.AppendLine("  , T_TL.TASK_GROUP_CODE ");
                query.AppendLine("  , M_UTGC.TASK_GROUP_NAME ");
                query.AppendLine("  , T_TL.TASK_NAME ");
                query.AppendLine("  , T_TL.PLAN_TIME ");
                query.AppendLine("  , T_TL.RESULT_TIME ");
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
                    first_Task = new Task
                    {
                        TASK_NO             = (string)reader.GetValue(0),
                        USER_NO             = (string)reader.GetValue(1),
                        CREATE_YMD          = (DateTime)reader.GetValue(2),
                        TODO_YMD            = (DateTime)reader.GetValue(3),
                        TASK_STATUS_CODE    = (string)reader.GetValue(4),
                        TASK_STATUS_NAME    = (string)reader.GetValue(5),
                        TASK_KIND_CODE      = (string)reader.GetValue(6),
                        TASK_KIND_NAME      = (string)reader.GetValue(7),
                        TASK_GROUP_CODE     = (string)reader.GetValue(8),
                        TASK_GROUP_NAME     = (string)reader.GetValue(9),
                        TASK_NAME           = (string)reader.GetValue(10),
                        PLAN_TIME           = (TimeSpan)reader.GetValue(11),
                        RESULT_TIME         = (TimeSpan)reader.GetValue(12)
                    };

                    taskList.Add(first_Task);
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
        public void Task_Insert(Task task_Add)
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
                query.AppendLine("  , TASK_STATUS_CODE ");
                query.AppendLine("  , TASK_KIND_CODE ");
                query.AppendLine("  , TASK_GROUP_CODE ");
                query.AppendLine("  , TASK_NAME ");
                query.AppendLine("  , PLAN_TIME ");
                query.AppendLine("  , RESULT_TIME ");
                query.AppendLine(") ");
                query.AppendLine("VALUES ");
                query.AppendLine("( ");
                query.AppendLine("    @TASK_NO ");
                query.AppendLine("  , @USER_NO ");
                query.AppendLine("  , @CREATE_YMD ");
                query.AppendLine("  , NULL ");
                query.AppendLine("  , @TODO_YMD ");
                query.AppendLine("  , @TASK_STATUS_CODE ");
                query.AppendLine("  , @TASK_KIND_CODE ");
                query.AppendLine("  , @TASK_GROUP_CODE ");
                query.AppendLine("  , @TASK_NAME ");
                query.AppendLine("  , @PLAN_TIME ");
                query.AppendLine("  , '00:00' ");
                query.AppendLine(") ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@TASK_NO", this.Get_TaskNo_Max(task_Add.USER_NO)));
                command.Parameters.Add(new SqlParameter("@USER_NO", task_Add.USER_NO));
                command.Parameters.Add(new SqlParameter("@CREATE_YMD", task_Add.CREATE_YMD));
                command.Parameters.Add(new SqlParameter("@TODO_YMD", task_Add.TODO_YMD));
                command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", task_Add.TASK_STATUS_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", task_Add.TASK_KIND_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", task_Add.TASK_GROUP_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_NAME", task_Add.TASK_NAME));
                command.Parameters.Add(new SqlParameter("@PLAN_TIME", task_Add.PLAN_TIME));

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
        public Task LoadForm_Task_Select(string task_No, string login_User_No)
        {
            // タスクインスタンス生成
            Task task_Return = null;

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
                query.AppendLine("  , T_TL.TODO_YMD ");
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
                command.Parameters.Add(new SqlParameter("@TASK_NO", task_No));
                query.AppendLine("  AND T_TL.USER_NO                =   @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", login_User_No));

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read() == true)
                { 
                    // 値のセット
                    task_Return = new Task
                    {
                        TASK_NO = (string)reader.GetValue(0),
                        USER_NO = (string)reader.GetValue(1),
                        CREATE_YMD = (DateTime)reader.GetValue(2),
                        TODO_YMD = (DateTime)reader.GetValue(3),
                        TASK_STATUS_CODE = (string)reader.GetValue(4),
                        TASK_STATUS_NAME = (string)reader.GetValue(5),
                        TASK_KIND_CODE = (string)reader.GetValue(6),
                        TASK_KIND_NAME = (string)reader.GetValue(7),
                        TASK_GROUP_CODE = (string)reader.GetValue(8),
                        TASK_GROUP_NAME = (string)reader.GetValue(9),
                        TASK_NAME = (string)reader.GetValue(10),
                        PLAN_TIME = (TimeSpan)reader.GetValue(11),
                        RESULT_TIME = (TimeSpan)reader.GetValue(12),
                        MEMO = (reader.GetValue(13) ?? "").ToString()
                    };
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

            return task_Return;
        }
        #endregion

        #region タスク更新画面　タスク更新処理
        public void Task_Update(Task task_Update)
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
                command.Parameters.Add(new SqlParameter("@TASK_NO", task_Update.TASK_NO));
                command.Parameters.Add(new SqlParameter("@USER_NO", task_Update.USER_NO));
                command.Parameters.Add(new SqlParameter("@UPDATE_YMD", task_Update.UPDATE_YMD));
                command.Parameters.Add(new SqlParameter("@TODO_YMD", task_Update.TODO_YMD));
                command.Parameters.Add(new SqlParameter("@FINISHED_YMD", task_Update.FINISHED_YMD));
                command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", task_Update.TASK_STATUS_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", task_Update.TASK_KIND_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", task_Update.TASK_GROUP_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_NAME", task_Update.TASK_NAME));
                command.Parameters.Add(new SqlParameter("@PLAN_TIME", task_Update.PLAN_TIME));
                command.Parameters.Add(new SqlParameter("@RESULT_TIME", task_Update.RESULT_TIME));
                command.Parameters.Add(new SqlParameter("@MEMO", task_Update.MEMO ?? ""));

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
        public void Task_Stop(Task task_Stop)
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
                query.AppendLine("      RESULT_TIME         = @RESULT_TIME ");
                query.AppendLine("  ,   MEMO                = @MEMO ");
                query.AppendLine("WHERE ");
                query.AppendLine("    TASK_NO           = @TASK_NO ");
                query.AppendLine("AND USER_NO           = @USER_NO ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@TASK_NO", task_Stop.TASK_NO));
                command.Parameters.Add(new SqlParameter("@USER_NO", task_Stop.USER_NO));
                command.Parameters.Add(new SqlParameter("@RESULT_TIME", task_Stop.RESULT_TIME));
                command.Parameters.Add(new SqlParameter("@MEMO", task_Stop.MEMO ?? ""));

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

        #region タスク更新画面　タスク削除処理
        public void Task_Delete(Task task_Delete)
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
                query.AppendLine("      TASK_STATUS_CODE    = @TASK_STATUS_CODE ");
                query.AppendLine("  ,   MEMO                = @MEMO ");
                query.AppendLine("WHERE ");
                query.AppendLine("    TASK_NO           = @TASK_NO ");
                query.AppendLine("AND USER_NO           = @USER_NO ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@TASK_NO", task_Delete.TASK_NO));
                command.Parameters.Add(new SqlParameter("@USER_NO", task_Delete.USER_NO));
                command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", task_Delete.TASK_STATUS_CODE));
                command.Parameters.Add(new SqlParameter("@MEMO", task_Delete.MEMO ?? ""));

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
