using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Data;
using Entity;
using System.Collections.Generic;

namespace Form
{
    #region ログイン画面
    /// <summary>
    /// ユーザーおよびパスワードの確認を実施する。
    /// </summary>
    /// <todo> UPD:20190430
    /// 　①パスワードの暗号化
    /// 　⇒画面表示およびＤＢ保持
    /// 　②パスワード大文字小文字区別
    /// </todo>
    public partial class Form_LogIn : System.Windows.Forms.Form
    {
        #region 初期表示処理
        /// <summary>
        /// ログイン画面の初期表示の処理を行う。
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public Form_LogIn()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        #endregion

        #region　クローズボタン押下
        private void Button_Close_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("画面を閉じますか？", "クローズ",
                              MessageBoxButtons.YesNo,
                              MessageBoxIcon.Information);

            if(dr == DialogResult.Yes)
                this.Close();
        }
        #endregion

        #region　ログインボタン押下
        private void Button_Login_Click(object sender, EventArgs e)
        {
            #region エラー処理

            // ユーザーＩＤが空の場合、エラー
            if (TextBox_User_Id.Text == "")
            {
                MessageBox.Show("ユーザーＩＤを入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // パスワードが空の場合、エラー
            if (TextBox_Password.Text == "")
            {
                MessageBox.Show("パスワードを入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            // ユーザー検索検索インスタンス生成
            UserData s_User = new UserData();

            // ユーザーＩＤとパスワードで取得
            User login_User = s_User.LogIn_User_Select(TextBox_User_Id.Text, TextBox_Password.Text);

            // 検索結果が存在しない場合は、エラー
            if(login_User == null)
            {
                MessageBox.Show("ユーザーＩＤまたはパスワードが異なります。",
                    "ログイン",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }
            #endregion

            this.Visible = false;

            // 検索結果が存在する場合は次画面へ遷移する。
            // 次画面フォーム　タスク管理画面
            Form_TaskManager f_TaskManager = new Form_TaskManager(login_User);
            f_TaskManager.ShowDialog(this);
            f_TaskManager.Dispose();

            // ログイン画面を閉じる。
            this.Close();
        }
        #endregion

        #region ユーザーIDエンター押下
        private void TextBox_User_Id_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) TextBox_Password.Focus();
        }
        #endregion

        #region パスワードエンター押下
        private void TextBox_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Button_Login.Focus();
        }
        #endregion
    }
    #endregion
}
