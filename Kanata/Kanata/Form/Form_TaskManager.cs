using Data;
using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Form
{
    #region タスク管理画面
    /// <summary>
    /// 以下の機能をタブ毎に実装。
    /// 　①タスク一覧管理。タスク予実データ蓄積。
    /// 　②タスク日別サマリ。日別残業予測時間見える化。
    /// 　③タスク予実推移。予実改善度合見える化。
    /// </summary>
    /// <todo> UPD:20190430
    /// 　①実績時間からタスク予定を予測。グループごとを想定。
    /// 　②日別の作業量一覧の実装。
    /// 　③機能②／③確認時、対象タスクを一覧画面表示。
    /// </todo>
    public partial class Form_TaskManager : System.Windows.Forms.Form
    {
        #region 初期表示
        public Form_TaskManager(User loginUser)
        {
            #region 画面共通

            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 備忘録：ユーザーNoの設定は事前に行っておく。
            //         コンボボックスのデータ検索時に「ユーザーNo」を利用するため。
            TextBox_UserNo.Text = loginUser.USER_NO;

            #region　コンボボックス設定処理

            // 備忘録：コンボボックスのデータ検索時に「ユーザーNo」が必要であるため、
            //         画面のユーザーNoテキストボックスに値を入れてから検索する。
            // 備忘録：タスク検索処理はコンボボックスに値設定後に実施する。
            //         検索条件に「全て」(00)を利用するため。

            ComboData comboData = new ComboData();

            // 備忘録：タスク検索系コンボボックス設定
            //         「全て」コード(01)を含んで検索設定を行う。
            //       ※「全て」コード(01)はタスク検索時に条件なしで利用するため。

            // 種別（タスク一覧検索）
            ComboBox_KindName_SelectTaskList.DataSource         = comboData.TaskKindCodeTable_Select(TextBox_UserNo.Text);
            ComboBox_KindName_SelectTaskList.DisplayMember      = Constants.TaskKind.TASK_KIND_NAME;
            ComboBox_KindName_SelectTaskList.ValueMember        = Constants.TaskKind.TASK_KIND_CODE;

            // ステータス（タスク一覧検索）
            ComboBox_StatusName_SelectTaskList.DataSource       = comboData.TaskStatusCodeTable_Select();
            ComboBox_StatusName_SelectTaskList.DisplayMember    = Constants.TaskStatus.TASK_STATUS_NAME;
            ComboBox_StatusName_SelectTaskList.ValueMember      = Constants.TaskStatus.TASK_STATUS_CODE;

            // グループ（タスク一覧検索）
            ComboBox_GroupName_SelectTaskList.DataSource        = comboData.TaskGroupCodeTable_Select(TextBox_UserNo.Text);
            ComboBox_GroupName_SelectTaskList.DisplayMember     = Constants.TaskGroup.TASK_GROUP_NAME;
            ComboBox_GroupName_SelectTaskList.ValueMember       = Constants.TaskGroup.TASK_GROUP_CODE;

            // 種別（タスク予実推移）
            ComboBox_KindName_SelectTaskChart.DataSource        = comboData.TaskKindCodeTable_Select(TextBox_UserNo.Text);
            ComboBox_KindName_SelectTaskChart.DisplayMember     = Constants.TaskKind.TASK_KIND_NAME;
            ComboBox_KindName_SelectTaskChart.ValueMember       = Constants.TaskKind.TASK_KIND_CODE;

            // グループ（タスク予実推移）
            ComboBox_GroupName_SelectTaskChart.DataSource       = comboData.TaskGroupCodeTable_Select(TextBox_UserNo.Text);
            ComboBox_GroupName_SelectTaskChart.DisplayMember    = Constants.TaskGroup.TASK_GROUP_NAME;
            ComboBox_GroupName_SelectTaskChart.ValueMember      = Constants.TaskGroup.TASK_GROUP_CODE;

            // 備忘録：タスク追加処理のコンボボックスについては、
            //         「全て」コード(01)は表示してはいけないため、別メソッドを利用する。
            //       ※「全て」コード(01)はタスク検索時に条件なしで利用するため。

            // 種別コード（タスク追加）
            ComboBox_KindName_Add.DataSource                    = comboData.TaskKindCodeTable(TextBox_UserNo.Text);
            ComboBox_KindName_Add.DisplayMember                 = Constants.TaskKind.TASK_KIND_NAME;
            ComboBox_KindName_Add.ValueMember                   = Constants.TaskKind.TASK_KIND_CODE;

            // グループコード（タスク追加）
            ComboBox_GroupName_Add.DataSource                   = comboData.TaskGroupCodeTable(TextBox_UserNo.Text);
            ComboBox_GroupName_Add.DisplayMember                = Constants.TaskGroup.TASK_GROUP_NAME;
            ComboBox_GroupName_Add.ValueMember                  = Constants.TaskGroup.TASK_GROUP_CODE;

            // コンボボックスは手入力不可とする。選択のみ。
            ComboBox_KindName_SelectTaskList.DropDownStyle      = ComboBoxStyle.DropDownList;
            ComboBox_StatusName_SelectTaskList.DropDownStyle    = ComboBoxStyle.DropDownList;
            ComboBox_GroupName_SelectTaskList.DropDownStyle     = ComboBoxStyle.DropDownList;
            ComboBox_KindName_SelectTaskChart.DropDownStyle     = ComboBoxStyle.DropDownList;
            ComboBox_GroupName_SelectTaskChart.DropDownStyle    = ComboBoxStyle.DropDownList;
            ComboBox_KindName_Add.DropDownStyle                 = ComboBoxStyle.DropDownList;
            ComboBox_GroupName_Add.DropDownStyle                = ComboBoxStyle.DropDownList;

            #endregion

            #endregion

            #region タスク管理画面

            // ユーザー情報を設定
            // タスク検索～更新時にユーザーNoが必要なため。
            // TODO：ユーザー情報テキストボックスを見易くする。
            TextBox_UserId.Text         = loginUser.USER_ID;
            TextBox_UserName.Text       = loginUser.USER_NAME;
            TextBox_UserNo.Enabled      = false;
            TextBox_UserNo.BackColor    = Color.White;
            TextBox_UserId.Enabled      = false;
            TextBox_UserId.BackColor    = Color.White;
            TextBox_UserName.Enabled    = false;
            TextBox_UserName.BackColor  = Color.White;

            // タスク追加の「期限日」は本日日付を入れておく。
            TextBox_TodoDay_Add.Text = DateTime.Today.ToString("yyyy/MM/dd");

            // 非表示項目の設定。利用者に関係ないテキストボックスは非表示。
            // 非表示対象はコンボボックスのコード保持がメイン。
            TextBox_KindCode_Select.Visible     = false;
            TextBox_StatusCode_Select.Visible   = false;
            TextBox_GroupCode_Select.Visible    = false;
            TextBox_KindCode_Add.Visible        = false;
            TextBox_GroupCode_Add.Visible       = false;
            TextBox_UserNo.Visible              = false;

            #region タスク一覧リストビュー
            this.ListView_Task.FullRowSelect = true;
            this.ListView_Task.Columns[9].Width = 0;

            // タスク一覧検索のユーザー毎初期検索条件を取得する。
            this.TaskSelectFlg_Set();

            // タスク一覧を検索し画面表示する。
            this.TaskListSelectForListView();
            #endregion

            #endregion

            #region タスク予実推移画面

            // 非表示項目の設定。利用者に関係ないテキストボックスは非表示。
            // 非表示対象はコンボボックスのコード保持がメイン。
            TextBox_KindCode_SelectTaskChart.Visible = false;
            TextBox_GroupCode_SelectTaskChart.Visible = false;
            CheckBox_Week_SelectTaskChart.Checked = true;

            // タスク予実推移タブ
            Chart_TaskBudget.Series.Clear();

            // 外枠の設定
            Chart_TaskBudget.BorderColor = Color.SlateGray;
            Chart_TaskBudget.BorderDashStyle = ChartDashStyle.Solid;
            Chart_TaskBudget.BorderWidth = 1;

            // グラフタイトルの変更
            Chart_TaskBudget.Titles.Add("タスク予実推移");
            Chart_TaskBudget.Titles[0].Alignment = ContentAlignment.TopCenter;
            Chart_TaskBudget.Titles[0].Font = new Font("MS UI Gothic", 12, FontStyle.Regular | FontStyle.Underline);

            #endregion
        }

        #region 検索フラグ取得
        private void TaskSelectFlg_Set()
        {
            TaskFlgData taskFlgData = new TaskFlgData();
            Flg taskFlg_Select = taskFlgData.Select_FlgSelect(TextBox_UserNo.Text);

            #region 検索条件設定の変更

            // タスクステータス
            // 01(済)の場合はチェックボックス有効化
            if (taskFlg_Select.TASK_STATUS_CODE == Constants.TaskStatus.UNCOMPLETED_01)
                CheckBox_Uncompleted_SelectTaskList.Checked = true;
            else
                ComboBox_StatusName_SelectTaskList.SelectedValue = taskFlg_Select.TASK_STATUS_CODE;

            // タスク種別
            // 01(個人)の場合はチェックボックス有効化
            if (taskFlg_Select.TASK_KIND_CODE == Constants.TaskKind.PERSONAL_01)
                CheckBox_Personal_SelectTaskList.Checked = true;
            else
                ComboBox_KindName_SelectTaskList.SelectedValue = taskFlg_Select.TASK_KIND_CODE;

            // タスクグループ
            ComboBox_GroupName_SelectTaskList.SelectedValue = taskFlg_Select.TASK_GROUP_CODE;

            #endregion
        }
        #endregion

        #endregion

        #region タスク一覧管理画面
        /// <summary>
        /// タスクを管理する。以下の機能を実装。
        /// 　①タスクを一覧検索し画面表示。
        /// 　　※検索／追加／更新時に再度検索表示を実施。
        /// 　②タスク一覧検索の条件保存（ユーザー毎）。
        /// 　③タスク追加し一覧検索。
        /// 　④ログインユーザーのログアウト。
        /// </summary>
        /// <todo> UPD:20190430
        /// 　①コード／DBカラムのオンコーディングをCommonプロジェクト移行。
        /// 　②リストビューのインデックス直指定の変更。
        /// 　③
        /// </todo>

        #region 検索ボタン　★済★
        private void Button_Task_Select_Click(object sender, EventArgs e)
        {
            #region エラー制御
            // 日付FROMが日付型に変換できない場合はエラー
            if (TextBox_Day_From_SelectTaskList.Text != "" && !DateTime.TryParse(TextBox_Day_From_SelectTaskList.Text, out DateTime dt_from))
            {
                MessageBox.Show("日付を正しく入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TextBox_Day_From_SelectTaskList.Clear();

                return;
            }

            // 日付TOが日付型に変換できない場合はエラー
            if (TextBox_Day_To_SelectTaskList.Text != "" && !DateTime.TryParse(TextBox_Day_To_SelectTaskList.Text, out DateTime dt_to))
            {
                MessageBox.Show("日付を正しく入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TextBox_Day_To_SelectTaskList.Clear();

                return;
            }
            #endregion

            // タスク一覧の再検索および表示
            this.TaskListSelectForListView();
        }
        #endregion

        #region 条件保存（タスク検索）ボタン押下　★済★
        private void Button_TaskSetting_Update_Click(object sender, EventArgs e)
        {
            DialogResult dr_YESNO = MessageBox.Show("検索条件を保存しますか？",
                                "条件保存",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

            if (dr_YESNO == DialogResult.No)
                return;

            TaskFlgData taskFlgData = new TaskFlgData();
            Flg_Task_Select flgTaskSelect = new Flg_Task_Select()
            {
                USER_NO = TextBox_UserNo.Text,
                // ステータス「未」チェックボックスがTRUEの場合、TASK_STATUS_CODEは「未」(01)の固定値を採用
                TASK_STATUS_CODE = CheckBox_Uncompleted_SelectTaskList.Checked
                    ? Constants.TaskStatus.UNCOMPLETED_01 : TextBox_StatusCode_Select.Text,
                // 種別「個人」チェックボックスがTRUEの場合、TASK_KIND_CODEは「個人」(01)の固定値を採用
                TASK_KIND_CODE = CheckBox_Personal_SelectTaskList.Checked 
                    ? Constants.TaskKind.PERSONAL_01      : TextBox_KindCode_Select.Text,
                TASK_GROUP_CODE = TextBox_GroupCode_Select.Text,
            };

            taskFlgData.Update_FlgSelect(flgTaskSelect);

            DialogResult dr_OK = MessageBox.Show("検索条件を保存しました。",
                                "保存",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

        }
        #endregion

        #region タスク追加ボタン　★済★
        private void Button_Task_Add_Click(object sender, EventArgs e)
        {
            #region エラー制御
            // 期限日が空の場合はエラー
            if (TextBox_TodoDay_Add.Text == "")
            {
                MessageBox.Show("期限日を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 期限日が日付型に変換できない場合はエラー
            if (!DateTime.TryParse(TextBox_TodoDay_Add.Text, out DateTime dt))
            {
                MessageBox.Show("期限日には日付を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TextBox_TodoDay_Add.Clear();

                return;
            }

            // 予定時間が空の場合はエラー
            if (TextBox_PlanTime_Add.Text == "")
            {
                MessageBox.Show("予定時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // 予定時間が時間型に変換できない場合はエラー
            if (!TimeSpan.TryParse(TextBox_PlanTime_Add.Text, out TimeSpan ts))
            {
                MessageBox.Show("予定時間には時間を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TextBox_PlanTime_Add.Clear();

                return;
            }

            // 種別が空の場合はエラー
            if (ComboBox_KindName_Add.Text == "")
            {
                MessageBox.Show("種別を選択してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // タスク内容が空の場合はエラー
            if (TextBox_TaskName_Add.Text == "")
            {
                MessageBox.Show("タスク内容を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // タスク内容が50バイトを超える場合はエラー
            if (Encoding.GetEncoding("Shift_JIS").GetByteCount(TextBox_TaskName_Add.Text) > 100)
            {
                MessageBox.Show("タスク内容は最大全角50文字です。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }
            #endregion

            Task task_Add = new Task
            {
                USER_NO             = TextBox_UserNo.Text,
                CREATE_YMD          = DateTime.Today,
                TODO_YMD            = DateTime.Parse(TextBox_TodoDay_Add.Text),
                // 追加タスクのステータスは、「未」(01)で固定。
                TASK_STATUS_CODE    = Constants.TaskStatus.UNCOMPLETED_01,
                TASK_KIND_CODE      = TextBox_KindCode_Add.Text,
                TASK_GROUP_CODE     = TextBox_GroupCode_Add.Text,
                TASK_NAME           = TextBox_TaskName_Add.Text,
                PLAN_TIME           = TimeSpan.Parse(TextBox_PlanTime_Add.Text),
            };

            TaskData taskData = new Data.TaskData();

            // タスクの追加処理
            taskData.Task_Insert(task_Add);

            // タスク一覧の再検索および表示
            this.TaskListSelectForListView();

            // 画面のタスク追加設定の初期化
            TextBox_PlanTime_Add.Text = "";
            TextBox_TaskName_Add.Text = "";
        }
        #endregion

        #region ユーザー設定ボタン　★済★
        private void Button_UserSetting_Click(object sender, EventArgs e)
        {
            this.Visible = false;

            Form_UserMst f_UserMst = new Form_UserMst(TextBox_UserNo.Text);
            f_UserMst.ShowDialog(this);
            f_UserMst.Dispose();

            // コードマスタを更新するため、タスク管理画面を再起動する。
            // 備忘録：パスワードの再認証は不要のため、NULLでユーザーを検索する。
            UserData s_User = new UserData();
            User login_User = s_User.LogIn_User_Select(TextBox_UserId.Text, null);

            Form_TaskManager f_TaskManager = new Form_TaskManager(login_User);
            f_TaskManager.ShowDialog(this);
            f_TaskManager.Dispose();
        }
        #endregion

        #region ログアウトボタン　★済★
        private void Button_LogOut_Click(object sender, EventArgs e)
        {
            Form_LogIn f_Login = new Form_LogIn();

            this.Visible = false;
            f_Login.ShowDialog(this);
            f_Login.Dispose();

            this.Close();
        }
        #endregion

        #region ステータス「未」チェックボックス　★済★
        private void CheckBox_Completed_CheckedChanged(object sender, EventArgs e)
        {
            // ＯＮ　：ステータスドロップボックスは選択不可
            if (CheckBox_Uncompleted_SelectTaskList.Checked)
                ComboBox_StatusName_SelectTaskList.Enabled = false;
            // ＯＦＦ：ステータスドロップボックスは選択可
            else
                ComboBox_StatusName_SelectTaskList.Enabled = true;
        }
        #endregion

        #region 種別「個人」チェックボックス　★済★
        private void CheckBox_Personal_CheckedChanged(object sender, EventArgs e)
        {
            // ＯＮ　：ステータスドロップボックスは選択不可
            if (CheckBox_Personal_SelectTaskList.Checked)
                ComboBox_KindName_SelectTaskList.Enabled = false;
            // ＯＦＦ：ステータスドロップボックスは選択可
            else
                ComboBox_KindName_SelectTaskList.Enabled = true;
        }
        #endregion

        #region タスク一覧リストビュー明細　☆残☆
        // TODO：リストビューのインデックス直指定の変更。
        private void List_Task_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TaskData taskData = new Data.TaskData();

            // 更新タスクの取得　LoadForm_Task_Select(タスクNo, ユーザーNo)
            Task task_Update = taskData.LoadForm_Task_Select(ListView_Task.SelectedItems[0].SubItems[9].Text, TextBox_UserNo.Text);

            // タスク更新画面
            Form_TaskUpdate f_TaskUpdate = new Form_TaskUpdate(task_Update);
            f_TaskUpdate.ShowDialog(this);
            f_TaskUpdate.Dispose();

            // タスク一覧の再検索および表示
            this.TaskListSelectForListView();
        }
        #endregion

        #region タスク一覧検索　★済★
        private void TaskListSelectForListView()
        {
            // リストビューの初期化
            ListView_Task.Items.Clear();

            // タスク検索条件を画面から取得
            // TODO：下記記述内容の見直し
            Flg_Task_Select flgTaskSelect = new Flg_Task_Select()
            {
                USER_NO             = TextBox_UserNo.Text,
                DAY_FROM            = TextBox_Day_From_SelectTaskList.Text == "" 
                    ? new DateTime(0) : DateTime.Parse(TextBox_Day_From_SelectTaskList.Text),
                DAY_TO              = TextBox_Day_To_SelectTaskList.Text == "" 
                    ? new DateTime(0) : DateTime.Parse(TextBox_Day_To_SelectTaskList.Text),
                // ステータス「未」チェックボックスがTRUEの場合、TASK_STATUS_CODEは「未」(01)の固定値を採用
                TASK_STATUS_CODE = CheckBox_Uncompleted_SelectTaskList.Checked
                    ? Constants.TaskStatus.UNCOMPLETED_01 : TextBox_StatusCode_Select.Text,
                // 種別「個人」チェックボックスがTRUEの場合、TASK_KIND_CODEは「個人」(01)の固定値を採用
                TASK_KIND_CODE = CheckBox_Personal_SelectTaskList.Checked
                    ? Constants.TaskKind.PERSONAL_01      : TextBox_KindCode_Select.Text,
                TASK_GROUP_CODE = TextBox_GroupCode_Select.Text
            };

            // タスク一覧の取得(SELECT)
            TaskData taskData = new TaskData();
            List<Task> taskList = taskData.LoadForm_TaskList_Select(flgTaskSelect);

            // タスク一覧をリストビューに表示。
            TaskListShowListview(taskList);
        }
        #endregion

        #region タスク一覧表示　☆残☆
        // TODO：LINQを利用した記述に変更する。
        private void TaskListShowListview(List<Task> taskList)
        {
            // タスク一覧リストビューの列番号。
            // 列の背景色変更時に利用するため定義。
            int countForList = 0;

            // TODO：LINQを利用した記述に変更する。
            foreach (Task task in taskList)
            {
                string[] view = {
                                      (countForList + 1).ToString()
                                    , task.TODO_YMD.ToString().Substring(0,10)
                                    , task.TASK_STATUS_NAME
                                    , task.TASK_KIND_NAME
                                    , task.TASK_GROUP_NAME
                                    , task.TASK_NAME
                                    , task.PLAN_TIME.ToString(@"hh\:mm")
                                    , task.RESULT_TIME.ToString(@"hh\:mm")
                                    , task.PLAN_TIME >= task.RESULT_TIME
                                    ? (task.PLAN_TIME - task.RESULT_TIME).ToString(@"hh\:mm")
                                    : "-"+ (task.PLAN_TIME - task.RESULT_TIME).ToString(@"hh\:mm")
                                    , task.TASK_NO
                                };

                // リストビューに追加表示
                ListView_Task.Items.Add(new ListViewItem(view));

                #region リストビュー表示設定の変更（背景色等）
                // タスクステータスが「済」(10)の場合、背景：薄グレー
                if (task.TASK_STATUS_CODE == Constants.TaskStatus.COMPLETED_10)
                    ListView_Task.Items[countForList].BackColor = Color.Gainsboro;

                // タスクステータスが「削除」(20)の場合、背景：濃グレー
                if (task.TASK_STATUS_CODE == Constants.TaskStatus.DELETE_20)
                    ListView_Task.Items[countForList].BackColor = Color.SlateGray;

                // 期限日を過ぎている場合、背景：ネイビー　文字：白
                if (task.TASK_STATUS_CODE == Constants.TaskStatus.UNCOMPLETED_01 && task.TODO_YMD < DateTime.Today)
                {
                    ListView_Task.Items[countForList].BackColor = Color.SteelBlue;
                    ListView_Task.Items[countForList].ForeColor = Color.White;
                }
                #endregion

                countForList++;
            }
        }
        #endregion

        #region コンボボックス

        #region タスクステータスコンボボックス選択時処理
        private void ComboBox_StatusName_Select_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_StatusName_SelectTaskList.SelectedIndex != -1)
                TextBox_StatusCode_Select.Text = ComboBox_StatusName_SelectTaskList.SelectedValue.ToString();
        }
        #endregion

        #region タスク種別(SELECT)コンボボックス選択時処理
        private void ComboBox_KindName_Select_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_KindName_SelectTaskList.SelectedIndex != -1)
                TextBox_KindCode_Select.Text = ComboBox_KindName_SelectTaskList.SelectedValue.ToString();
        }
        #endregion

        #region タスク種別(INSERT)コンボボックス選択時処理
        private void ComboBox_KindName_Add_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_KindName_Add.SelectedIndex != -1)
                TextBox_KindCode_Add.Text = ComboBox_KindName_Add.SelectedValue.ToString();
        }
        #endregion

        #region タスクグループ(SELECT)コンボボックス選択時処理
        private void ComboBox_GroupName_Select_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_GroupName_SelectTaskList.SelectedIndex != -1)
                TextBox_GroupCode_Select.Text = ComboBox_GroupName_SelectTaskList.SelectedValue.ToString();
        }
        #endregion

        #region タスクグループ(INSERT)コンボボックス選択時処理
        private void ComboBox_GroupName_Add_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_GroupName_Add.SelectedIndex != -1)
                TextBox_GroupCode_Add.Text = ComboBox_GroupName_Add.SelectedValue.ToString();
        }
        #endregion

        #endregion

        #endregion

        #region タスク日別サマリ画面

        #endregion

        #region タスク予実推移画面

        #region 個人チェックボックスチェック処理
        private void CheckBox_Personal_SelectTaskChart_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Personal_SelectTaskChart.Checked)
                ComboBox_KindName_SelectTaskChart.Enabled = false;
            else
                ComboBox_KindName_SelectTaskChart.Enabled = true;
        }
        #endregion

        #region 日毎チェックボックスチェック処理
        private void CheckBox_Day_SelectTaskChart_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Day_SelectTaskChart.Checked)
            {
                CheckBox_Week_SelectTaskChart.Checked = false;
                CheckBox_Month_SelectTaskChart.Checked = false;
            }
        }
        #endregion

        #region 週毎チェックボックスチェック処理
        private void CheckBox_Week_SelectTaskChart_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Week_SelectTaskChart.Checked)
            {
                CheckBox_Day_SelectTaskChart.Checked = false;
                CheckBox_Month_SelectTaskChart.Checked = false;
            }
        }
        #endregion

        #region 月毎チェックボックスチェック処理
        private void CheckBox_Month_SelectTaskChart_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Month_SelectTaskChart.Checked)
            {
                CheckBox_Day_SelectTaskChart.Checked = false;
                CheckBox_Week_SelectTaskChart.Checked = false;
            }
        }
        #endregion

        #region 検索ボタン押下処理
        private void Button_SelectTaskChart_Click(object sender, EventArgs e)
        {
            #region エラー制御
            // 日付FROMが日付型に変換できない場合はエラー
            if (TextBox_Day_From_SelectTaskChart.Text != "" && !DateTime.TryParse(TextBox_Day_From_SelectTaskChart.Text, out DateTime dt_from))
            {
                MessageBox.Show("日付を正しく入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TextBox_Day_From_SelectTaskChart.Clear();

                return;
            }

            // 日付TOが日付型に変換できない場合はエラー
            if (TextBox_Day_To_SelectTaskChart.Text != "" && !DateTime.TryParse(TextBox_Day_To_SelectTaskChart.Text, out DateTime dt_to))
            {
                MessageBox.Show("日付を正しく入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TextBox_Day_To_SelectTaskChart.Clear();

                return;
            }

            // 間隔をチェックしていない場合はエラー
            if (!CheckBox_Day_SelectTaskChart.Checked && !CheckBox_Week_SelectTaskChart.Checked && !CheckBox_Month_SelectTaskChart.Checked)
            {
                MessageBox.Show("間隔はどれかにチェックが必要です。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                TextBox_Day_To_SelectTaskChart.Clear();

                return;
            }
            #endregion

            // チャートの初期化
            Chart_TaskBudget.Series.Clear();

            // チャート名
            string chartTitlePlus = "過剰見積り率";
            string chartTitleMinus = "過少見積り率";

            // チャートデータインスタンス生成
            ChartData chartData = new ChartData();

            // チャート検索フラグインスタンス生成
            Flg_Chart_Select flgChartSelect = new Flg_Chart_Select()
            {
                USER_NO             = TextBox_UserNo.Text,
                DAY_FROM            = TextBox_Day_From_SelectTaskChart.Text == "" ? new DateTime(0) : DateTime.Parse(TextBox_Day_From_SelectTaskChart.Text),
                DAY_TO              = TextBox_Day_To_SelectTaskChart.Text == "" ? new DateTime(0) : DateTime.Parse(TextBox_Day_To_SelectTaskChart.Text),
                TASK_KIND_CODE      = CheckBox_Personal_SelectTaskChart.Checked == true ? "01" : TextBox_KindCode_SelectTaskChart.Text,
                TASK_GROUP_CODE     = TextBox_GroupCode_SelectTaskChart.Text,
                CHART_INTERVAL      = CheckBox_Day_SelectTaskChart.Checked == true  ? "DAY" :
                                      CheckBox_Week_SelectTaskChart.Checked == true ? "WEEK" : "MONTH"
            };

            // タスク予実推移データの取得
            DataTable taskChartTable = chartData.TaskChartTable_Select(flgChartSelect);

            // チャートの追加
            Chart_TaskBudget.Series.Add(chartTitlePlus);
            Chart_TaskBudget.Series.Add(chartTitleMinus);

            // チャート種類の変更
            Chart_TaskBudget.Series[chartTitlePlus].ChartType   = SeriesChartType.Line;
            Chart_TaskBudget.Series[chartTitlePlus].MarkerStyle = MarkerStyle.Square;
            Chart_TaskBudget.Series[chartTitlePlus].MarkerSize  = 10;
            Chart_TaskBudget.Series[chartTitlePlus].BorderWidth = 3;
            Chart_TaskBudget.Series[chartTitlePlus].Color       = Color.SteelBlue;
            Chart_TaskBudget.Series[chartTitleMinus].ChartType   = SeriesChartType.Line;
            Chart_TaskBudget.Series[chartTitleMinus].MarkerStyle = MarkerStyle.Square;
            Chart_TaskBudget.Series[chartTitleMinus].MarkerSize  = 10;
            Chart_TaskBudget.Series[chartTitleMinus].BorderWidth = 3;
            Chart_TaskBudget.Series[chartTitleMinus].Color       = Color.SlateGray;

            // X・Y軸のタイトル設定
            Chart_TaskBudget.ChartAreas[0].AxisX.Title = "完了帯";
            Chart_TaskBudget.ChartAreas[0].AxisY.Title = "上限超過率";

            // Y軸上限値の設定
            Chart_TaskBudget.ChartAreas[0].AxisY.Maximum = 100;

            // チャートにデータ値の登録
            for (int i = 0; i < taskChartTable.Rows.Count; i++)
            {
                Chart_TaskBudget.Series[chartTitlePlus].Points.AddXY(taskChartTable.Rows[i][0], taskChartTable.Rows[i][1]);
                Chart_TaskBudget.Series[chartTitleMinus].Points.AddXY(taskChartTable.Rows[i][0], taskChartTable.Rows[i][2]);
            }
        }
        #endregion

        #region コンボボックス選択時処理

        #region タスクステータスコンボボックス選択時処理
        private void ComboBox_KindName_SelectTaskChart_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_KindName_SelectTaskChart.SelectedIndex != -1)
                TextBox_KindCode_SelectTaskChart.Text = ComboBox_KindName_SelectTaskChart.SelectedValue.ToString();
        }
        #endregion

        #region タスクグループコンボボックス選択時処理
        private void ComboBox_GroupName_SelectTaskChart_SelectedValueChanged(object sender, EventArgs e)
        {
            // コンボボックス選択時のみ有効
            if (ComboBox_GroupName_SelectTaskChart.SelectedIndex != -1)
                TextBox_GroupCode_SelectTaskChart.Text = ComboBox_GroupName_SelectTaskChart.SelectedValue.ToString();
        }
        #endregion

        #endregion

        #endregion
    }
    #endregion
}
