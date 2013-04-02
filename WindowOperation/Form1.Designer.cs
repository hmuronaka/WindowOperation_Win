namespace WindowOperation
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.listViewWindow = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lastupdateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(44, 6);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(100, 19);
            this.textBoxPort.TabIndex = 3;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(150, 4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "接続";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // listViewWindow
            // 
            this.listViewWindow.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewWindow.Location = new System.Drawing.Point(12, 63);
            this.listViewWindow.Name = "listViewWindow";
            this.listViewWindow.Size = new System.Drawing.Size(422, 290);
            this.listViewWindow.TabIndex = 5;
            this.listViewWindow.UseCompatibleStateImageBehavior = false;
            this.listViewWindow.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "title";
            this.columnHeader1.Width = 400;
            // 
            // lastupdateLabel
            // 
            this.lastupdateLabel.AutoSize = true;
            this.lastupdateLabel.Location = new System.Drawing.Point(12, 48);
            this.lastupdateLabel.Name = "lastupdateLabel";
            this.lastupdateLabel.Size = new System.Drawing.Size(43, 12);
            this.lastupdateLabel.TabIndex = 6;
            this.lastupdateLabel.Text = "request";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 364);
            this.Controls.Add(this.lastupdateLabel);
            this.Controls.Add(this.listViewWindow);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Window Operation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ListView listViewWindow;
        private System.Windows.Forms.Label lastupdateLabel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}

