using Data;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Form
{
    public partial class Form_UserMst : System.Windows.Forms.Form
    {
        #region 初期表示
        public Form_UserMst(string logInUserNo)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // 備忘録：ユーザーNoの設定は事前に行っておく。
            //         ユーザーのマスタからデータ取得する際にキーとして利用するため。
            TextBox_UserNo_UserMst.Text     = logInUserNo;
            TextBox_UserNo_UserMst.Visible  = false;

            ComboData comboData = new ComboData();

            DataGridView_TaskGroup_UserMst.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;

            #region タスクグループ一覧初期表示

            DataGridView_TaskGroup_UserMst.DataSource               = comboData.TaskGroupCodeTable(TextBox_UserNo_UserMst.Text);
            DataGridView_TaskGroup_UserMst.Columns[0].HeaderText    = "コード";
            DataGridView_TaskGroup_UserMst.Columns[1].HeaderText    = "グループ名";
            DataGridView_TaskGroup_UserMst.Columns[0].Width         = 60;
            DataGridView_TaskGroup_UserMst.Columns[1].Width         = 120;

            // DataGridViewの選択マークカラムは非表示
            DataGridView_TaskGroup_UserMst.RowHeadersVisible        = false;

            // 暫定対応：コードは手入力不可（コード数も可変にする？？）
            //         ※コードも変更できるようにした場合、タスクも変更する必要有。
            DataGridView_TaskGroup_UserMst.Columns[0].ReadOnly      = true;

            #endregion

        }
        #endregion

        #region 戻るボタン押下
        private void Button_Return_UserMst_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
        #endregion

        #region 更新(グループ)ボタン押下
        private void Button_Start_Update_Click(object sender, EventArgs e)
        {
            #region 更新確認処理
            DialogResult dr_YESNO = MessageBox.Show("グループ名を更新しますか？",
                                "更新",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

            if (dr_YESNO == DialogResult.No)
                return;
            #endregion

            CodeData codeData   = new CodeData(TextBox_UserNo_UserMst.Text);

            for (int rowCount = 0; rowCount < DataGridView_TaskGroup_UserMst.Rows.Count; rowCount++)
            {
                Code taskGroupCode = new Code(  DataGridView_TaskGroup_UserMst.Rows[rowCount].Cells[0].Value.ToString(),
                                                DataGridView_TaskGroup_UserMst.Rows[rowCount].Cells[1].Value.ToString()
                                            );

                // タスクグループ名を更新する。
                // TaskGroupMstUpdate(タスクグループコードインスタンス)
                codeData.TaskGroupMstUpdate(taskGroupCode);
            }

            #region 更新通知処理
            DialogResult dr_OK = MessageBox.Show("更新が完了しました。",
                                "更新",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            #endregion
        }
        #endregion
    }
}
