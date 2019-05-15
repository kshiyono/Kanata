using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data 
{
    public class FlgData
    {
        #region 条件取得処理
        public virtual Flg Select_FlgSelect(string login_User_No)
        {
            // タスクインスタンス生成
            Flg flg = null;

            // 戻り値
            return flg;
        }
        #endregion

        #region 条件更新処理
        public virtual void Update_FlgSelect(Flg flg)
        {
        }
        #endregion
    }

    public class TaskFlgData : FlgData
    {
        #region タスク検索条件取得処理
        public override Flg Select_FlgSelect(string login_User_No)
        {
            // タスクインスタンス生成
            Flg taskFlg_Return = null;

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
                query.AppendLine("    USER_NO ");
                query.AppendLine("  , TASK_STATUS_CODE ");
                query.AppendLine("  , TASK_KIND_CODE ");
                query.AppendLine("  , TASK_GROUP_CODE ");
                // ユーザータスク検索初期設定マスタ
                query.AppendLine("FROM MST_USER_TASK_SETTING_SELECT ");
                // 検索条件
                query.AppendLine("WHERE USER_NO                     =   @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", login_User_No));

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read() == true)
                {
                    // 値のセット
                    taskFlg_Return = new Flg_Task_Select
                    {
                        USER_NO             = (string)reader.GetValue(0),
                        TASK_STATUS_CODE    = (string)reader.GetValue(1),
                        TASK_KIND_CODE      = (string)reader.GetValue(2),
                        TASK_GROUP_CODE     = (string)reader.GetValue(3),
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

            return taskFlg_Return;
        }
        #endregion

        #region タスク検索条件更新処理
        public override void Update_FlgSelect(Flg taskFlg_Select)
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

                query.AppendLine("UPDATE MST_USER_TASK_SETTING_SELECT ");
                query.AppendLine("SET ");
                query.AppendLine("      TASK_STATUS_CODE    = @TASK_STATUS_CODE ");
                query.AppendLine("  ,   TASK_KIND_CODE      = @TASK_KIND_CODE ");
                query.AppendLine("  ,   TASK_GROUP_CODE     = @TASK_GROUP_CODE ");
                query.AppendLine("WHERE ");
                query.AppendLine("    USER_NO               = @USER_NO ");

                command.CommandText = query.ToString();

                // パラメーターの設定
                command.Parameters.Add(new SqlParameter("@USER_NO", taskFlg_Select.USER_NO));
                command.Parameters.Add(new SqlParameter("@TASK_STATUS_CODE", taskFlg_Select.TASK_STATUS_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", taskFlg_Select.TASK_KIND_CODE));
                command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", taskFlg_Select.TASK_GROUP_CODE));

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
