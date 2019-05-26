namespace Form
{
    partial class Form_UserMst
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button_Start_Update = new System.Windows.Forms.Button();
            this.Button_Return_UserMst = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.GroupBox_TaskGroup_UserMst = new System.Windows.Forms.GroupBox();
            this.DataGridView_TaskGroup_UserMst = new System.Windows.Forms.DataGridView();
            this.TextBox_UserNo_UserMst = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.GroupBox_TaskGroup_UserMst.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_TaskGroup_UserMst)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Button_Start_Update
            // 
            this.Button_Start_Update.Location = new System.Drawing.Point(280, 471);
            this.Button_Start_Update.Name = "Button_Start_Update";
            this.Button_Start_Update.Size = new System.Drawing.Size(75, 40);
            this.Button_Start_Update.TabIndex = 15;
            this.Button_Start_Update.Text = "更新";
            this.Button_Start_Update.UseVisualStyleBackColor = true;
            this.Button_Start_Update.Click += new System.EventHandler(this.Button_Start_Update_Click);
            // 
            // Button_Return_UserMst
            // 
            this.Button_Return_UserMst.Location = new System.Drawing.Point(656, 538);
            this.Button_Return_UserMst.Name = "Button_Return_UserMst";
            this.Button_Return_UserMst.Size = new System.Drawing.Size(90, 40);
            this.Button_Return_UserMst.TabIndex = 1;
            this.Button_Return_UserMst.Text = "戻る";
            this.Button_Return_UserMst.UseVisualStyleBackColor = true;
            this.Button_Return_UserMst.Click += new System.EventHandler(this.Button_Return_UserMst_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("MS UI Gothic", 6.5F);
            this.button3.Location = new System.Drawing.Point(468, 538);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 40);
            this.button3.TabIndex = 35;
            this.button3.Text = "パスワード";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(562, 538);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 40);
            this.button4.TabIndex = 40;
            this.button4.Text = "上限";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // GroupBox_TaskGroup_UserMst
            // 
            this.GroupBox_TaskGroup_UserMst.Controls.Add(this.DataGridView_TaskGroup_UserMst);
            this.GroupBox_TaskGroup_UserMst.Controls.Add(this.Button_Start_Update);
            this.GroupBox_TaskGroup_UserMst.Location = new System.Drawing.Point(12, 12);
            this.GroupBox_TaskGroup_UserMst.Name = "GroupBox_TaskGroup_UserMst";
            this.GroupBox_TaskGroup_UserMst.Size = new System.Drawing.Size(364, 520);
            this.GroupBox_TaskGroup_UserMst.TabIndex = 5;
            this.GroupBox_TaskGroup_UserMst.TabStop = false;
            this.GroupBox_TaskGroup_UserMst.Text = "グループ";
            // 
            // DataGridView_TaskGroup_UserMst
            // 
            this.DataGridView_TaskGroup_UserMst.AllowUserToAddRows = false;
            this.DataGridView_TaskGroup_UserMst.AllowUserToDeleteRows = false;
            this.DataGridView_TaskGroup_UserMst.AllowUserToOrderColumns = true;
            this.DataGridView_TaskGroup_UserMst.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridView_TaskGroup_UserMst.Location = new System.Drawing.Point(23, 24);
            this.DataGridView_TaskGroup_UserMst.Name = "DataGridView_TaskGroup_UserMst";
            this.DataGridView_TaskGroup_UserMst.RowTemplate.Height = 27;
            this.DataGridView_TaskGroup_UserMst.Size = new System.Drawing.Size(307, 441);
            this.DataGridView_TaskGroup_UserMst.TabIndex = 10;
            // 
            // TextBox_UserNo_UserMst
            // 
            this.TextBox_UserNo_UserMst.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.TextBox_UserNo_UserMst.Location = new System.Drawing.Point(25, 555);
            this.TextBox_UserNo_UserMst.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.TextBox_UserNo_UserMst.Name = "TextBox_UserNo_UserMst";
            this.TextBox_UserNo_UserMst.Size = new System.Drawing.Size(135, 25);
            this.TextBox_UserNo_UserMst.TabIndex = 65;
            this.TextBox_UserNo_UserMst.Text = "ユーザーNo";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(382, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 520);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "種別";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(307, 441);
            this.dataGridView1.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(280, 471);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 40);
            this.button1.TabIndex = 15;
            this.button1.Text = "更新";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Form_UserMst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 585);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GroupBox_TaskGroup_UserMst);
            this.Controls.Add(this.TextBox_UserNo_UserMst);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Button_Return_UserMst);
            this.Name = "Form_UserMst";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ユーザマスタメンテナンス";
            this.GroupBox_TaskGroup_UserMst.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_TaskGroup_UserMst)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button_Start_Update;
        private System.Windows.Forms.Button Button_Return_UserMst;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox GroupBox_TaskGroup_UserMst;
        private System.Windows.Forms.DataGridView DataGridView_TaskGroup_UserMst;
        private System.Windows.Forms.TextBox TextBox_UserNo_UserMst;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
    }
}