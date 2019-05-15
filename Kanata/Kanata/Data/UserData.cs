using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Common;
using Entity;
using System.Text;

namespace Data
{
    public class UserData
    {
        public User LogIn_User_Select(string user_Id, string password)
        {
            User login_User = null;

            // 共通部品を生成
            DataBaceAccess dbAccess = new DataBaceAccess();

            // 共通部品からSQLserverとの接続オブジェクトを取得
            SqlConnection connection = dbAccess.GetSqlSvrConnect();

            SqlCommand command = connection.CreateCommand();
            DataTable dt = new DataTable();

            try
            {
                connection.Open();
                StringBuilder query = new StringBuilder();

                query.AppendLine("SELECT");
                query.AppendLine("      USER_NO ");
                query.AppendLine("    , USER_ID ");
                query.AppendLine("    , USER_NAME ");
                query.AppendLine("    , OVER_TIME ");
                query.AppendLine("    , PERCENT_TIME ");
                query.AppendLine("FROM MST_USER_INFO ");
                query.AppendLine("WHERE　USER_ID     =   @USER_ID ");
                command.Parameters.Add(new SqlParameter("@USER_ID", user_Id));

                // 初回ログイン時はパスワードを条件とする。
                if (!string.IsNullOrEmpty(password))
                {
                    query.AppendLine("  AND PASSWORD =   @PASSWORD ");
                    command.Parameters.Add(new SqlParameter("@PASSWORD", password));
                }

                // SQLの実行
                command.CommandText = query.ToString();
                SqlDataReader reader = command.ExecuteReader();

                // 結果を表示します。
                while (reader.Read())
                {
                    // ユーザーインスタンスの生成
                    login_User = new User
                    {
                        USER_NO = (string)reader.GetValue(0),
                        USER_ID = (string)reader.GetValue(1),
                        USER_NAME = (string)reader.GetValue(2),
                        OVER_TIME = (int)reader.GetValue(3),
                        PERCENT_TIME = (int)reader.GetValue(4)
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

            return login_User;
        }
    }
}