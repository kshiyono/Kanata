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
        public User LogIn_User_Select(string userId, string password)
        {
            // 戻り値用インスタンス生成(先頭に定義しないほうがいいの？)
            User loginUser = null;

            DataBaceAccess dbAccess = new DataBaceAccess();
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
                command.Parameters.Add(new SqlParameter("@USER_ID", userId));

                // 初回ログイン時はパスワードを条件とする。
                if (!string.IsNullOrEmpty(password))
                {
                    query.AppendLine("  AND PASSWORD =   @PASSWORD ");
                    command.Parameters.Add(new SqlParameter("@PASSWORD", password));
                }

                command.CommandText = query.ToString();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loginUser = new User
                    (
                       (string)reader.GetValue(0),
                       (string)reader.GetValue(1),
                       (string)reader.GetValue(2),
                       (int)reader.GetValue(3),
                       (int)reader.GetValue(4)
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
                connection.Close();
            }

            return loginUser;
        }
    }
}