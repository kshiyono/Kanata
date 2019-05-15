using Common;
using Data;
using Entity;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Form
{
    public partial class Form_TaskUpdate : System.Windows.Forms.Form
    {
        // ストップウォッチ判断用インスタンス
        Boolean sw_Switch = false;

        // ストップウォッチインスタンス
        Stopwatch myStopWatch = new Stopwatch();

        // 時間計測の初期値インスタンス
        TimeSpan start_ResultTime;

        #region タスク更新画面初期処理
        public Form_TaskUpdate(Task taskUpdate)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 備忘録：ユーザーNoの設定は事前に行っておく。
            //         コンボボックスのデータ検索時に「ユーザーNo」を利用するため。
            TextBox_UserNo_Update.Text = taskUpdate.USER_NO;

            #region　コンボボックスデータ設定処理

            // 備忘録：コンボボックスのデータ検索時に「ユーザーNo」が必要であるため、
            //         画面のユーザーNoテキストボックスに値を入れてから検索する。
            // 備忘録：タスク検索処理はコンボボックスに値設定後に実施する。
            //         検索条件に「全て」(00)を利用するため。

            // コンボデータ用インスタンス生成
            ComboData comboData = new ComboData();

            // タスク種別(SELECT)コンボボックス初期設定
            ComboBox_KindName_Update.DataSource         = comboData.TaskKindCodeTable(TextBox_UserNo_Update.Text);
            ComboBox_KindName_Update.DisplayMember      = Constants.TaskKind.TASK_KIND_NAME;
            ComboBox_KindName_Update.ValueMember        = Constants.TaskKind.TASK_KIND_CODE;

            // タスクステータスコンボボックス初期設定
            ComboBox_StatusName_Update.DataSource       = comboData.TaskStatusCodeTable_Select();
            ComboBox_StatusName_Update.DisplayMember    = Constants.TaskStatus.TASK_STATUS_NAME;
            ComboBox_StatusName_Update.ValueMember      = Constants.TaskStatus.TASK_STATUS_CODE;

            // タスクグループコンボボックス初期設定
            ComboBox_GroupName_Update.DataSource        = comboData.TaskGroupCodeTable(TextBox_UserNo_Update.Text);
            ComboBox_GroupName_Update.DisplayMember     = Constants.TaskGroup.TASK_GROUP_NAME;
            ComboBox_GroupName_Update.ValueMember       = Constants.TaskGroup.TASK_GROUP_CODE;

            // コンボボックスは手入力不可
            ComboBox_KindName_Update.DropDownStyle      = ComboBoxStyle.DropDownList;
            ComboBox_StatusName_Update.DropDownStyle    = ComboBoxStyle.DropDownList;
            ComboBox_GroupName_Update.DropDownStyle     = ComboBoxStyle.DropDownList;

            #endregion

            #region 初期表示処理
            TextBox_TodoDay_Update.Text      = taskUpdate.TODO_YMD.ToString().Substring(0, 10);
            TextBox_PlanTime_Update.Text     = taskUpdate.PLAN_TIME.ToString();
            ComboBox_StatusName_Update.Text  = taskUpdate.TASK_STATUS_NAME;
            ComboBox_KindName_Update.Text    = taskUpdate.TASK_KIND_NAME;
            ComboBox_GroupName_Update.Text   = taskUpdate.TASK_GROUP_NAME;
            TextBox_TaskName_Update.Text     = taskUpdate.TASK_NAME;
            Label_ResultTime_Update.Text     = taskUpdate.RESULT_TIME.ToString(@"hh\:mm");
            TextBox_ResultTimeReadOnly_Update.Text   = taskUpdate.RESULT_TIME.ToString(@"hh\:mm");
            TextBox_TaskNo_Update.Text       = taskUpdate.TASK_NO;
            TextBox_Memo_Update.Text         = taskUpdate.MEMO;
            #endregion

            // ユーザーNoは入力不可
            TextBox_UserNo_Update.Enabled               = false;
            TextBox_UserNo_Update.BackColor             = Color.White;
            TextBox_UserNo_Update.ForeColor             = Color.Black;

            // 非表示項目の設定
            TextBox_TaskNo_Update.Visible               = false;
            TextBox_StatusCode_Update.Visible           = false;
            TextBox_KindCode_Update.Visible             = false;
            TextBox_GroupCode_Update.Visible            = false;
            TextBox_ResultTimeReadOnly_Update.Visible   = false;
            TextBox_UserNo_Update.Visible               = false;

            // 時刻ラベルの初期表示
            Label_ResultTime_Update.Font                = new Font(Label_ResultTime_Update.Font.FontFamily, 37);
            //Label_ResultTime_Update.Text              = myStopWatch.Elapsed.ToString();

            // 表示更新用タイマーの間隔プロパティ
            // 1秒ごとに更新
            Timer_ResultTime_Update.Interval            = 1000;
        }
        #endregion

        #region コンボボックス選択時処理

        #region タスクステータスコンボボックス選択時処理
        private void ComboBox_StatusName_Update_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_StatusName_Update.SelectedIndex != -1)
                TextBox_StatusCode_Update.Text = ComboBox_StatusName_Update.SelectedValue.ToString();
        }
        #endregion

        #region タスク種別コンボボックス選択時処理
        private void ComboBox_KindName_Update_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_KindName_Update.SelectedIndex != -1)
                TextBox_KindCode_Update.Text = ComboBox_KindName_Update.SelectedValue.ToString();
        }
        #endregion

        #region タスクグループコンボボックス選択時処理
        private void ComboBox_GroupName_Update_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_GroupName_Update.SelectedIndex != -1)
                TextBox_GroupCode_Update.Text = ComboBox_GroupName_Update.SelectedValue.ToString();
        }
        #endregion

        #endregion

        /*★★TODO 重複記述のリファクタリング ストップウォッチ開始／終了処理★★*/
        #region 開始／中断ボタン押下処理
        private void Button_Start_Update_Click(object sender, EventArgs e)
        {

            #region 開始ボタン押下処理
            if (sw_Switch == false)
            {
                // 計測開始
                myStopWatch.Start();

                // 表示更新タイマー開始
                Timer_ResultTime_Update.Start();

                // スイッチON
                sw_Switch = true;

                // ボタンの表示テキストを変更
                Button_Start_Update.Text = "中断";

                // 時間計測の初期値
                start_ResultTime = TimeSpan.Parse(TextBox_ResultTimeReadOnly_Update.Text);

                #region ボタンの状態設定
                TextBox_TodoDay_Update.Enabled       = false;
                TextBox_PlanTime_Update.Enabled      = false;
                ComboBox_KindName_Update.Enabled     = false;
                ComboBox_GroupName_Update.Enabled    = false;
                TextBox_TaskName_Update.Enabled      = false;
                TextBox_ResultTime_Update.Enabled    = false;
                ComboBox_StatusName_Update.Enabled   = false;
                Button_Update_Update.Enabled         = false;
                Button_Delete_Update.Enabled         = false;
                Button_Return_Update.Enabled         = false;
                TextBox_UserNo_Update.BackColor      = Color.White;
                TextBox_UserNo_Update.ForeColor      = Color.Black;
                TextBox_TodoDay_Update.BackColor     = Color.White;
                TextBox_TodoDay_Update.ForeColor     = Color.Black;
                TextBox_PlanTime_Update.BackColor    = Color.White;
                TextBox_PlanTime_Update.ForeColor    = Color.Black;
                TextBox_TaskName_Update.BackColor    = Color.White;
                TextBox_TaskName_Update.ForeColor    = Color.Black;
                TextBox_ResultTime_Update.BackColor  = Color.White;
                TextBox_ResultTime_Update.ForeColor  = Color.Black;
                #endregion
            }
            #endregion

            #region 中断ボタン押下処理
            else if (sw_Switch == true)
            {
                //計測終了
                myStopWatch.Stop();

                //表示固定
                Timer_ResultTime_Update.Stop();

                //スイッチOFF
                sw_Switch = false;

                // ボタンの表示テキストを変更
                Button_Start_Update.Text = "開始";

                #region 中断状態のタスクを更新
                // 中断タスク用インスタンス生成
                Task task_Stop = new Task
                {
                    // 値の代入
                    TASK_NO = TextBox_TaskNo_Update.Text,
                    USER_NO = TextBox_UserNo_Update.Text,
                    RESULT_TIME = TimeSpan.Parse(Label_ResultTime_Update.Text),
                    MEMO = TextBox_Memo_Update.Text
                };

                // タスクデータインスタンス生成
                TaskData taskData = new Data.TaskData();

                // タスク更新
                taskData.Task_Stop(task_Stop);
                #endregion

                #region インスタンス状態設定
                TextBox_TodoDay_Update.Enabled      = true;
                TextBox_PlanTime_Update.Enabled     = true;
                ComboBox_KindName_Update.Enabled    = true;
                ComboBox_GroupName_Update.Enabled   = true;
                TextBox_TaskName_Update.Enabled     = true;
                ComboBox_StatusName_Update.Enabled  = true;
                TextBox_ResultTime_Update.Enabled   = true;
                Button_Update_Update.Enabled        = true;
                Button_Delete_Update.Enabled        = true;
                Button_Return_Update.Enabled        = true;
                TextBox_UserNo_Update.BackColor     = Color.White;
                TextBox_UserNo_Update.ForeColor     = Color.Black;
                TextBox_TodoDay_Update.BackColor    = Color.White;
                TextBox_TodoDay_Update.ForeColor    = Color.Black;
                TextBox_PlanTime_Update.BackColor   = Color.White;
                TextBox_PlanTime_Update.ForeColor   = Color.Black;
                TextBox_TaskName_Update.BackColor   = Color.White;
                TextBox_TaskName_Update.ForeColor   = Color.Black;
                TextBox_ResultTime_Update.BackColor = Color.White;
                TextBox_ResultTime_Update.ForeColor = Color.Black;
                #endregion
            }
            #endregion
        }
        #endregion

        #region タイマーラベル更新処理
        private void Timer_ResultTime_Update_Tick(object sender, EventArgs e)
        {
            Label_ResultTime_Update.Text = (start_ResultTime + myStopWatch.Elapsed).ToString(@"hh\:mm");
        }
        #endregion

        #region 戻るボタンを押下処理
        private void Button_Return_Update_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
        #endregion

        #region 更新ボタン押下処理
        private void Button_Update_Update_Click(object sender, EventArgs e)
        {
            #region エラー制御
            // 期限日が空の場合、エラー
            if (TextBox_TodoDay_Update.Text == "")
            {
                MessageBox.Show("期限日を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 期限日が日付型に変換できない場合はエラー
            if (!DateTime.TryParse(TextBox_TodoDay_Update.Text, out DateTime dt))
            {
                MessageBox.Show("期限日には日付を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 予定時間が空の場合、エラー
            if (TextBox_PlanTime_Update.Text == "")
            {
                MessageBox.Show("予定時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 予定時間が時間型に変換できない場合はエラー
            if (!TimeSpan.TryParse(TextBox_PlanTime_Update.Text, out TimeSpan ts_Plan))
            {
                MessageBox.Show("予定時間には時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 実績時間が時間型に変換できない場合はエラー
            if (TextBox_ResultTime_Update.Text != "" && TextBox_ResultTime_Update.Text != null && !TimeSpan.TryParse(TextBox_ResultTime_Update.Text, out TimeSpan ts_Result))
            {
                MessageBox.Show("実績時間には時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 状況が空の場合、エラー
            if (ComboBox_StatusName_Update.Text == "")
            {
                MessageBox.Show("種別を選択してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 種別が空の場合、エラー
            if (ComboBox_KindName_Update.Text == "")
            {
                MessageBox.Show("種別を選択してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }
            
            // タスク内容が空の場合、エラー
            if (TextBox_TaskName_Update.Text == "")
            {
                MessageBox.Show("タスク内容を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // タスク内容が100バイトを超える場合はエラー
            if (Encoding.GetEncoding("Shift_JIS").GetByteCount(TextBox_TaskName_Update.Text) > 100)
            {
                MessageBox.Show("タスク内容は最大全角50文字です。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // メモが400バイトを超える場合はエラー
            if (Encoding.GetEncoding("Shift_JIS").GetByteCount(TextBox_Memo_Update.Text) > 400)
            {
                MessageBox.Show("メモは最大全角200文字です。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }
            #endregion

            #region 更新確認処理
            DialogResult dr_YESNO = MessageBox.Show("タスクを更新しますか？",
                                "更新",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

            // 「はい」を選択した場合、画面を閉じる。
            if (dr_YESNO == DialogResult.No)
                return;
            #endregion

            // 更新タスク用インスタンス生成
            Task task_Update = new Task
           {
                // 値の代入
                TASK_NO             = TextBox_TaskNo_Update.Text,
                USER_NO             = TextBox_UserNo_Update.Text,
                UPDATE_YMD          = DateTime.Today,
                TODO_YMD            = DateTime.Parse(TextBox_TodoDay_Update.Text),
                FINISHED_YMD        = DateTime.Today,
                TASK_STATUS_CODE    = TextBox_StatusCode_Update.Text,
                TASK_KIND_CODE      = TextBox_KindCode_Update.Text,
                TASK_GROUP_CODE     = TextBox_GroupCode_Update.Text,
                TASK_NAME           = TextBox_TaskName_Update.Text,
                PLAN_TIME           = TimeSpan.Parse(TextBox_PlanTime_Update.Text),
                // 実績テキストボックスが空文字かNULLの場合はラベルを採用
                RESULT_TIME         = TimeSpan.Parse(TextBox_ResultTime_Update.Text == ""
                                        ? Label_ResultTime_Update.Text
                                        : TextBox_ResultTime_Update.Text ?? Label_ResultTime_Update.Text),
                MEMO                = TextBox_Memo_Update.Text
            };

            // タスクデータインスタンス生成
            TaskData taskData = new Data.TaskData();

            // タスク更新
            taskData.Task_Update(task_Update);

            #region 更新通知処理
            DialogResult dr_OK = MessageBox.Show("更新が完了しました。",
                                "更新",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            #endregion

            // 更新画面をクローズ
            this.Close();
            return;
        }
        #endregion

        #region 完了ボタン押下処理
        private void Button_End_Update_Click(object sender, EventArgs e)
        {
            #region エラー制御
            // 期限日が空の場合、エラー
            if (TextBox_TodoDay_Update.Text == "")
            {
                MessageBox.Show("期限日を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 期限日が日付型に変換できない場合はエラー
            if (!DateTime.TryParse(TextBox_TodoDay_Update.Text, out DateTime dt))
            {
                MessageBox.Show("期限日には日付を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 予定時間が空の場合、エラー
            if (TextBox_PlanTime_Update.Text == "")
            {
                MessageBox.Show("予定時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 予定時間が時間型に変換できない場合はエラー
            if (!TimeSpan.TryParse(TextBox_PlanTime_Update.Text, out TimeSpan ts))
            {
                MessageBox.Show("予定時間には時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 実績時間が時間型に変換できない場合はエラー
            if (TextBox_ResultTime_Update.Text != "" && TextBox_ResultTime_Update.Text != null && !TimeSpan.TryParse(TextBox_ResultTime_Update.Text, out TimeSpan ts_Result))
            {
                MessageBox.Show("実績時間には時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 状況が空の場合、エラー
            if (ComboBox_StatusName_Update.Text == "")
            {
                MessageBox.Show("種別を選択してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 種別が空の場合、エラー
            if (ComboBox_KindName_Update.Text == "")
            {
                MessageBox.Show("種別を選択してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // タスク内容が空の場合、エラー
            if (TextBox_TaskName_Update.Text == "")
            {
                MessageBox.Show("タスク内容を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // タスク内容が50バイトを超える場合はエラー
            if (Encoding.GetEncoding("Shift_JIS").GetByteCount(TextBox_TaskName_Update.Text) > 100)
            {
                MessageBox.Show("タスク内容は最大50文字です。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }
            #endregion

            #region 更新確認処理
            DialogResult dr_YESNO = MessageBox.Show("タスクを完了しますか？",
                                "完了",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

            // 「いいえ」を選択した場合、画面戻る。
            if (dr_YESNO == DialogResult.No)
                return;
            #endregion

            // 更新タスク用インスタンス生成
            Task task_Update = new Task
            {
                // 値の代入
                TASK_NO = TextBox_TaskNo_Update.Text,
                USER_NO = TextBox_UserNo_Update.Text,
                UPDATE_YMD = DateTime.Today,
                TODO_YMD = DateTime.Parse(TextBox_TodoDay_Update.Text),
                FINISHED_YMD = DateTime.Today,
                // 完了処理時は、ステータスは"10"(済)固定
                TASK_STATUS_CODE = "10",
                TASK_KIND_CODE = TextBox_KindCode_Update.Text,
                TASK_GROUP_CODE = TextBox_GroupCode_Update.Text,
                TASK_NAME = TextBox_TaskName_Update.Text,
                PLAN_TIME = TimeSpan.Parse(TextBox_PlanTime_Update.Text),
                // 実績テキストボックスが空文字かNULLの場合はラベルを採用
                RESULT_TIME = TimeSpan.Parse(TextBox_ResultTime_Update.Text == ""
                                        ? Label_ResultTime_Update.Text
                                        : TextBox_ResultTime_Update.Text ?? Label_ResultTime_Update.Text),
                MEMO = TextBox_Memo_Update.Text
            };

            // タスクデータインスタンス生成
            TaskData taskData = new Data.TaskData();

            // タスク更新
            taskData.Task_Update(task_Update);

            #region 更新通知処理
            DialogResult dr_OK = MessageBox.Show("更新が完了しました。",
                                "完了",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            #endregion

            // 更新画面をクローズ
            this.Close();
            return;

        }
        #endregion

        #region 削除ボタン押下処理
        private void Button_Delete_Update_Click(object sender, EventArgs e)
        {
            #region エラー制御
            // 期限日が空の場合、エラー
            if (TextBox_TodoDay_Update.Text == "")
            {
                MessageBox.Show("期限日を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 期限日が日付型に変換できない場合はエラー
            if (!DateTime.TryParse(TextBox_TodoDay_Update.Text, out DateTime dt))
            {
                MessageBox.Show("期限日には日付を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 予定時間が空の場合、エラー
            if (TextBox_PlanTime_Update.Text == "")
            {
                MessageBox.Show("予定時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 予定時間が時間型に変換できない場合はエラー
            if (!TimeSpan.TryParse(TextBox_PlanTime_Update.Text, out TimeSpan ts))
            {
                MessageBox.Show("予定時間には時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 状況が空の場合、エラー
            if (ComboBox_StatusName_Update.Text == "")
            {
                MessageBox.Show("種別を選択してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 種別が空の場合、エラー
            if (ComboBox_KindName_Update.Text == "")
            {
                MessageBox.Show("種別を選択してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // タスク内容が空の場合、エラー
            if (TextBox_TaskName_Update.Text == "")
            {
                MessageBox.Show("タスク内容を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // タスク内容が50バイトを超える場合はエラー
            if (Encoding.GetEncoding("Shift_JIS").GetByteCount(TextBox_TaskName_Update.Text) > 100)
            {
                MessageBox.Show("タスク内容は最大50文字です。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }
            #endregion

            #region 削除確認処理
            DialogResult dr_YESNO = MessageBox.Show("タスクを削除しますか？",
                                "削除",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

            // 「いいえ」を選択した場合、画面戻る。
            if (dr_YESNO == DialogResult.No)
                return;
            #endregion

            // 削除タスク用インスタンス生成
            Task Task_Delete = new Task
            {
                // 値の代入
                TASK_NO = TextBox_TaskNo_Update.Text,
                USER_NO = TextBox_UserNo_Update.Text,
                // 削除処理時は、ステータスは"20"(削除)固定
                TASK_STATUS_CODE = "20",
                MEMO = TextBox_Memo_Update.Text
            };

            // タスクデータインスタンス生成
            TaskData taskData = new Data.TaskData();

            // タスク更新
            taskData.Task_Delete(Task_Delete);

            #region 更新通知処理
            DialogResult dr_OK = MessageBox.Show("削除が完了しました。",
                                "削除",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            #endregion

            // 更新画面をクローズ
            this.Close();
            return;
        }
        #endregion

    }
}

