using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Common
{
    public class DataBaceAccess
    {
        // 接続文字列の取得
        private readonly string constr = ConfigurationManager.AppSettings["sqlsvr"];

        // SQLserverとの接続オブジェクト
        private SqlConnection connection;

        #region　SQLserverとの接続を実施するメソッド
        /// <summary>
        /// SQLserver接続インスタンスを作成する。
        /// </summary>
        /// <param name="">パラメータ無し</param>
        /// <returns>接続インスタンス</returns>
        public SqlConnection GetSqlSvrConnect()
        {
            try
            {
                connection = new SqlConnection(constr);
                SqlCommand command = connection.CreateCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return connection;
        }
        #endregion
    }
}
