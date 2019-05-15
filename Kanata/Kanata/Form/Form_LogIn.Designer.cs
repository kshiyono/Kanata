namespace Form
{
    partial class Form_LogIn
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_Login = new System.Windows.Forms.Button();
            this.TextBox_User_Id = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.TextBox_Password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Button_Login
            // 
            this.Button_Login.Location = new System.Drawing.Point(53, 102);
            this.Button_Login.Name = "Button_Login";
            this.Button_Login.Size = new System.Drawing.Size(98, 36);
            this.Button_Login.TabIndex = 10;
            this.Button_Login.Text = "ログイン";
            this.Button_Login.UseVisualStyleBackColor = true;
            this.Button_Login.Click += new System.EventHandler(this.Button_Login_Click);
            // 
            // TextBox_User_Id
            // 
            this.TextBox_User_Id.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TextBox_User_Id.Location = new System.Drawing.Point(123, 22);
            this.TextBox_User_Id.Name = "TextBox_User_Id";
            this.TextBox_User_Id.Size = new System.Drawing.Size(167, 25);
            this.TextBox_User_Id.TabIndex = 1;
            this.TextBox_User_Id.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_User_Id_KeyDown);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(180, 102);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 36);
            this.button2.TabIndex = 15;
            this.button2.Text = "閉じる";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button_Close_Click);
            // 
            // TextBox_Password
            // 
            this.TextBox_Password.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.TextBox_Password.Location = new System.Drawing.Point(123, 58);
            this.TextBox_Password.Name = "TextBox_Password";
            this.TextBox_Password.Size = new System.Drawing.Size(167, 25);
            this.TextBox_Password.TabIndex = 5;
            this.TextBox_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_Password_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "パスワード";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "ユーザーID";
            // 
            // Form_LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 153);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBox_Password);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.TextBox_User_Id);
            this.Controls.Add(this.Button_Login);
            this.Name = "Form_LogIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ログイン";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Login;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox TextBox_Password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox TextBox_User_Id;
    }
}

