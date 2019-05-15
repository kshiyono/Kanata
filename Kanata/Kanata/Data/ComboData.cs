using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// コンボボックスクラス
    /// </summary>
    public class ComboData
    {
        /*★★TODO 重複記述のリファクタリング ポリモーフィズム記述★★*/

        #region タスク種別一覧取得処理
        /// <summary>
        /// タスク種別一覧を取得する。
        /// </summary>
        /// <param name="logInUserNo">ログインユーザーNo</param>
        /// <returns>タスク種別一覧</returns>
        public DataTable TaskKindCodeTable_Select(string logInUserNo)
        {
            // タスク種別インスタンス生成
            DataTable taskKindTable = new DataTable();

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
                query.AppendLine("      TASK_KIND_CODE ");
                query.AppendLine("    , TASK_KIND_NAME ");
                query.AppendLine("FROM MST_USER_TASK_KIND_CODE ");
                query.AppendLine("WHERE　USER_NO     =   @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", logInUserNo));
                query.AppendLine("ORDER BY TASK_KIND_CODE ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(taskKindTable);
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

            return taskKindTable;
        }

        /// <summary>
        /// タスク種別一覧を取得する。
        /// </summary>
        /// <param name="logInUserNo">ログインユーザーNo</param>
        /// <returns>タスク種別一覧</returns>
        public DataTable TaskKindCodeTable(string logInUserNo)
        {
            // タスク種別インスタンス生成
            DataTable taskKindTable = new DataTable();

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
                query.AppendLine("      TASK_KIND_CODE ");
                query.AppendLine("    , TASK_KIND_NAME ");
                query.AppendLine("FROM MST_USER_TASK_KIND_CODE ");
                query.AppendLine("WHERE USER_NO         = @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", logInUserNo));
                // タスク追加用コンボボックスは「全て」(00)は除外する。
                query.AppendLine("  AND TASK_KIND_CODE <> @TASK_KIND_CODE ");
                command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", Constants.TaskKind.ALL_00));
                query.AppendLine("ORDER BY TASK_KIND_CODE ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(taskKindTable);
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

            return taskKindTable;
        }
        #endregion

        #region タスクステータス一覧取得処理
        /// <summary>
        /// タスクステータス一覧を取得する。
        /// </summary>
        /// <param name="">パラメータ無し</param>
        /// <returns>タスクステータス一覧</returns>
        public DataTable TaskStatusCodeTable_Select()
        {
            // タスク種別インスタンス生成
            DataTable taskStatusTable = new DataTable();

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
                query.AppendLine("      TASK_STATUS_CODE ");
                query.AppendLine("    , TASK_STATUS_NAME ");
                query.AppendLine("FROM MST_TASK_STATUS_CODE ");
                query.AppendLine("ORDER BY TASK_STATUS_CODE ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(taskStatusTable);
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

            return taskStatusTable;
        }

        /// <summary>
        /// タスクステータス一覧を取得する。
        /// </summary>
        /// <param name="">パラメータ無し</param>
        /// <returns>タスクステータス一覧</returns>
        public DataTable TaskStatusCodeTable()
        {
            // タスク種別インスタンス生成
            DataTable taskStatusTable = new DataTable();

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
                query.AppendLine("      TASK_STATUS_CODE ");
                query.AppendLine("    , TASK_STATUS_NAME ");
                query.AppendLine("FROM MST_TASK_STATUS_CODE ");
                query.AppendLine("WHERE TASK_STATUS_CODE <> '00' ");
                query.AppendLine("ORDER BY TASK_STATUS_CODE ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(taskStatusTable);
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

            return taskStatusTable;
        }
        #endregion

        #region タスクグループ

        #region タスクグループ一覧取得
        /// <summary>
        /// タスクステータス一覧を取得する。
        /// </summary>
        /// <param name="logInUserNo">ログインユーザーNo</param>
        /// <returns>タスクグループ一覧</returns>
        public DataTable TaskGroupCodeTable_Select(string logInUserNo)
        {
            // タスク種別インスタンス生成
            DataTable taskGroupTable = new DataTable();

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
                query.AppendLine("      TASK_GROUP_CODE ");
                query.AppendLine("    , TASK_GROUP_NAME ");
                query.AppendLine("FROM MST_USER_TASK_GROUP_CODE ");
                query.AppendLine("WHERE　USER_NO     =   @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", logInUserNo));
                query.AppendLine("ORDER BY TASK_GROUP_CODE ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(taskGroupTable);
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

            return taskGroupTable;
        }

        /// <summary>
        /// タスクステータス一覧を取得する。
        /// </summary>
        /// <param name="logInUserNo">ログインユーザーNo</param>
        /// <returns>タスクグループ一覧</returns>
        public DataTable TaskGroupCodeTable(string logInUserNo)
        {
            // タスク種別インスタンス生成
            DataTable taskGroupTable = new DataTable();

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
                query.AppendLine("      TASK_GROUP_CODE ");
                query.AppendLine("    , TASK_GROUP_NAME ");
                query.AppendLine("FROM MST_USER_TASK_GROUP_CODE ");
                query.AppendLine("WHERE USER_NO         =  @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", logInUserNo));
                // タスク追加用コンボボックスは「全て」(00)は除外する。
                query.AppendLine("  AND TASK_GROUP_CODE <> @TASK_GROUP_CODE ");
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", Constants.TaskGroup.ALL_00));
                query.AppendLine("ORDER BY TASK_GROUP_CODE ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(taskGroupTable);
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

            return taskGroupTable;
        }
        #endregion

        #region タスクグループ更新
        /// <summary>
        /// タスクグループ名を更新する。
        /// </summary>
        /// <param name="logInUserNo">ログインユーザーNo</param>
        /// <param name="taskGroupCode">タスクグループコード</param>
        /// <param name="taskGroupMei">タスクグループ名</param>
        /// <returns></returns>
        public void TaskGroupName_Update(string logInUserNo, string taskGroupCode, string taskGroupMei)
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

                command.Parameters.Add(new SqlParameter("@TASK_GROUP_NAME", taskGroupMei));
                command.Parameters.Add(new SqlParameter("@USER_NO", logInUserNo));
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", taskGroupCode));

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
