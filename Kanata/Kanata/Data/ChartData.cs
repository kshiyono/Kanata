using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ChartData
    {
        #region タスク予実推移データ取得処理
        public DataTable TaskChartTable_Select(Flg_Chart_Select flgChartSelect)
        {
            // タスク種別インスタンス生成
            DataTable taskChartTable = new DataTable();

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

                query.AppendLine("SELECT ");
                query.AppendLine("		REPLACE(FINISHED_YMD, '-', '/') AS FINISHED_YMD ");
                query.AppendLine("	,	FLOOR ( ");
                query.AppendLine("			CONVERT( FLOAT,  ");
                query.AppendLine("					SUM ( ");
                query.AppendLine("					CASE WHEN DIFFERENCE_PERCENT < (-1) * PERCENT_TIME ");
                query.AppendLine("						 THEN 1 ");
                query.AppendLine("						 ELSE 0 ");
                query.AppendLine("					END ");
                query.AppendLine("				) ");
                query.AppendLine("			) ");
                query.AppendLine("			/ ");
                query.AppendLine("			NULLIF(CONVERT( FLOAT, COUNT(1)), 0) ");
                query.AppendLine("			* ");
                query.AppendLine("			100 ");
                query.AppendLine("		) AS MINUS_OVER_PERCENT ");
                query.AppendLine("	,	FLOOR ( ");
                query.AppendLine("			CONVERT( FLOAT,  ");
                query.AppendLine("					SUM ( ");
                query.AppendLine("					CASE WHEN DIFFERENCE_PERCENT > PERCENT_TIME ");
                query.AppendLine("						 THEN 1 ");
                query.AppendLine("						 ELSE 0 ");
                query.AppendLine("					END ");
                query.AppendLine("				) ");
                query.AppendLine("			) ");
                query.AppendLine("			/ ");
                query.AppendLine("			NULLIF(CONVERT( FLOAT, COUNT(1)), 0) ");
                query.AppendLine("			* ");
                query.AppendLine("			100 ");
                query.AppendLine("		) AS PLUS_OVER_PERCENT ");
                query.AppendLine("FROM ");
                query.AppendLine("	( ");
                query.AppendLine("		SELECT ");
                if(flgChartSelect.CHART_INTERVAL == "DAY")
                {
                    query.AppendLine("			T_TL.TASK_NO ");
                    query.AppendLine("		,	T_TL.FINISHED_YMD ");
                }
                else if (flgChartSelect.CHART_INTERVAL == "WEEK")
                {
                    query.AppendLine("			LEFT(T_TL.FINISHED_YMD, 9) AS FINISHED_YMD ");
                }
                else if (flgChartSelect.CHART_INTERVAL == "MONTH")
                {
                    query.AppendLine("		   	LEFT(T_TL.FINISHED_YMD, 7) AS FINISHED_YMD ");
                }
                query.AppendLine("			,	FLOOR (  ");
                query.AppendLine("					CONVERT( FLOAT, DATEDIFF( MINUTE, T_TL.PLAN_TIME, T_TL.RESULT_TIME ) )  ");
                query.AppendLine("					/ ");
                query.AppendLine("					NULLIF( CONVERT( FLOAT, DATEDIFF( MINUTE, 0, T_TL.PLAN_TIME ) ), 0) ");
                query.AppendLine("					* ");
                query.AppendLine("					100 ");
                query.AppendLine("				) AS DIFFERENCE_PERCENT ");
                query.AppendLine("			,	M_UI.PERCENT_TIME ");
                query.AppendLine("		FROM TRN_TASK_LIST T_TL ");
                query.AppendLine("		INNER JOIN MST_USER_INFO M_UI ");
                query.AppendLine("			ON	T_TL.USER_NO = M_UI.USER_NO ");

                #region 検索条件
                // ユーザーNo
                query.AppendLine("		WHERE T_TL.USER_NO                =   @USER_NO ");
                command.Parameters.Add(new SqlParameter("@USER_NO", flgChartSelect.USER_NO));

                // タスクステータスコード
                query.AppendLine("		AND T_TL.TASK_STATUS_CODE         =   '10' ");

                // 検索日付条件
                if (flgChartSelect.DAY_FROM != new DateTime(0))
                {
                    query.AppendLine("  AND T_TL.TODO_YMD                 >=   @DAY_FROM ");
                    command.Parameters.Add(new SqlParameter("@DAY_FROM", flgChartSelect.DAY_FROM));
                }
                if (flgChartSelect.DAY_TO != new DateTime(0))
                {
                    query.AppendLine("  AND T_TL.TODO_YMD                 <=   @DAY_TO ");
                    command.Parameters.Add(new SqlParameter("@DAY_TO", flgChartSelect.DAY_TO));
                }
                // タスク種別コード条件
                if (flgChartSelect.TASK_KIND_CODE != Constants.TaskKind.ALL_00)
                {
                    query.AppendLine("  AND T_TL.TASK_KIND_CODE            =   @TASK_KIND_CODE ");
                    command.Parameters.Add(new SqlParameter("@TASK_KIND_CODE", flgChartSelect.TASK_KIND_CODE));
                }
                // タスクグループコード条件
                if (flgChartSelect.TASK_GROUP_CODE != Constants.TaskGroup.ALL_00)
                {
                    query.AppendLine("  AND T_TL.TASK_GROUP_CODE           =   @TASK_GROUP_CODE ");
                    command.Parameters.Add(new SqlParameter("@TASK_GROUP_CODE", flgChartSelect.TASK_GROUP_CODE));
                }
                #endregion

                if (flgChartSelect.CHART_INTERVAL == "DAY")
                {
                    query.AppendLine("		GROUP BY FINISHED_YMD, TASK_NO, PLAN_TIME, RESULT_TIME, PERCENT_TIME ");
                }
                else if (flgChartSelect.CHART_INTERVAL == "WEEK")
                {
                    query.AppendLine("		GROUP BY LEFT(T_TL.FINISHED_YMD, 9), TASK_NO, PLAN_TIME, RESULT_TIME, PERCENT_TIME ");
                }
                else if (flgChartSelect.CHART_INTERVAL == "MONTH")
                {
                    query.AppendLine("		GROUP BY LEFT(T_TL.FINISHED_YMD, 7), TASK_NO, PLAN_TIME, RESULT_TIME, PERCENT_TIME ");
                }
                query.AppendLine("	) TASKLIST_FOR_CHART ");
                query.AppendLine("GROUP BY FINISHED_YMD ");
                query.AppendLine("ORDER BY FINISHED_YMD ");

                command.CommandText = query.ToString();

                // SQLの実行
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(taskChartTable);
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

            return taskChartTable;
        }
        #endregion
    }
}
